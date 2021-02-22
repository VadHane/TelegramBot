using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.handlers
{
    public static class AddTask
    {
        

        public static async Task AddNewTask(long msgFromID)
        {
            int step = 1;
            UserTask _task = new UserTask();
            
            var add = new ManualResetEvent(false);

            EventHandler<MessageEventArgs> addT = (object sender, MessageEventArgs e) =>
            {
                if (msgFromID != e.Message.From.Id) return;
                if (msgFromID != e.Message.Chat.Id) return;

                if (step == 1)
                {
                    _task.Owner = AllUser.GetUser(e.Message.Chat.Id);
                    _task.Description = e.Message.Text;
                    _task.IsComplite = false;
                    step++;
                    AllTasks.TaskList.Add(_task);
                    Program.botClient.SendTextMessageAsync(e.Message.Chat.Id, "Завдання успішно додано!",
                        replyMarkup: Keyboard.UserMenuFemale());
                    
                    DataBase.DataBase.AddTaskDB(_task);
                    Console.WriteLine($"[LOG]Author = {_task.Owner.UserName} TaskID = {_task.id}; TaskDesc = '{_task.Description}'");
                }
            };
            await Program.botClient.SendTextMessageAsync(
                msgFromID,
                "Напишіть детальний опис вашого завдання до найменшої деталі.",
                replyMarkup:new ReplyKeyboardRemove()
            );

            Program.botClient.OnMessage += addT;
            add.WaitOne();
            Program.botClient.OnMessage -= addT;
        }
    }
}