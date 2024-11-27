using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Library.BankSim;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Telegram.Bot.Polling;

namespace TelegramBot.Library.Bot
{
    /// <summary>
    /// Clase que gestiona la interacción con el bot de Telegram utilizando la biblioteca Telegram.Bot.
    /// </summary>
    internal class TelBot
    {
        /// <summary>
        /// Cliente del bot de Telegram para manejar las operaciones principales.
        /// </summary>
        public TelegramBotClient botClient;

        /// <summary>
        /// Lista de chats activos que están siendo manejados por el bot.
        /// </summary>
        private List<TelegramChat> chatList = new List<TelegramChat>();

        /// <summary>
        /// Inicia el bot de Telegram, configurando eventos y comenzando la interacción.
        /// </summary>
        /// <returns>Una tarea asincrónica que se completa cuando el bot se detiene.</returns>
        public async Task Iniciate()
        {
            using var cts = new CancellationTokenSource();

            // Obtiene el token del bot desde las variables de entorno.
            string token = Environment.GetEnvironmentVariable("TELEGRAM_TOKEN");
            Console.WriteLine(token);

            // Inicializa el cliente del bot de Telegram.
            botClient = new TelegramBotClient(token, cancellationToken: cts.Token);

            // Obtiene información sobre el bot (nombre, usuario, etc.).
            var me = await botClient.GetMe();

            // Registra los manejadores de eventos.
            botClient.OnMessage += OnMessage;
            botClient.OnError += OnError;
            botClient.OnUpdate += OnUpdate;

            // Notifica que el bot está en ejecución y espera la entrada para detenerse.
            Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
            Console.ReadLine();
        }

        /// <summary>
        /// Manejador del evento que se activa cuando el bot recibe un mensaje.
        /// </summary>
        /// <param name="msg">El mensaje recibido.</param>
        /// <param name="type">El tipo de actualización correspondiente.</param>
        async Task OnMessage(Message msg, UpdateType type)
        {
            // Obtiene o crea una instancia de TelegramChat asociada al chat del mensaje.
            var telChat = GetTelegramChat(msg.Chat);

            // Maneja el mensaje dentro del contexto del chat correspondiente.
            await telChat.OnMessage(msg, type);
        }

        /// <summary>
        /// Manejador del evento que se activa cuando ocurre un error en el bot.
        /// </summary>
        /// <param name="exception">La excepción que ocurrió.</param>
        /// <param name="source">La fuente del error.</param>
        async Task OnError(Exception exception, HandleErrorSource source)
        {
            // Registra el error en la consola.
            Console.WriteLine(exception);
        }

        /// <summary>
        /// Manejador del evento que se activa cuando el bot recibe una actualización. Normalmente los botones
        /// </summary>
        /// <param name="update">La actualización recibida.</param>
        async Task OnUpdate(Update update)
        {
            // Maneja las consultas de callback si están presentes en la actualización.
            if (update is { CallbackQuery: { } query })
            {
                // Obtiene o crea una instancia de TelegramChat asociada al chat de la consulta.
                var telChat = GetTelegramChat(query.Message!.Chat);

                // Maneja la actualización dentro del contexto del chat correspondiente.
                await telChat.OnUpdate(query);
            }
        }

        /// <summary>
        /// Obtiene una instancia de <see cref="TelegramChat"/> asociada al chat dado.
        /// Si no existe, se crea una nueva instancia y se agrega a la lista.
        /// </summary>
        /// <param name="chat">El chat de Telegram que se va a manejar.</param>
        /// <returns>Una instancia de <see cref="TelegramChat"/> asociada al chat.</returns>
        public TelegramChat GetTelegramChat(Chat chat)
        {
            // Busca si ya existe un chat registrado con el mismo ID.
            foreach (var telChat in chatList)
            {
                if (telChat.Id == chat.Id)
                {
                    return telChat;
                }
            }

            // Si no existe, crea un nuevo TelegramChat y lo agrega a la lista.
            var newChat = new TelegramChat(chat, botClient);
            chatList.Add(newChat);

            return newChat;
        }
    }
}
