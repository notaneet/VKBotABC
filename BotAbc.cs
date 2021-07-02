using System;
using System.Threading.Tasks;
using VKBotABC.Events;
using VKBotABC.Objects;
using VKBotABC.Polling;
using VKBotABC.Utils;

// Внимание!! Куча говнокода.
// Все сделано в одном классе чтобы без гомоебли можно использовать в пасрале.
// Ладно. Кого я обманываю. Я просто говнокодер.


namespace VKBotABC
{
    
    public class BotAbc
    {
        public const string ApiEndpoint = "https://api.vk.com/method/";
        public const string ApiVersion = "5.126";
        public const long ChatOffset = 2000000000;
        
        private readonly string _token;
        
        public readonly LongPoll Longpoll;

        
        public BotAbc(string token, long groupId)
        {
            _token = token;

            Longpoll = new LongPoll(token, groupId);
        }

        public void Run()
        {
            Console.Out.WriteLine("[BotAbc] Запущен");
            Longpoll.Polling().Wait();
        }

        public static Message.KeyboardObject CreateKeyboard()
        {
            return new Message.KeyboardObject();
        }

        public async Task Reply(MessageEvent ev, string text)
        {
            await Reply(ev, text, null);
        }
        
        public async Task Reply(MessageEvent ev, string text, Message.KeyboardObject keyboard)
        {
            var msg = new Message
            {
                PeerId = ev.Message.PeerId,
                MessageText = text
            };
            
            if (ev.Message.PeerId > ChatOffset) 
                msg.ReplyTo = ev.Message.Id;

            if (keyboard != null)
                msg.Keyboard = keyboard;
            
            
            await SendMessage(msg);
        }
        
        public async Task SendMessage(Message message)
        {
            var parameters = message.ToParams();
            parameters.Add("access_token", _token);

            await HttpUtils.GetVk("messages.send", parameters);
        }
    }
}
