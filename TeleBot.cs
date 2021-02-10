using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Args;
using System.Threading;
using System.Diagnostics;

namespace Hhru
{
    public class TeleBot
    {
        static string token = @""; // Your bot token here
        static string fileUrl = $@"https://api.telegram.org/file/bot{token}/";
        static long id = ; // Your chart id
        static TelegramBotClient botClient;

        static TeleBot()
        {
            botClient = new TelegramBotClient(token);
            botClient.OnMessage += MessageListner;
        }

        private static void MessageListner(object sender, MessageEventArgs e)
        {
            //id = e.Message.Chat.Id;
            //Console.WriteLine(e.Message.Chat.Id);
        }      
        
        public static void Start(int minutes)
        {
            botClient.StartReceiving();
            

            while (true)
            {                
                ChatId chatId = new ChatId(id);
                string text = new Operating("Стажер C%23").Parse();                    
                botClient.SendTextMessageAsync(chatId, text);
                Debug.WriteLine($"Message recived {text}");                            
                Debug.WriteLine(" Delay start");
                Thread.Sleep(60000 * minutes);
                Debug.WriteLine("Delay end");
            }
        }
    }
}
