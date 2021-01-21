
namespace _11_MaskedTextBox
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtPost = new System.Windows.Forms.MaskedTextBox();
            this.txtDate = new System.Windows.Forms.MaskedTextBox();
            this.txtId = new System.Windows.Forms.MaskedTextBox();
            this.txtPhone = new System.Windows.Forms.MaskedTextBox();
            this.txtAddr = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lDate = new System.Windows.Forms.Label();
            this.lPost = new System.Windows.Forms.Label();
            this.lAddr = new System.Windows.Forms.Label();
            this.lPhone = new System.Windows.Forms.Label();
            this.lId = new System.Windows.Forms.Label();
            this.lEmail = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtPost
            // 
            this.txtPost.Location = new System.Drawing.Point(153, 90);
            this.txtPost.Mask = "000-000";
            this.txtPost.Name = "txtPost";
            this.txtPost.Size = new System.Drawing.Size(239, 25);
            this.txtPost.TabIndex = 0;
            // 
            // txtDate
            // 
            this.txtDate.Location = new System.Drawing.Point(153, 40);
            this.txtDate.Mask = "0000-00-00";
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(239, 25);
            this.txtDate.TabIndex = 0;
            this.txtDate.ValidatingType = typeof(System.DateTime);
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(153, 238);
            this.txtId.Mask = "000000-0000000";
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(239, 25);
            this.txtId.TabIndex = 0;
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(153, 191);
            this.txtPhone.Mask = "000-9000-0000";
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(239, 25);
            this.txtPhone.TabIndex = 0;
            // 
            // txtAddr
            // 
            this.txtAddr.Location = new System.Drawing.Point(153, 137);
            this.txtAddr.Name = "txtAddr";
            this.txtAddr.Size = new System.Drawing.Size(583, 25);
            this.txtAddr.TabIndex = 1;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(153, 292);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(436, 25);
            this.txtEmail.TabIndex = 1;
            // 
            // lDate
            // 
            this.lDate.AutoSize = true;
            this.lDate.Location = new System.Drawing.Point(47, 40);
            this.lDate.Name = "lDate";
            this.lDate.Size = new System.Drawing.Size(52, 15);
            this.lDate.TabIndex = 2;
            this.lDate.Text = "입사일";
            // 
            // lPost
            // 
            this.lPost.AutoSize = true;
            this.lPost.Location = new System.Drawing.Point(47, 90);
            this.lPost.Name = "lPost";
            this.lPost.Size = new System.Drawing.Size(67, 15);
            this.lPost.TabIndex = 2;
            this.lPost.Text = "우편번호";
            // 
            // lAddr
            // 
            this.lAddr.AutoSize = true;
            this.lAddr.Location = new System.Drawing.Point(47, 137);
            this.lAddr.Name = "lAddr";
            this.lAddr.Size = new System.Drawing.Size(37, 15);
            this.lAddr.TabIndex = 2;
            this.lAddr.Text = "주소";
            // 
            // lPhone
            // 
            this.lPhone.AutoSize = true;
            this.lPhone.Location = new System.Drawing.Point(47, 191);
            this.lPhone.Name = "lPhone";
            this.lPhone.Size = new System.Drawing.Size(82, 15);
            this.lPhone.TabIndex = 2;
            this.lPhone.Text = "휴대폰번호";
            // 
            // lId
            // 
            this.lId.AutoSize = true;
            this.lId.Location = new System.Drawing.Point(47, 238);
            this.lId.Name = "lId";
            this.lId.Size = new System.Drawing.Size(97, 15);
            this.lId.TabIndex = 2;
            this.lId.Text = "주민등록번호";
            // 
            // lEmail
            // 
            this.lEmail.AutoSize = true;
            this.lEmail.Location = new System.Drawing.Point(47, 292);
            this.lEmail.Name = "lEmail";
            this.lEmail.Size = new System.Drawing.Size(52, 15);
            this.lEmail.TabIndex = 2;
            this.lEmail.Text = "이메일";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(327, 369);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(169, 47);
            this.button1.TabIndex = 3;
            this.button1.Text = "등록";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lEmail);
            this.Controls.Add(this.lId);
            this.Controls.Add(this.lPhone);
            this.Controls.Add(this.lAddr);
            this.Controls.Add(this.lPost);
            this.Controls.Add(this.lDate);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.txtAddr);
            this.Controls.Add(this.txtDate);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.txtPost);
            this.Name = "Form1";
            this.Text = "사원정보 등록";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox txtPost;
        private System.Windows.Forms.MaskedTextBox txtDate;
        private System.Windows.Forms.MaskedTextBox txtId;
        private System.Windows.Forms.MaskedTextBox txtPhone;
        private System.Windows.Forms.TextBox txtAddr;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lDate;
        private System.Windows.Forms.Label lPost;
        private System.Windows.Forms.Label lAddr;
        private System.Windows.Forms.Label lPhone;
        private System.Windows.Forms.Label lId;
        private System.Windows.Forms.Label lEmail;
        private System.Windows.Forms.Button button1;
    }
}

