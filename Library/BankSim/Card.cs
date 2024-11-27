using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Library.BankSim
{
    /// <summary>
    /// Representa una tarjeta asociada a una cuenta bancaria.
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Identificador de la cuenta bancaria asociada a esta tarjeta.
        /// </summary>
        public string AccountID;

        /// <summary>
        /// Número de la tarjeta.
        /// </summary>
        public long Number;

        /// <summary>
        /// Lista de transacciones asociadas a la tarjeta.
        /// </summary>
        public List<Transaction> Moves;

        /// <summary>
        /// Genera un mensaje con los movimientos de la tarjeta.
        /// </summary>
        /// <returns>Una cadena con los detalles de todas las transacciones de la tarjeta.</returns>
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
