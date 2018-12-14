using System;
using System.Collections.Generic;
using System.Text;

namespace HuyaHelper
{
    public class HuyaGiftMsg : HuyaChatApiMsg
    {
        public HuyaGiftMsg()
        {
            setActionType(ActionType.getSendItemNotice);
        }
    }
}
