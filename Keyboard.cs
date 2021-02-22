using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot
{
    static class Keyboard
    {
        static public ReplyKeyboardMarkup CreateRegKeybord(long UserID)
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton("Чоловіча"),
                new KeyboardButton("Жіноча")
            });
        }

        static public ReplyKeyboardMarkup YesOrNo()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton("Так"),
                new KeyboardButton("Ні")
            });
        }

        static public InlineKeyboardMarkup ForMale()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Отримати завдання", "takemetask")
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Про мене", "aboutme"),
                    InlineKeyboardButton.WithCallbackData("Моє завдання", "mytasksComp"),
                    },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Про івент", "aboutEvent"),
                    InlineKeyboardButton.WithCallbackData("Автор", "author")
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Я виконав завдання!", "ComplTask"), 
                }
            });
        }

        static public InlineKeyboardMarkup Start()
        {
            return new InlineKeyboardMarkup(new[]
            {
                InlineKeyboardButton.WithCallbackData("Зареєструватись", "reg"),
            });
        }

        static public InlineKeyboardMarkup AdminPanel()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Ввести команду", "ADMINCmd"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(
                        "Вивести інформацію про користувача", "ADMINGetAboutUser"),  
                }
            });
        }

        static public InlineKeyboardMarkup UserMenuFemale()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new []
                {
                InlineKeyboardButton.WithCallbackData("Створити нове завдання", "createNewTask"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Про мене", "aboutme"),
                    InlineKeyboardButton.WithCallbackData("Мої завдання", "mytasks"), 
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Про івент", "aboutEvent"),
                    InlineKeyboardButton.WithCallbackData("Автор", "author"), 
                }
            });
        }

        public static InlineKeyboardMarkup ChoiseTask()
        {
            return new InlineKeyboardMarkup(new[]
            {
                InlineKeyboardButton.WithCallbackData("Я приймаю!", "YEST"),
                InlineKeyboardButton.WithCallbackData("Я відмовляюсь", "NOT"),
            });
        }

        public static InlineKeyboardMarkup EnterCompCode()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Ввести код", "enterCode"),
                },
                new[]
                {
                    InlineKeyboardButton.WithUrl("Instagram", url: "instagram.com/m._i_.f/"),
                }
            });
        }

        public static InlineKeyboardMarkup FMI()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithUrl("Instagram", url: "instagram.com/m._i_.f/"),
                }
            });
        }

        public static InlineKeyboardMarkup EditUser()
        {
            return new InlineKeyboardMarkup(new[]
            {
                InlineKeyboardButton.WithCallbackData("Змінити ім'я", "EditName"),
                InlineKeyboardButton.WithCallbackData("Зінити інстаграм", "EditInst"), 
            });
        }

        public static InlineKeyboardMarkup ShowAllUser()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Знайти користувача по ID", "SearchUserID"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Знайти користувача по імені", "SearchUserUsername"), 
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Вивести усіх користувачів (!)", "ShowAllUsers"), 
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Усіх хлопців", "ShowAllMale"),
                    InlineKeyboardButton.WithCallbackData("Усіх дівчат", "ShowAllFemale"), 
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("<- Назад", "ShowUserBack"), 
                }
            });
        }
    }
}