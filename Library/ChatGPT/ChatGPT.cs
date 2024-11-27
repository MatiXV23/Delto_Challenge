using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OpenAI.Chat;
using OpenAI.Models;
using Telegram.Bot.Types;
using System.Reflection;
using OpenAI.Assistants;
using OpenAI;
using OpenAI.Files;

namespace TelegramBot.Library.ChatGPT
{
    public class ChatGPT4
    {
        private string Promt = "Eres un asistente virtual del Banco UDSA. Proporcionas información sobre sucursales, números de teléfono y servicios del banco. Delegas preguntas técnicas o relacionadas con precios a profesionales de la sucursal más cercana. Mantienes un tono profesional y amigable con mensajes cortos y simples. Puedes inventar una direccion y numero de telefono para las sucursales, siempre y cuando sigan el prefijo de cada departamento. Esta es la conversasion entre el usuario y tu hasta ahora, debes contestar el ultimo mensaje: ";

        private ChatMessage[] ChatHistory = new ChatMessage[9];
        public ChatGPT4()
        {
            FillFirstHistory();
        }

        public async Task<string> GetAIResponse(string msg)
        {
            CycleHistory();
            ChatHistory[ChatHistory.Length - 1] = new UserChatMessage(msg);
           
            ChatClient client = new(model: "gpt-4o-mini", apiKey: Environment.GetEnvironmentVariable("CHATGPT_TOKEN"));  
            ChatCompletion completion = await client.CompleteChatAsync(ChatHistory);


            CycleHistory();
            ChatHistory[ChatHistory.Length - 1] = new AssistantChatMessage(msg);


            return completion.Content[0].Text;
        }

        private void CycleHistory()
        {
            ChatHistory[0] = new SystemChatMessage(Promt);
            for (int i = ChatHistory.Length - 1; i > 2; i--)
            {
                ChatHistory[i - 1] = ChatHistory[i];     
            }
        }

        private void FillFirstHistory()
        {
            for (int i = 0; i < ChatHistory.Length; i++)           
            {
                ChatHistory[i] = new UserChatMessage();
            }
        }
    }   
}
