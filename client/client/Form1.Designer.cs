namespace client03
{
    partial class Client
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.trackBarSteering = new System.Windows.Forms.TrackBar();
            this.trackBarActualSteerPosition = new System.Windows.Forms.TrackBar();
            this.trackBarAcc = new System.Windows.Forms.TrackBar();
            this.groupBoxGearSelect = new System.Windows.Forms.GroupBox();
            this.labelSelectedGear = new System.Windows.Forms.Label();
            this.radioButtonD = new System.Windows.Forms.RadioButton();
            this.radioButtonN = new System.Windows.Forms.RadioButton();
            this.radioButtonR = new System.Windows.Forms.RadioButton();
            this.checkBoxIgnition = new System.Windows.Forms.CheckBox();
            this.labelIgnition = new System.Windows.Forms.Label();
            this.labelSteeringWheel = new System.Windows.Forms.Label();
            this.labelActSteerPos = new System.Windows.Forms.Label();
            this.labelAcc = new System.Windows.Forms.Label();
            this.labelDeAcc = new System.Windows.Forms.Label();
            this.labelSpeed = new System.Windows.Forms.Label();
            this.checkBoxTurnLeft = new System.Windows.Forms.CheckBox();
            this.checkBoxTurnRight = new System.Windows.Forms.CheckBox();
            this.labelTurnInd = new System.Windows.Forms.Label();
            this.labelTurnIndLeft = new System.Windows.Forms.Label();
            this.labelTurnIndRight = new System.Windows.Forms.Label();
            this.textBoxSteeringPosition = new System.Windows.Forms.TextBox();
            this.textBoxAccel = new System.Windows.Forms.TextBox();
            this.labelActSteer = new System.Windows.Forms.Label();
            this.textBoxReceivedData = new System.Windows.Forms.TextBox();
            this.textBoxLastInputData = new System.Windows.Forms.TextBox();
            this.labelTimeout = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSteering)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarActualSteerPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAcc)).BeginInit();
            this.groupBoxGearSelect.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(13, 13);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(94, 13);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(75, 23);
            this.buttonDisconnect.TabIndex = 1;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // trackBarSteering
            // 
            this.trackBarSteering.Location = new System.Drawing.Point(12, 147);
            this.trackBarSteering.Maximum = 450;
            this.trackBarSteering.Minimum = -450;
            this.trackBarSteering.Name = "trackBarSteering";
            this.trackBarSteering.Size = new System.Drawing.Size(400, 45);
            this.trackBarSteering.TabIndex = 2;
            this.trackBarSteering.Scroll += new System.EventHandler(this.trackBarSteering_Scroll);
            // 
            // trackBarActualSteerPosition
            // 
            this.trackBarActualSteerPosition.Enabled = false;
            this.trackBarActualSteerPosition.Location = new System.Drawing.Point(12, 211);
            this.trackBarActualSteerPosition.Maximum = 450;
            this.trackBarActualSteerPosition.Minimum = -450;
            this.trackBarActualSteerPosition.Name = "trackBarActualSteerPosition";
            this.trackBarActualSteerPosition.Size = new System.Drawing.Size(400, 45);
            this.trackBarActualSteerPosition.TabIndex = 3;
            // 
            // trackBarAcc
            // 
            this.trackBarAcc.Location = new System.Drawing.Point(427, 8);
            this.trackBarAcc.Maximum = 100;
            this.trackBarAcc.Minimum = -100;
            this.trackBarAcc.Name = "trackBarAcc";
            this.trackBarAcc.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarAcc.Size = new System.Drawing.Size(45, 251);
            this.trackBarAcc.TabIndex = 4;
            this.trackBarAcc.Scroll += new System.EventHandler(this.trackBarAcc_Scroll);
            // 
            // groupBoxGearSelect
            // 
            this.groupBoxGearSelect.Controls.Add(this.labelSelectedGear);
            this.groupBoxGearSelect.Controls.Add(this.radioButtonD);
            this.groupBoxGearSelect.Controls.Add(this.radioButtonN);
            this.groupBoxGearSelect.Controls.Add(this.radioButtonR);
            this.groupBoxGearSelect.Location = new System.Drawing.Point(139, 42);
            this.groupBoxGearSelect.Name = "groupBoxGearSelect";
            this.groupBoxGearSelect.Size = new System.Drawing.Size(164, 64);
            this.groupBoxGearSelect.TabIndex = 5;
            this.groupBoxGearSelect.TabStop = false;
            this.groupBoxGearSelect.Text = "Gear Select";
            // 
            // labelSelectedGear
            // 
            this.labelSelectedGear.AutoSize = true;
            this.labelSelectedGear.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelSelectedGear.Location = new System.Drawing.Point(123, 19);
            this.labelSelectedGear.Name = "labelSelectedGear";
            this.labelSelectedGear.Size = new System.Drawing.Size(34, 31);
            this.labelSelectedGear.TabIndex = 3;
            this.labelSelectedGear.Text = "N";
            // 
            // radioButtonD
            // 
            this.radioButtonD.AutoSize = true;
            this.radioButtonD.Location = new System.Drawing.Point(84, 32);
            this.radioButtonD.Name = "radioButtonD";
            this.radioButtonD.Size = new System.Drawing.Size(33, 17);
            this.radioButtonD.TabIndex = 2;
            this.radioButtonD.Text = "D";
            this.radioButtonD.UseVisualStyleBackColor = true;
            this.radioButtonD.CheckedChanged += new System.EventHandler(this.radioButtonR_CheckedChanged);
            // 
            // radioButtonN
            // 
            this.radioButtonN.AutoSize = true;
            this.radioButtonN.Checked = true;
            this.radioButtonN.Location = new System.Drawing.Point(45, 32);
            this.radioButtonN.Name = "radioButtonN";
            this.radioButtonN.Size = new System.Drawing.Size(33, 17);
            this.radioButtonN.TabIndex = 1;
            this.radioButtonN.TabStop = true;
            this.radioButtonN.Text = "N";
            this.radioButtonN.UseVisualStyleBackColor = true;
            this.radioButtonN.CheckedChanged += new System.EventHandler(this.radioButtonR_CheckedChanged);
            // 
            // radioButtonR
            // 
            this.radioButtonR.AutoSize = true;
            this.radioButtonR.Location = new System.Drawing.Point(6, 32);
            this.radioButtonR.Name = "radioButtonR";
            this.radioButtonR.Size = new System.Drawing.Size(33, 17);
            this.radioButtonR.TabIndex = 0;
            this.radioButtonR.Text = "R";
            this.radioButtonR.UseVisualStyleBackColor = true;
            this.radioButtonR.CheckedChanged += new System.EventHandler(this.radioButtonR_CheckedChanged);
            // 
            // checkBoxIgnition
            // 
            this.checkBoxIgnition.AutoSize = true;
            this.checkBoxIgnition.Location = new System.Drawing.Point(13, 74);
            this.checkBoxIgnition.Name = "checkBoxIgnition";
            this.checkBoxIgnition.Size = new System.Drawing.Size(60, 17);
            this.checkBoxIgnition.TabIndex = 6;
            this.checkBoxIgnition.Text = "Ignition";
            this.checkBoxIgnition.UseVisualStyleBackColor = true;
            this.checkBoxIgnition.CheckedChanged += new System.EventHandler(this.checkBoxIgnition_CheckedChanged);
            // 
            // labelIgnition
            // 
            this.labelIgnition.AutoSize = true;
            this.labelIgnition.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelIgnition.Location = new System.Drawing.Point(78, 67);
            this.labelIgnition.Name = "labelIgnition";
            this.labelIgnition.Size = new System.Drawing.Size(55, 26);
            this.labelIgnition.TabIndex = 7;
            this.labelIgnition.Text = "OFF";
            // 
            // labelSteeringWheel
            // 
            this.labelSteeringWheel.AutoSize = true;
            this.labelSteeringWheel.Location = new System.Drawing.Point(13, 127);
            this.labelSteeringWheel.Name = "labelSteeringWheel";
            this.labelSteeringWheel.Size = new System.Drawing.Size(116, 13);
            this.labelSteeringWheel.TabIndex = 8;
            this.labelSteeringWheel.Text = "Steering wheel position";
            // 
            // labelActSteerPos
            // 
            this.labelActSteerPos.AutoSize = true;
            this.labelActSteerPos.Location = new System.Drawing.Point(13, 195);
            this.labelActSteerPos.Name = "labelActSteerPos";
            this.labelActSteerPos.Size = new System.Drawing.Size(102, 13);
            this.labelActSteerPos.TabIndex = 9;
            this.labelActSteerPos.Text = "Actual steer position";
            // 
            // labelAcc
            // 
            this.labelAcc.AutoSize = true;
            this.labelAcc.Location = new System.Drawing.Point(466, 13);
            this.labelAcc.Name = "labelAcc";
            this.labelAcc.Size = new System.Drawing.Size(58, 13);
            this.labelAcc.TabIndex = 10;
            this.labelAcc.Text = "Accelerate";
            // 
            // labelDeAcc
            // 
            this.labelDeAcc.AutoSize = true;
            this.labelDeAcc.Location = new System.Drawing.Point(466, 243);
            this.labelDeAcc.Name = "labelDeAcc";
            this.labelDeAcc.Size = new System.Drawing.Size(71, 13);
            this.labelDeAcc.TabIndex = 11;
            this.labelDeAcc.Text = "Deaccelerate";
            // 
            // labelSpeed
            // 
            this.labelSpeed.AutoSize = true;
            this.labelSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelSpeed.Location = new System.Drawing.Point(309, 51);
            this.labelSpeed.Name = "labelSpeed";
            this.labelSpeed.Size = new System.Drawing.Size(51, 55);
            this.labelSpeed.TabIndex = 12;
            this.labelSpeed.Text = "0";
            // 
            // checkBoxTurnLeft
            // 
            this.checkBoxTurnLeft.AutoSize = true;
            this.checkBoxTurnLeft.Location = new System.Drawing.Point(93, 262);
            this.checkBoxTurnLeft.Name = "checkBoxTurnLeft";
            this.checkBoxTurnLeft.Size = new System.Drawing.Size(15, 14);
            this.checkBoxTurnLeft.TabIndex = 13;
            this.checkBoxTurnLeft.UseVisualStyleBackColor = true;
            this.checkBoxTurnLeft.CheckedChanged += new System.EventHandler(this.checkBoxTurnLeft_CheckedChanged);
            // 
            // checkBoxTurnRight
            // 
            this.checkBoxTurnRight.AutoSize = true;
            this.checkBoxTurnRight.Location = new System.Drawing.Point(114, 262);
            this.checkBoxTurnRight.Name = "checkBoxTurnRight";
            this.checkBoxTurnRight.Size = new System.Drawing.Size(15, 14);
            this.checkBoxTurnRight.TabIndex = 14;
            this.checkBoxTurnRight.UseVisualStyleBackColor = true;
            this.checkBoxTurnRight.CheckedChanged += new System.EventHandler(this.checkBoxTurnLeft_CheckedChanged);
            // 
            // labelTurnInd
            // 
            this.labelTurnInd.AutoSize = true;
            this.labelTurnInd.Location = new System.Drawing.Point(16, 262);
            this.labelTurnInd.Name = "labelTurnInd";
            this.labelTurnInd.Size = new System.Drawing.Size(73, 13);
            this.labelTurnInd.TabIndex = 15;
            this.labelTurnInd.Text = "Turn Indicator";
            // 
            // labelTurnIndLeft
            // 
            this.labelTurnIndLeft.AutoSize = true;
            this.labelTurnIndLeft.Location = new System.Drawing.Point(94, 280);
            this.labelTurnIndLeft.Name = "labelTurnIndLeft";
            this.labelTurnIndLeft.Size = new System.Drawing.Size(13, 13);
            this.labelTurnIndLeft.TabIndex = 16;
            this.labelTurnIndLeft.Text = "0";
            // 
            // labelTurnIndRight
            // 
            this.labelTurnIndRight.AutoSize = true;
            this.labelTurnIndRight.Location = new System.Drawing.Point(115, 280);
            this.labelTurnIndRight.Name = "labelTurnIndRight";
            this.labelTurnIndRight.Size = new System.Drawing.Size(13, 13);
            this.labelTurnIndRight.TabIndex = 17;
            this.labelTurnIndRight.Text = "0";
            // 
            // textBoxSteeringPosition
            // 
            this.textBoxSteeringPosition.Location = new System.Drawing.Point(187, 124);
            this.textBoxSteeringPosition.MaxLength = 4;
            this.textBoxSteeringPosition.Name = "textBoxSteeringPosition";
            this.textBoxSteeringPosition.Size = new System.Drawing.Size(50, 20);
            this.textBoxSteeringPosition.TabIndex = 18;
            this.textBoxSteeringPosition.Text = "0";
            this.textBoxSteeringPosition.TextChanged += new System.EventHandler(this.textBoxSteeringPosition_TextChanged);
            // 
            // textBoxAccel
            // 
            this.textBoxAccel.Location = new System.Drawing.Point(469, 124);
            this.textBoxAccel.MaxLength = 4;
            this.textBoxAccel.Name = "textBoxAccel";
            this.textBoxAccel.Size = new System.Drawing.Size(50, 20);
            this.textBoxAccel.TabIndex = 19;
            this.textBoxAccel.Text = "0";
            this.textBoxAccel.TextChanged += new System.EventHandler(this.textBoxAccel_TextChanged);
            // 
            // labelActSteer
            // 
            this.labelActSteer.AutoSize = true;
            this.labelActSteer.Location = new System.Drawing.Point(184, 195);
            this.labelActSteer.Name = "labelActSteer";
            this.labelActSteer.Size = new System.Drawing.Size(13, 13);
            this.labelActSteer.TabIndex = 20;
            this.labelActSteer.Text = "0";
            // 
            // textBoxReceivedData
            // 
            this.textBoxReceivedData.Location = new System.Drawing.Point(427, 280);
            this.textBoxReceivedData.Name = "textBoxReceivedData";
            this.textBoxReceivedData.Size = new System.Drawing.Size(105, 20);
            this.textBoxReceivedData.TabIndex = 21;
            this.textBoxReceivedData.Visible = false;
            this.textBoxReceivedData.TextChanged += new System.EventHandler(this.textBoxReceivedData_TextChanged);
            // 
            // textBoxLastInputData
            // 
            this.textBoxLastInputData.Location = new System.Drawing.Point(187, 279);
            this.textBoxLastInputData.Name = "textBoxLastInputData";
            this.textBoxLastInputData.Size = new System.Drawing.Size(225, 20);
            this.textBoxLastInputData.TabIndex = 22;
            // 
            // labelTimeout
            // 
            this.labelTimeout.AutoSize = true;
            this.labelTimeout.Location = new System.Drawing.Point(427, 286);
            this.labelTimeout.Name = "labelTimeout";
            this.labelTimeout.Size = new System.Drawing.Size(0, 13);
            this.labelTimeout.TabIndex = 23;
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 311);
            this.Controls.Add(this.labelTimeout);
            this.Controls.Add(this.textBoxLastInputData);
            this.Controls.Add(this.textBoxReceivedData);
            this.Controls.Add(this.labelActSteer);
            this.Controls.Add(this.textBoxAccel);
            this.Controls.Add(this.textBoxSteeringPosition);
            this.Controls.Add(this.labelTurnIndRight);
            this.Controls.Add(this.labelTurnIndLeft);
            this.Controls.Add(this.labelTurnInd);
            this.Controls.Add(this.checkBoxTurnRight);
            this.Controls.Add(this.checkBoxTurnLeft);
            this.Controls.Add(this.labelSpeed);
            this.Controls.Add(this.labelDeAcc);
            this.Controls.Add(this.labelAcc);
            this.Controls.Add(this.labelActSteerPos);
            this.Controls.Add(this.labelSteeringWheel);
            this.Controls.Add(this.labelIgnition);
            this.Controls.Add(this.checkBoxIgnition);
            this.Controls.Add(this.groupBoxGearSelect);
            this.Controls.Add(this.trackBarAcc);
            this.Controls.Add(this.trackBarActualSteerPosition);
            this.Controls.Add(this.trackBarSteering);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.buttonConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Client";
            this.Text = "Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Client_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSteering)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarActualSteerPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAcc)).EndInit();
            this.groupBoxGearSelect.ResumeLayout(false);
            this.groupBoxGearSelect.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.TrackBar trackBarSteering;
        private System.Windows.Forms.TrackBar trackBarActualSteerPosition;
        private System.Windows.Forms.TrackBar trackBarAcc;
        private System.Windows.Forms.GroupBox groupBoxGearSelect;
        private System.Windows.Forms.RadioButton radioButtonR;
        private System.Windows.Forms.CheckBox checkBoxIgnition;
        private System.Windows.Forms.RadioButton radioButtonD;
        private System.Windows.Forms.RadioButton radioButtonN;
        private System.Windows.Forms.Label labelSelectedGear;
        private System.Windows.Forms.Label labelIgnition;
        private System.Windows.Forms.Label labelSteeringWheel;
        private System.Windows.Forms.Label labelActSteerPos;
        private System.Windows.Forms.Label labelAcc;
        private System.Windows.Forms.Label labelDeAcc;
        private System.Windows.Forms.Label labelSpeed;
        private System.Windows.Forms.CheckBox checkBoxTurnLeft;
        private System.Windows.Forms.CheckBox checkBoxTurnRight;
        private System.Windows.Forms.Label labelTurnInd;
        private System.Windows.Forms.Label labelTurnIndLeft;
        private System.Windows.Forms.Label labelTurnIndRight;
        private System.Windows.Forms.TextBox textBoxSteeringPosition;
        private System.Windows.Forms.TextBox textBoxAccel;
        private System.Windows.Forms.Label labelActSteer;
        private System.Windows.Forms.TextBox textBoxReceivedData;
        private System.Windows.Forms.TextBox textBoxLastInputData;
        private System.Windows.Forms.Label labelTimeout;
    }
}

