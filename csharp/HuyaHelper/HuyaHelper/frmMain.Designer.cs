namespace HuyaHelper
{
    partial class frmMain
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
            closing.Set(1);

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
            this.chatContent = new System.Windows.Forms.RichTextBox();
            this.txtBoxRoomId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblRoomId = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.cbBoxRoomId = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // chatContent
            // 
            this.chatContent.AcceptsTab = true;
            this.chatContent.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chatContent.Location = new System.Drawing.Point(12, 51);
            this.chatContent.Name = "chatContent";
            this.chatContent.ReadOnly = true;
            this.chatContent.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.chatContent.Size = new System.Drawing.Size(656, 524);
            this.chatContent.TabIndex = 0;
            this.chatContent.Text = "";
            // 
            // txtBoxRoomId
            // 
            this.txtBoxRoomId.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBoxRoomId.Location = new System.Drawing.Point(118, 17);
            this.txtBoxRoomId.Name = "txtBoxRoomId";
            this.txtBoxRoomId.Size = new System.Drawing.Size(100, 23);
            this.txtBoxRoomId.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            // 
            // lblRoomId
            // 
            this.lblRoomId.AutoSize = true;
            this.lblRoomId.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRoomId.Location = new System.Drawing.Point(23, 22);
            this.lblRoomId.Name = "lblRoomId";
            this.lblRoomId.Size = new System.Drawing.Size(91, 14);
            this.lblRoomId.TabIndex = 2;
            this.lblRoomId.Text = "虎牙房间号：";
            // 
            // btnLogin
            // 
            this.btnLogin.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLogin.Location = new System.Drawing.Point(373, 16);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // cbBoxRoomId
            // 
            this.cbBoxRoomId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBoxRoomId.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbBoxRoomId.FormattingEnabled = true;
            this.cbBoxRoomId.Location = new System.Drawing.Point(228, 17);
            this.cbBoxRoomId.Name = "cbBoxRoomId";
            this.cbBoxRoomId.Size = new System.Drawing.Size(137, 22);
            this.cbBoxRoomId.TabIndex = 4;
            this.cbBoxRoomId.SelectedIndexChanged += new System.EventHandler(this.cbBoxRoomId_SelectedIndexChanged);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 590);
            this.Controls.Add(this.cbBoxRoomId);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.lblRoomId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBoxRoomId);
            this.Controls.Add(this.chatContent);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HuyaHelper v1.0";
            this.Activated += new System.EventHandler(this.frmMain_Activated);
            this.Deactivate += new System.EventHandler(this.frmMain_Deactivate);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox chatContent;
        private System.Windows.Forms.TextBox txtBoxRoomId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblRoomId;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.ComboBox cbBoxRoomId;
    }
}

