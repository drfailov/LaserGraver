namespace LaserDrawerApp
{
    partial class ContinueForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDivider = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBoxPreview = new System.Windows.Forms.PictureBox();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.labelResolution = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBoxVerticalOptimisation = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxBeginFromLine = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxBurnTime = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonMakeBW = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelProgress = new System.Windows.Forms.Label();
            this.labelTimeRemain = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(309, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "1:";
            // 
            // textBoxDivider
            // 
            this.textBoxDivider.Location = new System.Drawing.Point(325, 6);
            this.textBoxDivider.Name = "textBoxDivider";
            this.textBoxDivider.Size = new System.Drawing.Size(39, 20);
            this.textBoxDivider.TabIndex = 1;
            this.textBoxDivider.Text = "10";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(288, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Использование разрешения (меньше - точнее, дольше)";
            // 
            // pictureBoxPreview
            // 
            this.pictureBoxPreview.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pictureBoxPreview.InitialImage = null;
            this.pictureBoxPreview.Location = new System.Drawing.Point(18, 15);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new System.Drawing.Size(356, 287);
            this.pictureBoxPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxPreview.TabIndex = 3;
            this.pictureBoxPreview.TabStop = false;
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(12, 25);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(95, 23);
            this.buttonGenerate.TabIndex = 4;
            this.buttonGenerate.Text = "Генерировать";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // labelResolution
            // 
            this.labelResolution.AutoSize = true;
            this.labelResolution.Location = new System.Drawing.Point(116, 30);
            this.labelResolution.Name = "labelResolution";
            this.labelResolution.Size = new System.Drawing.Size(24, 13);
            this.labelResolution.TabIndex = 5;
            this.labelResolution.Text = "0x0";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBoxVerticalOptimisation);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.textBoxBeginFromLine);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.textBoxBurnTime);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.labelResolution);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.buttonGenerate);
            this.panel1.Controls.Add(this.textBoxDivider);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(663, 117);
            this.panel1.TabIndex = 6;
            // 
            // checkBoxVerticalOptimisation
            // 
            this.checkBoxVerticalOptimisation.AutoSize = true;
            this.checkBoxVerticalOptimisation.Location = new System.Drawing.Point(186, 88);
            this.checkBoxVerticalOptimisation.Name = "checkBoxVerticalOptimisation";
            this.checkBoxVerticalOptimisation.Size = new System.Drawing.Size(168, 17);
            this.checkBoxVerticalOptimisation.TabIndex = 14;
            this.checkBoxVerticalOptimisation.Text = "Вертикальная оптимизация";
            this.checkBoxVerticalOptimisation.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(118, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "стоки";
            // 
            // textBoxBeginFromLine
            // 
            this.textBoxBeginFromLine.Location = new System.Drawing.Point(73, 85);
            this.textBoxBeginFromLine.Name = "textBoxBeginFromLine";
            this.textBoxBeginFromLine.Size = new System.Drawing.Size(39, 20);
            this.textBoxBeginFromLine.TabIndex = 12;
            this.textBoxBeginFromLine.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Начать с";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(146, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "mks.";
            // 
            // textBoxBurnTime
            // 
            this.textBoxBurnTime.Location = new System.Drawing.Point(101, 58);
            this.textBoxBurnTime.Name = "textBoxBurnTime";
            this.textBoxBurnTime.Size = new System.Drawing.Size(39, 20);
            this.textBoxBurnTime.TabIndex = 9;
            this.textBoxBurnTime.Text = "500";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Время обжига";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelTimeRemain);
            this.panel2.Controls.Add(this.labelProgress);
            this.panel2.Controls.Add(this.buttonMakeBW);
            this.panel2.Controls.Add(this.buttonStop);
            this.panel2.Controls.Add(this.buttonPrint);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(406, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(257, 117);
            this.panel2.TabIndex = 7;
            // 
            // buttonMakeBW
            // 
            this.buttonMakeBW.Location = new System.Drawing.Point(67, 51);
            this.buttonMakeBW.Name = "buttonMakeBW";
            this.buttonMakeBW.Size = new System.Drawing.Size(86, 23);
            this.buttonMakeBW.TabIndex = 14;
            this.buttonMakeBW.Text = "Сделать Ч\\Б";
            this.buttonMakeBW.UseVisualStyleBackColor = true;
            this.buttonMakeBW.Click += new System.EventHandler(this.buttonMakeBW_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonStop.Location = new System.Drawing.Point(159, 12);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(86, 33);
            this.buttonStop.TabIndex = 7;
            this.buttonStop.Text = "Стоп";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonPrint
            // 
            this.buttonPrint.Enabled = false;
            this.buttonPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonPrint.Location = new System.Drawing.Point(67, 12);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(86, 33);
            this.buttonPrint.TabIndex = 6;
            this.buttonPrint.Text = "Печать";
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.buttonPrint_Click);
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.Controls.Add(this.pictureBoxPreview);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 117);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(663, 452);
            this.panel3.TabIndex = 7;
            // 
            // labelProgress
            // 
            this.labelProgress.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labelProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelProgress.Location = new System.Drawing.Point(4, 12);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(57, 33);
            this.labelProgress.TabIndex = 15;
            this.labelProgress.Text = "0%";
            this.labelProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTimeRemain
            // 
            this.labelTimeRemain.AutoSize = true;
            this.labelTimeRemain.Location = new System.Drawing.Point(4, 98);
            this.labelTimeRemain.Name = "labelTimeRemain";
            this.labelTimeRemain.Size = new System.Drawing.Size(121, 13);
            this.labelTimeRemain.TabIndex = 16;
            this.labelTimeRemain.Text = "Осталось: неизвестно";
            // 
            // ContinueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 569);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "ContinueForm";
            this.Text = "Подготовка к печати";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDivider;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBoxPreview;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.Label labelResolution;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonPrint;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxBurnTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxBeginFromLine;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonMakeBW;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox checkBoxVerticalOptimisation;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.Label labelTimeRemain;
    }
}