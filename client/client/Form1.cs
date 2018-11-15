using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;

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


        delegate void SetTextCallback(string text);
        String recvString;
        //VehicleData myVehicleData = new VehicleData();
        VehicleData setVehicleData = new VehicleData();

        private const int portNum = 3939;
        private const string hostName = "localhost";
        TcpClient client;
        NetworkStream ns;
        Thread t = null;


        public Client()
        {
            InitializeComponent();

            buttonConnect.Enabled = true;
            buttonDisconnect.Enabled = false;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            IPAddress[] ipv4Addresses = Array.FindAll(Dns.GetHostEntry(string.Empty).AddressList, a => a.AddressFamily == AddressFamily.InterNetwork);
            IPAddress ipAddr = ipv4Addresses[0];
            try
            {
                client = new TcpClient(ipAddr.ToString(), portNum);
                ns = client.GetStream();
                /*String s = "Connected";
                byte[] byteTime = Encoding.ASCII.GetBytes(s);
                ns.Write(byteTime, 0, byteTime.Length);*/
                t = new Thread(DoWork);
                t.Start();

                buttonConnect.Enabled = false;
                buttonDisconnect.Enabled = true;

                SendData(setVehicleData);
            }
            catch { }
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
                t.Abort();
                client.Close();

                buttonDisconnect.Enabled = false;
                buttonConnect.Enabled = true;

        }

        // This is run as a thread

        public void DoWork()
        {
            byte[] bytes = new byte[1024];
            int bytesRead = 0;

            while (true)
            {
                try
                {
                    bytesRead = ns.Read(bytes, 0, bytes.Length);
                }
                catch
                {
                    this.SetText("Disconnected");
                }

                this.SetText(Encoding.ASCII.GetString(bytes, 0, bytesRead));
            }
        }

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBoxReceivedData.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                try
                {
                    this.Invoke(d, new object[] { text });
                }
                catch { }
            }
            else
            {
                this.textBoxReceivedData.Text = text;
            }
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

            SendData(setVehicleData);
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

            SendData(setVehicleData);
        }

        private void textBoxReceivedData_TextChanged(object sender, EventArgs e)
        {
            VehicleData myvhData = new VehicleData();
            int value;

            myvhData = setVehicleData;

            if(textBoxReceivedData.Text == "Disconnected")
            {
                buttonDisconnect_Click(sender, e);
                return;
            }

            recvString = textBoxReceivedData.Text;

            if (recvString.Length > 0)
            {
                string[] subStrings = recvString.Split(',');

                Int32.TryParse(subStrings[0], out value);
                myvhData.newData = value;

                Int32.TryParse(subStrings[1], out value);
                myvhData.ignition = value;

                myvhData.gear = subStrings[2][0];

                Int32.TryParse(subStrings[3], out value);
                myvhData.turnSignal = value;

                Int32.TryParse(subStrings[4], out value);
                myvhData.speed = value;

                Int32.TryParse(subStrings[5], out value);
                myvhData.wheelDegree = value;

                Int32.TryParse(subStrings[6], out value);
                myvhData.acceleration = value;

                Int32.TryParse(subStrings[7], out value);
                myvhData.checkSum = value;

                if (calculateChecksum(recvString) == myvhData.checkSum)
                {
                    setVehicleData = myvhData;
                    UpdateGUI(setVehicleData);
                }
            }
        }


        private int calculateChecksum(string sData)
        {
            int i;
            int chkSum = 0;

            i = sData.Length - 1;

            while (sData[i] != ',' && i > 0) i--;       // find last parameter before checksum

            while ((i--) != 0)
            {
                chkSum += sData[i];
            }

            return chkSum;
        }

        private void UpdateGUI(VehicleData updataVh)
        {
            if (updataVh.ignition != 0)
                labelIgnition.Text = "ON";
            else
                labelIgnition.Text = "OFF";

            labelSelectedGear.Text = updataVh.gear.ToString();

            labelSpeed.Text = updataVh.speed.ToString();

            labelActSteer.Text = updataVh.wheelDegree.ToString();
            trackBarActualSteerPosition.Value = updataVh.wheelDegree;

            switch (updataVh.turnSignal)
            {
                default:
                case 0:
                    labelTurnIndLeft.Text = "0";
                    labelTurnIndRight.Text = "0";
                    break;
                case 1:
                    labelTurnIndLeft.Text = "1";
                    labelTurnIndRight.Text = "0";
                    break;
                case 2:
                    labelTurnIndLeft.Text = "0";
                    labelTurnIndRight.Text = "1";
                    break;
                case 3:
                    labelTurnIndLeft.Text = "1";
                    labelTurnIndRight.Text = "1";
                    break;
            }

        }

        private void SendData(VehicleData sendVhData)
        {
            if (buttonDisconnect.Enabled)
            {
                String s;

                s = sendVhData.ignition.ToString() + ',';
                s += sendVhData.gear.ToString() + ',';
                s += sendVhData.turnSignal.ToString() + ',';
                s += sendVhData.speed.ToString() + ',';
                s += sendVhData.wheelDegreeSet.ToString() + ',';
                s += sendVhData.acceleration.ToString() + ',';
                s += sendVhData.checkSum.ToString() + '\n';

                sendVhData.checkSum = calculateChecksum(s);

                s = sendVhData.ignition.ToString() + ',';
                s += sendVhData.gear.ToString() + ',';
                s += sendVhData.turnSignal.ToString() + ',';
                s += sendVhData.speed.ToString() + ',';
                s += sendVhData.wheelDegreeSet.ToString() + ',';
                s += sendVhData.acceleration.ToString() + ',';
                s += sendVhData.checkSum.ToString() + '\n';

                byte[] byteTime = Encoding.ASCII.GetBytes(s);
                ns.Write(byteTime, 0, byteTime.Length);
            }
        }

        private void checkBoxIgnition_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxIgnition.Checked)
                setVehicleData.ignition = 1;
            else
                setVehicleData.ignition = 0;

            SendData(setVehicleData);

        }

        private void radioButtonR_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonR.Checked) setVehicleData.gear = 'R';
            else if (radioButtonN.Checked) setVehicleData.gear = 'N';
            else if (radioButtonD.Checked) setVehicleData.gear = 'D';
            else
                setVehicleData.gear = 'N';

            SendData(setVehicleData);
        }

        private void checkBoxTurnLeft_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTurnLeft.Checked) setVehicleData.turnSignal |= 1; 
            else
                setVehicleData.turnSignal &= ~1;

            if (checkBoxTurnRight.Checked) setVehicleData.turnSignal |= 2;
            else
                setVehicleData.turnSignal &= ~2;

            SendData(setVehicleData);
        }

        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            buttonDisconnect_Click(sender, e);
        }
    }
}
