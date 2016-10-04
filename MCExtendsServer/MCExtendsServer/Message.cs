using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCExtendsServer
{
    class Message
    {
        public string ID;
        public string Type;
        public List<string> Attribute;
        public string Msg; 
    }
}

//发送聊天消息样例：{"ID":"HundunStar","Type":"Message","Attribute":{"Fedora25","2016/10/3 10:52:52"},"Msg":"你好"}
//登陆样例：{"ID":"HundunStar","Type":"Login","Attribute":{"Password"},"Msg":""}
//人数统计样例：{"ID":"^&^WebSite^&^","Type":"Arithmetic","Attribute":{},"Msg":"13454"}