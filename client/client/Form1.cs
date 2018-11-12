using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace client03
{
    public partial class Client : Form
    {
        public class VehicleData
        {
            public int newData { get; set; }
            public int ignition { get; set; }
            public char gear { get; set; }
            public int turnSignal { get; set; }
            public int speed { get; set; }
            public int wheelDegreeSet { get; set; }
            public int wheelDegree { get; set; }
            public int acceleration { get; set; }
            public int checkSum { get; set; }

            public VehicleData()
            {
                newData = 0;
                ignition = 0;
                gear = 'N';
                turnSignal = 0;
                speed = 0;
                wheelDegreeSet = 0;
                wheelDegree = 0;
                acceleration = 0;
                checkSum = 0;
            }
        }

        public class ObjectState
        {
            public const int BufferSize = 256;
            public Socket wSocket = null;
            public byte[] Buffer = new byte[BufferSize];
            public StringBuilder sb = new StringBuilder();
        }

        public class MyEventArgs : EventArgs    // guideline: derive from EventArgs
        {
            public string Msg { get; set; }
        }

        public class AsyncSocketClient
        {
            private const int Port = 3939;
            private static ManualResetEvent connectCompleted = new ManualResetEvent(false);
            private static ManualResetEvent sendCompleted = new ManualResetEvent(false);
            private static ManualResetEvent receiveCompleted = new ManualResetEvent(false);
            private static string response = String.Empty;

            

            public event EventHandler<MyEventArgs> Changed;    // the Event

            protected virtual void OnChanged(string msg)      // the Trigger
            {
                var args = new MyEventArgs { Msg = msg };    // this part will vary
                Changed?.Invoke(this, args);
            }

            public void StartClientReceive()
            {
                //try
                {
                    IPAddress[] ipv4Addresses = Array.FindAll(Dns.GetHostEntry(string.Empty).AddressList, a => a.AddressFamily == AddressFamily.InterNetwork);
                    IPAddress ipAddr = ipv4Addresses[0];
                    IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 3939);

                    Socket client = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    client.BeginConnect(ipEndPoint, new AsyncCallback(ConnectionCallback), client);

                    for (; ; )
                    {
                        /*Send(client, "This is a message <EOF>");
                        sendCompleted.WaitOne();
                        sendCompleted.Reset();*/

                        Receive(client);
                        receiveCompleted.WaitOne();
                        receiveCompleted.Reset();

                        OnChanged(response);  // raise the event
                                              // Console.WriteLine($"Response {response}");
                    }

                    client.Shutdown(SocketShutdown.Both);
                    client.Close();

                }
                //catch
                { }
            }

            private static void Receive(Socket client)
            {
                try
                {
                    ObjectState state = new ObjectState();
                    state.wSocket = client;
                    client.BeginReceive(state.Buffer, 0, ObjectState.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                }
                catch
                { }
            }

            private static void ReceiveCallback(IAsyncResult ar)
            {
                try
                {
                    ObjectState state = (ObjectState)ar.AsyncState;
                    var client = state.wSocket;
                    int byteRead = client.EndReceive(ar);
                    if(byteRead >0)
                    {
                        state.sb.Append(Encoding.ASCII.GetString(state.Buffer, 0, byteRead));
                        //client.BeginReceive(state.Buffer, 0, ObjectState.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                    }
                    //else
                    {
                        if(state.sb.Length > 1)
                        {
                            response = state.sb.ToString();
                        }

                        receiveCompleted.Set();
                    }
                }
                catch
                { }
            }

            private static void Send(Socket client, string data)
            {
                byte[] byteData = Encoding.ASCII.GetBytes(data);
                client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
            }

            private static void SendCallback(IAsyncResult ar)
            {
                try
                {
                    Socket client = (Socket)ar.AsyncState;
                    int byteSent = client.EndSend(ar);
                    // Console.WriteLine($"Sent: {byteSent}");
                    sendCompleted.Set();
                }
                catch
                { }
            }

            private static void ConnectionCallback(IAsyncResult ar)
            {
                try
                {
                    Socket client = (Socket)ar.AsyncState;
                    client.EndConnect(ar);
                    connectCompleted.Set();
                }
                catch
                { }
            }
        }

        // Receiving byte array  
        //byte[] bytes = new byte[1024];
        String recvString;
        //Socket senderSock;
        Thread _socketThread;

        // VehicleData myVehicleData = new VehicleData();
        VehicleData setVehicleData = new VehicleData();
        


        public Client()
        {
            InitializeComponent();
            buttonConnect.Enabled = true;
            buttonDisconnect.Enabled = false;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            _socketThread = new Thread(SocketThreadFunc);
            _socketThread.Start();
            /*try
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
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 3939);

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
            catch (Exception exc) { MessageBox.Show(exc.ToString()); }*/
        }

        private void SocketThreadFunc()
        {
            AsyncSocketClient asyncClient = new AsyncSocketClient();
            asyncClient.Changed += asyncClient_Changed;
            asyncClient.StartClientReceive();
        }

        private void asyncClient_Changed(object sender, MyEventArgs e)
        {
            
            string s = e.Msg;
            this.Invoke((MethodInvoker)delegate {
                textBoxReceivedData.Text = s; // runs on UI thread
            });
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            /*try
            {
                // Disables sends and receives on a Socket. 
                senderSock.Shutdown(SocketShutdown.Both);

                //Closes the Socket connection and releases all resources 
                senderSock.Close();

                buttonDisconnect.Enabled = false;
                buttonConnect.Enabled = true;
            }
            catch (Exception exc) { MessageBox.Show(exc.ToString()); }*/
        }

        private void trackBarSteering_Scroll(object sender, EventArgs e)
        {
            setVehicleData.wheelDegreeSet = trackBarSteering.Value;
            textBoxSteeringPosition.Text = setVehicleData.wheelDegreeSet.ToString();

        }

        private void textBoxSteeringPosition_TextChanged(object sender, EventArgs e)
        {
            int value;

            Int32.TryParse(textBoxSteeringPosition.Text, out value);
            if (value > trackBarSteering.Maximum) value = trackBarSteering.Maximum;
            if (value < trackBarSteering.Minimum) value = trackBarSteering.Minimum;

            textBoxSteeringPosition.Text = value.ToString();
            setVehicleData.wheelDegreeSet = value;
            
            trackBarSteering.Value = setVehicleData.wheelDegreeSet;
        }

        private void trackBarActualSteerPosition_ValueChanged(object sender, EventArgs e)
        {
            labelActSteer.Text = trackBarActualSteerPosition.Value.ToString();
        }

        private void trackBarAcc_Scroll(object sender, EventArgs e)
        {
            setVehicleData.acceleration = trackBarAcc.Value;
            textBoxAccel.Text = setVehicleData.acceleration.ToString();
        }

        private void textBoxAccel_TextChanged(object sender, EventArgs e)
        {
            int value;

            Int32.TryParse(textBoxAccel.Text, out value);
            if (value > trackBarAcc.Maximum) value = trackBarAcc.Maximum;
            if (value < trackBarAcc.Minimum) value = trackBarAcc.Minimum;

            textBoxAccel.Text = value.ToString();
            setVehicleData.acceleration = value;

            trackBarAcc.Value = setVehicleData.acceleration;
        }

        private void textBoxReceivedData_TextChanged(object sender, EventArgs e)
        {
            recvString = textBoxReceivedData.Text;
        }
    }
}
