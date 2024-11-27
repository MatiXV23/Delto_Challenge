using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Library.BankSim
{
    /// <summary>
    /// Representa una transacción bancaria entre dos cuentas.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Identificador de la cuenta del pagador.
        /// </summary>
        public string PayerAccountID;

        /// <summary>
        /// Identificador de la cuenta del receptor (beneficiario).
        /// </summary>
        public string PayeeAccountID;

        /// <summary>
        /// Monto de la transacción.
        /// </summary>
        public float Import;

        /// <summary>
        /// Fecha en la que se realizó la transacción.
        /// </summary>
        public string Date;

        /// <summary>
        /// Moneda utilizada en la transacción.
        /// </summary>
        public ECurrency Currency;
    }
}
