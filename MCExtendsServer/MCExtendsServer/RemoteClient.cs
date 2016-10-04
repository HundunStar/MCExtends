using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MCExtendsServer
{
    public class RemoteClient {
        public string ClientID;
        private TcpClient client;
        private NetworkStream streamToClient;
        private const int BufferSize = 8192;
        private byte[] buffer;
        private RequestHandler handler;
        public delegate void DlgShowMsg(RemoteClient client,string Msg);
        DlgShowMsg Dsm;
        /// <summary>
        /// 初始化包装TCP通信的远程客户端
        /// </summary>
        /// <param name="client">TCP客户端</param>
        /// <param name="Dsm">服务器端的回调函数(用于显示或处理消息)</param>
        public RemoteClient(TcpClient client, DlgShowMsg Dsm)
        {
            //this.cID = cID;
            this.client = client;
            this.Dsm = Dsm;
            // 打印连接到的客户端信息
            Dsm(this,string.Format("获取到来自客户端的连接！{0} <-- {1}",
                client.Client.LocalEndPoint, client.Client.RemoteEndPoint));

            // 获得流
            streamToClient = client.GetStream();
            buffer = new byte[BufferSize];

            // 设置RequestHandler
            handler = new RequestHandler();

            // 在构造函数中就开始准备读取
            AsyncCallback callBack = new AsyncCallback(ReadComplete);
            streamToClient.BeginRead(buffer, 0, BufferSize, callBack, null);
        }

        // 再读取完成时进行回调
        private void ReadComplete(IAsyncResult ar) 
        {
            int bytesRead = 0;
            try 
            {
                lock (streamToClient) 
                {
                    if (!streamToClient.CanRead) return;
                    bytesRead = streamToClient.EndRead(ar);
                }
                if (bytesRead == 0)
                    return;
                    //throw new Exception("读取到0字节");

                string msg = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                Array.Clear(buffer,0,buffer.Length);        // 清空缓存，避免脏读
        
                string[] msgArray = handler.GetActualString(msg);   // 获取实际的字符串

                // 遍历获得到的字符串
                foreach (string s in msgArray) 
                {
                    //string pattern = @"(?<=^\[ID=)(\d+)(?=\])";
                    //if (Regex.IsMatch(s, pattern))
                    //{
                    //    Match m = Regex.Match(s, pattern);
                    //    ClientID = m.ToString();
                    //   // OnClientIDChanged?.Invoke(this, new ClientIDEventArgs(ClientID));
                    //    //一个客户端上线了，则服务端从消息队列中查找是否有此客户端应该要接受的消息，如有，则进行发送，使用回调函数进行处理
                    //    //在下面的onRecieveMessage函数调用中通过回调函数来进行发送
                    //}
                    
                    onRecieveMessage(s);
                }

                // 再次调用BeginRead()，完成时调用自身，形成无限循环
                lock (streamToClient) {
                    AsyncCallback callBack = new AsyncCallback(ReadComplete);
                    streamToClient.BeginRead(buffer, 0, BufferSize, callBack, null);
                }
            } 
            catch(Exception ex) {
                if(streamToClient!=null)
                    streamToClient.Dispose();
                client.Close();
                //MessageBox.Show(ex.Message);
                Dsm(this,ex.Message);      // 捕获异常时退出程序              
            }
        }

        /// <summary>
        /// 服务器端向当前客户端发送消息
        /// </summary>
        /// <param name="msg"></param>
        public void SendMessage(string msg)
        {
            byte[] temp = Encoding.Unicode.GetBytes(msg);
            streamToClient.Write(temp, 0, temp.Length);
            streamToClient.Flush();
        }

        public void Close()
        {
            SendMessage("[Server=Close]服务器关闭连接。");
            client.Close();
        }
        private void onRecieveMessage(string msg)
        {
            Dsm(this,msg);
            //Program.MainForm.ShowMessage(msg);
        }
    }
}
