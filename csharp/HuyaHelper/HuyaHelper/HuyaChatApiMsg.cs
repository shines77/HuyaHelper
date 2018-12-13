using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using WebSocketSharp;
using System.IO;
using System.Web.Script.Serialization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HuyaHelper
{

    public enum ActionType
    {
        None,
        getMessageNotice,
        getSendItemNotice,
        Last
    }

    public class HuyaChatApiMsg
    {
        private frmMain parent = null;
        private ActionType actionType = ActionType.None;

        private object locker = new object();

        //private ClientWebSocket ws = null;
        //private CancellationToken cancellationToken;

        private WebSocketSharp.WebSocket websocket = null;
        private System.Threading.Timer heartbeatTimer = null;

        private bool isWss = false;
        private const string ApiHost = "openapi.huya.com";

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, ref COPYDATASTRUCT lParam);

        public HuyaChatApiMsg()
        {
            //ws = new ClientWebSocket();
            //cancellationToken = new CancellationToken();
        }

        public void setParent(frmMain form)
        {
            this.parent = form;
        }

        public ActionType getActionType()
        {
            return actionType;
        }

        public void setActionType(ActionType actionType)
        {
            this.actionType = actionType;
        }

        public string actionTypeToString()
        {
            switch (this.actionType)
            {
                case ActionType.None:
                    return "None";

                case ActionType.getMessageNotice:
                    return "getMessageNotice";

                case ActionType.getSendItemNotice:
                    return "getSendItemNotice";

                case ActionType.Last:
                    return "Last";

                default:
                    return "Unknown";
            }
        }

        //
        // action = [getMessageNotice, getSendItemNotice];
        //
        private string prepareApiUrl(string appId, string secretId, string roomId, string action)
        {
            uint timestamp = TimeStamp.now();
            string data = string.Format("{{\"roomId\":{0}}}", roomId);
            string plaintext = string.Format("data={0}&key={1}&timestamp={2}",
                                             data, secretId, timestamp);
            string sign = MD5Crypto.encrypt32(plaintext);

            string apiUrl;
            if (!this.isWss)
            {
                apiUrl = string.Format("ws://{0}/index.html?do={1}&data={2}" +
                                       "&appId={3}&timestamp={4}&sign={5}",
                                       ApiHost, action, data, appId, timestamp, sign);
            }
            else
            {
                apiUrl = string.Format("wss://{0}/index.html?do={1}&data={2}" +
                                       "&appId={3}&timestamp={4}&sign={5}",
                                       ApiHost, action, data, appId, timestamp, sign);
            }
            return apiUrl;
        }

        private void onHeartbeat(object state)
        {
            Debug.WriteLine("HuyaChatApiMsg::onHeartbeat()");
            websocket.Send("ping");
        }

        private void onOpen(object sender, EventArgs eventArgs)
        {
            Debug.WriteLine("HuyaChatApiMsg::onOpen() enter");
            if (websocket != null)
            {
                if (websocket.ReadyState == WebSocketSharp.WebSocketState.Open)
                {
                    //
                    // See: https://www.cnblogs.com/arxive/p/7015853.html
                    //
                    heartbeatTimer = new System.Threading.Timer(new TimerCallback(onHeartbeat), null, 0, 15000);
                }
            }
            Debug.WriteLine("HuyaChatApiMsg::onOpen() leave");
        }

        private void onMessage(object sender, WebSocketSharp.MessageEventArgs eventArgs)
        {
            //Debug.WriteLine("HuyaChatApiMsg::onMessage() enter");

            if ((websocket == null) ||
                (websocket.ReadyState != WebSocketSharp.WebSocketState.Open))
            {
                return;
            }

            string jsonStr;
            if (eventArgs.IsText)
            {
                jsonStr = eventArgs.Data;
            }
            else if (eventArgs.IsBinary)
            {
                jsonStr = Encoding.UTF8.GetString(eventArgs.RawData);
            }
            else if (eventArgs.IsPing)
            {
                return;
            }
            else
            {
                //StreamReader reader = new StreamReader(eventArgs.Data);
                //jsonStr = reader.ReadToEnd();
                jsonStr = "ping";
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                if (actionType == ActionType.getMessageNotice)
                {
                    var msg = json.Deserialize<HuyaNoticeMessage>(jsonStr);
                    if (parent != null)
                    {
                        lock (locker)
                        {
                            parent.appendChatMsg(msg.data.sendNick, msg.data.content);
                        }

                        /*
                        SimpleNoticeMessage data = new SimpleNoticeMessage();
                        data.sendNick = msg.data.sendNick;
                        data.content = msg.data.content;

                        int buff_len = Marshal.SizeOf(typeof(SimpleNoticeMessage));

                        COPYDATASTRUCT cds;
                        cds.dwData = (IntPtr)Messages.NoticeType;
                        cds.cbData = buff_len;
                        cds.lpData = Marshal.AllocHGlobal(buff_len);
                        Marshal.StructureToPtr(data, cds.lpData, true);
                        //Marshal.Copy(data[], 0, cds.lpData, buff_len);

                        SendMessage(parent.Handle, Messages.WM_COPYDATA, parent.Handle, ref cds);

                        data = (SimpleNoticeMessage)Marshal.PtrToStructure(cds.lpData, typeof(SimpleNoticeMessage));
                        Marshal.FreeHGlobal(cds.lpData);
                        //*/
                    }

                    //Debug.WriteLine("[" + msg.data.sendNick + "]: " + msg.data.content);
                }
                else if (actionType == ActionType.getSendItemNotice)
                {
                    var msg = json.Deserialize<HuyaGiftMessage>(jsonStr);
                    if (parent != null)
                    {
                        lock (locker)
                        {
                            parent.appendGiftMsg(msg.data.sendNick, msg.data.itemName, msg.data.sendItemCount);
                        }

                        /*
                        SimpleGiftMessage data = new SimpleGiftMessage();
                        data.sendNick = msg.data.sendNick;
                        data.itemName = msg.data.itemName;
                        data.sendItemCount = msg.data.sendItemCount;

                        int buff_len = Marshal.SizeOf(typeof(SimpleGiftMessage));

                        COPYDATASTRUCT cds;
                        cds.dwData = (IntPtr)Messages.GiftType;
                        cds.cbData = buff_len;
                        cds.lpData = Marshal.AllocHGlobal(buff_len);
                        Marshal.StructureToPtr(data, cds.lpData, true);
                        //Marshal.Copy(data[], 0, cds.lpData, buff_len);

                        //SendMessage(parent.Handle, Messages.WM_COPYDATA, parent.Handle, ref cds);

                        data = (SimpleGiftMessage)Marshal.PtrToStructure(cds.lpData, typeof(SimpleGiftMessage));
                        Marshal.FreeHGlobal(cds.lpData);
                        //*/
                    }

                    //Debug.WriteLine("[" + msg.data.sendNick + "]: " + msg.data.itemName + " x " + msg.data.sendItemCount);
                }
                else
                {
                    //
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            //Debug.WriteLine("HuyaChatApiMsg::onMessage() leave");
        }

        private void onError(object sender, WebSocketSharp.ErrorEventArgs eventArgs)
        {
            Debug.WriteLine("HuyaChatApiMsg::onError() enter");
            Debug.WriteLine("HuyaChatApiMsg::onError() leave");
        }

        private void onClose(object sender, WebSocketSharp.CloseEventArgs eventArgs)
        {
            Debug.WriteLine("HuyaChatApiMsg::onClose() enter");
            if (heartbeatTimer != null)
            {
                heartbeatTimer.Dispose();
                heartbeatTimer = null;
            }
            Debug.WriteLine("HuyaChatApiMsg::onClose() leave");
        }

        //
        // A known bug:
        //
        //   WebSocketSharp.WebSocketException: The header of a frame cannot be read from the stream.
        //
        // Bug Fixed: ext.cs line 759:
        //
        // if (nread == 0 || nread == length) {
        //
        // Modified to: if (nread == length) {
        //
        // See: https://github.com/sta/websocket-sharp/issues/71
        //

        public int login(string appId, string secretId, string roomId)
        {
            Debug.WriteLine("HuyaChatApiMsg::login() enter");

            int result = -1;
            if (websocket == null)
            {
                string action = actionTypeToString();
                string apiUrl = prepareApiUrl(appId, secretId, roomId, action);

                try
                {
                    //websocket.ConnectAsync(new Uri(apiUrl), cancellationToken).Wait();
                    websocket = new WebSocketSharp.WebSocket(apiUrl);
                    {
                        websocket.OnOpen += onOpen;
                        websocket.OnMessage += onMessage;
                        websocket.OnError += onError;
                        websocket.OnClose += onClose;

                        websocket.Connect();
                    }
                }
                catch (Exception ex)
                {
                    string what = ex.ToString();
                    Debug.WriteLine("Exception: " + what);
                }

                byte[] sendBuffer = Encoding.UTF8.GetBytes("ping");
                //websocket.SendAsync(new ArraySegment<byte>(sendBuffer), WebSocketMessageType.Text, true, cancellationToken).Wait();
                //websocket.SendAsync(new ArraySegment<byte>(sendBuffer), WebSocketMessageType.Binary, true, cancellationToken).Wait();
                //webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "1", cancellation).Wait();

                //Console.WriteLine("HuyaChatApiMsg[action = {0}]", action);
                //Console.WriteLine("login: {0}", apiUrl);

                Debug.Print("HuyaChatApiMsg[action = {0}]\n", action);
                Debug.Print("login: {0}\n", apiUrl);
                result = 0;
            }

            Debug.WriteLine("HuyaChatApiMsg::login() leave");
            return result;
        }

        public void close()
        {
            if (websocket != null)
            {
                websocket.Close();
                websocket = null;
            }
        }
    }
}
