using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Library.BankSim;
using TelegramBot.Library.ChatGPT;

namespace TelegramBot.Library.Bot
{
    /// <summary>
    /// Clase que representa un chat individual en Telegram.
    /// </summary>
    public class TelegramChat
    {
        /// <summary>
        /// ID único del chat de Telegram.
        /// </summary>
        public long Id;

        /// <summary>
        /// Objeto <see cref="Chat"/> de Telegram que representa el chat actual.
        /// </summary>
        public Chat chat;

        /// <summary>
        /// Comandos bancarios disponibles para interactuar con el sistema.
        /// </summary>
        public Bank commands = new Bank();

        /// <summary>
        /// Cliente del bot de Telegram para enviar mensajes y realizar otras operaciones.
        /// </summary>
        private TelegramBotClient Bot;

        /// <summary>
        /// Usuario actualmente logueado en el chat. Null si no hay un usuario logueado.
        /// </summary>
        private BankUser LoggedUser = null!;

        /// <summary>
        /// Indica si el usuario está intentando iniciar sesión.
        /// </summary>
        public bool TryingToLogIn = false;

        /// <summary>
        /// Contraseña ingresada por el usuario durante el proceso de inicio de sesión.
        /// </summary>
        private string password = null!;

        /// <summary>
        /// Nombre de usuario ingresado por el usuario durante el proceso de inicio de sesión.
        /// </summary>
        private string username = null!;

        /// <summary>
        /// Indica si el usuario está interactuando con la IA.
        /// </summary>
        private bool TalkingToAI = false;

        /// <summary>
        /// Instancia de ChatGPT para proporcionar respuestas de inteligencia artificial.
        /// </summary>
        private ChatGPT4 AI = new ChatGPT4();

        /// <summary>
        /// Cuenta bancaria actualmente seleccionada por el usuario.
        /// </summary>
        private Account ActualAccount;

        /// <summary>
        /// Tarjeta actualmente seleccionada por el usuario.
        /// </summary>
        private Card ActualCard;

        /// <summary>
        /// Último botón seleccionado en el menú.
        /// </summary>
        private string PrevBoton;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="TelegramChat"/>.
        /// </summary>
        /// <param name="chat">El chat de Telegram asociado.</param>
        /// <param name="botClient">El cliente del bot de Telegram.</param>
        public TelegramChat(Chat chat, TelegramBotClient botClient) 
        {
            Id = chat.Id;
            this.chat = chat;
            Bot = botClient;
        }

        /// <summary>
        /// Intenta iniciar sesión con las credenciales ingresadas.
        /// </summary>
        /// <returns>
        /// Un mensaje que indica si el inicio de sesión fue exitoso o fallido.
        /// </returns>
        public string TryToLogIn()
        {
            BankUser user = commands.GetUserByCredentials(username, password);
            LoggedUser = user;
            if (user == null)
            {
                username = null;
                password = null;
                TryingToLogIn = false;
                return "Credenciales incorrectas";
            }

            
            return $"{user.Name} te has logeado correctamente";
        }

        /// <summary>
        /// Devuelve el mensaje inicial de bienvenida.
        /// </summary>
        /// <returns>
        /// Mensaje de bienvenida.
        /// </returns>
        public string InitialMsg()
        {
            return "Bienvendo al chat de telegram del Banco UDSA\n\n" +
                "Aqui podras ver el balance de tus cuentas y todos tus movimientos.\n\n" +
                "Para continuar debes iniciar sesion con tus credenciales";
        }

        /// <summary>
        /// Maneja los mensajes entrantes del usuario en el chat.
        /// </summary>
        /// <param name="msg">El mensaje recibido.</param>
        /// <param name="type">El tipo de actualización correspondiente.</param>
        public async Task OnMessage(Message msg, UpdateType type)
        {
            if (TryingToLogIn)
            {
                if (username == null)
                {
                    username = msg.Text;
                    await Bot.SendMessage(chat, $"Ingresa tu contraseña");
                }
                else if (password == null)
                {
                    password = msg.Text;
                    string msgLog = TryToLogIn();
                    
                    if (LoggedUser == null)
                    {
                        await Bot.SendMessage(msg.Chat, msgLog,
                            replyMarkup: new InlineKeyboardMarkup().AddButtons("Vuelve a intentar"));
                    }
                    else
                    {
                        TryingToLogIn = false;
                        await Bot.SendMessage(msg.Chat, msgLog  + "\n\nSi deseas puedes ver tus movimientos bancarios a traves de este menu.",
                            replyMarkup: new InlineKeyboardMarkup().AddButtons("Inicio"));
                    }
                }
                return;
            }

            if (TalkingToAI) {
                await Bot.SendMessage(msg.Chat,  await AI.GetAIResponse(msg.Text), replyMarkup: new InlineKeyboardMarkup().AddButtons("Finalizar ContactoAI"));  
            }
            else
            {
                if (LoggedUser == null)
                {
                    await Bot.SendMessage(msg.Chat, InitialMsg(),
                    replyMarkup: new InlineKeyboardMarkup().AddButtons("Iniciar Sesion"));
                }
                else {
                    await Bot.SendMessage(msg.Chat, "Si deseas puedes ver tus movimientos bancarios a traves de este menu.",
                    replyMarkup: new InlineKeyboardMarkup().AddButtons("Inicio"));
                }
            }
        }

