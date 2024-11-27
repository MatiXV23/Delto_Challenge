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
    public class TelegramChat
    {
        public long Id;
        public Chat chat;
        public BankCommands commands = new BankCommands();
        private TelegramBotClient Bot;


        private BankUser LoggedUser = null;
        public bool TryingToLogIn = false;
        private string password = null;
        private string username = null;

        private bool TalkingToAI = false;
        private ChatGPT4 AI = new ChatGPT4();

        private Account ActualAccount;
        private Card ActualCard;
        private string PrevBoton;
        public TelegramChat(Chat chat, TelegramBotClient botClient) 
        {
            Id = chat.Id;
            this.chat = chat;
            Bot = botClient;
        }

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

        public string InitialMsg()
        {
            return "Bienvendo al chat de telegram del Banco UDSA\n\n" +
                "Aqui podras ver el balance de tus cuentas y todos tus movimientos.\n\n" +
                "Para continuar debes iniciar sesion con tus credenciales";
        }

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

                    else if (ActualAccount.IsAnCardButton(boton, out Card card))
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
