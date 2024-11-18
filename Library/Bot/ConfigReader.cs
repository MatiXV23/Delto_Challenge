using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace TelegramBot.Library.Bot
{
    internal class ConfigReader
    {
        public static Config Read(string configURL)
        {
            string jsonConfig = File.ReadAllText(configURL);
            Config config = JsonConvert.DeserializeObject<Config>(jsonConfig);

            return config;
        }
    }

    internal class Config
    {
        public string token;
    }
}
