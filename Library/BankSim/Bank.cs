using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Library.BankSim
{
    /// <summary>
    /// Clase que representa el Banco.
    /// </summary>
    public class Bank
    {
        /// <summary>
        /// Lista de usuarios registrados en el banco.
        /// Realiza una llamada a la base de datos del banco para obtenerlos.
        /// </summary>
        public List<BankUser> users = JsonAdapter.GetUsersFromDB();

        /// <summary>
        /// Obtiene un usuario del banco a partir de credenciales.
        /// </summary>
        /// <param name="username">El nombre de usuario.</param>
        /// <param name="password">La contraseña.</param>
        /// <returns>
        /// El usuario con las respectivas credenciales. En su defecto null.
        /// </returns>
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
