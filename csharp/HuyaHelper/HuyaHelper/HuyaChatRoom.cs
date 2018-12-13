using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuyaHelper
{
    class HuyaChatRoom
    {
        private HuyaChatMsg chatMessage = null;
        private HuyaGiftMsg giftMessage = null;

        private const string appId = "154440619445824126";
        private const string secretId = "790130ee";

        public HuyaChatRoom()
        {
            chatMessage = new HuyaChatMsg();
            giftMessage = new HuyaGiftMsg();
        }

        public void setParent(frmMain form)
        {
            chatMessage.setParent(form);
            giftMessage.setParent(form);
        }

        public int run(string roomId)
        {
            int resultChat = -1;
            if (chatMessage != null)
            {
                resultChat = chatMessage.run(appId, secretId, roomId);
            }

            int resultGift = -1;
            if (chatMessage != null)
            {
                resultGift = giftMessage.run(appId, secretId, roomId);
                resultGift = 1;
            }
            return ((resultChat >= 0) && (resultGift >= 0)) ? 1 : 0;
        }
    }
}
