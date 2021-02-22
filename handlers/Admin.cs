using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Requests;

namespace TelegramBot.handlers
{
    public static class Admin
    {
        public static bool IsInUserList(string usID)
        {
            long id = 0;
            try
            {
                id = Convert.ToInt32(usID);
            }
            catch (Exception exception)
            {
                return false;
            }

            foreach (var user in AllUser.AllUsersList)
            {
                if (user.UserId == id)
                {
                    return true;
                }
            }

            return false;
        }

        public static async Task ACmd(long msgFromID)
        {
            int step = 1;
            var pr = new ManualResetEvent(false);
            

            EventHandler<MessageEventArgs> cmd = (object sender, MessageEventArgs e) =>
            {
                if (msgFromID != e.Message.From.Id) return;
                if (msgFromID != e.Message.Chat.Id) return;

                if (step == 1)
                {
                    step++;
                    var commandList = e.Message.Text.Split(' ');
                    if (commandList.Length < 4 && commandList.Length > 4)
                    {
                        Program.botClient.SendTextMessageAsync(
                            e.Message.From.Id,
                            "Недопустима кількість параметрів!");
                    }
                    
                    if (commandList[0] == "!admin")
                    {
                        if (commandList[1] == "upname")
                        {
                            if (IsInUserList(commandList[2]))
                            {
                                string name = " ";
                                for (int i = 3; i < commandList.Length; i++)
                                {
                                    name += commandList[i] + " ";
                                }

                                var user = AllUser.GetUser(Convert.ToInt32(commandList[2]));
                                if (user == null)
                                {
                                    Program.botClient.SendTextMessageAsync(
                                        e.Message.From.Id,
                                        "User is null!");
                                    return;
                                }

                                string _old_name = user.UserName;
                                user.UserName = name;
                                DataBase.DataBase.UpdUserName(user);
                                Program.botClient.SendTextMessageAsync(
                                    user.UserId,
                                    $"Адміністратор {AllUser.GetUser(e.Message.From.Id).UserName} змінив ваше ім'я!" +
                                    $"\n\n {_old_name} --> {user.UserName}"
                                );
                                foreach (var adm in Settings.AdminsList)
                                {
                                    Program.botClient.SendTextMessageAsync(
                                        adm,
                                        $"Адміністратор {AllUser.GetUser(e.Message.From.Id).UserName} змінив ім'я користувачу!" +
                                        $"\n\n {_old_name} --> {user.UserName}"
                                    );
                                }

                            }
                            else
                            {
                                Program.botClient.SendTextMessageAsync(
                                    e.Message.From.Id,
                                    "Користувача з таким ID не знайдено!");
                            }
                        }
                        else if (commandList[1] == "upstore")
                        {
                            if (IsInUserList(commandList[2]))
                            {
                                try
                                {
                                    var newStore = Convert.ToInt32(commandList[3]);
                                    var user = AllUser.GetUser(Convert.ToInt32(commandList[2]));
                                    var _oldStore = user.Store;
                                    user.Store = newStore;
                                    DataBase.DataBase.UpdUserStore(user);
                                    
                                    Program.botClient.SendTextMessageAsync(
                                        user.UserId,
                                        $"Адміністратор {AllUser.GetUser(e.Message.From.Id).UserName} змінив ваш рахунок!" +
                                        $"\n\n {_oldStore} --> {user.Store}"
                                    );
                                    foreach (var adm in Settings.AdminsList)
                                    {
                                        Program.botClient.SendTextMessageAsync(
                                            adm,
                                            $"Адміністратор {AllUser.GetUser(e.Message.From.Id).UserName} змінив рахунок користувачу!" +
                                            $"\n\n {_oldStore} --> {user.Store}"
                                        );
                                    }

                                }
                                catch (Exception exception)
                                {
                                    Program.botClient.SendTextMessageAsync(
                                        e.Message.From.Id,
                                        "Недопустиме значення для рахунку!"
                                    );
                                }
                            }
                            else
                            {
                                Program.botClient.SendTextMessageAsync(
                                    e.Message.From.Id,
                                    "Користувача з таким ID не знайдено!");
                            }
                        }
                        else if (commandList[1] == "upinst")
                        {
                            if (IsInUserList(commandList[2]))
                            {
                                var user = AllUser.GetUser(Convert.ToInt32(commandList[2]));
                                var _oldInst = user.inst;
                                user.inst = commandList[3];
                                DataBase.DataBase.UpdUserInst(user);
                                
                                Program.botClient.SendTextMessageAsync(
                                    user.UserId,
                                    $"Адміністратор {AllUser.GetUser(e.Message.From.Id).UserName} змінив ваш instagram!" +
                                    $"\n\n {_oldInst} --> {user.inst}"
                                );
                                foreach (var adm in Settings.AdminsList)
                                {
                                    Program.botClient.SendTextMessageAsync(
                                        adm,
                                        $"Адміністратор {AllUser.GetUser(e.Message.From.Id).UserName} змінив instagram користувачу!" +
                                        $"\n\n {_oldInst} --> {user.inst}"
                                    );
                                }
                            }
                            else
                            {
                                Program.botClient.SendTextMessageAsync(
                                    e.Message.From.Id,
                                    "Користувача з таким ID не знайдено!");
                            }
                        }
                        else if (commandList[1] == "sendmsg")
                        {
                            if (IsInUserList(commandList[2]))
                            {
                                string msg = "";
                                for (int i = 3; i < commandList.Length; i++)
                                {
                                    msg += commandList[i] = " ";
                                }

                                Program.botClient.SendTextMessageAsync(
                                    Convert.ToInt32(commandList[2]),
                                    $"Адміністратор {AllUser.GetUser(e.Message.From.Id).UserName} надіслав вам повідомлення: \n\n" +
                                    $"    - {msg}"
                                );
                                Program.botClient.SendTextMessageAsync(
                                    e.Message.From.Id,
                                    $"Ви надіслали повідомлення для {AllUser.GetUser(Convert.ToInt32(commandList[2])).UserName}: \n\n" +
                                    $"    - {msg}"
                                );
                            }
                            else
                            {
                                Program.botClient.SendTextMessageAsync(
                                    e.Message.From.Id,
                                    "Користувача з таким ID не знайдено!");
                            }
                        }
                        else
                        {
                            Program.botClient.SendTextMessageAsync(
                                msgFromID,
                                "Синтаксична помилка у другому слові!");
                        }
                    }
                    else if (commandList[0] == "!STARTEVENT")
                    {
                        if (e.Message.From.Id == Settings.Owner)
                        {
                            Handlers.startEvent = true;
                            foreach (var adm in Settings.AdminsList)
                            {
                                Program.botClient.SendTextMessageAsync(
                                    adm,
                                    $"Адміністратор {AllUser.GetUser(e.Message.From.Id).UserName} запустив івент!"
                                );
                            }
                        }
                        else
                        {
                            Handlers.startEvent = false;
                            Program.botClient.SendTextMessageAsync(
                                e.Message.Chat.Id,
                                "У вас немає доступу до цієї команди!"
                            );
                        }
                    }
                    else if (commandList[0] == "!STOPEVENT")
                    {
                        if (e.Message.From.Id == Settings.Owner)
                        {
                            Handlers.startEvent = false;
                            foreach (var adm in Settings.AdminsList)
                            {
                                Program.botClient.SendTextMessageAsync(
                                    adm,
                                    $"Адміністратор {AllUser.GetUser(e.Message.From.Id).UserName} запустив івент!"
                                );
                            }
                        }
                        else
                        {
                            Handlers.startEvent = false;
                            Program.botClient.SendTextMessageAsync(
                                e.Message.Chat.Id,
                                "У вас немає доступу до цієї команди!"
                            );
                        }
                    }
                }
            };
            await Program.botClient.SendTextMessageAsync(
                msgFromID,
                "Список команд: \n\n\n" +
                "!admin upname <id user> <new_name> \n\n" +
                "!admin upstore <id user> <new store> \n\n" +
                "!admin upinst <id user> <new inst> \n\n" +
                "!admin sendmsg <id user> <message> \n\n" +
                "!STARTEVENT \n\n" +
                "!STOPEVENT \n\n" +
                "Введіть команду:"
            );
            
            
            Program.botClient.OnMessage += cmd;
            pr.WaitOne();
            Program.botClient.OnMessage -= cmd;
        }

