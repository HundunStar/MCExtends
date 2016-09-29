using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Collections;
using System.Text.RegularExpressions;

namespace MCExtendsServer
{
    public partial class MCExtendsServer : DevExpress.XtraEditors.XtraForm
    {
        public MCExtendsServer()
        {
            InitializeComponent();
        }

        const int BufferSize = 8192;
        private TcpListener Listener;
        private List<RemoteClient> LstClient;
        private List<string> LstMsg;
        delegate void DlgStart();
        Thread ThrServer;
        string IP = "0.0.0.0";
        int Port = 6934;

        private void BtnStart_Click(object sender, EventArgs e)
        {
            StartServer();
        }

        private void StartServer()
        {
            LstMsg = new List<string>();
            ShowRecvMsg("Eve服务器启动中......");
            ShowRecvMsg("监听IP：" + Listener.LocalEndpoint.ToString());
            Listener.Start();
            ThrServer = new Thread(AcceptClient);
            ThrServer.Start();
            ShowRecvMsg("Eve服务器启动完毕！");
        }

        private RemoteClient GetClient(string cID)
        {
            foreach (RemoteClient rc in LstClient)
            {
                if (rc.ClientID == cID)
                    return rc;
            }
            return null;
        }

        private void ShowRecvMsg(string msg)
        {
            DlgAddMsg Dam = new DlgAddMsg(ShowMessage);
            this.Invoke(Dam, msg);

            //当消息串中含有要发送给某一目标客户端的ID，并且此ID的客户端在线时，给其发送当前消息

            string cID = ParseRecvID(msg);
            if (cID != "")
            {
                LstMsg.Add(msg);
                RemoteClient rc = GetClient(cID);//当msg消息指定的接受端的客户端在线时，给其发送当前消息
                if (rc != null)
                    SendMessage(rc, msg);
            }
            //如果消息串中含有客户端ID号的信息，则在待发送消息的列表中搜索是否有要发送给此客户端的消息，如有，则进行发送
            string pID = ParseClientID(msg);
            if (pID != "")//检测到消息中表明客户端的ID号，将消息队列中属于此ID号客户端的消息逐一
            {
                RemoteClient rc = GetClient(pID);
                List<string> LstSuc = new List<string>(), LstFail = new List<string>();
                SendMessage(rc, ref LstSuc, ref LstFail);
            }
            //如果收到的消息中含有某一个客户端下线，则从远程客户端列表中将其删除
            ParseClientClose(msg);
        }

        private string ParseClientID(string msg)
        {
            string pattern = @"(?<=^\[ID=)(.+)(?=\])";
            if (Regex.IsMatch(msg, pattern))
            {
                Match m = Regex.Match(msg, pattern);
                return m.ToString();
            }
            return "";
        }
        private void ParseClientClose(string msg)
        {
            string pattern = @"(?<=^\[ClientClose=)(.+)(?=\])";
            if (Regex.IsMatch(msg, pattern))
            {
                Match m = Regex.Match(msg, pattern);
                string ClientID = m.ToString();
                //一个客户端关闭连接，需要从客户端列表中将其删除
                int n = LstClient.Count;
                for (int i = 0; i < n; ++i)
                {
                    if (LstClient[i].ClientID == ClientID)
                    {
                        LstClient.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 给指定的客户端发送属于他的消息
        /// </summary>
        /// <param name="rc">RemoteClient客户对象</param>
        /// <param name="LstFail">发送失败的消息列表</param>
        /// <param name="LstSuc">成功发送的消息列表</param>
        private void SendMessage(RemoteClient rc, ref List<string> LstSuc, ref List<string> LstFail)
        {
            if (LstMsg == null) return;
            if (LstSuc == null)
                LstSuc = new List<string>();
            else if (LstSuc.Count > 0)
                LstSuc.Clear();
            if (LstFail == null)
                LstFail = new List<string>();
            else if (LstFail.Count > 0)
                LstFail.Clear();
            for (int i = 0; i < LstMsg.Count; ++i)
            {
                string msg = LstMsg[i];
                //只有消息队列中属于RemoteClient的对象rc的消息才进行发送
                if (rc.ClientID == ParseRecvID(msg))
                {
                    bool Suc = SendMessage(rc, msg);
                    if (Suc)
                    {
                        LstSuc.Add(msg);
                        LstMsg.RemoveAt(i);//发送成功后，将已发送的消息从消息列表中删除，删除后LstMsg.Count会减少1
                        --i;//删除后LstMsg.Count会减少1，因此下标先减1，然后再加1，保持不变
                    }
                    else
                        LstFail.Add(msg);
                }
            }
        }
        private bool SendMessage(RemoteClient rc, string msg)
        {
            rc.SendMessage(msg);
            return true;
        }

        delegate void DlgAddMsg(string msg);
        private void ShowMessage(string msg)
        {
            LbcMsg.Items.Add(msg);
        }

        private void AcceptClient()
        {
            try
            {
                while (true)
                {
                    TcpClient remoteClient = Listener.AcceptTcpClient();
                    ShowRecvMsg("接受来自客户端的连接！");
                    ShowRecvMsg("客户端IP：" + remoteClient.Client.RemoteEndPoint.ToString());
                    //将连接到服务器的客户端添加到客户端列表，但此时客户端的ID还没有传递过来，要等连接完成后，通过
                    //客户端所发送的格式为：[ID=000]的消息串才确定此客户端形如：000的ID号
                    RemoteClient rc = new RemoteClient(remoteClient, ShowRecvMsg);
                    rc.OnClientIDChanged += new RemoteClient.DlgIDChanged(rc_OnClientIDChanged);
                    LstClient.Add(rc);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void rc_OnClientIDChanged(object sender, ClientIDEventArgs e)
        {
            //此处当一个客户端ID发生变化时，可以对客户端列表进行维护
            int n = LstClient.Count;
            string ClientID = e.ClientID;
            RemoteClient trc = sender as RemoteClient;
            for (int i = 0; i < n; ++i)
            {
                RemoteClient rc = LstClient[i];
                if (rc.ClientID == ClientID && trc != rc)
                {
                    rc.Close();
                    LstClient.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// 解析消息接受端的ID号
        /// </summary>
        /// <param name="msg">消息串，格式为："[From=abc][To=bcd]hi"，其中abc为发送消息的客户端ID号，bcd为消息接受端的ID号，hi为消息文本</param>
        /// <returns>形如："[From=000][To=001]hi"(消息串对From及To区分大小写)的消息串所解析出的接受端的ID号：bcd，如果不是上述格式，则返回空串</returns>
        private string ParseRecvID(string msg)
        {
            string patternFrom = @"(?<=^\[From=)(.+)(?=\])";
            if (Regex.IsMatch(msg, patternFrom))
            {
                int startIndex = msg.IndexOf(']') + 1;
                string s = msg.Substring(startIndex);
                string patternTo = @"(?<=^\[To=)(.+)(?=\])";
                if (Regex.IsMatch(s, patternTo))
                {
                    Match m = Regex.Match(s, patternTo);
                    return m.ToString();
                }
            }
            return "";
        }

    }

}
