using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuyaHelper
{
    public partial class frmMain : Form
    {
        private HuyaChatRoom chatroom = null;

        private AtomicInt msg_cnt = new AtomicInt();
        private AtomicInt closing = new AtomicInt();

        private bool isActived = false;

        private delegate void delegateChatMsg(string nickname, string content);
        private delegate void delegateGiftMsg(string nickname, string itemName, int itemCount);

        private struct RoomIdInfo
        {
            public string roomId;
            public string roomIntro;
        }

        private static RoomIdInfo[] roomIdInfos = {
            new RoomIdInfo { roomId = "0",        roomIntro = "请选择一个主播" },
            new RoomIdInfo { roomId = "520880",   roomIntro = "520880 (拉风龙)" },
            new RoomIdInfo { roomId = "521000",   roomIntro = "521000 (卡尔)" },
            new RoomIdInfo { roomId = "626813",   roomIntro = "626813 (女王盐)" },
            new RoomIdInfo { roomId = "666007",   roomIntro = "666007 (大申屠)" },
            new RoomIdInfo { roomId = "908400",   roomIntro = "908400 (董小飒)" },
            new RoomIdInfo { roomId = "931827",   roomIntro = "931827 (大圣归来)" },
            new RoomIdInfo { roomId = "15382773", roomIntro = "15382773 (郭子)" },
        };

        public frmMain()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
            closing.Set(0);
        }

        public void clearChatContent()
        {
            chatContent.Focus();
            chatContent.Clear();
            chatContent.ClearUndo();
        }

        public void appendChatMsg(string nickname, string content)
        {
            if (this.Handle == null || closing.GetAndAdd(0) == 1)
            {
                return;
            }

            if (chatContent.InvokeRequired)
            {
                delegateChatMsg delegates = new delegateChatMsg(appendChatMsg);
                chatContent.Invoke(delegates, nickname, content);
            }
            else
            {
                int cnt = msg_cnt.Increment();
                if (cnt > 500)
                {
                    chatContent.Clear();
                    chatContent.ClearUndo();
                    msg_cnt.Set(0);
                    GC.Collect();
                }

                if (!isActived && (cnt % 10) == 9)
                {
                    chatContent.Focus();
                    chatContent.Select(chatContent.TextLength, 0);
                    chatContent.ScrollToCaret();
                }

                string text;
                //text = string.Format("[{0}]: {1}\n", nickname, content);
                //text = cnt.ToString() + "\n";
                //chatContent.AppendText(text);

                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Black;
                chatContent.AppendText("[");
                
                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Blue;
                chatContent.AppendText(nickname);

                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Black;
                text = string.Format("]: {0}\n", content);
                chatContent.AppendText(text);

                //chatContent.Focus();
            }
        }

        public void appendGiftMsg(string nickname, string itemName, int itemCount)
        {
            if (this.Handle == null || closing.GetAndAdd(0) == 1)
            {
                return;
            }

            if (chatContent.InvokeRequired)
            {
                delegateGiftMsg delegates = new delegateGiftMsg(appendGiftMsg);
                chatContent.Invoke(delegates, nickname, itemName, itemCount);
            }
            else
            {
                int cnt = msg_cnt.Increment();
                if (cnt > 500)
                {
                    chatContent.Clear();
                    chatContent.ClearUndo();
                    msg_cnt.Set(0);
                    GC.Collect();
                }

                //chatContent.Focus();

                if (!isActived && (cnt % 10) == 9)
                {
                    chatContent.Focus();
                    chatContent.Select(chatContent.TextLength, 0);
                    chatContent.ScrollToCaret();
                }

                string text;
                //text = string.Format("[{0}]: {1} x {2}\n",
                //                     nickname, itemName, itemCount);
                //text = cnt.ToString() + "\n";
                //chatContent.AppendText(text);

                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Black;
                chatContent.AppendText("[");

                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Blue;
                chatContent.AppendText(nickname);

                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Black;
                chatContent.AppendText("]: ");

                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Red;
                chatContent.AppendText(itemName);

                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Black;
                text = string.Format(" x {0}\n", itemCount);
                chatContent.AppendText(text);

                //chatContent.Focus();
            }
        }

        /*
        protected override void DefWndProc(ref System.Windows.Forms.Message messsage)
        {
            SimpleNoticeMessage noticeData;
            SimpleGiftMessage giftData;
            switch (messsage.Msg)
            {
                //接收CopyData消息，读取发送过来的数据
                case Messages.WM_COPYDATA:
                    {
                        COPYDATASTRUCT cds = new COPYDATASTRUCT();
                        Type type = cds.GetType();
                        cds = (COPYDATASTRUCT)messsage.GetLParam(type);
                        uint flag = (uint)(cds.dwData);
                        //byte[] data = new byte[cds.cbData];
                        //Marshal.Copy(cds.lpData, data, 0, data.Length);

                        if (flag <= Messages.TypeFlag)
                        {
                            // String
                        }
                        else if (flag == Messages.NoticeType)
                        {
                            // Binary bytes
                            noticeData = new SimpleNoticeMessage();
                            noticeData = (SimpleNoticeMessage)Marshal.PtrToStructure(cds.lpData, typeof(SimpleNoticeMessage));
                            appendChatMsg(noticeData.sendNick, noticeData.content);
                        }
                        else if (flag == Messages.GiftType)
                        {
                            // Binary bytes
                            giftData = new SimpleGiftMessage();
                            giftData = (SimpleGiftMessage)Marshal.PtrToStructure(cds.lpData, typeof(SimpleGiftMessage));
                            appendGiftMsg(giftData.sendNick, giftData.itemName, giftData.sendItemCount);
                        }
                    }
                    break;

                default:
                    base.DefWndProc(ref messsage);
                    break;
            }
        }
        //*/

        private void frmMain_Activated(object sender, EventArgs e)
        {
            isActived = true;
        }

        private void frmMain_Deactivate(object sender, EventArgs e)
        {
            isActived = false;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            chatroom = new HuyaChatRoom();
            if (chatroom != null)
            {
                chatroom.setParent(this);
            }

            isActived = true;

            cbBoxRoomId.Items.Clear();

            int infoLength = roomIdInfos.Length;
            ListItem[] listItems = new ListItem[infoLength];
            for (int i = 0; i < infoLength; i++)
            {
                RoomIdInfo info = roomIdInfos[i];
                listItems[i] = new ListItem(info.roomId, info.roomIntro);
                cbBoxRoomId.Items.Add(listItems[i]);
            }

            cbBoxRoomId.SelectedIndex = 0;
            cbBoxRoomId.SelectedItem = listItems[0];
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            int result;
            if (chatroom != null)
            {
                if (chatroom.isRunning())
                {
                    chatroom.logout();
                }

                string roomId = txtBoxRoomId.Text;
                roomId.Trim();
                if (roomId != "")
                {
                    result = chatroom.login(roomId);
                }
                else
                {
                    MessageBox.Show("房间号不能为空!", "HuyaHelper");
                }
            }
        }

        private void cbBoxRoomId_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem listItem = cbBoxRoomId.SelectedItem as ListItem;
            if (listItem != null)
            {
                string roomId = listItem.Id;
                roomId.Trim();
                if (roomId != "" && roomId != "0")
                {
                    txtBoxRoomId.Text = roomId;
                }
            }
        }
    }
}
