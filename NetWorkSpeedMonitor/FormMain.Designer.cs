namespace NetWorkSpeedMonitor
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ListAdapters = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LabelDownloadValue = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LabelUploadValue = new System.Windows.Forms.Label();
            this.TimerCounter = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // ListAdapters
            // 
            this.ListAdapters.FormattingEnabled = true;
            this.ListAdapters.ItemHeight = 12;
            this.ListAdapters.Location = new System.Drawing.Point(26, 32);
            this.ListAdapters.Name = "ListAdapters";
            this.ListAdapters.Size = new System.Drawing.Size(300, 112);
            this.ListAdapters.TabIndex = 0;
            this.ListAdapters.SelectedIndexChanged += new System.EventHandler(this.ListAdapters_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 169);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Download Speed:";
            // 
            // LabelDownloadValue
            // 
            this.LabelDownloadValue.AutoSize = true;
            this.LabelDownloadValue.Location = new System.Drawing.Point(182, 169);
            this.LabelDownloadValue.Name = "LabelDownloadValue";
            this.LabelDownloadValue.Size = new System.Drawing.Size(41, 12);
            this.LabelDownloadValue.TabIndex = 2;
            this.LabelDownloadValue.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 204);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "Upload Speed:";
            // 
            // LabelUploadValue
            // 
            this.LabelUploadValue.AutoSize = true;
            this.LabelUploadValue.Location = new System.Drawing.Point(182, 204);
            this.LabelUploadValue.Name = "LabelUploadValue";
            this.LabelUploadValue.Size = new System.Drawing.Size(41, 12);
            this.LabelUploadValue.TabIndex = 4;
            this.LabelUploadValue.Text = "label4";
            // 
            // TimerCounter
            // 
            this.TimerCounter.Enabled = true;
            this.TimerCounter.Interval = 1000;
            this.TimerCounter.Tick += new System.EventHandler(this.TimerCounter_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 274);
            this.Controls.Add(this.LabelUploadValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LabelDownloadValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ListAdapters);
            this.Name = "FormMain";
            this.Text = "Network Monitor Demo";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox ListAdapters;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LabelDownloadValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label LabelUploadValue;
        private System.Windows.Forms.Timer TimerCounter;
    }
}

