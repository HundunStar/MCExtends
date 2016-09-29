namespace MCExtendsServer
{
    partial class MCExtendsServer
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
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.ClientList = new DevExpress.XtraTab.XtraTabPage();
            this.XPConsole = new DevExpress.XtraTab.XtraTabPage();
            this.LbcMsg = new DevExpress.XtraEditors.ListBoxControl();
            this.BtnStart = new DevExpress.XtraEditors.SimpleButton();
            this.BtnStop = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.XPConsole.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LbcMsg)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Location = new System.Drawing.Point(15, 15);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.ClientList;
            this.xtraTabControl1.Size = new System.Drawing.Size(551, 436);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.XPConsole,
            this.ClientList});
            // 
            // ClientList
            // 
            this.ClientList.Name = "ClientList";
            this.ClientList.Size = new System.Drawing.Size(545, 407);
            this.ClientList.Text = "客户端列表";
            // 
            // XPConsole
            // 
            this.XPConsole.Controls.Add(this.LbcMsg);
            this.XPConsole.Name = "XPConsole";
            this.XPConsole.Size = new System.Drawing.Size(545, 407);
            this.XPConsole.Text = "控制台";
            // 
            // LbcMsg
            // 
            this.LbcMsg.Location = new System.Drawing.Point(3, 3);
            this.LbcMsg.Name = "LbcMsg";
            this.LbcMsg.Size = new System.Drawing.Size(539, 401);
            this.LbcMsg.TabIndex = 0;
            // 
            // BtnStart
            // 
            this.BtnStart.Appearance.Font = new System.Drawing.Font("微软雅黑", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnStart.Appearance.Options.UseFont = true;
            this.BtnStart.Location = new System.Drawing.Point(581, 39);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(145, 60);
            this.BtnStart.TabIndex = 2;
            this.BtnStart.Text = "启动";
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // BtnStop
            // 
            this.BtnStop.Appearance.Font = new System.Drawing.Font("微软雅黑", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnStop.Appearance.Options.UseFont = true;
            this.BtnStop.Location = new System.Drawing.Point(581, 383);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(145, 60);
            this.BtnStop.TabIndex = 3;
            this.BtnStop.Text = "停止";
            // 
            // MCExtendsServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 465);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.xtraTabControl1);
            this.Name = "MCExtendsServer";
            this.Text = "MC扩展接单系统服务器";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.XPConsole.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LbcMsg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage XPConsole;
        private DevExpress.XtraTab.XtraTabPage ClientList;
        private DevExpress.XtraEditors.ListBoxControl LbcMsg;
        private DevExpress.XtraEditors.SimpleButton BtnStart;
        private DevExpress.XtraEditors.SimpleButton BtnStop;
    }
}

