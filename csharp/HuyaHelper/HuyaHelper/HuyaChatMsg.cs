using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HuyaHelper
{
    public class HuyaChatMsg : HuyaChatApiMsg
    {
        public HuyaChatMsg()
        {
            setActionType(ActionType.getMessageNotice);
        }
    }
}
