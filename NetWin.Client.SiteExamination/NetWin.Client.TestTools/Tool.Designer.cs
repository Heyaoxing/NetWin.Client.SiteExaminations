﻿namespace NetWin.Client.TestTools
{
    partial class Tool
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.ToolWb = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // ToolWb
            // 
            this.ToolWb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolWb.Location = new System.Drawing.Point(0, 0);
            this.ToolWb.MinimumSize = new System.Drawing.Size(20, 20);
            this.ToolWb.Name = "ToolWb";
            this.ToolWb.Size = new System.Drawing.Size(696, 541);
            this.ToolWb.TabIndex = 0;
            // 
            // Tool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 541);
            this.Controls.Add(this.ToolWb);
            this.Name = "Tool";
            this.Text = "网站体检系统测试工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Tool_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser ToolWb;
    }
}

