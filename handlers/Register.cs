using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;


namespace TelegramBot.handlers
{
    public static class Register
    {

        public static async Task RegisterNewUser(long msgFromID, ITelegramBotClient _bot)
        {
            int step = 1;
            var reg = new ManualResetEvent(false);
            User user = new User();

            EventHandler<MessageEventArgs> register = (object sender, MessageEventArgs e) =>
            {
                if (msgFromID != e.Message.From.Id) return;
                if (msgFromID != e.Message.Chat.Id) return;

                if (step == 1)
                {
                    user.UserName = e.Message.Text;
                    user.UserId = e.Message.From.Id;
                    step++;
                    _bot.SendTextMessageAsync(
                        msgFromID, "Вкажіть лінк на Вашу інстаграм сторінку: ", replyMarkup: new ReplyKeyboardRemove()
                    );
                }
                else if (step == 2)
                {
                    user.inst = e.Message.Text;
                    step++;
                    _bot.SendTextMessageAsync(
                        msgFromID, "Виберіть вашу стать: ", replyMarkup: Keyboard.CreateRegKeybord(msgFromID)
                    );
                }
                else if (step == 3)
                {
                    Settings.UsersList.Add(user.UserId);
                    AllUser.AllUsersList.Add(user);
                    
                    if (e.Message.Text == "Чоловіча")
                    {
                        user.Male = true;
                        step++;
                        _bot.SendTextMessageAsync(
                            chatId: msgFromID,
                            "Ви успішно зареєструвались! \n!menu - щоб отримати список усіх доступних вам дій",
                            replyMarkup: new ReplyKeyboardRemove()
                        );
                        _bot.SendTextMessageAsync(e.Message.From.Id, "Виберіть дію: \n", replyMarkup: Keyboard.ForMale());
                    }
                    else
                    {
                        user.Male = false;
                        step++;
                        _bot.SendTextMessageAsync(
                            msgFromID,
                            "Дякую за реєстрацію! " +
                            "\nТепер Ви можете добавити завдання для випадкового студента! \n" +
                            "!menu - щоб отримати список усіх доступних вам дій",
                            replyMarkup: new ReplyKeyboardRemove()
                        );
                        _bot.SendTextMessageAsync(
                            e.Message.From.Id, 
                            "Виберіть дію: \n", 
                            replyMarkup: Keyboard.UserMenuFemale());
                        Settings.UsersList.Add(user.UserId);
                    }
                    DataBase.DataBase.AddUserDB(user);
                }
            };
            
            await _bot.SendTextMessageAsync(
                msgFromID,
                "Давай знайомитись? Я - звичайний бот \nА як звуть тебе?",
                replyMarkup: new ReplyKeyboardRemove()
            );
            
            
            _bot.OnMessage += register;
            reg.WaitOne();
            _bot.OnMessage -= register;
        }
    }
}