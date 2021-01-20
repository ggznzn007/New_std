
namespace _08_RadioButton
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
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbUK = new System.Windows.Forms.RadioButton();
            this.rbAus = new System.Windows.Forms.RadioButton();
            this.rbChina = new System.Windows.Forms.RadioButton();
            this.rbKorea = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbFemale = new System.Windows.Forms.RadioButton();
            this.rbMale = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("굴림", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button1.Location = new System.Drawing.Point(527, 254);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(233, 85);
            this.button1.TabIndex = 0;
            this.button1.Text = "제출";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbUK);
            this.groupBox1.Controls.Add(this.rbAus);
            this.groupBox1.Controls.Add(this.rbChina);
            this.groupBox1.Controls.Add(this.rbKorea);
            this.groupBox1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(31, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(374, 284);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "국적";
            // 
            // rbUK
            // 
            this.rbUK.AutoSize = true;
            this.rbUK.Location = new System.Drawing.Point(16, 199);
            this.rbUK.Name = "rbUK";
            this.rbUK.Size = new System.Drawing.Size(72, 24);
            this.rbUK.TabIndex = 3;
            this.rbUK.TabStop = true;
            this.rbUK.Text = "영국";
            this.rbUK.UseVisualStyleBackColor = true;
            // 
            // rbAus
            // 
            this.rbAus.AutoSize = true;
            this.rbAus.Location = new System.Drawing.Point(16, 145);
            this.rbAus.Name = "rbAus";
            this.rbAus.Size = new System.Drawing.Size(177, 24);
            this.rbAus.TabIndex = 2;
            this.rbAus.TabStop = true;
            this.rbAus.Text = "오스트레일리아";
            this.rbAus.UseVisualStyleBackColor = true;
            // 
            // rbChina
            // 
            this.rbChina.AutoSize = true;
            this.rbChina.Location = new System.Drawing.Point(16, 91);
            this.rbChina.Name = "rbChina";
            this.rbChina.Size = new System.Drawing.Size(72, 24);
            this.rbChina.TabIndex = 1;
            this.rbChina.TabStop = true;
            this.rbChina.Text = "중국";
            this.rbChina.UseVisualStyleBackColor = true;
            // 
            // rbKorea
            // 
            this.rbKorea.AutoSize = true;
            this.rbKorea.Location = new System.Drawing.Point(16, 35);
            this.rbKorea.Name = "rbKorea";
            this.rbKorea.Size = new System.Drawing.Size(114, 24);
            this.rbKorea.TabIndex = 0;
            this.rbKorea.TabStop = true;
            this.rbKorea.Text = "대한민국";
            this.rbKorea.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbFemale);
            this.groupBox2.Controls.Add(this.rbMale);
            this.groupBox2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox2.Location = new System.Drawing.Point(454, 57);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(306, 95);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "성별";
            // 
            // rbFemale
            // 
            this.rbFemale.AutoSize = true;
            this.rbFemale.Location = new System.Drawing.Point(160, 37);
            this.rbFemale.Name = "rbFemale";
            this.rbFemale.Size = new System.Drawing.Size(51, 24);
            this.rbFemale.TabIndex = 1;
            this.rbFemale.TabStop = true;
            this.rbFemale.Text = "여";
            this.rbFemale.UseVisualStyleBackColor = true;
            this.rbFemale.CheckedChanged += new System.EventHandler(this.rbFemale_CheckedChanged);
            // 
            // rbMale
            // 
            this.rbMale.AutoSize = true;
            this.rbMale.Location = new System.Drawing.Point(22, 37);
            this.rbMale.Name = "rbMale";
            this.rbMale.Size = new System.Drawing.Size(51, 24);
            this.rbMale.TabIndex = 0;
            this.rbMale.TabStop = true;
            this.rbMale.Text = "남";
            this.rbMale.UseVisualStyleBackColor = true;
            this.rbMale.CheckedChanged += new System.EventHandler(this.rbMale_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 367);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Radio Button";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbUK;
        private System.Windows.Forms.RadioButton rbAus;
        private System.Windows.Forms.RadioButton rbChina;
        private System.Windows.Forms.RadioButton rbKorea;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbFemale;
        private System.Windows.Forms.RadioButton rbMale;
    }
}

