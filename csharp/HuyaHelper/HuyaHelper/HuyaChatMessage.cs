using System;
using System.Runtime.InteropServices;
using System.Text;

namespace HuyaHelper
{
    public class ChatMessage
    {
        public int    roomId;
        public string sendNick;
        public string senderAvatarUrl;
        public string senderGender;
        public string showMode;
        public string content;
        public int    nobleLevel;
        public int    fansLevel;
    }

    //[StructLayout(LayoutKind.Sequential)]
    public class SimpleChatMessage
    {
        public string sendNick;
        public string content;
    }

    public class HuyaChatMessage : HuyaApiMessage<ChatMessage>
    {
    }
}
