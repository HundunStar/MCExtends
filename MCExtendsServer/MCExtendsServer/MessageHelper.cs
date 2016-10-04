using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MCExtendsServer
{
    class MessageHelper
    {
        public static Message getMsg(string json)
        {
            return JsonConvert.DeserializeObject<Message>(json);
        }

        public static string getJson(Message msg)
        {
            return JsonConvert.SerializeObject(msg);
        }

        public static string getShowMessage(string msg)
        {
            Message message = new Message();
            message.ID = "^&^Show^&^";
            message.Type = "Show";
            message.Msg = msg;
            return getJson(message);
        }
    }
}
