﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris_WF
{
    public partial class form : Form
    {
        Game game;
        
        int bx;
        int by;
        int bwidth;
        int bheight;
        
        public form()
        {
            InitializeComponent();
        }


        

        /*private Color[] Colors = { Color.Red, Color.Yellow, Color.Green, Color.Blue };
        private Color GetColor()
        {
            Random random = new Random();
            return Colors[random.Next(0, 4)];
        }*/

        private void Form1_Load(object sender, EventArgs e)
        {
            game = Game.Singleton;
            
            bx = GameRule.BX; // 보드 폭
            by = GameRule.BY; // 보드 높이
            bwidth = GameRule.B_WIDTH; // X좌표 1의 픽셀
            bheight = GameRule.B_HEIGHT; // Y좌표 1의 픽셀
            SetClientSizeCore(bx * bwidth, by * bheight);
            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DoubleBuffered = true;
            DrawBoard(e.Graphics);
            //DrawGraduation(e.Graphics);
            DrawDiagram(e.Graphics);
        }

        private void DrawBoard(Graphics graphics)
        {
            for (int xx = 0; xx < bx; xx++)
            {
                for (int yy = 0; yy < by; yy++)
                {
                    if (game[xx, yy] != 0)
                    {

                        Rectangle now_rt = new Rectangle(xx * bwidth + 2,
                            yy * bheight + 2, bwidth - 4, bheight - 4);
                        //graphics.DrawRectangle(Pens.Green, now_rt);
                        graphics.FillRectangle(Brushes.HotPink, now_rt);
                    }
                }
            }
        }

        private void DrawDiagram(Graphics graphics)
        {
            //Pen dpen = new Pen(Color.Red, 4);
            Point now = game.NowPosition;
            int bn = game.BlockNum;
            int tn = game.Turn;
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[bn, tn, xx, yy] != 0)
                    {
                        Rectangle now_rt = new Rectangle((now.X + xx) * bwidth + 2,
                            (now.Y + yy) * bheight + 2, bwidth - 4, bheight - 4);
                        graphics.FillRectangle(Brushes.DodgerBlue, now_rt);

                        //graphics.DrawRectangle(dpen, now_rt);
                    }
                }

            }
        }

        private void DrawGraduation(Graphics graphics)
        {
            //DrawHorizons(graphics);
            DrawVerticals(graphics);
        }

        private void DrawVerticals(Graphics graphics)
        {
            Point st = new Point();
            Point et = new Point();
            for (int cx = 0; cx < bx; cx++)
            {
                st.X = cx * bwidth;
                st.Y = 0;
                et.X = st.X;
                et.Y = by * bheight;
                graphics.DrawLine(Pens.LightPink, st, et);
            }
        }
        private void DrawHorizons(Graphics graphics)
        {
            Point st = new Point();
            Point et = new Point();
            for (int cy = 0; cy < by; cy++)
            {
                st.X = 0;
                st.Y = cy * bheight;
                et.X = bx * bwidth;
                et.Y = st.Y;
                graphics.DrawLine(Pens.LightPink, st, et);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right: MoveRight(); return;
                case Keys.Left: MoveLeft(); return;
                case Keys.Space: MoveSSDown(); return;
                case Keys.Up: MoveTurn(); return;
                case Keys.Down: MoveDown(); return;
            }
        }

        private void MoveSSDown()
        {
            while (game.MoveDown())
            {
                Region rg = MakeRegion(0, -1);
                Invalidate(rg);
            }
            EndingCheck();
        }

        private void MoveTurn()
        {
            if (game.MoveTurn())
            {
                Region rg = MakeRegion();
                Invalidate(rg);
            }
        }

        private void MoveDown()
        {
            if (game.MoveDown())
            {
                Region rg = MakeRegion(0, -1);
                Invalidate(rg);
            }
            else
            {
                EndingCheck();
            }
        }

        private void EndingCheck()
        {
            if (game.Next())
            {
                Invalidate();
            }
            else
            {
                timer_down.Enabled = false;
                DialogResult re = MessageBox.Show("다시 시작 하시겠습니까?", "GAME OVER", MessageBoxButtons.YesNo);
                if (re == DialogResult.Yes)
                {
                    game.Restart();
                    timer_down.Enabled = true;
                    Invalidate();
                }
                else
                {
                    Close();
                }
            }
        }
        private void MoveLeft()
        {
            if (game.MoveLeft())
            {
                Region rg = MakeRegion(1, 0);
                Invalidate(rg);
            }
        }

        private void MoveRight()
        {
            if (game.MoveRight())
            {
                Region rg = MakeRegion(-1, 0);
                Invalidate(rg);
            }
        }

        private Region MakeRegion(int cx, int cy)
        {
            Point now = game.NowPosition;
            int bn = game.BlockNum;
            int tn = game.Turn;

            Region region = new Region();
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[bn, tn, xx, yy] != 0)
                    {
                        Rectangle rect1 = new Rectangle((now.X + xx) * bwidth, (now.Y + yy) * bheight, bwidth, bheight);
                        Rectangle rect2 = new Rectangle((now.X + cx + xx) * bwidth, (now.Y + cy + yy) * bheight, bwidth, bheight);
                        Region rg1 = new Region(rect1);
                        Region rg2 = new Region(rect2);
                        region.Union(rg1);
                        region.Union(rg2);
                    }
                }
            }
            return region;
        }
        private Region MakeRegion()
        {
            Point now = game.NowPosition;
            int bn = game.BlockNum;
            int tn = game.Turn;
            int oldtn = (tn + 3) % 4;

            Region region = new Region();
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[bn, tn, xx, yy] != 0)
                    {
                        Rectangle rect1 = new Rectangle((now.X + xx) * bwidth, (now.Y + yy) * bheight, bwidth, bheight);
                        Region rg1 = new Region(rect1);
                        region.Union(rg1);
                    }
                    if (BlockValue.bvals[bn, oldtn, xx, yy] != 0)
                    {
                        Rectangle rect1 = new Rectangle((now.X + xx) * bwidth, (now.Y + yy) * bheight, bwidth, bheight);
                        Region rg1 = new Region(rect1);
                        region.Union(rg1);
                    }

                }
            }
            return region;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MoveDown();
        }
    }
}
