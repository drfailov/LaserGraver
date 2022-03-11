namespace LaserDrawerApp
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBoxCOMs = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.labelReceived = new System.Windows.Forms.Label();
            this.textBoxToSend = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.buttonReload = new System.Windows.Forms.Button();
            this.labelJob = new System.Windows.Forms.Label();
            this.buttonResolution = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonBMP = new System.Windows.Forms.Button();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxCOMs
            // 
            this.comboBoxCOMs.FormattingEnabled = true;
            this.comboBoxCOMs.Location = new System.Drawing.Point(52, 12);
            this.comboBoxCOMs.Name = "comboBoxCOMs";
            this.comboBoxCOMs.Size = new System.Drawing.Size(121, 21);
            this.comboBoxCOMs.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(179, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Load COM list";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "COM:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(179, 39);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(104, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Connect";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // labelReceived
            // 
            this.labelReceived.Location = new System.Drawing.Point(315, 75);
            this.labelReceived.Name = "labelReceived";
            this.labelReceived.Size = new System.Drawing.Size(475, 115);
            this.labelReceived.TabIndex = 4;
            this.labelReceived.Text = "Received";
            // 
            // textBoxToSend
            // 
            this.textBoxToSend.Location = new System.Drawing.Point(12, 72);
            this.textBoxToSend.Name = "textBoxToSend";
            this.textBoxToSend.Size = new System.Drawing.Size(161, 20);
            this.textBoxToSend.TabIndex = 5;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(179, 69);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(104, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "Send";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // buttonReload
            // 
            this.buttonReload.Location = new System.Drawing.Point(179, 98);
            this.buttonReload.Name = "buttonReload";
            this.buttonReload.Size = new System.Drawing.Size(104, 23);
            this.buttonReload.TabIndex = 7;
            this.buttonReload.Text = "Reload";
            this.buttonReload.UseVisualStyleBackColor = true;
            this.buttonReload.Click += new System.EventHandler(this.buttonReload_Click);
            // 
            // labelJob
            // 
            this.labelJob.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelJob.Location = new System.Drawing.Point(313, 39);
            this.labelJob.Name = "labelJob";
            this.labelJob.Size = new System.Drawing.Size(484, 30);
            this.labelJob.TabIndex = 8;
            this.labelJob.Text = "Задача";
            // 
            // buttonResolution
            // 
            this.buttonResolution.Location = new System.Drawing.Point(179, 127);
            this.buttonResolution.Name = "buttonResolution";
            this.buttonResolution.Size = new System.Drawing.Size(104, 23);
            this.buttonResolution.TabIndex = 9;
            this.buttonResolution.Text = "Resolution";
            this.buttonResolution.UseVisualStyleBackColor = true;
            this.buttonResolution.Click += new System.EventHandler(this.buttonResolution_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox1.Location = new System.Drawing.Point(17, 196);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // buttonBMP
            // 
            this.buttonBMP.Location = new System.Drawing.Point(15, 110);
            this.buttonBMP.Name = "buttonBMP";
            this.buttonBMP.Size = new System.Drawing.Size(104, 23);
            this.buttonBMP.TabIndex = 11;
            this.buttonBMP.Text = "Open BMP";
            this.buttonBMP.UseVisualStyleBackColor = true;
            this.buttonBMP.Click += new System.EventHandler(this.buttonBMP_Click);
            // 
            // buttonPrint
            // 
            this.buttonPrint.Location = new System.Drawing.Point(15, 139);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(74, 39);
            this.buttonPrint.TabIndex = 12;
            this.buttonPrint.Text = "Print";
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.buttonPrint_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(95, 158);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(64, 20);
            this.buttonStop.TabIndex = 13;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(179, 158);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(104, 23);
            this.button4.TabIndex = 14;
            this.button4.Text = "TestImage";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 502);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonPrint);
            this.Controls.Add(this.buttonBMP);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonResolution);
            this.Controls.Add(this.labelJob);
            this.Controls.Add(this.buttonReload);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBoxToSend);
            this.Controls.Add(this.labelReceived);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBoxCOMs);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxCOMs;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label labelReceived;
        private System.Windows.Forms.TextBox textBoxToSend;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button buttonReload;
        private System.Windows.Forms.Label labelJob;
        private System.Windows.Forms.Button buttonResolution;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonBMP;
        private System.Windows.Forms.Button buttonPrint;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button button4;
    }
}

