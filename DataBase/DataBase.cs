using System;
using Npgsql;

namespace TelegramBot.DataBase
{
    public static class DataBase
    {
        private static NpgsqlConnection conn = DBSettings.CreateConn();
        
        public static void AddUserDB(User u)
        {
            string script;
            conn.Open();
            
            script = $"INSERT INTO users (name, telegram_id, male, instagram, store, is_perf) " + 
                     $"VALUES ('{u.UserName}', '{u.UserId}', '{u.Male}', " + 
                     $"'{u.inst}', {u.Store}, 'false');"; 
            var cmd = new NpgsqlCommand(script, conn);
            cmd.ExecuteNonQuery();

                conn.Close();
                
            Console.WriteLine($"[DB][{DateTime.Now.TimeOfDay}] Додано нового користувача - {u.UserName};");
        }

        public static void ReadUsers()
        {
            int counter = 0;
            conn.Open();
            var cmd = new NpgsqlCommand("SELECT * FROM users;", conn);
            var reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                User user = new User();
                user.UserName = reader.GetString(1);
                user.UserId = Convert.ToInt32(reader.GetString(2));
                user.Male = reader.GetBoolean(3);
                user.inst = reader.GetString(4);
                user.Store = reader.GetInt32(5);
                user.IsPerf = reader.GetBoolean(6);
                counter++;
                AllUser.AllUsersList.Add(user);
                Settings.UsersList.Add(user.UserId);
            }
            
            
            
            conn.Close();
            Console.WriteLine(
                $"[DB][{DateTime.Now.TimeOfDay}] Загружено користувачів із бази ({counter})"
            );
        }

        public static void AddTaskDB(UserTask task)
        {
            conn.Open();
            var cmd = new NpgsqlCommand(
                $"INSERT INTO tasks (owner_id, description, is_compl, in_exec, id_perf) VALUES " + 
                $"('{task.Owner.UserId}', '{task.Description}', '{task.IsComplite}', 'false', '0');", 
                conn
                );
            cmd.ExecuteNonQuery();

            cmd = new NpgsqlCommand(
                $"SELECT * FROM tasks WHERE owner_id = '{task.Owner.UserId}' AND description = '{task.Description}';",
                conn
            );
            var reader = cmd.ExecuteReader();
            reader.Read();
            task.id = reader.GetInt32(0);
            
            conn.Close();
            Console.WriteLine($"[DB][{DateTime.Now.TimeOfDay}] Додано запис із завданнями!");
        }
        public static void ReadAllTasks()
        {
            conn.Open();
            int counter = 0;
            var cmd = new NpgsqlCommand("SELECT * FROM tasks;", conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                UserTask task = new UserTask();
                task.id = reader.GetInt32(0);
                task.Owner = AllUser.GetUser(Convert.ToInt32(reader.GetString(1)));
                if (task.Owner == null)
                {
                    continue;
                }
                task.Description = reader.GetString(2);
                task.IsComplite = reader.GetBoolean(3);
                task.in_exec = reader.GetBoolean(4);
                task.id_perf = Convert.ToInt32(reader.GetString(5));
                counter++;
                AllTasks.TaskList.Add(task);
            }
            conn.Close();
            
            
            
            Console.WriteLine($"[DB][{DateTime.Now.TimeOfDay}] Загружено {counter} записів із завданнями.");
        }
        
        
        
        

        public static void AddTaskToUser(UserTask task, User user)
        {
            conn.Open();
            var cmdUpUser = new NpgsqlCommand(
                $"UPDATE users SET is_perf = 'true' WHERE telegram_id = '{user.UserId}';",
                conn
            );
            var cmdUpTask = new NpgsqlCommand(
                $"UPDATE tasks SET in_exec = 'true', id_perf = '{user.UserId}' WHERE id = {task.id};",
                conn
            );

            cmdUpTask.ExecuteNonQuery();
            cmdUpUser.ExecuteNonQuery();
            conn.Close();

            Console.WriteLine(
                $"[DB][{DateTime.Now.TimeOfDay}] Добавлено завдання [{task.id}] для користувача {user.UserName}" +
                $"[{user.UserId}];");
        }

        public static void TaskIsComp(UserTask task)
        {
            conn.Open();

            var cmd = new NpgsqlCommand(
                $"UPDATE tasks SET is_compl = 'true' WHERE id = {task.id};",
                conn
            );
            cmd.ExecuteNonQuery();

            cmd = new NpgsqlCommand(
                $"UPDATE users SET store = store+20, is_perf = 'false' WHERE telegram_id = '{task.id_perf}';",
                conn
            );
            
            cmd.ExecuteNonQuery();
            task.IsComplite = true;
            conn.Close();
            Console.WriteLine(
                $"[DB][{DateTime.Now.TimeOfDay}] {AllUser.GetUser(task.id_perf).UserName} завершив завдання [{task.id}]!"
            );
        }

        public static void UpdUserName(User user)
        {
            conn.Open();
            string script = $"UPDATE users SET name = '{user.UserName}' WHERE telegram_id = '{user.UserId}';";
            var cmd = new NpgsqlCommand(script, conn);
            cmd.ExecuteNonQuery();
            
            conn.Close();
            Console.WriteLine($"[DB][{DateTime.Now.TimeOfDay}] Користувач змінив імя. Нове - {user.UserName}");
        }
        
        public static void UpdUserInst(User user)
        {
            conn.Open();
            string script = $"UPDATE users SET instagram = '{user.inst}' WHERE telegram_id = '{user.UserId}';";
            var cmd = new NpgsqlCommand(script, conn);
            cmd.ExecuteNonQuery();
            
            conn.Close();
            Console.WriteLine($"[DB][{DateTime.Now.TimeOfDay}] Користувач змінив instagram. Новеий - {user.inst}");
        }
        
        public static void UpdUserStore(User user)
        {
            conn.Open();
            string script = $"UPDATE users SET store = {user.Store} WHERE telegram_id = '{user.UserId}';";
            var cmd = new NpgsqlCommand(script, conn);
            cmd.ExecuteNonQuery();
            
            conn.Close();
            Console.WriteLine(
                $"[DB][{DateTime.Now.TimeOfDay}] Обновлено рахунок користувача {user.UserName} [{user.UserId}]"
            );
        }
        
    }
}