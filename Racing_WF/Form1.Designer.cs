
namespace Racing_WF
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label_gameOver = new System.Windows.Forms.Label();
            this.pictureBox_coin1 = new System.Windows.Forms.PictureBox();
            this.pictureBox_Block2 = new System.Windows.Forms.PictureBox();
            this.pictureBox_Block1 = new System.Windows.Forms.PictureBox();
            this.pictureBox_Car = new System.Windows.Forms.PictureBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox_coin2 = new System.Windows.Forms.PictureBox();
            this.pictureBox_coin3 = new System.Windows.Forms.PictureBox();
            this.label_coins = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_coin1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Block2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Block1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Car)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_coin2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_coin3)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label_gameOver
            // 
            this.label_gameOver.AutoSize = true;
            this.label_gameOver.BackColor = System.Drawing.Color.Indigo;
            this.label_gameOver.Font = new System.Drawing.Font("맑은 고딕", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_gameOver.ForeColor = System.Drawing.Color.GreenYellow;
            this.label_gameOver.Location = new System.Drawing.Point(50, 225);
            this.label_gameOver.Name = "label_gameOver";
            this.label_gameOver.Size = new System.Drawing.Size(389, 81);
            this.label_gameOver.TabIndex = 3;
            this.label_gameOver.Text = "GAME OVER";
            this.label_gameOver.Visible = false;
            // 
            // pictureBox_coin1
            // 
            this.pictureBox_coin1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox_coin1.Image = global::Racing_WF.Properties.Resources.gold2;
            this.pictureBox_coin1.Location = new System.Drawing.Point(37, 103);
            this.pictureBox_coin1.Name = "pictureBox_coin1";
            this.pictureBox_coin1.Size = new System.Drawing.Size(57, 57);
            this.pictureBox_coin1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_coin1.TabIndex = 4;
            this.pictureBox_coin1.TabStop = false;
            // 
            // pictureBox_Block2
            // 
            this.pictureBox_Block2.Image = global::Racing_WF.Properties.Resources.car02;
            this.pictureBox_Block2.Location = new System.Drawing.Point(378, 340);
            this.pictureBox_Block2.Name = "pictureBox_Block2";
            this.pictureBox_Block2.Size = new System.Drawing.Size(61, 102);
            this.pictureBox_Block2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Block2.TabIndex = 2;
            this.pictureBox_Block2.TabStop = false;
            // 
            // pictureBox_Block1
            // 
            this.pictureBox_Block1.Image = global::Racing_WF.Properties.Resources.car02;
            this.pictureBox_Block1.Location = new System.Drawing.Point(26, 383);
            this.pictureBox_Block1.Name = "pictureBox_Block1";
            this.pictureBox_Block1.Size = new System.Drawing.Size(56, 100);
            this.pictureBox_Block1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Block1.TabIndex = 2;
            this.pictureBox_Block1.TabStop = false;
            // 
            // pictureBox_Car
            // 
            this.pictureBox_Car.Image = global::Racing_WF.Properties.Resources.car012;
            this.pictureBox_Car.Location = new System.Drawing.Point(219, 434);
            this.pictureBox_Car.Name = "pictureBox_Car";
            this.pictureBox_Car.Size = new System.Drawing.Size(66, 114);
            this.pictureBox_Car.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Car.TabIndex = 1;
            this.pictureBox_Car.TabStop = false;
            // 
            // pictureBox6
            // 
            this.pictureBox6.BackColor = System.Drawing.Color.White;
            this.pictureBox6.Location = new System.Drawing.Point(-2, -3);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(10, 658);
            this.pictureBox6.TabIndex = 0;
            this.pictureBox6.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.BackColor = System.Drawing.Color.White;
            this.pictureBox5.Location = new System.Drawing.Point(475, -14);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(10, 670);
            this.pictureBox5.TabIndex = 0;
            this.pictureBox5.TabStop = false;
            // 
            // pictureBox7
            // 
            this.pictureBox7.BackColor = System.Drawing.Color.White;
            this.pictureBox7.Location = new System.Drawing.Point(243, 554);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(12, 127);
            this.pictureBox7.TabIndex = 0;
            this.pictureBox7.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.White;
            this.pictureBox4.Location = new System.Drawing.Point(243, 400);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(12, 127);
            this.pictureBox4.TabIndex = 0;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.White;
            this.pictureBox3.Location = new System.Drawing.Point(243, 240);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(12, 127);
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.White;
            this.pictureBox2.Location = new System.Drawing.Point(243, 86);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(12, 127);
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(243, -64);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(12, 127);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox_coin2
            // 
            this.pictureBox_coin2.Image = global::Racing_WF.Properties.Resources.gold2;
            this.pictureBox_coin2.Location = new System.Drawing.Point(270, 27);
            this.pictureBox_coin2.Name = "pictureBox_coin2";
            this.pictureBox_coin2.Size = new System.Drawing.Size(57, 57);
            this.pictureBox_coin2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_coin2.TabIndex = 4;
            this.pictureBox_coin2.TabStop = false;
            // 
            // pictureBox_coin3
            // 
            this.pictureBox_coin3.Image = global::Racing_WF.Properties.Resources.gold2;
            this.pictureBox_coin3.Location = new System.Drawing.Point(365, 121);
            this.pictureBox_coin3.Name = "pictureBox_coin3";
            this.pictureBox_coin3.Size = new System.Drawing.Size(57, 57);
            this.pictureBox_coin3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_coin3.TabIndex = 4;
            this.pictureBox_coin3.TabStop = false;
            // 
            // label_coins
            // 
            this.label_coins.AutoSize = true;
            this.label_coins.Font = new System.Drawing.Font("맑은 고딕", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_coins.ForeColor = System.Drawing.Color.LavenderBlush;
            this.label_coins.Location = new System.Drawing.Point(23, 18);
            this.label_coins.Name = "label_coins";
            this.label_coins.Size = new System.Drawing.Size(101, 25);
            this.label_coins.TabIndex = 5;
            this.label_coins.Text = "Coins = 0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(482, 653);
            this.Controls.Add(this.label_coins);
            this.Controls.Add(this.pictureBox_coin3);
            this.Controls.Add(this.pictureBox_coin2);
            this.Controls.Add(this.pictureBox_coin1);
            this.Controls.Add(this.label_gameOver);
            this.Controls.Add(this.pictureBox_Block2);
            this.Controls.Add(this.pictureBox_Block1);
            this.Controls.Add(this.pictureBox_Car);
            this.Controls.Add(this.pictureBox6);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.pictureBox7);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "비트코인 레이스";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_coin1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Block2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Block1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Car)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_coin2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_coin3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pictureBox_Car;
        private System.Windows.Forms.PictureBox pictureBox_Block1;
        private System.Windows.Forms.PictureBox pictureBox_Block2;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.Label label_gameOver;
        private System.Windows.Forms.PictureBox pictureBox_coin1;
        private System.Windows.Forms.PictureBox pictureBox_coin2;
        private System.Windows.Forms.PictureBox pictureBox_coin3;
        private System.Windows.Forms.Label label_coins;
    }
}

