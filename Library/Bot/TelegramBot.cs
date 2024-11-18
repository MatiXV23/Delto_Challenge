using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot.Library.Bot
{
    internal class Bot
    {
        public ITelegramBotClient botClient;

        public async Task Iniciate()
        {
            var config = ConfigReader.Read("../../../Library/Bot/config.json");
            Console.WriteLine(config.token);
            botClient = new TelegramBotClient(config.token);
            var bot = botClient.GetMe().Result;

            Console.WriteLine($"buenas mi id:{bot.Id} y mi nombre: {bot.FirstName}");
        }
    }
}
