using System.Collections.Generic;
using System.Threading.Tasks;

namespace TelegramBot
{
    public class User
    {
        public string UserName { get; set; }
        public int UserId { get; set; }
        public bool Male { get; set; } // True - Male, False - Female
        public int Store = 5;
        public string inst { get; set; }
        public bool IsPerf { get; set; }
    }

    public static class AllUser
    {
        public static List<User> AllUsersList = new List<User>();


        static public User GetUser(long UserID)
        {
            foreach (var user in AllUsersList)
            {
                if (user.UserId == UserID)
                {
                    return user;
                }
            }

            return null;
        }


        static public string AboutMe(User user)
        {
            return $"Ім'я користувача - {user.UserName} " +
                   $"\nID користувача - {user.UserId} \nСтать - " +  (user.Male == true ? "Чоловіча" : "Жіноча" ) + 
                   "\n" + $"Інстаграм - {user.inst} \n" + $"Кількість монет - {user.Store}";
        }

        public static async Task SendMsgToAdmin(string msg)
        {
            foreach (var adm in Settings.AdminsList)
            {
                await Program.botClient.SendTextMessageAsync(adm, msg);
            }
        }
    }
}