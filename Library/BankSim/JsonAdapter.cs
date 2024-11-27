using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace TelegramBot.Library.BankSim
{
    ////////////////////////////////////////////////////////////////////////
    /// CLASE CREADA PARA LA SIMULACION DE LA BASE DE DATOS DE USDA BANK ///
    ////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Clase estática para manejar operaciones de la base de datos JSON del sistema bancario. 
    /// </summary>
    public static class JsonAdapter
    {
        /// <summary>
        /// Ruta del archivo JSON que contiene la base de datos.
        /// </summary>
        private static string JsonPath = "..\\..\\..\\Library\\BankSim\\DataBase.json";

        /// <summary>
        /// Obtiene todos los usuarios almacenados en la base de datos JSON.
        /// </summary>
        /// <returns>Una lista de usuarios (<see cref="BankUser"/>) obtenidos del archivo JSON.</returns>
        public static List<BankUser> GetUsersFromDB()
        {
            string usersFromJson = File.ReadAllText(JsonPath);

            var users = JsonConvert.DeserializeObject<List<BankUser>>(usersFromJson);

            return users!;
        }

        /// <summary>
        /// Agrega un nuevo usuario a la base de datos JSON.
        /// </summary>
        /// <param name="user">El usuario que se desea agregar.</param>
        public static void AddUserToDB(BankUser user)
        {
            string userJson = JsonConvert.SerializeObject(user, Formatting.Indented);
            File.AppendAllText(JsonPath, userJson);
        }

        /// <summary>
        /// Reescribe todos los usuarios en la base de datos JSON con la lista proporcionada.
        /// </summary>
        /// <param name="users">Lista de usuarios que reemplazará el contenido actual del archivo JSON.</param>
        public static void RewriteUsersOfDB(List<BankUser> users) 
        {
            string userJson = JsonConvert.SerializeObject(users.ToArray(), Formatting.Indented);
            File.WriteAllText(JsonPath, userJson);
        }

        
    }
}
