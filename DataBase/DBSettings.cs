using Npgsql;

namespace TelegramBot.DataBase
{
    public static class DBSettings
    {
        public const string Host = "ec2-54-155-5.eu-west-1.compute.amazonaws.com";
        public const int Port = 5432;
        public const string Username = "exoobzialqkoz";
        public const string Password = "820cf7299fbd1b4bd7b7fb66cbf5d13cb2aceeb048f2c5c5198f16620a12ea72";
        public const string Datebase = "d6ethqs75shh4";
        public const bool Pooling = true;
        public const string SSLMode = "Require";
        public const bool TrueServerCertificate = true;

        public static readonly string ConnectionString = $"host={Host};" +
                                                         $"username={Username};" +
                                                         $"password={Password};" +
                                                         $"database={Datebase};" +
                                                         $"port={Port};" +
                                                         $"Sslmode=Require;" +
                                                         $"Trust Server Certificate=true";
    
        public static NpgsqlConnection CreateConn()
        {
            var conn = new NpgsqlConnection(DBSettings.ConnectionString);

            return conn;
        }
    }
    
    
}
