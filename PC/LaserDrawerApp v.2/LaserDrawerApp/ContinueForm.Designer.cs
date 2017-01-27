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
            this.labelTimeRemain = new System.Windows.Forms.Label();
            this.labelProgress = new System.Windows.Forms.Label();
            this.buttonMakeBW = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(412, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "1:";
            // 
            // textBoxDivider
            // 
            this.textBoxDivider.Location = new System.Drawing.Point(433, 7);
            this.textBoxDivider.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxDivider.Name = "textBoxDivider";
            this.textBoxDivider.Size = new System.Drawing.Size(51, 22);
            this.textBoxDivider.TabIndex = 1;
            this.textBoxDivider.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 11);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(379, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Использование разрешения (меньше - точнее, дольше)";
            // 
            // pictureBoxPreview
            // 
            this.pictureBoxPreview.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pictureBoxPreview.InitialImage = null;
            this.pictureBoxPreview.Location = new System.Drawing.Point(24, 18);
            this.pictureBoxPreview.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new System.Drawing.Size(475, 353);
            this.pictureBoxPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxPreview.TabIndex = 3;
            this.pictureBoxPreview.TabStop = false;
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(16, 31);
            this.buttonGenerate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(127, 28);
            this.buttonGenerate.TabIndex = 4;
            this.buttonGenerate.Text = "Генерировать";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // labelResolution
            // 
            this.labelResolution.AutoSize = true;
            this.labelResolution.Location = new System.Drawing.Point(155, 37);
            this.labelResolution.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelResolution.Name = "labelResolution";
            this.labelResolution.Size = new System.Drawing.Size(30, 17);
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
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(884, 144);
            this.panel1.TabIndex = 6;
            // 
            // checkBoxVerticalOptimisation
            // 
            this.checkBoxVerticalOptimisation.AutoSize = true;
            this.checkBoxVerticalOptimisation.Location = new System.Drawing.Point(248, 108);
            this.checkBoxVerticalOptimisation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxVerticalOptimisation.Name = "checkBoxVerticalOptimisation";
            this.checkBoxVerticalOptimisation.Size = new System.Drawing.Size(215, 21);
            this.checkBoxVerticalOptimisation.TabIndex = 14;
            this.checkBoxVerticalOptimisation.Text = "Вертикальная оптимизация";
            this.checkBoxVerticalOptimisation.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(157, 108);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 17);
            this.label6.TabIndex = 13;
            this.label6.Text = "стоки";
            // 
            // textBoxBeginFromLine
            // 
            this.textBoxBeginFromLine.Location = new System.Drawing.Point(97, 105);
            this.textBoxBeginFromLine.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxBeginFromLine.Name = "textBoxBeginFromLine";
            this.textBoxBeginFromLine.Size = new System.Drawing.Size(51, 22);
            this.textBoxBeginFromLine.TabIndex = 12;
            this.textBoxBeginFromLine.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 108);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "Начать с";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(195, 75);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "ms.";
            // 
            // textBoxBurnTime
            // 
            this.textBoxBurnTime.Location = new System.Drawing.Point(135, 71);
            this.textBoxBurnTime.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxBurnTime.Name = "textBoxBurnTime";
            this.textBoxBurnTime.Size = new System.Drawing.Size(51, 22);
            this.textBoxBurnTime.TabIndex = 9;
            this.textBoxBurnTime.Text = "5";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 75);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 17);
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
            this.panel2.Location = new System.Drawing.Point(541, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(343, 144);
            this.panel2.TabIndex = 7;
            // 
            // labelTimeRemain
            // 
            this.labelTimeRemain.AutoSize = true;
            this.labelTimeRemain.Location = new System.Drawing.Point(5, 121);
            this.labelTimeRemain.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTimeRemain.Name = "labelTimeRemain";
            this.labelTimeRemain.Size = new System.Drawing.Size(155, 17);
            this.labelTimeRemain.TabIndex = 16;
            this.labelTimeRemain.Text = "Осталось: неизвестно";
            // 
            // labelProgress
            // 
            this.labelProgress.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labelProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelProgress.Location = new System.Drawing.Point(5, 15);
            this.labelProgress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(75, 40);
            this.labelProgress.TabIndex = 15;
            this.labelProgress.Text = "0%";
            this.labelProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonMakeBW
            // 
            this.buttonMakeBW.Location = new System.Drawing.Point(89, 63);
            this.buttonMakeBW.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonMakeBW.Name = "buttonMakeBW";
            this.buttonMakeBW.Size = new System.Drawing.Size(115, 28);
            this.buttonMakeBW.TabIndex = 14;
            this.buttonMakeBW.Text = "Сделать Ч\\Б";
            this.buttonMakeBW.UseVisualStyleBackColor = true;
            this.buttonMakeBW.Click += new System.EventHandler(this.buttonMakeBW_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonStop.Location = new System.Drawing.Point(212, 15);
            this.buttonStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(115, 41);
            this.buttonStop.TabIndex = 7;
            this.buttonStop.Text = "Стоп";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonPrint
            // 
            this.buttonPrint.Enabled = false;
            this.buttonPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonPrint.Location = new System.Drawing.Point(89, 15);
            this.buttonPrint.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(115, 41);
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
            this.panel3.Location = new System.Drawing.Point(0, 144);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(884, 556);
            this.panel3.TabIndex = 7;
            // 
            // ContinueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 700);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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