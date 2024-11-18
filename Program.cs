
using Telegram.Bot;
using TelegramBot.Library.Bot;

namespace Program
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var bot = new Bot();
            await bot.Iniciate();
        }
    }
}
