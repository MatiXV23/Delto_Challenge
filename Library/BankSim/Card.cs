using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Library.BankSim
{
    public class Card
    {
        public string AccountID;
        public long Number;
        public List<Transaction> Moves;

        public string GetMovesMsg()
        {
            string msg = $"Tarjeta: **** **** **** {(int)(Number % 10000)} \n"; 
            foreach (Transaction transaction in Moves)
            {
                if (transaction.PayeeAccountID == AccountID)
                {
                    msg += $"({transaction.Date}) Has recivido {transaction.Import} {transaction.Currency.ToString()} de {transaction.PayerAccountID}\n";
                }
                else 
                {
                    msg += $"({transaction.Date}) Has enviado {transaction.Import} {transaction.Currency.ToString()} a {transaction.PayeeAccountID}\n";
                }
            }

            return msg;
        }
    }
}
