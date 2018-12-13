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
            this.SuspendLayout();
            // 
            // chatContent
            // 
            this.chatContent.AcceptsTab = true;
            this.chatContent.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chatContent.Location = new System.Drawing.Point(23, 21);
            this.chatContent.Name = "chatContent";
            this.chatContent.ReadOnly = true;
            this.chatContent.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.chatContent.Size = new System.Drawing.Size(656, 524);
            this.chatContent.TabIndex = 0;
            this.chatContent.Text = "";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 570);
            this.Controls.Add(this.chatContent);
            this.Name = "frmMain";
            this.Text = "HuyaHelper v1.0";
            this.Activated += new System.EventHandler(this.frmMain_Activated);
            this.Deactivate += new System.EventHandler(this.frmMain_Deactivate);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox chatContent;
    }
}

