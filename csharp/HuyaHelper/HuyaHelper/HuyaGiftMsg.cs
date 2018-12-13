using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuyaHelper
{
    public class HuyaGiftMsg : HuyaChatApiMsg
    {
        public HuyaGiftMsg()
        {
            setActionType(ActionType.getSendItemNotice);
        }

        public int run(string appId, string secretId, string roomId)
        {
            int result = login(appId, secretId, roomId);
            return result;
        }
    }
}
