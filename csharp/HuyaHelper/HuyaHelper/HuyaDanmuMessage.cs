using System;
using System.Collections.Generic;
using System.Text;

namespace HuyaHelper
{
    public class HuyaDanmuMessage<TDataType>
    {
        public int statusCode;
        public string statusMsg;
        public TDataType data;
    }
}
