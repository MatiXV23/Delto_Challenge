using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Library.BankSim
{
    public class Transaction
    {
        public string PayerAccountID;
        public string PayeeAccountID;
        public float Import;
        public string Date;
        public CurrencyEnum Currency;

        
    }
}
