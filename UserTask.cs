using System;
using System.Collections.Generic;

namespace TelegramBot
{
    public class UserTask
    {
        public int id { get; set; }
        public User Owner { get; set; }
        public string Description { get; set; }
        
        public bool IsComplite
        {
            get;
            set;
        }

        public int CompliteCode = new Random().Next(1000, 9999);
        public int cost = 20;
        public long id_perf { get; set; }
        public bool in_exec { get; set; }

        public bool Complite(int key)
        {
            if (key == this.CompliteCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }

    static class AllTasks
    {
        public static List<UserTask> TaskList = new List<UserTask>();


        public static string ReturnAllTaskByID(long UserID)
        {
            string resString = "Список ваших завдань: \n";
            string r = "------------------ \n";

            foreach (var task in TaskList)
            {
                if (task.Owner.UserId == UserID)
                {
                    resString +=r + "Завдання: \n" + task.Description + "\n" + 
                                (task.IsComplite == true ? "Завдання виконано" : "Завдання ще не виконано") + "\n\n" +
                                r + "\n\n";
                }
            }
            return resString;
        }

        public static string GetTaskToPerformer(long UserID)
        {
            UserTask _task = null;
            string resString = "Ваше завданя: \n";

            foreach (var task in TaskList)
            {
                if (task.id_perf == UserID)
                {
                    _task = task;
                    break;
                }
            }

            if (_task == null)
            {
                resString += "У вас немає завдань!";
            }
            else
            {
                resString += $"{_task.Description} \nАвтор завдання - {_task.Owner.UserName}";
            }
            
            return resString;
        }

        public static string ToString(UserTask task)
        {
            return $"Завдання: \n\n{task.Description} \n\nАвтор завдання:\n{task.Owner.UserName} " +
                   $"\n\nІнстаграм автора:\n{task.Owner.inst}";
        }
        
        public static string RamdomChoise(long UserID)
        {
            while (true)
            {
                int choise = new Random().Next(0, TaskList.Count - 1);
                Console.WriteLine(choise);
                if (TaskList[choise].in_exec != true)
                {
                    TaskList[choise].in_exec = true;
                    TaskList[choise].id_perf = UserID;
                    AllUser.GetUser(UserID).IsPerf = true;
                    return ToString(TaskList[choise]);
                }
            }
        }

        public static UserTask GetTaskPerfID(long UserID)
        {
            foreach (var task in TaskList)
            {
                if (task.id_perf == UserID && task.IsComplite == false)
                {
                    return task;
                }
            }

            return null;
        }

        public static string GetDescTaskToPerf(UserTask task)
        {
            return  AllTasks.ToString(task) + "\n\n" +
                   $"\n\nДля того, щоб завершити виконання цього завдання, вам потрібно зняти інстаграм-історію " +
                   $"та запостити її із #FMI та відміткою нашої сторінки - @m._i_.f та натиснути 'Я виконав завдання'" +
                   $"\n\nНаші модератори отримають сповіщення із 4-значним кодом.\n Перевіривши правильність виконаного" +
                   $"вами завдання, вони надішлють цей код вам.";
        }

        public static bool IsFreeTask()
        {
            foreach (var task in TaskList)
            {
                if (!task.in_exec)
                {
                    return true;
                }
            }

            return false;
        }
    }
}