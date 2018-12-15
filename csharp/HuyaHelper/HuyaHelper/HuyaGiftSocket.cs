using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuyaHelper
{
    public class HuyaGiftSocket : HuyaApiSocket
    {
        public HuyaGiftSocket()
        {
            setActionType(ActionType.getSendItemNotice);
        }
    }
}
