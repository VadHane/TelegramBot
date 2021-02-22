using System;
using System.Threading;
using Telegram.Bot;



namespace TelegramBot
{
    class Program
    {
        static public ITelegramBotClient botClient;


        static void Main(string[] args)
        {
            botClient = new TelegramBotClient(Settings.TOKEN);
            DataBase.DataBase.ReadUsers();
            DataBase.DataBase.ReadAllTasks();
            Console.WriteLine(
                $"[LOG][{DateTime.Now.TimeOfDay}] Bot is running!"
                );

            botClient.SendTextMessageAsync(
                chatId: Settings.Owner, text: "Я запустився! \n /start \n" + DateTime.Now
                );
            
            botClient.OnMessage += Handlers.Bot_OnMessage;
            botClient.OnCallbackQuery += Handlers.Bot_OnCallbackQuery;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }
    }
}