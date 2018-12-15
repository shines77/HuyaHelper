using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuyaHelper
{
    public class HuyaChatSocket : HuyaApiSocket
    {
        public HuyaChatSocket()
        {
            setActionType(ActionType.getMessageNotice);
        }
    }
}
