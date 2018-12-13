using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HuyaHelper
{
    public class GiftMessage
    {
        public int    roomId;
        public string presenterNick;
        public string sendNick;
        public string senderAvatarUrl;
        public string itemName;
        public int    sendItemCount;
    }

    // [StructLayout(LayoutKind.Sequential)]
    public class SimpleGiftMessage
    {
        public string sendNick;
        public string itemName;
        public int sendItemCount;
    }

    public class HuyaGiftMessage : HuyaDanmuMessage<GiftMessage>
    {
    }
}
