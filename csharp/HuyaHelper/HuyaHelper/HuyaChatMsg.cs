using System;
using System.Collections.Generic;
using System.Linq;
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

        public int run(string appId, string secretId, string roomId)
        {
            int result = login(appId, secretId, roomId);
            return result;
        }
    }
}
