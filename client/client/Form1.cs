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
using System.Net;

namespace client03
{
    public partial class Client : Form
    {
        // Receiving byte array  
        byte[] bytes = new byte[1024];
        Socket senderSock;


        public Client()
        {
            InitializeComponent();
            buttonConnect.Enabled = true;
            buttonDisconnect.Enabled = false;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                // Create one SocketPermission for socket access restrictions 
                SocketPermission permission = new SocketPermission(
                    NetworkAccess.Connect,    // Connection permission 
                    TransportType.Tcp,        // Defines transport types 
                    "",                       // Gets the IP addresses 
                    SocketPermission.AllPorts // All ports 
                    );

                // Ensures the code to have permission to access a Socket 
                permission.Demand();

                // Resolves a host name to an IPHostEntry instance            
                // Gets first IP address associated with a localhost 
                IPAddress[] ipv4Addresses = Array.FindAll(Dns.GetHostEntry(string.Empty).AddressList, a => a.AddressFamily == AddressFamily.InterNetwork);

                IPAddress ipAddr = ipv4Addresses[0];

                // Creates a network endpoint 
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 9876);

                // Create one Socket object to setup Tcp connection 
                senderSock = new Socket(
                    ipAddr.AddressFamily,// Specifies the addressing scheme 
                    SocketType.Stream,   // The type of socket  
                    ProtocolType.Tcp     // Specifies the protocols  
                    );

                senderSock.NoDelay = false;   // Using the Nagle algorithm 

                // Establishes a connection to a remote host 
                senderSock.Connect(ipEndPoint);
                //tbStatus.Text = "Socket connected to " + senderSock.RemoteEndPoint.ToString();

                buttonConnect.Enabled = false;
                buttonDisconnect.Enabled = true;
            }
            catch (Exception exc) { MessageBox.Show(exc.ToString()); }
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                // Disables sends and receives on a Socket. 
                senderSock.Shutdown(SocketShutdown.Both);

                //Closes the Socket connection and releases all resources 
                senderSock.Close();

                buttonDisconnect.Enabled = false;
                buttonConnect.Enabled = true;
            }
            catch (Exception exc) { MessageBox.Show(exc.ToString()); }
        }
    }
}
