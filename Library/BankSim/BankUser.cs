using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Library.BankSim
{
    public class BankUser
    {
        public string Name;
        public string Username;
        public string Password;
        public List<Account> Accounts;


        public string GetAccountListMsg()
        {
            string msg = "Cuentas Disponibles: \n\n";
            int cont = 0;
            foreach (Account account in Accounts)
            {
                msg += $"-{account.AccountID}: ${account.Balance} ({account.Currency})\n";
            }
            msg += "\nSelecciona la cuenta que deseas ver los movimientos";
            return msg;
        }

        public string GetAllMovesMsg()
        {
            string msg = $"Cuentas a nombre de {Name}\n\n";

            foreach (var account in Accounts)
            {
                msg += account.GetAllAccountMovesMsg() + "\n\n";
            }

            return msg;
        }

        public bool IsAnAccountId(string msg, out Account account)
        {
            foreach (Account acc in Accounts)
            {
                if (acc.AccountID == msg)
                {
                    account = acc;
                    return true;
                }
            }
            account = null;
            return false;
        }
    }
}
