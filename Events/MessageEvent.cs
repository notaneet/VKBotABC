using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VKBotABC.Utils;

namespace VKBotABC.Events
{
    public class MessageEvent : IBaseEvent
    {
        [JsonProperty("message")] public MessageVKObject Message { get; set; }
        
        public class MessageVKObject
        {
            [JsonProperty("id")] public int Id { get; set; }

            [JsonProperty("date")] public long Date { get; set; }

            [JsonProperty("peer_id")] public long PeerId { get; set; }

            [JsonProperty("from_id")] public long FromId { get; set; }

            [JsonProperty("text")] public string Text { get; set; }

            [JsonProperty("random_id")] public long RandomId { get; set; }

            //ref
            //ref_source
            //аттачи

            [JsonProperty("important")] public bool? Important { get; set; }

            //geo

            [JsonProperty("payload")] public string Payload { get; set; }

            private Assoc<string, object> _payload;

            //keyboard

            [JsonProperty("fwd_messages")] public MessageVKObject[] ForwardMessages { get; set; }

            [JsonProperty("reply_message")] public MessageVKObject ReplyMessage { get; set; }

            [JsonProperty("action")] public ActionObject Action { get; set; }

            [JsonProperty("admin_author_id")] public int? AdminAuthorId { get; set; }

            [JsonProperty("conversation_message_id")]
            public int? ConversationMessageId { get; set; }

            [JsonProperty("is_cropped")] public bool? IsCropped { get; set; }

            [JsonProperty("members_count")] public int? MembersCount { get; set; }

            [JsonProperty("update_time")] public long? UpdateTime { get; set; }

            [JsonProperty("was_listened")] public bool? WasListened;


            [JsonProperty("pinned_at")] public long? PinnedAt { get; set; }


            public Assoc<string, object> JsonPayload()
            {
                if (Payload == null)
                {
                    _payload = new Assoc<string, object>();
                }
                
                return _payload ?? (_payload = JsonConvert.DeserializeObject<Assoc<string, object>>(Payload));
            }
            
            public class ActionObject
            {
                [JsonProperty("type")] public ActionType Type { get; set; }

                [JsonProperty("member_id")] public int MemberId { get; set; }

                [JsonProperty("text")] public string Text { get; set; }

                [JsonProperty("email")] public string Email { get; set; }

                public PhotoObject Photo { get; set; }

                [JsonConverter(typeof(StringEnumConverter))]
                public enum ActionType
                {
                    [EnumMember(Value = "chat_photo_update")]
                    ChatPhotoUpdate,

                    [EnumMember(Value = "chat_photo_remove")]
                    ChatPhotoRemove,

                    [EnumMember(Value = "chat_create")]
                    ChatCreate,

                    [EnumMember(Value = "chat_title_update")]
                    ChatTitleUpdate,

                    [EnumMember(Value = "chat_invite_user")]
                    ChatInviteUser,

                    [EnumMember(Value = "chat_kick_user")]
                    ChatKickUser,

                    [EnumMember(Value = "chat_pin_message")]
                    ChatPinMessage,

                    [EnumMember(Value = "chat_unpin_message")]
                    ChatUnpinMessage,

                    [EnumMember(Value = "chat_invite_user_by_link")]
                    ChatInviteUserByLink
                }

                public class PhotoObject
                {
                    [JsonProperty("photo_50")] public string PhotoSmall { get; set; }

                    [JsonProperty("photo_100")] public string PhotoMedium { get; set; }

                    [JsonProperty("photo_200")] public string PhotoBig { get; set; }
                }
            }
        }
    }

    public class NewMessageEvent : MessageEvent
    {

    }
}