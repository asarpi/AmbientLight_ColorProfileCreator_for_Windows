namespace AmbientLight_ColorProfileCreator_for_Windows
{
    partial class AmbiLight
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AmbiLight));
            this.button_startStreaming = new System.Windows.Forms.Button();
            this.button_setManualColor = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_basicSetup = new System.Windows.Forms.TabPage();
            this.tabPage_manualControl = new System.Windows.Forms.TabPage();
            this.tabPage_advancedSetup = new System.Windows.Forms.TabPage();
            this.textBox_board = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_connectionState = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.comboBox_comPortList = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_messages = new System.Windows.Forms.TextBox();
            this.button_stopStreaming = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_resetLeds = new System.Windows.Forms.Button();
            this.button_reconnectSerial = new System.Windows.Forms.Button();
            this.button_disconnectSerial = new System.Windows.Forms.Button();
            this.tabPage_log = new System.Windows.Forms.TabPage();
            this.textBox_logger = new System.Windows.Forms.TextBox();
            this.button_getLogs = new System.Windows.Forms.Button();
            this.textBox_manualColor = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDown_factorR = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_factorG = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_factorB = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabControl1.SuspendLayout();
            this.tabPage_basicSetup.SuspendLayout();
            this.tabPage_manualControl.SuspendLayout();
            this.tabPage_advancedSetup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage_log.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_factorR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_factorG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_factorB)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_startStreaming
            // 
            this.button_startStreaming.Location = new System.Drawing.Point(52, 383);
            this.button_startStreaming.Name = "button_startStreaming";
            this.button_startStreaming.Size = new System.Drawing.Size(133, 23);
            this.button_startStreaming.TabIndex = 0;
            this.button_startStreaming.Text = "Start Streaming";
            this.button_startStreaming.UseVisualStyleBackColor = true;
            this.button_startStreaming.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_setManualColor
            // 
            this.button_setManualColor.Location = new System.Drawing.Point(135, 39);
            this.button_setManualColor.Name = "button_setManualColor";
            this.button_setManualColor.Size = new System.Drawing.Size(128, 23);
            this.button_setManualColor.TabIndex = 1;
            this.button_setManualColor.Text = "Set manual Color";
            this.button_setManualColor.UseVisualStyleBackColor = true;
            this.button_setManualColor.Click += new System.EventHandler(this.button2_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_basicSetup);
            this.tabControl1.Controls.Add(this.tabPage_manualControl);
            this.tabControl1.Controls.Add(this.tabPage_advancedSetup);
            this.tabControl1.Controls.Add(this.tabPage_log);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(437, 563);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage_basicSetup
            // 
            this.tabPage_basicSetup.Controls.Add(this.button_disconnectSerial);
            this.tabPage_basicSetup.Controls.Add(this.button_reconnectSerial);
            this.tabPage_basicSetup.Controls.Add(this.button_resetLeds);
            this.tabPage_basicSetup.Controls.Add(this.groupBox1);
            this.tabPage_basicSetup.Controls.Add(this.button_stopStreaming);
            this.tabPage_basicSetup.Controls.Add(this.button3);
            this.tabPage_basicSetup.Controls.Add(this.button_startStreaming);
            this.tabPage_basicSetup.Controls.Add(this.comboBox_comPortList);
            this.tabPage_basicSetup.Controls.Add(this.label3);
            this.tabPage_basicSetup.Controls.Add(this.label2);
            this.tabPage_basicSetup.Controls.Add(this.label1);
            this.tabPage_basicSetup.Controls.Add(this.textBox_connectionState);
            this.tabPage_basicSetup.Controls.Add(this.textBox_board);
            this.tabPage_basicSetup.Location = new System.Drawing.Point(4, 22);
            this.tabPage_basicSetup.Name = "tabPage_basicSetup";
            this.tabPage_basicSetup.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_basicSetup.Size = new System.Drawing.Size(429, 537);
            this.tabPage_basicSetup.TabIndex = 0;
            this.tabPage_basicSetup.Text = "Basic Setup";
            this.tabPage_basicSetup.UseVisualStyleBackColor = true;
            // 
            // tabPage_manualControl
            // 
            this.tabPage_manualControl.Controls.Add(this.textBox_manualColor);
            this.tabPage_manualControl.Controls.Add(this.button_setManualColor);
            this.tabPage_manualControl.Location = new System.Drawing.Point(4, 22);
            this.tabPage_manualControl.Name = "tabPage_manualControl";
            this.tabPage_manualControl.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_manualControl.Size = new System.Drawing.Size(429, 537);
            this.tabPage_manualControl.TabIndex = 1;
            this.tabPage_manualControl.Text = "Manual Control";
            this.tabPage_manualControl.UseVisualStyleBackColor = true;
            this.tabPage_manualControl.Click += new System.EventHandler(this.tabPage_manualControl_Click);
            // 
            // tabPage_advancedSetup
            // 
            this.tabPage_advancedSetup.Controls.Add(this.groupBox2);
            this.tabPage_advancedSetup.Controls.Add(this.label7);
            this.tabPage_advancedSetup.Controls.Add(this.textBox1);
            this.tabPage_advancedSetup.Location = new System.Drawing.Point(4, 22);
            this.tabPage_advancedSetup.Name = "tabPage_advancedSetup";
            this.tabPage_advancedSetup.Size = new System.Drawing.Size(429, 537);
            this.tabPage_advancedSetup.TabIndex = 2;
            this.tabPage_advancedSetup.Text = "Advanced Setup";
            this.tabPage_advancedSetup.UseVisualStyleBackColor = true;
            // 
            // textBox_board
            // 
            this.textBox_board.Enabled = false;
            this.textBox_board.Location = new System.Drawing.Point(165, 22);
            this.textBox_board.Name = "textBox_board";
            this.textBox_board.Size = new System.Drawing.Size(121, 20);
            this.textBox_board.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Board";
            // 
            // textBox_connectionState
            // 
            this.textBox_connectionState.Enabled = false;
            this.textBox_connectionState.Location = new System.Drawing.Point(165, 66);
            this.textBox_connectionState.Name = "textBox_connectionState";
            this.textBox_connectionState.Size = new System.Drawing.Size(120, 20);
            this.textBox_connectionState.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Connection State";
            // 
            // comboBox_comPortList
            // 
            this.comboBox_comPortList.FormattingEnabled = true;
            this.comboBox_comPortList.Location = new System.Drawing.Point(164, 106);
            this.comboBox_comPortList.Name = "comboBox_comPortList";
            this.comboBox_comPortList.Size = new System.Drawing.Size(121, 21);
            this.comboBox_comPortList.TabIndex = 2;
            this.comboBox_comPortList.Text = "Default";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(291, 105);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(113, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Get portnames";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(58, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Communication Ports";
            // 
            // textBox_messages
            // 
            this.textBox_messages.Location = new System.Drawing.Point(13, 34);
            this.textBox_messages.Multiline = true;
            this.textBox_messages.Name = "textBox_messages";
            this.textBox_messages.Size = new System.Drawing.Size(296, 151);
            this.textBox_messages.TabIndex = 4;
            // 
            // button_stopStreaming
            // 
            this.button_stopStreaming.Location = new System.Drawing.Point(232, 383);
            this.button_stopStreaming.Name = "button_stopStreaming";
            this.button_stopStreaming.Size = new System.Drawing.Size(125, 23);
            this.button_stopStreaming.TabIndex = 5;
            this.button_stopStreaming.Text = "Stop Streaming";
            this.button_stopStreaming.UseVisualStyleBackColor = true;
            this.button_stopStreaming.Click += new System.EventHandler(this.button_stopStreaming_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_messages);
            this.groupBox1.Location = new System.Drawing.Point(48, 186);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(322, 191);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Messages";
            // 
            // button_resetLeds
            // 
            this.button_resetLeds.Location = new System.Drawing.Point(156, 432);
            this.button_resetLeds.Name = "button_resetLeds";
            this.button_resetLeds.Size = new System.Drawing.Size(100, 23);
            this.button_resetLeds.TabIndex = 7;
            this.button_resetLeds.Text = "Reset Leds";
            this.button_resetLeds.UseVisualStyleBackColor = true;
            this.button_resetLeds.Click += new System.EventHandler(this.button_resetLeds_Click);
            // 
            // button_reconnectSerial
            // 
            this.button_reconnectSerial.Location = new System.Drawing.Point(110, 143);
            this.button_reconnectSerial.Name = "button_reconnectSerial";
            this.button_reconnectSerial.Size = new System.Drawing.Size(75, 23);
            this.button_reconnectSerial.TabIndex = 8;
            this.button_reconnectSerial.Text = "Reconnect";
            this.button_reconnectSerial.UseVisualStyleBackColor = true;
            this.button_reconnectSerial.Click += new System.EventHandler(this.button_reconnectSerial_Click);
            // 
            // button_disconnectSerial
            // 
            this.button_disconnectSerial.Location = new System.Drawing.Point(247, 143);
            this.button_disconnectSerial.Name = "button_disconnectSerial";
            this.button_disconnectSerial.Size = new System.Drawing.Size(75, 23);
            this.button_disconnectSerial.TabIndex = 9;
            this.button_disconnectSerial.Text = "Disconnect";
            this.button_disconnectSerial.UseVisualStyleBackColor = true;
            this.button_disconnectSerial.Click += new System.EventHandler(this.button_disconnectSerial_Click);
            // 
            // tabPage_log
            // 
            this.tabPage_log.Controls.Add(this.button_getLogs);
            this.tabPage_log.Controls.Add(this.textBox_logger);
            this.tabPage_log.Location = new System.Drawing.Point(4, 22);
            this.tabPage_log.Name = "tabPage_log";
            this.tabPage_log.Size = new System.Drawing.Size(429, 537);
            this.tabPage_log.TabIndex = 3;
            this.tabPage_log.Text = "log";
            this.tabPage_log.UseVisualStyleBackColor = true;
            // 
            // textBox_logger
            // 
            this.textBox_logger.Location = new System.Drawing.Point(28, 25);
            this.textBox_logger.Multiline = true;
            this.textBox_logger.Name = "textBox_logger";
            this.textBox_logger.Size = new System.Drawing.Size(377, 449);
            this.textBox_logger.TabIndex = 0;
            // 
            // button_getLogs
            // 
            this.button_getLogs.Location = new System.Drawing.Point(183, 497);
            this.button_getLogs.Name = "button_getLogs";
            this.button_getLogs.Size = new System.Drawing.Size(75, 23);
            this.button_getLogs.TabIndex = 1;
            this.button_getLogs.Text = "Get logs";
            this.button_getLogs.UseVisualStyleBackColor = true;
            this.button_getLogs.Click += new System.EventHandler(this.button_getLogs_Click);
            // 
            // textBox_manualColor
            // 
            this.textBox_manualColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBox_manualColor.Location = new System.Drawing.Point(135, 97);
            this.textBox_manualColor.Name = "textBox_manualColor";
            this.textBox_manualColor.Size = new System.Drawing.Size(128, 21);
            this.textBox_manualColor.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(208, 47);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(52, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = ":(";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "factor R";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "factor G";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(34, 123);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "factor B";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(104, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "number of LEDs";
            // 
            // numericUpDown_factorR
            // 
            this.numericUpDown_factorR.Location = new System.Drawing.Point(117, 45);
            this.numericUpDown_factorR.Name = "numericUpDown_factorR";
            this.numericUpDown_factorR.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.numericUpDown_factorR.Size = new System.Drawing.Size(43, 20);
            this.numericUpDown_factorR.TabIndex = 5;
            this.numericUpDown_factorR.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown_factorR.ValueChanged += new System.EventHandler(this.numericUpDown_factorR_ValueChanged);
            // 
            // numericUpDown_factorG
            // 
            this.numericUpDown_factorG.Location = new System.Drawing.Point(117, 83);
            this.numericUpDown_factorG.Name = "numericUpDown_factorG";
            this.numericUpDown_factorG.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.numericUpDown_factorG.Size = new System.Drawing.Size(43, 20);
            this.numericUpDown_factorG.TabIndex = 5;
            this.numericUpDown_factorG.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown_factorG.ValueChanged += new System.EventHandler(this.numericUpDown_factorG_ValueChanged);
            // 
            // numericUpDown_factorB
            // 
            this.numericUpDown_factorB.Location = new System.Drawing.Point(117, 119);
            this.numericUpDown_factorB.Name = "numericUpDown_factorB";
            this.numericUpDown_factorB.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.numericUpDown_factorB.Size = new System.Drawing.Size(43, 20);
            this.numericUpDown_factorB.TabIndex = 5;
            this.numericUpDown_factorB.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown_factorB.ValueChanged += new System.EventHandler(this.numericUpDown_factorB_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numericUpDown_factorG);
            this.groupBox2.Controls.Add(this.numericUpDown_factorB);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.numericUpDown_factorR);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(92, 107);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 179);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Color correction";
            // 
            // AmbiLight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1095, 754);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AmbiLight";
            this.Text = "AmbiLight";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_basicSetup.ResumeLayout(false);
            this.tabPage_basicSetup.PerformLayout();
            this.tabPage_manualControl.ResumeLayout(false);
            this.tabPage_manualControl.PerformLayout();
            this.tabPage_advancedSetup.ResumeLayout(false);
            this.tabPage_advancedSetup.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage_log.ResumeLayout(false);
            this.tabPage_log.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_factorR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_factorG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_factorB)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_startStreaming;
        private System.Windows.Forms.Button button_setManualColor;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.TabPage tabPage_advancedSetup;
        private System.Windows.Forms.TabPage tabPage_manualControl;
        private System.Windows.Forms.TabPage tabPage_basicSetup;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_board;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_connectionState;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox comboBox_comPortList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.TextBox textBox_messages;
        private System.Windows.Forms.Button button_stopStreaming;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_resetLeds;
        private System.Windows.Forms.Button button_disconnectSerial;
        private System.Windows.Forms.Button button_reconnectSerial;
        private System.Windows.Forms.TabPage tabPage_log;
        private System.Windows.Forms.TextBox textBox_logger;
        private System.Windows.Forms.Button button_getLogs;
        private System.Windows.Forms.TextBox textBox_manualColor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown_factorB;
        private System.Windows.Forms.NumericUpDown numericUpDown_factorG;
        private System.Windows.Forms.NumericUpDown numericUpDown_factorR;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}

