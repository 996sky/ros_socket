
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//↓添加的
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace 客户端
{
    public partial class Form1 : Form
    {

        //创建连接的Socket
        Socket sendSocket;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void showInfo(string msg)
        {
            textBox3.AppendText(msg + "\r\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(this.textBox1.Text.Trim());    //ip地址.
                sendSocket.Connect(ip, Convert.ToInt32(this.textBox2.Text.Trim()));    //端口地址
                showInfo("连接成功^_^");
                showInfo("服务器" + sendSocket.RemoteEndPoint.ToString());
                showInfo("客户端" + sendSocket.LocalEndPoint.ToString());
                //启动线程发送消息
                Thread receiveThread = new Thread(ReceiveMsg);
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                showInfo(ex.Message);
                MessageBox.Show("连接服务器出错:" + ex.ToString());
            }
        }

        private void ReceiveMsg()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024 * 1024];
                    int len = sendSocket.Receive(buffer);   //获取长度
                    string s = Encoding.UTF8.GetString(buffer, 0, len);
                    //将数据显示到textbox3中
                    showInfo(sendSocket.RemoteEndPoint.ToString() + ":" + s);
                }
                catch (Exception ex)
                {
                    showInfo(ex.Message);
                    break;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //发送信息
            if (sendSocket != null)
            {
                try
                {
                    showInfo(textBox4.Text);
                    byte[] buffer = Encoding.UTF8.GetBytes(textBox4.Text);
                    sendSocket.Send(buffer);
                }
                catch (Exception ex)
                {
                    //显示错误
                    showInfo(ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
