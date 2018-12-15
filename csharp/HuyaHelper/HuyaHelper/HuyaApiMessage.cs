using System;
using System.Collections.Generic;
using System.Text;

namespace HuyaHelper
{
    public class HuyaApiMessage<TDataType>
    {
        public int statusCode;
        public string statusMsg;
        public TDataType data;
    }
}
