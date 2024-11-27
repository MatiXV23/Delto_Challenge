using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Requests;

namespace TelegramBot.Library.BankSim
{
    public class Account
    {
        public float Balance;
        public string AccountID;
        public CurrencyEnum Currency;
        public List<Card> Cards;

        public List<Transaction> Moves;

        public string GetMovesMsg()
        {
            string msg = "";
            foreach (Transaction transaction in Moves)
            {
                if (transaction.PayeeAccountID == AccountID)
                {
                    msg += $"({transaction.Date}){transaction.PayerAccountID} te ha transferido {transaction.Import} {transaction.Currency.ToString()}\n";
                }
                else
                {
                    msg += $"({transaction.Date})Has transferido {transaction.Import} {transaction.Currency.ToString()} a {transaction.PayeeAccountID}\n";
                }
            }

            return msg;
        }

        public string GetCardsListMsg()
        {
            string msg = $"Tarjetas de la cuenta {AccountID}\n\n";
            int cont = 0;
            foreach (Card card in Cards)
            {
                msg += $"{cont++})Tarjeta: **** **** **** {(int)(card.Number % 10000)}\n";
            }
            msg += "\nSelecciona la tarjeta que deseas ver los movimientos";
            return msg;
        }

        public string GetCardMsgByIndex(int index)
        {
            if (index < 1 || index > Cards.Count) 
            {
                return "Tarjeta no encontrada, intenta con otro indice\n";
            }
            return $"Tarjeta: **** **** **** {(int)(Cards[index-1].Number % 10000)} \n{Cards[index - 1].GetMovesMsg()}\n";
        }

        public string GetAllAccountMovesMsg()
        {
            string msg = $"Account: {AccountID}    Balance: ${Balance} ({Currency.ToString()})\n\n";
            foreach (Card card in Cards)
            {
                msg += $"{card.GetMovesMsg()}\n";
            }

            if (Moves != null && Moves.Count > 0)
            {
                msg += "Transferencias: \n";
                msg += GetMovesMsg() + "\n";
            }
            

            return msg;
        }

        public bool IsAnCardButton(string msg, out Card card)
        {
            foreach (Card c in Cards)
            {
                if ($"...{(int)(c.Number % 10000)}" == msg)
                {
                    card = c;
                    return true;
                }
            }
            card = null;
            return false;
        }
    }
}
