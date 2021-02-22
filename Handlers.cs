using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.handlers;
using static TelegramBot.AllTasks;

namespace TelegramBot
{
    public class Handlers
    {
        private ITelegramBotClient _bot;
        public Handlers(ITelegramBotClient botClient)
        {
            this._bot = botClient;
        }

        public static bool startEvent = true;
        
        static public async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            bool IsUser()
            {
                return Settings.UsersList.Any(User => User == e.Message.From.Id);
            }

            bool IsAdmin()
            {
                return Settings.AdminsList.Any(Admin => Admin == e.Message.From.Id);
            }

            static bool IsMale(long UserID)
            {
                return AllUser.GetUser(UserID).Male;
            }

            if (e.Message.Text != null)
            {
                if (!IsUser())
                {
                    switch (e.Message.Text)
                    {
                        case "/start":
                            await Program.botClient.SendTextMessageAsync(
                                chatId: e.Message.Chat.Id, 
                                "Привіт! \nДавай ми тебе зареєструємо?",
                                replyMarkup:Keyboard.Start()
                            );
                            break;
                    }
                } else if (IsAdmin())
                {
                    switch (e.Message.Text)
                    {
                        case "!STARTEVENT":
                            
                            break;
                        case "!admin":
                            await Program.botClient.SendTextMessageAsync(
                                e.Message.From.Id,
                                "\t Адмін-панель",
                                replyMarkup: Keyboard.AdminPanel()
                            );
                            break;
                        case "!menu":
                            if (IsMale(e.Message.From.Id))
                            {
                                await Program.botClient.SendTextMessageAsync(
                                    e.Message.Chat.Id,
                                    "Виберіть дію: \n",
                                    replyMarkup: Keyboard.ForMale());
                            }
                            else
                            {
                                await Program.botClient.SendTextMessageAsync(
                                    e.Message.Chat.Id,
                                    "\t Дії:",
                                    replyMarkup: Keyboard.UserMenuFemale());
                            }
                            break;
                    }
                }
                else if (IsUser())
                {
                    if (IsMale(e.Message.From.Id))
                    {
                        switch (e.Message.Text)
                        {
                            case "!menu":
                                await Program.botClient.SendTextMessageAsync(
                                    e.Message.Chat.Id,
                                    "Виберіть дію: \n",
                                    replyMarkup: Keyboard.ForMale());
                                break;
                        }
                    }
                    else
                    {
                        switch (e.Message.Text)
                        {
                            case "!menu":
                                await Program.botClient.SendTextMessageAsync(
                                    e.Message.Chat.Id,
                                    "\t Дії:",
                                    replyMarkup: Keyboard.UserMenuFemale());
                                break;
                        }
                    }

                    switch (e.Message.Text)
                    {
                        case "/start":
                            await Program.botClient.SendTextMessageAsync(
                                e.Message.From.Id, 
                                "Ви уже зареєстровані!",
                                replyMarkup: new ReplyKeyboardRemove()
                            );
                            break;
                        
                    }
                    
                }
            }
        }

