using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Args;

namespace TelegramBot.handlers
{
    public static class Edit
    {
        public static async Task EditUserName(long UserID)
        {
            int step = 1;

            var pr = new ManualResetEvent(false);

            EventHandler<MessageEventArgs> name = (object sender, MessageEventArgs e) =>
            {
                if (UserID != e.Message.From.Id) return;
                if (UserID != e.Message.Chat.Id) return;

                if (step == 1)
                {
                    step++;
                    var user = AllUser.GetUser(UserID);
                    user.UserName = e.Message.Text;
                    DataBase.DataBase.UpdUserName(user);
                    Program.botClient.SendTextMessageAsync(
                        UserID,
                        "Ваше ім'я успішно змінено!"
                    );
                }
                

            };
            await Program.botClient.SendTextMessageAsync(
                UserID,
                "Введіть нове ім'я:");

            Program.botClient.OnMessage += name;
            pr.WaitOne();
            Program.botClient.OnMessage -= name;
        }
        
        public static async Task EditUserInst(long UserID)
        {
            int step = 1;

            var pr = new ManualResetEvent(false);

            EventHandler<MessageEventArgs> inst = (object sender, MessageEventArgs e) =>
            {
                if (UserID != e.Message.From.Id) return;
                if (UserID != e.Message.Chat.Id) return;

                if (step == 1)
                {
                    step++;
                    var user = AllUser.GetUser(UserID);
                    user.inst = e.Message.Text;
                    DataBase.DataBase.UpdUserInst(user);
                    Program.botClient.SendTextMessageAsync(
                        UserID,
                        "Ваш інстаграм успішно змінено!"
                    );
                }
                

            };
            await Program.botClient.SendTextMessageAsync(
                UserID,
                "Введіть новий інстаграм лінк:");

            Program.botClient.OnMessage += inst;
            pr.WaitOne();
            Program.botClient.OnMessage -= inst;
        }
    }
}