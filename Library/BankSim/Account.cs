using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Requests;

namespace TelegramBot.Library.BankSim
{
    /// <summary>
    /// Clase que representa una cuenta bancaria.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Numero que representa el balance de la cuenta.
        /// </summary>
        public float Balance;

        /// <summary>
        /// Identificador de la cuenta.
        /// </summary>
        public string AccountID;

        /// <summary>
        /// Enum que representa los tipo de moneda de la cuenta.
        /// </summary>
        public ECurrency Currency;

        /// <summary>
        /// Lista de tarjetas asociadas a la cuenta.
        /// </summary>
        public List<Card> Cards;

        /// <summary>
        /// Lista de transacciones asociadas a la tarjeta.
        /// </summary>
        public List<Transaction> Moves;

        /// <summary>
        /// Genera un mensaje con informacion minima de cada una de sus tarjetas.
        /// </summary>
        /// <returns>
        /// Una cadena con los detalles de minimos de sus tarjetas.
        /// </returns>
        private string GetTransferesMSG()
        {
            string msg = "";
            foreach (Transaction transfer in Moves)
            {
                if (transfer.PayeeAccountID == AccountID)
                {
                    msg += $"({transfer.Date}){transfer.PayerAccountID} te ha transferido {transfer.Import} {transfer.Currency.ToString()}\n";
                }
                else
                {
                    msg += $"({transfer.Date})Has transferido {transfer.Import} {transfer.Currency.ToString()} a {transfer.PayeeAccountID}\n";
                }
            }

            return msg;
        }

        /// <summary>
        /// Genera un mensaje con informacion minima de cada una de sus tarjetas.
        /// </summary>
        /// <returns>
        /// Una cadena con los detalles de minimos de sus tarjetas.
        /// </returns>
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


        /// <summary>
        /// Genera un mensaje con todos los movimientos de la cuenta.
        /// </summary>
        /// <returns>
        /// Una cadena con los detalles de todas las transacciones de la cuenta.
        /// </returns>
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
                msg += GetTransferesMSG() + "\n";
            }
            

            return msg;
        }

        /// <summary>
        /// Verifica si el mensaje recive es un Identificador de alguna de las tarjetas del usuario
        /// </summary>
        /// <param name="msg">El mensaje recibido.</param>
        /// <param name="card">Parametro de salida con la instancia de la tarjeta obtenida.</param>
        /// <returns>
        /// Un boolean que indica si el mensaje es un ID de alguna de las tarjetas del usuario o no
        /// </returns>
        public bool IsAnCardNumber(string msg, out Card card)
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
