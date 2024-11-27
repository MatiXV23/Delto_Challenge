using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Library.BankSim
{
    public class BankCommands
    {

        public List<BankUser> users = JsonAdapter.GetUsersFromDB();

        public BankUser GetUserByCredentials(string username, string password)
        {
            foreach (var user in users)
            {
               
                if (user.Username == username && user.Password == password)
                {
                    return user;
                }
            }
            return null!;
        }


    }
}
