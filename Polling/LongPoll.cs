using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VKBotABC.Events;
using VKBotABC.Utils;

namespace VKBotABC.Polling
{
    public class LongPoll
    {

        public delegate void VkHandler<in T>(T ev);

        public event VkHandler<NewMessageEvent> OnNewMessage;

        private readonly string _token;
        private readonly long _groupId;


        public LongPoll(string token, long groupId)
        {
            _token = token;
            _groupId = groupId;
        }

        private bool _stopped = false;

        private string _key;
        private string _server;
        private long _ts;

        internal async Task Polling()
        {
            await Connect();
            while (!_stopped)
            {
                var response = await HttpUtils.Get(_server, new Assoc<string, object>
                {
                    {"act", "a_check"},
                    {"key", _key},
                    {"ts", _ts},
                    {"wait", 25},
                });
                
                

                var updates = (JArray) response["updates"];
                var failure = response["failed"];

                if (updates == null && failure != null)
                {
                    await Connect();
                    continue;
                }

                _ts = long.Parse(response["ts"].ToString());


                if (updates == null) continue;
                foreach (var update in updates)
                {
                    HandleEvent(update);
                }
            }
        }

        private void HandleEvent(JToken update)
        {
            var obj = update["object"];
            var eventName = update["type"].ToString();

            if (eventName == "message_new")
            {
                CallEvent(OnNewMessage, obj);
            }

        }

        private static void CallEvent<T>(VkHandler<T> handler, JToken eventData)
        {
            handler.Invoke(eventData.ToObject<T>());
        }

        private async Task Connect()
        {
            var response = await HttpUtils.GetVk("groups.getLongPollServer", new Assoc<string, object>
            {
                {"access_token", _token},
                {"group_id", _groupId}
            });


            _server = response["response"]["server"].ToString();
            _key = response["response"]["key"].ToString();
            _ts = long.Parse(response["response"]["ts"].ToString());
        }
    }
}