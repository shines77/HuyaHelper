using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HuyaHelper
{
    public class NoticeMessage
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
    public class SimpleNoticeMessage
    {
        public string sendNick;
        public string content;
    }

    public class HuyaNoticeMessage : HuyaDanmuMessage<NoticeMessage>
    {
    }
}
