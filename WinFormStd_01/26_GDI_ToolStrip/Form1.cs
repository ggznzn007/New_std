using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _26_GDI_ToolStrip
{
    enum DrawMode { LINE,RECTAGLE,CIRCLE,CURVED_LINE};
    public partial class Form1 : Form
    {
        private DrawMode drawMode;
        private Graphics g;
        private Pen pen = new Pen(Color.Black, 2);
        private Pen eraser;
        Point startP; // 시작점
        Point endP; // 끝점
        Point currP; // 현재 위치
        Point preP; // 이전 위치
        public Form1()
        {
            InitializeComponent();

            g = CreateGraphics();
            toolStatusStripLabel1.Text = "Line Mode";
            this.BackColor = Color.White;
            this.eraser = new Pen(this.BackColor, 2);
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            drawMode = DrawMode.LINE;
            toolStatusStripLabel1.Text = "Line Mode";
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            drawMode = DrawMode.RECTAGLE;
            toolStatusStripLabel1.Text = "Retangle Mode";
        }

        private void toolStripLabel3_Click(object sender, EventArgs e)
        {
            drawMode = DrawMode.CIRCLE;
            toolStatusStripLabel1.Text = "Circle Mode";
        }

        private void toolStripLabel4_Click(object sender, EventArgs e)
        {
            drawMode = DrawMode.CURVED_LINE;
            toolStatusStripLabel1.Text = "Curved_Line Mode";
        }

        private void toolStripLabel5_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
                pen.Color = colorDialog.Color;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            startP = new Point(e.X, e.Y);
            preP = startP;
            currP = startP;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            preP = currP;
            currP = new Point(e.X, e.Y);

            switch(drawMode)
            {
                case DrawMode.LINE:
                    g.DrawLine(eraser, startP, endP);
                    g.DrawLine(pen, startP, currP);
                    break;
                case DrawMode.RECTAGLE:
                    g.DrawRectangle(eraser, new Rectangle(startP,
                        new Size(preP.X - startP.X, preP.Y - startP.Y)));
                    g.DrawRectangle(pen, new Rectangle(startP,
                        new Size(currP.X - startP.X, currP.Y - startP.Y)));
                    break;
                case DrawMode.CIRCLE:
                    g.DrawEllipse(eraser, new Rectangle(startP,
                        new Size(preP.X - startP.X, currP.Y - startP.Y)));
                    g.DrawEllipse(pen, new Rectangle(startP,
                        new Size(currP.X - startP.X, currP.Y - startP.Y)));
                    break;
                case DrawMode.CURVED_LINE:
                    g.DrawLine(pen, preP, currP);
                    break;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            endP = new Point(e.X, e.Y);
            switch(drawMode)
            {
                case DrawMode.LINE:
                    g.DrawLine(pen, startP, endP);
                    break;
                case DrawMode.RECTAGLE:
                    g.DrawRectangle(pen, new Rectangle(startP,
                        new Size(endP.X - startP.X, endP.Y - startP.Y)));
                    break;
                case DrawMode.CIRCLE:
                    g.DrawEllipse(pen, new Rectangle(startP,
                        new Size(endP.X - startP.X, endP.Y - startP.Y)));
                    break;
                case DrawMode.CURVED_LINE:
                    break;
            }
        }
    }
}
