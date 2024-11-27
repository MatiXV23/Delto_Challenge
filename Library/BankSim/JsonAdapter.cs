using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace TelegramBot.Library.BankSim
{
    public static class JsonAdapter
    {
        private static string JsonPath = "..\\..\\..\\Library\\BankSim\\DataBase.json";

        public static void AddUserToDB(BankUser user)
        {
            string userJson = JsonConvert.SerializeObject(user, Formatting.Indented);
            File.AppendAllText(JsonPath, userJson);
        }

        public static void RewriteUsersOfDB(List<BankUser> users) 
        {
            string userJson = JsonConvert.SerializeObject(users.ToArray(), Formatting.Indented);
            File.WriteAllText(JsonPath, userJson);
        }


        public static List<BankUser> GetUsersFromDB()
        { 
            string usersFromJson = File.ReadAllText(JsonPath);

            var users = JsonConvert.DeserializeObject<List<BankUser>>(usersFromJson);

            return users!;
        }
    }
}
