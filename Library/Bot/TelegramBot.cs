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
    internal class TelBot
    {
        public TelegramBotClient botClient;

        private List<TelegramChat> chatList = new List<TelegramChat>();
        
        public async Task Iniciate()
        {
            using var cts = new CancellationTokenSource();

            string token = Environment.GetEnvironmentVariable("TELEGRAM_TOKEN");

            Console.WriteLine(token);

            botClient = new TelegramBotClient(token, cancellationToken: cts.Token);
            var me = await botClient.GetMe();

            botClient.OnMessage += OnMessage;
            botClient.OnError += OnError;
            botClient.OnUpdate += OnUpdate;

            Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
            Console.ReadLine();


        }

        async Task OnMessage(Message msg, UpdateType type)
        {
            var telChat = GetTelegramChat(msg.Chat);

            await telChat.OnMessage(msg, type);
        }

        async Task OnError(Exception exception, HandleErrorSource source)
        {
            Console.WriteLine(exception);
        }

        async Task OnUpdate(Update update)
        {
            if (update is { CallbackQuery: { } query })
            {
                var telChat = GetTelegramChat(query.Message!.Chat);
                await telChat.OnUpdate(query);
            }
        }



        public TelegramChat GetTelegramChat(Chat chat)
        {
            foreach (var telChat in chatList)
            {
                if (telChat.Id == chat.Id)
                {
                    return telChat;
                }
            }

            var newChat = new TelegramChat(chat, botClient);
            chatList.Add(newChat);

            return newChat;
        }
    }
}