        public static async void Bot_OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            if (e.CallbackQuery.Data != null)
            {
                switch (e.CallbackQuery.Data)
                {
                    case "aboutme":
                        User user = AllUser.GetUser(e.CallbackQuery.From.Id);
                        await Program.botClient.SendTextMessageAsync(
                            e.CallbackQuery.From.Id,
                            text: AllUser.AboutMe(user),
                            replyMarkup: Keyboard.EditUser()
                        );
                        break;
                    case "takemetask":
                        if (startEvent)
                        {
                            if (AllTasks.TaskList.Count <= 0)
                            {
                                await Program.botClient.AnswerCallbackQueryAsync(
                                    e.CallbackQuery.Id, 
                                    "Завдань поки-що немає!");
                            }
                            else if (AllTasks.GetTaskPerfID(e.CallbackQuery.From.Id) == null)
                            {
                                if (IsFreeTask())
                                {
                                    await Program.botClient.SendTextMessageAsync(
                                        e.CallbackQuery.From.Id,
                                        AllTasks.RamdomChoise(e.CallbackQuery.From.Id) +
                                        "\n\nВи можете відмовитись від цього завдання, проте втратити 1 монету із вашого рахунку.",
                                        replyMarkup: Keyboard.ChoiseTask()
                                    );
                                }
                                else
                                {
                                    await Program.botClient.AnswerCallbackQueryAsync(
                                        e.CallbackQuery.Id, 
                                        "Завдань поки-що немає!");
                                }
                            }
                            else
                            {
                                await Program.botClient.AnswerCallbackQueryAsync(
                                    e.CallbackQuery.Id, 
                                    "Ви уже отримали завдання!");
                            }
                        }
                        else
                        {
                            await Program.botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id, "Івент ще не розпочався!");
                        }
                        break;
                    case "NOT":
                        var task = AllTasks.GetTaskPerfID(e.CallbackQuery.From.Id);
                        task.id_perf = 0;
                        task.in_exec = false;
                        var _user = AllUser.GetUser(e.CallbackQuery.From.Id);
                        _user.IsPerf = false;
                        _user.Store -= 1;
                        DataBase.DataBase.UpdUserStore(_user);
                        await Program.botClient.AnswerCallbackQueryAsync(
                            e.CallbackQuery.Id, 
                            $"Ви не прийняли завдання! \nНа вашому рахунку залишилось {_user.Store} монет!"
                        );
                        await Program.botClient.DeleteMessageAsync(
                            e.CallbackQuery.From.Id,
                            e.CallbackQuery.Message.MessageId);
                        break;
                    case "YEST":
                        var Ytask = AllTasks.GetTaskPerfID(e.CallbackQuery.From.Id);
                        var Yuser = AllUser.GetUser(e.CallbackQuery.From.Id);
                        DataBase.DataBase.AddTaskToUser(Ytask, Yuser);
                        await Program.botClient.SendTextMessageAsync(
                            e.CallbackQuery.From.Id,
                            AllTasks.GetDescTaskToPerf(Ytask)
                        );
                        await Program.botClient.DeleteMessageAsync(
                            e.CallbackQuery.From.Id,
                            e.CallbackQuery.Message.MessageId);
                        break;
                    case "reg":
                        await Register.RegisterNewUser(e.CallbackQuery.From.Id, Program.botClient);
                        break;
                    case "ComplTask":
                        if (startEvent)
                        {
                            if (AllUser.GetUser(e.CallbackQuery.From.Id).IsPerf == true)
                            {
                                var Cuser = AllUser.GetUser(e.CallbackQuery.From.Id);
                                var Ctask = AllTasks.GetTaskPerfID(Cuser.UserId);
                                string msg = $"Потрібно підтвердження!\n" +
                                             $"Name - {Cuser.UserName} \n" +
                                             $"ID - {Cuser.UserId} \n" +
                                             $"Inst - {Cuser.inst} \n" +
                                             $"TaskDescription: \n {Ctask.Description}" +
                                             $"\n\n\nSecret code - {Ctask.CompliteCode}";
                                await AllUser.SendMsgToAdmin(msg);

                                await Program.botClient.SendTextMessageAsync(
                                    Cuser.UserId,
                                    "Очікуйте 4-значний код від наших модератораторів!",
                                    replyMarkup: Keyboard.EnterCompCode());
                            }
                            else
                            {
                                await Program.botClient.AnswerCallbackQueryAsync(
                                    e.CallbackQuery.Id,
                                    "У вас немає завдання!"
                                );
                            }
                        }
                        else
                        {
                            await Program.botClient.AnswerCallbackQueryAsync(
                                e.CallbackQuery.Id,
                                "Івент ще не розпочався!");
                        }
                        break;
                    case "enterCode":
                        await EnterCode.EnterCompCode(e.CallbackQuery.From.Id);
                        break;
                    case "createNewTask":
                        await AddTask.AddNewTask(e.CallbackQuery.From.Id);
                        break;
                    case "mytasks":
                        await Program.botClient.SendTextMessageAsync(
                            e.CallbackQuery.From.Id,
                            ReturnAllTaskByID(e.CallbackQuery.From.Id)
                        );
                        break;
                    case "aboutEvent":
                        await Program.botClient.SendTextMessageAsync(
                            e.CallbackQuery.From.Id,
                            Settings.AboutEvent
                        );
                        break;
                    case "author":
                        await Program.botClient.SendTextMessageAsync(
                            e.CallbackQuery.From.Id,
                            Settings.Author
                        );
                        break;
                    case "mytasksComp":
                        if (startEvent)
                        {
                            if (AllUser.GetUser(e.CallbackQuery.From.Id).IsPerf == true)
                            {
                                await Program.botClient.SendTextMessageAsync(
                                    e.CallbackQuery.From.Id,
                                    AllTasks.GetDescTaskToPerf(AllTasks.GetTaskPerfID(e.CallbackQuery.From.Id)),
                                    replyMarkup: Keyboard.FMI()
                                );
                            }
                            else
                            {
                                await Program.botClient.AnswerCallbackQueryAsync(
                                    e.CallbackQuery.Id,
                                    "У вас немає завдання!"
                                );
                            }
                        }
                        else
                        {
                            await Program.botClient.AnswerCallbackQueryAsync(
                                e.CallbackQuery.Id,
                                "Івент ще не розпочався!"
                            );
                        }
                        break;
                    case "EditName":
                        await Edit.EditUserName(e.CallbackQuery.From.Id);
                        break;
                    case "EditInst":
                        await Edit.EditUserInst(e.CallbackQuery.From.Id);
                        break;
                    case "ADMINCmd":
                        await Admin.ACmd(e.CallbackQuery.From.Id);
                        break;
                    case "ADMINGetAboutUser":
                        await Program.botClient.EditMessageReplyMarkupAsync(
                            e.CallbackQuery.From.Id,
                            e.CallbackQuery.Message.MessageId,
                            replyMarkup: Keyboard.ShowAllUser()
                        );
                        break;
                    case "SearchUserID":
                        await Admin.SearchUserID(e.CallbackQuery.From.Id);
                        break;
                    case "SearchUserUsername":
                        await Admin.SearchUserUsername(e.CallbackQuery.From.Id);
                        break;
                    case "ShowAllUsers":
                        await Admin.ShowAllUsers(e.CallbackQuery.From.Id);
                        break;
                    case "ShowAllMale":
                        await Admin.ShowAllMale(e.CallbackQuery.From.Id);
                        break;
                    case "ShowAllFemale":
                        await Admin.ShowAllFemale(e.CallbackQuery.From.Id);
                        break;
                    case "ShowUserBack":
                        await Program.botClient.EditMessageReplyMarkupAsync(
                            e.CallbackQuery.From.Id,
                            e.CallbackQuery.Message.MessageId,
                            replyMarkup: Keyboard.AdminPanel()
                        );
                        break;
                }
            }
        }
    }
}