using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VKBotABC.Utils;

namespace VKBotABC.Objects
{
    public class Message
    {
        public long RandomId { get; set; } = new Random().Next();
        
        //что
        public string MessageText { get; set; }
        public Attachment[] Attachments { get; set; }
        public long? StickerId { get; set; }
        public KeyboardObject Keyboard { get; set; }

        // public TemplateObject Template { get; set; }
        //аттачменты
        
        
        //кому
        public long? UserId { get; set; }
        public long? PeerId { get; set; }
        public string Domain { get; set; }
        public long? ChatId { get; set; }

        
        //Мета
        public double? Lat { get; set; }
        public double? Long { get; set; }
        
        public int? ReplyTo { get; set; }
        //форварды


        public Assoc<string, object> ToParams()
        {
            var ret = new Assoc<string, object>
            {
                { "random_id", RandomId }
            };

            if (MessageText != null) 
                ret.Add("message", MessageText);
            
            if (Attachments != null && Attachments.Length > 0) 
                ret.Add("attachment", string.Join(",", Attachments.Select(i => i.ToString())));
            
            if (StickerId != null) 
                ret.Add("sticker_id", StickerId);

            if (Keyboard != null)
                ret.Add("keyboard", JsonConvert.SerializeObject(Keyboard));
            
            if (UserId != null)
                ret.Add("user_id", UserId);

            if (PeerId != null)
                ret.Add("peer_id", PeerId);

            if (Domain != null)
                ret.Add("domain", Domain);

            if (ChatId != null)
                ret.Add("chat_id", ChatId);
            
            if (Lat != null)
                ret.Add("lat", Lat);
            
            if (Long != null)
                ret.Add("long", Long);
            
            if (ReplyTo != null)
                ret.Add("reply_to", ReplyTo);
            
            return ret;
        }

        public class KeyboardObject
        {
            [JsonProperty("one_time")]
            public bool OneTime;
            
            [JsonProperty("inline")]
            public bool Inline;

            [JsonProperty("buttons")]
            public List<List<Button>> Buttons = new List<List<Button>>();


            [JsonIgnore] private int _currentRow = 0;
            

            public void Row()
            {
                _currentRow++;
            }

            public KeyboardObject AddButton(Button button)
            {
                if (Buttons.Count == _currentRow)
                {
                    Buttons.Add(new List<Button>());
                }
                Buttons[_currentRow].Add(button);

                return this;
            }

            public KeyboardObject AddTextButton(string text)
            {
                return AddTextButton(text, Button.ColorType.Positive.ToString());
            }

            public KeyboardObject AddTextButton(string text, string color, string action = null)
            {
                if (!Enum.TryParse(color, true, out Button.ColorType parsedColor))
                    parsedColor = Button.ColorType.Positive;

                return AddTextButton(text, parsedColor, action == null ? null : new Assoc<string, object>
                {
                    {"action", action}
                });
            }

            
            public KeyboardObject AddTextButton(string text, Button.ColorType color, Assoc<string, object> payload)
            {
                AddButton(new Button
                {
                    Action = new Button.TextAction
                    {
                        Label = text,
                        Payload = payload
                    },
                    Color = color
                });

                return this;
            }
            
            public class Button
            {


                [JsonProperty("color")]
                public ColorType Color { get; set; }

                [JsonProperty("action")]
                public IButtonAction Action { get; set; }
                
                [JsonConverter(typeof(StringEnumConverter))]
                public enum ColorType
                {
                    [EnumMember(Value = "primary")]
                    Primary,
                    
                    [EnumMember(Value = "secondary")]
                    Secondary,
                    
                    [EnumMember(Value = "negative")]
                    Negative,
                    
                    [EnumMember(Value = "positive")]
                    Positive
                }
                
                public interface IButtonAction
                {
                }

                public class TextAction : IButtonAction
                {
                    [JsonProperty("type")]
                    public string Type { get; } = "text";
                    
                    [JsonProperty("label")]
                    public string Label { get; set; }
                    
                    [JsonProperty("payload")]
                    public Assoc<string, object> Payload { get; set; }
                }

                public class OpenLinkAction : IButtonAction
                {
                    
                    [JsonProperty("type")]
                    public string Type { get; } = "open_link";

                    [JsonProperty("link")]
                    public string Link { get; set; }
                    
                    [JsonProperty("label")]
                    public string Label { get; set; }
                    
                    [JsonProperty("payload")]
                    public Assoc<string, object> Payload { get; set; }
                }

                //Location
                
                //VKPay
                
                //VKApps
                
                //Callback
            }
        }

        public class Attachment
        {
            public string Type { get; set; }
            public long OwnerId { get; set; }
            public long MediaId { get; set; }

            public string AccessKey { get; set; }
            
            public override string ToString()
            {
                var ret = $"{Type}{OwnerId}_{MediaId}";
                if (AccessKey != null)
                {
                    ret += $"_{AccessKey}";
                }
                
                return ret;
            }
        }
    }
}