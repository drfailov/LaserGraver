namespace LaserDrawerApp
{
    partial class PrinterWindow
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
            this.panelFunctions = new System.Windows.Forms.Panel();
            this.buttonTestLaser = new System.Windows.Forms.Button();
            this.buttonReadString = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxGotoY = new System.Windows.Forms.TextBox();
            this.textBoxGotoX = new System.Windows.Forms.TextBox();
            this.buttonGoto = new System.Windows.Forms.Button();
            this.buttonResetMotors = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonSetBurningTime = new System.Windows.Forms.Button();
            this.textBoxBurningTime = new System.Windows.Forms.TextBox();
            this.buttonGetLaserTime = new System.Windows.Forms.Button();
            this.buttonTestMulti = new System.Windows.Forms.Button();
            this.buttonTestPlatform = new System.Windows.Forms.Button();
            this.buttonLaserMotorTest = new System.Windows.Forms.Button();
            this.buttonLaserBackward = new System.Windows.Forms.Button();
            this.buttonLaserForward = new System.Windows.Forms.Button();
            this.buttonPlatformBackward = new System.Windows.Forms.Button();
            this.buttonPlatformForward = new System.Windows.Forms.Button();
            this.buttonAnalogs = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelConnector = new System.Windows.Forms.Panel();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.buttonSelectFile = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxComPorts = new System.Windows.Forms.ComboBox();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonPause = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.labelJob = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelResolution = new System.Windows.Forms.Label();
            this.buttonWriteText = new System.Windows.Forms.Button();
            this.panelFunctions.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelConnector.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelFunctions
            // 
            this.panelFunctions.AutoScroll = true;
            this.panelFunctions.Controls.Add(this.buttonTestLaser);
            this.panelFunctions.Controls.Add(this.buttonReadString);
            this.panelFunctions.Controls.Add(this.groupBox2);
            this.panelFunctions.Controls.Add(this.buttonResetMotors);
            this.panelFunctions.Controls.Add(this.groupBox1);
            this.panelFunctions.Controls.Add(this.buttonTestMulti);
            this.panelFunctions.Controls.Add(this.buttonTestPlatform);
            this.panelFunctions.Controls.Add(this.buttonLaserMotorTest);
            this.panelFunctions.Controls.Add(this.buttonLaserBackward);
            this.panelFunctions.Controls.Add(this.buttonLaserForward);
            this.panelFunctions.Controls.Add(this.buttonPlatformBackward);
            this.panelFunctions.Controls.Add(this.buttonPlatformForward);
            this.panelFunctions.Controls.Add(this.buttonAnalogs);
            this.panelFunctions.Controls.Add(this.label1);
            this.panelFunctions.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelFunctions.Location = new System.Drawing.Point(0, 107);
            this.panelFunctions.Name = "panelFunctions";
            this.panelFunctions.Size = new System.Drawing.Size(185, 450);
            this.panelFunctions.TabIndex = 0;
            // 
            // buttonTestLaser
            // 
            this.buttonTestLaser.Location = new System.Drawing.Point(20, 419);
            this.buttonTestLaser.Name = "buttonTestLaser";
            this.buttonTestLaser.Size = new System.Drawing.Size(132, 23);
            this.buttonTestLaser.TabIndex = 12;
            this.buttonTestLaser.Text = "Тест лазера";
            this.buttonTestLaser.UseVisualStyleBackColor = true;
            this.buttonTestLaser.Click += new System.EventHandler(this.buttonTestLaser_Click);
            // 
            // buttonReadString
            // 
            this.buttonReadString.Location = new System.Drawing.Point(20, 475);
            this.buttonReadString.Name = "buttonReadString";
            this.buttonReadString.Size = new System.Drawing.Size(132, 23);
            this.buttonReadString.TabIndex = 11;
            this.buttonReadString.Text = "Прочесть до Done";
            this.buttonReadString.UseVisualStyleBackColor = true;
            this.buttonReadString.Click += new System.EventHandler(this.buttonReadString_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxGotoY);
            this.groupBox2.Controls.Add(this.textBoxGotoX);
            this.groupBox2.Controls.Add(this.buttonGoto);
            this.groupBox2.Location = new System.Drawing.Point(13, 238);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(149, 88);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Переход к";
            // 
            // textBoxGotoY
            // 
            this.textBoxGotoY.Location = new System.Drawing.Point(70, 22);
            this.textBoxGotoY.Name = "textBoxGotoY";
            this.textBoxGotoY.Size = new System.Drawing.Size(47, 20);
            this.textBoxGotoY.TabIndex = 2;
            this.textBoxGotoY.Text = "20";
            // 
            // textBoxGotoX
            // 
            this.textBoxGotoX.Location = new System.Drawing.Point(17, 22);
            this.textBoxGotoX.Name = "textBoxGotoX";
            this.textBoxGotoX.Size = new System.Drawing.Size(47, 20);
            this.textBoxGotoX.TabIndex = 1;
            this.textBoxGotoX.Text = "10";
            // 
            // buttonGoto
            // 
            this.buttonGoto.Location = new System.Drawing.Point(17, 48);
            this.buttonGoto.Name = "buttonGoto";
            this.buttonGoto.Size = new System.Drawing.Size(113, 23);
            this.buttonGoto.TabIndex = 0;
            this.buttonGoto.Text = "Перейти";
            this.buttonGoto.UseVisualStyleBackColor = true;
            this.buttonGoto.Click += new System.EventHandler(this.buttonGoto_Click);
            // 
            // buttonResetMotors
            // 
            this.buttonResetMotors.Location = new System.Drawing.Point(19, 119);
            this.buttonResetMotors.Name = "buttonResetMotors";
            this.buttonResetMotors.Size = new System.Drawing.Size(132, 23);
            this.buttonResetMotors.TabIndex = 10;
            this.buttonResetMotors.Text = "Каретку в начало";
            this.buttonResetMotors.UseVisualStyleBackColor = true;
            this.buttonResetMotors.Click += new System.EventHandler(this.buttonResetMotors_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonSetBurningTime);
            this.groupBox1.Controls.Add(this.textBoxBurningTime);
            this.groupBox1.Controls.Add(this.buttonGetLaserTime);
            this.groupBox1.Location = new System.Drawing.Point(13, 148);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(149, 84);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Время обжигания";
            // 
            // buttonSetBurningTime
            // 
            this.buttonSetBurningTime.Location = new System.Drawing.Point(18, 48);
            this.buttonSetBurningTime.Name = "buttonSetBurningTime";
            this.buttonSetBurningTime.Size = new System.Drawing.Size(60, 23);
            this.buttonSetBurningTime.TabIndex = 2;
            this.buttonSetBurningTime.Text = "Задать";
            this.buttonSetBurningTime.UseVisualStyleBackColor = true;
            this.buttonSetBurningTime.Click += new System.EventHandler(this.buttonSetBurningTime_Click);
            // 
            // textBoxBurningTime
            // 
            this.textBoxBurningTime.Location = new System.Drawing.Point(84, 51);
            this.textBoxBurningTime.Name = "textBoxBurningTime";
            this.textBoxBurningTime.Size = new System.Drawing.Size(47, 20);
            this.textBoxBurningTime.TabIndex = 1;
            this.textBoxBurningTime.Text = "700";
            // 
            // buttonGetLaserTime
            // 
            this.buttonGetLaserTime.Location = new System.Drawing.Point(18, 22);
            this.buttonGetLaserTime.Name = "buttonGetLaserTime";
            this.buttonGetLaserTime.Size = new System.Drawing.Size(113, 23);
            this.buttonGetLaserTime.TabIndex = 0;
            this.buttonGetLaserTime.Text = "Получить";
            this.buttonGetLaserTime.UseVisualStyleBackColor = true;
            this.buttonGetLaserTime.Click += new System.EventHandler(this.buttonGetLaserTime_Click);
            // 
            // buttonTestMulti
            // 
            this.buttonTestMulti.Location = new System.Drawing.Point(20, 448);
            this.buttonTestMulti.Name = "buttonTestMulti";
            this.buttonTestMulti.Size = new System.Drawing.Size(132, 23);
            this.buttonTestMulti.TabIndex = 8;
            this.buttonTestMulti.Text = "Мульти тест";
            this.buttonTestMulti.UseVisualStyleBackColor = true;
            this.buttonTestMulti.Click += new System.EventHandler(this.buttonTestMulti_Click);
            // 
            // buttonTestPlatform
            // 
            this.buttonTestPlatform.Location = new System.Drawing.Point(19, 390);
            this.buttonTestPlatform.Name = "buttonTestPlatform";
            this.buttonTestPlatform.Size = new System.Drawing.Size(132, 23);
            this.buttonTestPlatform.TabIndex = 7;
            this.buttonTestPlatform.Text = "Тест платформы";
            this.buttonTestPlatform.UseVisualStyleBackColor = true;
            this.buttonTestPlatform.Click += new System.EventHandler(this.buttonTestPlatform_Click);
            // 
            // buttonLaserMotorTest
            // 
            this.buttonLaserMotorTest.Location = new System.Drawing.Point(19, 361);
            this.buttonLaserMotorTest.Name = "buttonLaserMotorTest";
            this.buttonLaserMotorTest.Size = new System.Drawing.Size(132, 23);
            this.buttonLaserMotorTest.TabIndex = 6;
            this.buttonLaserMotorTest.Text = "Тест мотора лазера";
            this.buttonLaserMotorTest.UseVisualStyleBackColor = true;
            this.buttonLaserMotorTest.Click += new System.EventHandler(this.buttonLaserMotorTest_Click);
            // 
            // buttonLaserBackward
            // 
            this.buttonLaserBackward.Location = new System.Drawing.Point(18, 90);
            this.buttonLaserBackward.Name = "buttonLaserBackward";
            this.buttonLaserBackward.Size = new System.Drawing.Size(132, 23);
            this.buttonLaserBackward.TabIndex = 5;
            this.buttonLaserBackward.Text = "Лазер назад";
            this.buttonLaserBackward.UseVisualStyleBackColor = true;
            this.buttonLaserBackward.Click += new System.EventHandler(this.buttonLaserBackward_Click);
            // 
            // buttonLaserForward
            // 
            this.buttonLaserForward.Location = new System.Drawing.Point(18, 61);
            this.buttonLaserForward.Name = "buttonLaserForward";
            this.buttonLaserForward.Size = new System.Drawing.Size(132, 23);
            this.buttonLaserForward.TabIndex = 4;
            this.buttonLaserForward.Text = "Лазер вперед";
            this.buttonLaserForward.UseVisualStyleBackColor = true;
            this.buttonLaserForward.Click += new System.EventHandler(this.buttonLaserForward_Click);
            // 
            // buttonPlatformBackward
            // 
            this.buttonPlatformBackward.Location = new System.Drawing.Point(18, 32);
            this.buttonPlatformBackward.Name = "buttonPlatformBackward";
            this.buttonPlatformBackward.Size = new System.Drawing.Size(132, 23);
            this.buttonPlatformBackward.TabIndex = 3;
            this.buttonPlatformBackward.Text = "Платформа назад";
            this.buttonPlatformBackward.UseVisualStyleBackColor = true;
            this.buttonPlatformBackward.Click += new System.EventHandler(this.buttonPlatformBackward_Click);
            // 
            // buttonPlatformForward
            // 
            this.buttonPlatformForward.Location = new System.Drawing.Point(20, 3);
            this.buttonPlatformForward.Name = "buttonPlatformForward";
            this.buttonPlatformForward.Size = new System.Drawing.Size(132, 23);
            this.buttonPlatformForward.TabIndex = 2;
            this.buttonPlatformForward.Text = "Платформа вперед";
            this.buttonPlatformForward.UseVisualStyleBackColor = true;
            this.buttonPlatformForward.Click += new System.EventHandler(this.buttonPlatformForward_Click);
            // 
            // buttonAnalogs
            // 
            this.buttonAnalogs.Location = new System.Drawing.Point(19, 332);
            this.buttonAnalogs.Name = "buttonAnalogs";
            this.buttonAnalogs.Size = new System.Drawing.Size(132, 23);
            this.buttonAnalogs.TabIndex = 1;
            this.buttonAnalogs.Text = "Значения аналогов";
            this.buttonAnalogs.UseVisualStyleBackColor = true;
            this.buttonAnalogs.Click += new System.EventHandler(this.buttonAnalogs_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 599);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Конец списка";
            // 
            // textBoxLog
            // 
            this.textBoxLog.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.textBoxLog.Dock = System.Windows.Forms.DockStyle.Right;
            this.textBoxLog.ForeColor = System.Drawing.SystemColors.Info;
            this.textBoxLog.Location = new System.Drawing.Point(737, 107);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(272, 450);
            this.textBoxLog.TabIndex = 1;
            this.textBoxLog.Text = "Программа запущена.";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panelConnector);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.labelJob);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1009, 107);
            this.panel2.TabIndex = 2;
            // 
            // panelConnector
            // 
            this.panelConnector.Controls.Add(this.buttonWriteText);
            this.panelConnector.Controls.Add(this.buttonPrint);
            this.panelConnector.Controls.Add(this.buttonSelectFile);
            this.panelConnector.Controls.Add(this.buttonConnect);
            this.panelConnector.Controls.Add(this.buttonDisconnect);
            this.panelConnector.Controls.Add(this.label2);
            this.panelConnector.Controls.Add(this.comboBoxComPorts);
            this.panelConnector.Controls.Add(this.buttonRefresh);
            this.panelConnector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelConnector.Location = new System.Drawing.Point(0, 33);
            this.panelConnector.Name = "panelConnector";
            this.panelConnector.Size = new System.Drawing.Size(805, 74);
            this.panelConnector.TabIndex = 6;
            // 
            // buttonPrint
            // 
            this.buttonPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonPrint.Location = new System.Drawing.Point(522, 14);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(127, 45);
            this.buttonPrint.TabIndex = 7;
            this.buttonPrint.Text = "Печать";
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.buttonPrint_Click);
            // 
            // buttonSelectFile
            // 
            this.buttonSelectFile.Location = new System.Drawing.Point(308, 17);
            this.buttonSelectFile.Name = "buttonSelectFile";
            this.buttonSelectFile.Size = new System.Drawing.Size(93, 41);
            this.buttonSelectFile.TabIndex = 6;
            this.buttonSelectFile.Text = "Выбрать файл изображения";
            this.buttonSelectFile.UseVisualStyleBackColor = true;
            this.buttonSelectFile.Click += new System.EventHandler(this.buttonSelectFile_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(65, 38);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(117, 29);
            this.buttonConnect.TabIndex = 4;
            this.buttonConnect.Text = "Подключить";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(203, 39);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(75, 23);
            this.buttonDisconnect.TabIndex = 5;
            this.buttonDisconnect.Text = "Отключить";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "COM:";
            // 
            // comboBoxComPorts
            // 
            this.comboBoxComPorts.FormattingEnabled = true;
            this.comboBoxComPorts.Location = new System.Drawing.Point(55, 10);
            this.comboBoxComPorts.Name = "comboBoxComPorts";
            this.comboBoxComPorts.Size = new System.Drawing.Size(142, 21);
            this.comboBoxComPorts.TabIndex = 1;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(203, 8);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonRefresh.TabIndex = 2;
            this.buttonRefresh.Text = "Обновить";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonPause);
            this.panel1.Controls.Add(this.buttonStop);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(805, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(204, 74);
            this.panel1.TabIndex = 6;
            // 
            // buttonPause
            // 
            this.buttonPause.Location = new System.Drawing.Point(6, 26);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(105, 26);
            this.buttonPause.TabIndex = 1;
            this.buttonPause.Text = "Приостановить";
            this.buttonPause.UseVisualStyleBackColor = true;
            this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(117, 18);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 38);
            this.buttonStop.TabIndex = 0;
            this.buttonStop.Text = "Стоп";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // labelJob
            // 
            this.labelJob.BackColor = System.Drawing.Color.LavenderBlush;
            this.labelJob.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelJob.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelJob.Location = new System.Drawing.Point(0, 0);
            this.labelJob.Name = "labelJob";
            this.labelJob.Size = new System.Drawing.Size(1009, 33);
            this.labelJob.TabIndex = 3;
            this.labelJob.Text = "Ожидание подключения";
            this.labelJob.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelStatus
            // 
            this.labelStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelStatus.Location = new System.Drawing.Point(185, 544);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(552, 13);
            this.labelStatus.TabIndex = 3;
            this.labelStatus.Text = "Готов.";
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBoxImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxImage.InitialImage = null;
            this.pictureBoxImage.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Size = new System.Drawing.Size(552, 406);
            this.pictureBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxImage.TabIndex = 4;
            this.pictureBoxImage.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pictureBoxImage);
            this.panel3.Controls.Add(this.labelResolution);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(185, 107);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(552, 437);
            this.panel3.TabIndex = 5;
            // 
            // labelResolution
            // 
            this.labelResolution.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.labelResolution.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelResolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelResolution.Location = new System.Drawing.Point(0, 406);
            this.labelResolution.Name = "labelResolution";
            this.labelResolution.Size = new System.Drawing.Size(552, 31);
            this.labelResolution.TabIndex = 0;
            this.labelResolution.Text = "? x ?";
            this.labelResolution.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonWriteText
            // 
            this.buttonWriteText.Location = new System.Drawing.Point(404, 17);
            this.buttonWriteText.Name = "buttonWriteText";
            this.buttonWriteText.Size = new System.Drawing.Size(93, 41);
            this.buttonWriteText.TabIndex = 8;
            this.buttonWriteText.Text = "Написать текст";
            this.buttonWriteText.UseVisualStyleBackColor = true;
            this.buttonWriteText.Click += new System.EventHandler(this.buttonWriteText_Click);
            // 
            // PrinterWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 557);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.panelFunctions);
            this.Controls.Add(this.panel2);
            this.Name = "PrinterWindow";
            this.Text = "Лазерная гравировка";
            this.Load += new System.EventHandler(this.PrinterWindow_Load);
            this.panelFunctions.ResumeLayout(false);
            this.panelFunctions.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panelConnector.ResumeLayout(false);
            this.panelConnector.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelFunctions;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.PictureBox pictureBoxImage;
        private System.Windows.Forms.ComboBox comboBoxComPorts;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Label labelJob;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonAnalogs;
        private System.Windows.Forms.Button buttonPlatformForward;
        private System.Windows.Forms.Button buttonPlatformBackward;
        private System.Windows.Forms.Panel panelConnector;
        private System.Windows.Forms.Button buttonLaserBackward;
        private System.Windows.Forms.Button buttonLaserForward;
        private System.Windows.Forms.Button buttonLaserMotorTest;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonTestPlatform;
        private System.Windows.Forms.Button buttonTestMulti;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonSetBurningTime;
        private System.Windows.Forms.TextBox textBoxBurningTime;
        private System.Windows.Forms.Button buttonGetLaserTime;
        private System.Windows.Forms.Button buttonResetMotors;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxGotoY;
        private System.Windows.Forms.TextBox textBoxGotoX;
        private System.Windows.Forms.Button buttonGoto;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labelResolution;
        private System.Windows.Forms.Button buttonPrint;
        private System.Windows.Forms.Button buttonSelectFile;
        private System.Windows.Forms.Button buttonReadString;
        private System.Windows.Forms.Button buttonPause;
        private System.Windows.Forms.Button buttonTestLaser;
        private System.Windows.Forms.Button buttonWriteText;
    }
}