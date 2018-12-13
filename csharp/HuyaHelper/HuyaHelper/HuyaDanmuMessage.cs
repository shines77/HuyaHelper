using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuyaHelper
{
    public class HuyaDanmuMessage<TDataType>
    {
        public int statusCode;
        public string statusMsg;
        public TDataType data;
    }
}
