using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace PProyecto2
{
    public partial class Form1 : Form
    {
        Socket socket;
        EndPoint epLocal, epRemote;
        byte[] buffer; //This will be use to send or receive messages
        int count = 0;
        Form2 form2 = new Form2();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled=true;
            timer1.Interval = 60000;
            timer1.Tick += new System.EventHandler((send, eventarg) =>
            {
                form2.ShowDialog(); count++;
                if (count > 3)
                {
                    count = 0;
                }
            });
            form2.ShowDialog();

            //set up socket
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            //get user IP
            textLocalIp.Text = GetLocalIP();
            textRemoteIp.Text = GetLocalIP();
            textLocalPort.Text = "80";
            textLocalPort.Text = "81";
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            //binding Socket
            epLocal = new IPEndPoint(IPAddress.Parse(textLocalIp.Text), Convert.ToInt32(textLocalPort.Text));
            socket.Bind(epLocal);
            //Connecting to remote IP
            epRemote = new IPEndPoint(IPAddress.Parse(textRemoteIp.Text), Convert.ToInt32(textRemotePort.Text));
            socket.Connect(epRemote);
            //Listening the specific port
            buffer = new byte[1500];
            socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);
        }

        private string GetLocalIP()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily ==AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            //Convert string message to byte[]
            ASCIIEncoding aEnconding = new ASCIIEncoding();
            byte[] sendingMessage = new byte[1500];
            sendingMessage = aEnconding.GetBytes(textMessage.Text);
            int number = int.Parse(textExtPublickey.Text);
            switch (count)
            {
                case 0:
                    number = (number - 2) / 2;
                    break;
                case 1:
                    number = (number - 3) / 3;
                    break;
                case 2:
                    number = (number - 4) / 4;
                    break;
                case 3:
                    number = (number - 5) / 5;
                    break;
                default:
                    break;
            }
            for (int i = 0; i < sendingMessage.Length; i++)
            {
                sendingMessage[i] -= (byte)number;
            }
         
            //TODO Encrypt
            //Sending the Encoded message
            socket.Send(sendingMessage);
            //adding to the listbox
            listMessage.Items.Add("Me: " + textMessage.Text);
            textMessage.Text = "";
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (string item in Form2.passwords)
            {
                if (item==textPassword.Text)
                {
                    Random rand = new Random();
                    int number = rand.Next(1, 24);
                    textPrivatekey.Text = Convert.ToString(number);
                    switch (count)
                    {
                        case 0:
                            textPublickey.Text=Convert.ToString((number*2)+2);
                            break;
                        case 1:
                            textPublickey.Text = Convert.ToString((number*3)+3);
                            break;
                        case 2:
                            textPublickey.Text = Convert.ToString((number * 4) + 4);
                            break;
                        case 3:
                            textPublickey.Text = Convert.ToString((number * 5) + 5);
                            break;
                        default:
                            break;
                    }
                  
                }
                else
                {
                    MessageBox.Show("Wrong password");

                }
            }
        }

        private void MessageCallBack(IAsyncResult aResult)
        {
            try
            {
	            byte[] receievedData = new byte[1500];
	            receievedData = (byte[])aResult.AsyncState;
                ASCIIEncoding aEnconding = new ASCIIEncoding();
                string encryptedMessage = aEnconding.GetString(receievedData);
                //Converting byte[] to string
                //TODO Decrypt
                int number=0;
                int privateKey = int.Parse(textPrivatekey.Text);
                switch (count)
                {
                    case 0:
                        number = privateKey;
                        break;
                    case 1:
                        number = privateKey;
                        break;
                    case 2:
                        number = privateKey;
                        break;
                    case 3:
                        number = privateKey;
                        break;
                    default:
                        break;
                }

              

                for (int i = 0; i < receievedData.Length; i++)
                {
                    if (receievedData[i] == 0) break;
                    receievedData[i] += (byte)number;
                }
               
	            string recievedMessage = aEnconding.GetString(receievedData);
                //Adding this message into Listbox
                listMessage.Items.Add("Encrypted Message Friend: " + encryptedMessage);
                listMessage.Items.Add("Friend: " + recievedMessage);
	
	            buffer = new byte[1500];
	            socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);
            }
             catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