        public static async Task SearchUserID(long UserID)
        {
            int step = 1;
            var pr = new ManualResetEvent(false);

            EventHandler<MessageEventArgs> search = (object SendMessageRequest, MessageEventArgs e) =>
            {
                if (UserID != e.Message.From.Id) return;
                if (UserID != e.Message.Chat.Id) return;

                if (step == 1)
                {
                    step++;
                    if (IsInUserList(e.Message.Text))
                    {
                        Program.botClient.SendTextMessageAsync(
                            UserID,
                            AllUser.AboutMe(AllUser.GetUser(Convert.ToInt32(e.Message.Text)))
                        );
                    }
                    else
                    {
                        Program.botClient.SendTextMessageAsync(
                            UserID,
                            "Користувача з таким ID не знайдено!"
                        );
                    }
                }
            };
            await Program.botClient.SendTextMessageAsync(
                UserID,
                "Введіть ID користувача:"
            );

            Program.botClient.OnMessage += search;
            pr.WaitOne();
            Program.botClient.OnMessage -= search;
        }
        
        public static async Task SearchUserUsername(long UserID)
        {
            int step = 1;
            var pr = new ManualResetEvent(false);

            EventHandler<MessageEventArgs> search = (object SendMessageRequest, MessageEventArgs e) =>
            {
                if (UserID != e.Message.From.Id) return;
                if (UserID != e.Message.Chat.Id) return;

                if (step == 1)
                {
                    step++;
                    bool flag = false;
                    User user = null;
                    foreach (var u in AllUser.AllUsersList)
                    {
                        if (u.UserName == e.Message.Text)
                        {
                            flag = true;
                            user = u;
                            break;
                        }
                    }
                    
                    if (flag)
                    {
                        Program.botClient.SendTextMessageAsync(
                            UserID,
                            AllUser.AboutMe(user)
                        );
                    }
                    else
                    {
                        Program.botClient.SendTextMessageAsync(
                            UserID,
                            "Користувача з таким іменем не знайдено!"
                        );
                    }
                }
            };
            await Program.botClient.SendTextMessageAsync(
                UserID,
                "Введіть ID користувача:"
            );

            Program.botClient.OnMessage += search;
            pr.WaitOne();
            Program.botClient.OnMessage -= search;
        }

        public static async Task ShowAllUsers(long UserID)
        {
            string res = "";
            foreach (var user in AllUser.AllUsersList)
            {
                res += AllUser.AboutMe(user) + "\n\n";
            }

            await Program.botClient.SendTextMessageAsync(
                UserID,
                res
            );
        } 
        
        public static async Task ShowAllMale(long UserID)
        {
            string res = "";
            foreach (var user in AllUser.AllUsersList)
            {
                if (user.Male)
                {
                    res += AllUser.AboutMe(user) + "\n\n";
                }
            }

            await Program.botClient.SendTextMessageAsync(
                UserID,
                res
            );
        } 
        
        public static async Task ShowAllFemale(long UserID)
        {
            string res = "";
            foreach (var user in AllUser.AllUsersList)
            {
                if (!user.Male)
                {
                    res += AllUser.AboutMe(user) + "\n\n";
                }
            }

            await Program.botClient.SendTextMessageAsync(
                UserID,
                res
            );
        } 
    }
}