        /// <summary>
        /// Maneja las actualizaciones recibidas del bot de Telegram.
        /// </summary>
        /// <param name="query">La consulta de callback recibida.</param>
        public async Task OnUpdate(CallbackQuery query)
        {
            string boton = query.Data;
            if (boton == "Volver Atras")
            {
                boton = PrevBoton;
            }

            switch (boton)
            {
                case "Iniciar Sesion":
                case "Vuelve a intentar":
                    TryingToLogIn = true;
                    await Bot.SendMessage(query.Message!.Chat, $"Ingresa tu nombre de usuario");
                    break;

                case "Inicio":
                    await Bot.SendMessage(chat, $"{LoggedUser.Name} puedes consultar todos tus movimientos o adentrarte en tus cuentas y revisar mas a detalle cada una.",
                            replyMarkup: new InlineKeyboardMarkup().AddButtons("Ver Movimientos", "Ver cuentas", "Contacto AI"));
                    break;

                case "Ver Movimientos":
                    await Bot.SendMessage(chat, LoggedUser.GetAllMovesMsg(),
                            replyMarkup: new InlineKeyboardMarkup().AddButtons("Inicio"));
                    break;

                case "Ver cuentas":
                    PrevBoton = "Inicio";

                    var markup = new InlineKeyboardMarkup();

                    foreach (Account account in LoggedUser.Accounts)
                    {
                        markup.AddButton($"{account.AccountID}");
                    }

                    markup.AddButton("Volver Atras");
                    await Bot.SendMessage(chat, LoggedUser.GetAccountListMsg(),
                            replyMarkup: markup);

                    break;

                case "Movimientos":
                    PrevBoton = "Ver cuentas";
                    await Bot.SendMessage(chat, ActualAccount.GetAllAccountMovesMsg(),
                            replyMarkup: new InlineKeyboardMarkup().AddButtons
                            ("Inicio", "Volver Atras"));
                    break;

                case "Ver Tarjetas":
                    PrevBoton = "Ver cuentas";
                    await Bot.SendMessage(chat, LoggedUser.GetAccountListMsg(),
                            replyMarkup: new InlineKeyboardMarkup().AddButtons
                            ("Ver Movimientos de cuenta", "Ver Tarjetas", "Volver Atras"));
                    break;

                case "Contacto AI":
                    TalkingToAI = true;
                    await Bot.SendMessage(chat, "Ya te hemos puesto en contacto con nuestra IA, realiza tus preguntas");
                    break;

                case "Finalizar ContactoAI":
                    TalkingToAI = false;
                    await Bot.SendMessage(chat, "Conversacion con USDA AI terminada.\n\nSi deseas puedes ver tus movimientos bancarios a traves de este menu.",
                     replyMarkup: new InlineKeyboardMarkup().AddButtons("Inicio"));
                    break;

                default:

                    if (LoggedUser == null) break;
                    if (LoggedUser.IsAnAccountId(boton, out Account acc))
                    {
                        PrevBoton = "Ver cuentas";
                        ActualAccount = acc;
                        var markup2 = new InlineKeyboardMarkup();

                        foreach (Card card in ActualAccount.Cards)
                        {
                            markup2.AddButton($"...{(int)(card.Number % 10000)}");
                        }

                        markup2.AddButtons("Movimientos", "Volver Atras");
                        await Bot.SendMessage(chat, ActualAccount.GetCardsListMsg(),
                                replyMarkup: markup2);
                    }

                    else if (ActualAccount.IsAnCardNumber(boton, out Card card))
                    {
                        PrevBoton = $"{ActualAccount.AccountID}";
                        ActualCard = card;
                        await Bot.SendMessage(chat, ActualCard.GetMovesMsg(),
                                replyMarkup: new InlineKeyboardMarkup().AddButtons("Inicio", "Volver Atras"));
                    }

                    break;
            }
        }
    }
}
