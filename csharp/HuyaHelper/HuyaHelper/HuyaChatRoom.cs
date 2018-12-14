using System;
using System.Collections.Generic;
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

        public bool isRunning()
        {
            bool isRunningChat = false;
            if (chatMessage != null)
            {
                isRunningChat = chatMessage.isRunning();
            }

            bool isRunningGift = false;
            if (giftMessage != null)
            {
                isRunningGift = giftMessage.isRunning();
            }

            return (isRunningChat && isRunningGift);
        }

        public int login(string roomId)
        {
            int resultChat = -1;
            if (chatMessage != null)
            {
                resultChat = chatMessage.login(appId, secretId, roomId);
            }

            int resultGift = -1;
            if (giftMessage != null)
            {
                resultGift = giftMessage.login(appId, secretId, roomId);
            }
            return ((resultChat >= 0) && (resultGift >= 0)) ? 1 : 0;
        }

        public int logout()
        {
            if (chatMessage != null)
            {
                chatMessage.logout();
            }

            if (giftMessage != null)
            {
                giftMessage.logout();
            }
            return 1;
        }
    }
}
