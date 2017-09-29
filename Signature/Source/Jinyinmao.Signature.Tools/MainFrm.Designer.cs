namespace Jinyinmao.Signature.Tools
{
    partial class MainFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.btnStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtProductId = new System.Windows.Forms.TextBox();
            this.txtOrderiD = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btn_YEM = new System.Windows.Forms.Button();
            this.txtRequest = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(517, 263);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(179, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "开始生成";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "产品号:";
            // 
            // txtProductId
            // 
            this.txtProductId.Location = new System.Drawing.Point(60, 15);
            this.txtProductId.Name = "txtProductId";
            this.txtProductId.Size = new System.Drawing.Size(321, 21);
            this.txtProductId.TabIndex = 2;
            // 
            // txtOrderiD
            // 
            this.txtOrderiD.Location = new System.Drawing.Point(60, 47);
            this.txtOrderiD.Name = "txtOrderiD";
            this.txtOrderiD.Size = new System.Drawing.Size(321, 21);
            this.txtOrderiD.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "订单号:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(5, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(723, 318);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.txtProductId);
            this.tabPage1.Controls.Add(this.btnStart);
            this.tabPage1.Controls.Add(this.txtOrderiD);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(715, 292);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "定期生成电子合同";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(11, 181);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(383, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "温馨提示:当订单号为空时,会生成该产品下所有订单对应的电子合同!!!";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btn_YEM);
            this.tabPage2.Controls.Add(this.txtRequest);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(715, 292);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "余额猫生成电子合同";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btn_YEM
            // 
            this.btn_YEM.Location = new System.Drawing.Point(526, 264);
            this.btn_YEM.Name = "btn_YEM";
            this.btn_YEM.Size = new System.Drawing.Size(169, 22);
            this.btn_YEM.TabIndex = 1;
            this.btn_YEM.Text = "开始生成";
            this.btn_YEM.UseVisualStyleBackColor = true;
            this.btn_YEM.Click += new System.EventHandler(this.btn_YEM_Click);
            // 
            // txtRequest
            // 
            this.txtRequest.Location = new System.Drawing.Point(3, 6);
            this.txtRequest.Multiline = true;
            this.txtRequest.Name = "txtRequest";
            this.txtRequest.Size = new System.Drawing.Size(692, 252);
            this.txtRequest.TabIndex = 0;
            this.txtRequest.Text = resources.GetString("txtRequest.Text");
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 328);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainFrm";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生成电子合同工具";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtProductId;
        private System.Windows.Forms.TextBox txtOrderiD;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_YEM;
        private System.Windows.Forms.TextBox txtRequest;
    }
}

