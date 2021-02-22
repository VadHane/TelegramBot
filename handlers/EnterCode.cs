using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Args;

namespace TelegramBot.handlers
{
    public static class EnterCode
    {
        public static async Task EnterCompCode(long msgFromID)
        {
            int step = 1;
            var pr = new ManualResetEvent(false);

            EventHandler<MessageEventArgs> enter = (object sender, MessageEventArgs e) =>
            {
                if (msgFromID != e.Message.From.Id) return;
                if (msgFromID != e.Message.Chat.Id) return;
                
                if (step == 1)
                {
                    try
                    {
                        int key = Convert.ToInt32(e.Message.Text);
                        if (AllTasks.GetTaskPerfID(msgFromID).Complite(key))
                        {
                            var t = AllTasks.TaskList[1];
                            var u = AllUser.AllUsersList[1];
                            var task = AllTasks.GetTaskPerfID(msgFromID);
                            var user = AllUser.GetUser(task.id_perf);
                            DataBase.DataBase.TaskIsComp(task);
                            user.Store += 20;
                            user.IsPerf = false;
                            step++;
                            Program.botClient.SendTextMessageAsync(
                                msgFromID,
                                "Вітаю! \n" +
                                "Ви завершили завдання та отримали +20 монет на свій рахунок! \n" +
                                "Так тримати!"
                            );
                            Program.botClient.SendTextMessageAsync(
                                task.Owner.UserId,
                                $"{user.UserName} завершив ваше завдання! \n\n" +
                                $"Переглянути його реалізацію ви можете на інстаграм сторінці профбюро ФМІ!",
                                replyMarkup: Keyboard.FMI()
                            );

                        }
                        else
                        {
                            Program.botClient.SendTextMessageAsync(
                                msgFromID,
                                "Невірний код!",
                                replyMarkup: Keyboard.ForMale());
                        }
                        
                    }
                    catch (Exception exception)
                    {
                        Program.botClient.SendTextMessageAsync(
                            msgFromID,
                            "Невірний код! ERROR",
                            replyMarkup: Keyboard.ForMale());
                    }
                }
            };
            await Program.botClient.SendTextMessageAsync(
                msgFromID,
                "Введіть 4-значний код: "
            );

            Program.botClient.OnMessage += enter;
            pr.WaitOne();
            Program.botClient.OnMessage -= enter;
        }
    }
}