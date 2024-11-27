using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Library.BankSim
{
    /// <summary>
    /// Clase que representa un usuario bancario.
    /// </summary>
    public class BankUser
    {
        /// <summary>
        /// Nombre del propietario.
        /// </summary>
        public string Name;

        /// <summary>
        /// Nombre de usuario del propietario.
        /// </summary>
        public string Username;

        /// <summary>
        /// Contraseña del propietario.
        /// </summary>
        public string Password;

        /// <summary>
        /// Lista de cuentas pertenecientes al propietario.
        /// </summary>
        public List<Account> Accounts;

        /// <summary>
        /// Genera un mensaje con informacion minima de cada una de sus cuentas.
        /// </summary>
        /// <returns>Una cadena con los detalles de minimos de sus cuentas.</returns>
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

        /// <summary>
        /// Genera un mensaje con todos los movimientos del usuario.
        /// </summary>
        /// <returns>Una cadena con los detalles de todas las transacciones del usuario.</returns>
        public string GetAllMovesMsg()
        {
            string msg = $"Cuentas a nombre de {Name}\n\n";

            foreach (var account in Accounts)
            {
                msg += account.GetAllAccountMovesMsg() + "\n\n";
            }

            return msg;
        }

        /// <summary>
        /// Verifica si el mensaje recive es un Identificador de alguna de las cuentas del usuario
        /// </summary>
        /// <param name="msg">El mensaje recibido.</param>
        /// <param name="account">Parametro de salida con la instancia de la cuenta obtenida.</param>
        /// <returns>
        /// Un boolean que indica si el mensaje es un ID de alguna de las cuentas del usuario o no
        /// </returns>
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
