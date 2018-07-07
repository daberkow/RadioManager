namespace RadioManager
{
    partial class RadioManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RadioManager));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Current Playlist");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Next Playlist");
            this.Player = new AxWMPLib.AxWindowsMediaPlayer();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.setable_arduino_stat = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.Updates_setable = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.sd_card_setable = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.setable_volume = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.setable_station = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.setable_station_style = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.SD_Select = new System.Windows.Forms.ComboBox();
            this.COM_Select = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Player)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // Player
            // 
            this.Player.Enabled = true;
            this.Player.Location = new System.Drawing.Point(12, 373);
            this.Player.Name = "Player";
            this.Player.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("Player.OcxState")));
            this.Player.Size = new System.Drawing.Size(454, 77);
            this.Player.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.label1.Location = new System.Drawing.Point(12, 355);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Media Player";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.setable_arduino_stat);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.Updates_setable);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.sd_card_setable);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(472, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 108);
            this.panel1.TabIndex = 3;
            // 
            // setable_arduino_stat
            // 
            this.setable_arduino_stat.AutoSize = true;
            this.setable_arduino_stat.Location = new System.Drawing.Point(124, 39);
            this.setable_arduino_stat.Name = "setable_arduino_stat";
            this.setable_arduino_stat.Size = new System.Drawing.Size(80, 13);
            this.setable_arduino_stat.TabIndex = 10;
            this.setable_arduino_stat.Text = "NOT_LOADED";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(25, 39);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(101, 13);
            this.label18.TabIndex = 9;
            this.label18.Text = "Arduino Connected:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(105, 87);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(37, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "Offline";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 87);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Server Version: ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(105, 74);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(28, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "0.03";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Local Version: ";
            // 
            // Updates_setable
            // 
            this.Updates_setable.AutoSize = true;
            this.Updates_setable.Location = new System.Drawing.Point(124, 26);
            this.Updates_setable.Name = "Updates_setable";
            this.Updates_setable.Size = new System.Drawing.Size(33, 13);
            this.Updates_setable.TabIndex = 4;
            this.Updates_setable.Text = "None";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Updates: ";
            // 
            // sd_card_setable
            // 
            this.sd_card_setable.AutoSize = true;
            this.sd_card_setable.Location = new System.Drawing.Point(124, 13);
            this.sd_card_setable.Name = "sd_card_setable";
            this.sd_card_setable.Size = new System.Drawing.Size(80, 13);
            this.sd_card_setable.TabIndex = 2;
            this.sd_card_setable.Text = "NOT_LOADED";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "\tCard Loaded: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "System Status";
            // 
            // treeView1
            // 
            this.treeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.Location = new System.Drawing.Point(15, 31);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "Current_Playlist";
            treeNode1.Text = "Current Playlist";
            treeNode2.Name = "Next_Play";
            treeNode2.Text = "Next Playlist";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            this.treeView1.Size = new System.Drawing.Size(451, 321);
            this.treeView1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.setable_volume);
            this.panel2.Controls.Add(this.label16);
            this.panel2.Controls.Add(this.setable_station);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.setable_station_style);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Location = new System.Drawing.Point(472, 291);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 61);
            this.panel2.TabIndex = 5;
            // 
            // setable_volume
            // 
            this.setable_volume.AutoSize = true;
            this.setable_volume.Location = new System.Drawing.Point(108, 39);
            this.setable_volume.Name = "setable_volume";
            this.setable_volume.Size = new System.Drawing.Size(13, 13);
            this.setable_volume.TabIndex = 6;
            this.setable_volume.Text = "0";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(27, 39);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(45, 13);
            this.label16.TabIndex = 5;
            this.label16.Text = "Volume:";
            // 
            // setable_station
            // 
            this.setable_station.AutoSize = true;
            this.setable_station.Location = new System.Drawing.Point(108, 26);
            this.setable_station.Name = "setable_station";
            this.setable_station.Size = new System.Drawing.Size(13, 13);
            this.setable_station.TabIndex = 4;
            this.setable_station.Text = "0";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(25, 26);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(46, 13);
            this.label14.TabIndex = 3;
            this.label14.Text = "Station: ";
            // 
            // setable_station_style
            // 
            this.setable_station_style.AutoSize = true;
            this.setable_station_style.Location = new System.Drawing.Point(108, 13);
            this.setable_station_style.Name = "setable_station_style";
            this.setable_station_style.Size = new System.Drawing.Size(73, 13);
            this.setable_station_style.TabIndex = 2;
            this.setable_station_style.Text = "NO_LOADED";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(25, 13);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "AM/FM/AMC: ";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Knob Status";
            // 
            // SD_Select
            // 
            this.SD_Select.FormattingEnabled = true;
            this.SD_Select.Items.AddRange(new object[] {
            "A:",
            "B:",
            "D:",
            "E:",
            "F:",
            "G:",
            "H:",
            "I:"});
            this.SD_Select.Location = new System.Drawing.Point(473, 168);
            this.SD_Select.Name = "SD_Select";
            this.SD_Select.Size = new System.Drawing.Size(77, 21);
            this.SD_Select.TabIndex = 6;
            this.SD_Select.SelectedIndexChanged += new System.EventHandler(this.SD_Select_SelectedIndexChanged);
            // 
            // COM_Select
            // 
            this.COM_Select.FormattingEnabled = true;
            this.COM_Select.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8"});
            this.COM_Select.Location = new System.Drawing.Point(600, 168);
            this.COM_Select.Name = "COM_Select";
            this.COM_Select.Size = new System.Drawing.Size(72, 21);
            this.COM_Select.TabIndex = 7;
            this.COM_Select.SelectedValueChanged += new System.EventHandler(this.COM_Select_SelectedValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(473, 146);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "SD Card Drive:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(597, 146);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 13);
            this.label13.TabIndex = 9;
            this.label13.Text = "COM Port:";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.textBox1);
            this.panel3.Controls.Add(this.label15);
            this.panel3.Location = new System.Drawing.Point(473, 195);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(199, 90);
            this.panel3.TabIndex = 10;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 16);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(188, 69);
            this.textBox1.TabIndex = 1;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(37, 13);
            this.label15.TabIndex = 0;
            this.label15.Text = "Errors:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(450, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(103, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Create Playlist";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(556, 168);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(27, 21);
            this.button2.TabIndex = 13;
            this.button2.Text = "Scan";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(96, 5);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(346, 20);
            this.textBox2.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Music Directory";
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(559, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(113, 23);
            this.button3.TabIndex = 16;
            this.button3.Text = "Make Folder Structor at Location";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(473, 373);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(199, 23);
            this.button4.TabIndex = 17;
            this.button4.Text = "Write Settings File";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // RadioManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 462);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.COM_Select);
            this.Controls.Add(this.SD_Select);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Player);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "RadioManager";
            this.Text = "Radio Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RadioManager_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.Player)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxWMPLib.AxWindowsMediaPlayer Player;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label setable_arduino_stat;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label Updates_setable;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label sd_card_setable;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label setable_volume;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label setable_station;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label setable_station_style;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox SD_Select;
        private System.Windows.Forms.ComboBox COM_Select;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}

