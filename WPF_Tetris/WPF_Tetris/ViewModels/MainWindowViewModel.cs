﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPF_Tetris.Models;
using WPF_Tetris.Views;

namespace WPF_Tetris.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        int[,] _block_buf = new int[4, 4];
        int[,] _memory_pan = new int[21, 12];

        int level_interval = 1200;
        int next_block;

        int game_score;
        int game_line;
        int game_level;

        int double_bonus = 0;

        int block_kind;
        int status_x;
        int status_y;

        Random rnd = new Random(DateTime.Now.Millisecond);

        System.Windows.Threading.DispatcherTimer dispatcherTimer;

        BindableTwoDArray<int> _next_pan = new BindableTwoDArray<int>(4, 4);
        BindableTwoDArray<int> _block_pan = new BindableTwoDArray<int>(21, 12);


        public bool Is_gaming { get; set; } = false;

        public int Game_score { get => game_score; set { game_score = value; NotifyPropertyChanged("Game_score"); } }
        public int Game_line { get => game_line; set { game_line = value; NotifyPropertyChanged("Game_line"); } }
        public int Game_level { get => game_level; set { game_level = value; NotifyPropertyChanged("Game_level"); } }
        public BindableTwoDArray<int> Next_pan { get => _next_pan; set => _next_pan = value; }
        public BindableTwoDArray<int> Block_pan { get => _block_pan; set => _block_pan = value; }



        public MainWindowViewModel()
        {

            Init_game();
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
        }

        private void StartTimer()
        {
            level_interval = 1000 - ((game_level - 1) * 90);

            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, level_interval);
            dispatcherTimer.Start();
        }

        public void StopTimer()
        {
            dispatcherTimer.Stop();
        }


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (Is_gaming)
            {
                Block_down();

            }
            else
            {
                DrawCrash();
            }
        }

        private void Init_game()
        {
            block_kind = (int)rnd.Next(1, 8);
            next_block = (int)rnd.Next(1, 8);

            /*if (game_level == 1)
            {
                next_block = (int)rnd.Next(1, 9);
                block_kind = (int)rnd.Next(1, 9);
            }*/

            status_x = 4;
            status_y = -3;

            DrawBufBlock(block_kind, _block_buf);
            Game_level = 1;
            Game_line = 0;
            Game_score = 0;


            DrawPanBlock();
        }

        private void DrawPanBlock()
        {
            for (int i = 0; i < 21; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    if (j == 0 || j == 11 || i == 20)
                    {
                        _block_pan[i, j] = Constants.Block_wall;
                        _memory_pan[i, j] = Constants.Block_wall;
                    }
                    else
                    {
                        _block_pan[i, j] = Constants.Block_background;
                        _memory_pan[i, j] = Constants.Block_background;
                    }
                }
            }
            _block_pan.NotifyBlockChange();
        }

        private void DrawBufBlock(int block_kind, int[,] block_buf)
        {
            int i, j;
            for (i = 0; i < 4; i++) { for (j = 0; j < 4; j++) block_buf[i, j] = 0; }

            switch (block_kind)
            {
                case 1:
                    block_buf[2, 0] = block_buf[2, 1] = block_buf[2, 2] = block_buf[2, 3] = block_kind;
                    break;
                case 2:
                    block_buf[1, 1] = block_buf[1, 2] = block_buf[2, 1] = block_buf[2, 2] = block_kind;
                    break;
                case 3:
                    block_buf[1, 1] = block_buf[2, 0] = block_buf[2, 1] = block_buf[2, 2] = block_kind;
                    break;
                case 4:
                    block_buf[1, 2] = block_buf[2, 0] = block_buf[2, 1] = block_buf[2, 2] = block_kind;
                    break;
                case 5:
                    block_buf[1, 1] = block_buf[2, 1] = block_buf[2, 2] = block_buf[2, 3] = block_kind;
                    break;
                case 6:
                    block_buf[1, 1] = block_buf[1, 2] = block_buf[2, 0] = block_buf[2, 1] = block_kind;
                    break;
                case 7:
                    block_buf[1, 0] = block_buf[1, 1] = block_buf[2, 1] = block_buf[2, 2] = block_kind;
                    break;
                    /*case 8:
                        block_buf[3, 0] = block_buf[3, 1] = block_buf[3, 2] = block_buf[2, 2] = block_buf[1, 2] = block_kind;
                        break;
                    case 9:
                        block_buf[3, 0] = block_buf[3, 1] = block_buf[3, 2] = block_buf[3, 3] = 
                        block_buf[2, 0] = block_buf[2, 1] = block_buf[2, 2] = block_buf[2, 3]= block_kind;
                        break;*/
                    /*case 10:
                        block_buf[2, 0] = block_buf[2, 1] = block_buf[2, 2] = block_buf[2, 3] = block_buf[3, 3] = block_kind;
                        break;*/
            }
        }

        /*private void DrawBufBlock_1(int block_kind, int[,] block_buf)
        {
            int i, j;
            for (i = 0; i < 4; i++) { for (j = 0; j < 4; j++) block_buf[i, j] = 0; }

            switch (block_kind)
            {
                case 1:
                    block_buf[2, 0] = block_buf[2, 1] = block_buf[2, 2] = block_buf[2, 3] = block_kind;
                    break;
                case 2:
                    block_buf[1, 1] = block_buf[1, 2] = block_buf[2, 1] = block_buf[2, 2] = block_kind;
                    break;
                case 3:
                    block_buf[1, 1] = block_buf[2, 0] = block_buf[2, 1] = block_buf[2, 2] = block_kind;
                    break;
                case 4:
                    block_buf[1, 2] = block_buf[2, 0] = block_buf[2, 1] = block_buf[2, 2] = block_kind;
                    break;
                case 5:
                    block_buf[1, 1] = block_buf[2, 1] = block_buf[2, 2] = block_buf[2, 3] = block_kind;
                    break;
                case 6:
                    block_buf[1, 1] = block_buf[1, 2] = block_buf[2, 0] = block_buf[2, 1] = block_kind;
                    break;
                case 7:
                    block_buf[1, 0] = block_buf[1, 1] = block_buf[2, 1] = block_buf[2, 2] = block_kind;
                    break;
                case 8:
                    block_buf[1, 2] = block_buf[2, 1] = block_buf[2, 2] = block_buf[3, 1] = block_buf[3, 2]
                        = block_kind;
                    break;
                    *//*case 9:
                        block_buf[3, 0] = block_buf[3, 1] = block_buf[3, 2] = block_buf[3, 3] = 
                        block_buf[2, 0] = block_buf[2, 1] = block_buf[2, 2] = block_buf[2, 3]= block_kind;
                        break;*/
                    /*case 10:
                        block_buf[2, 0] = block_buf[2, 1] = block_buf[2, 2] = block_buf[2, 3] = block_buf[3, 3] = block_kind;
                        break;*//*
            }
        }*/

        bool Check_Can_Move(int direct)
        {
            int x = status_x;
            int y = status_y;

            switch (direct)
            {
                case Constants.TETRIT_MOVE_RIGHT:
                    x++;
                    break;
                case Constants.TETRIT_MOVE_LEFT:
                    x--;
                    break;
                case Constants.TETRIT_MOVE_DOWN:
                    y++;
                    break;
            }

            if (!Check_Crash(x, y, _block_buf))
                return false;
            return true;
        }

        private bool Check_Crash(int x, int y, int[,] block_buf)
        {
            int i, j;

            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < 4; j++)
                {
                    if (block_buf[i, j] != 0)
                    {
                        if ((x + j == 0 || x + j == 11) ||
                            (y + i >= 0 && x + j >= 0 && _memory_pan[y + i, x + j] != 0))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        void DrawCurrentBlock(int kind)
        {
            int i, j;

            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < 4; j++)
                {
                    if (_block_buf[i, j] != 0 && status_y + i >= 0)
                    {
                        _block_pan[status_y + i, status_x + j] = kind;
                    }
                }
            }
            _block_pan.NotifyBlockChange();
        }

        internal void EnterGame()
        {
            if (Is_gaming == false)
            {
                Init_game();

                Is_gaming = true;

                DrawNext();
                Block_down();
                StartTimer();
            }
            else
            {
                DrawCrash();
            }
        }

        private void DrawNext()
        {
            DrawBufBlock(next_block, _next_pan);
            /*if (game_level == 1)
            {
                DrawBufBlock_1(next_block, _next_pan);
                
            }*/
            _next_pan.NotifyBlockChange();
        }

        private async void DrawCrash()
        {
            await Task.Run(() =>
            {
                StopTimer();
                int[,] over = new int[21, 12]
                {
                    {-1,0,0,0,0,0,0,0,0,0,0,-1 },
                    {-1,0,1,0,1,0,0,0,0,0,0,-1 },
                    {-1,1,1,1,1,1,0,0,0,0,0,-1 },
                    {-1,0,1,1,1,0,0,0,0,0,0,-1 },
                    {-1,0,0,1,0,0,0,2,0,2,0,-1 },
                    {-1,0,0,0,0,0,2,2,2,2,2,-1 },
                    {-1,0,0,0,0,0,0,2,2,2,0,-1 },
                    {-1,0,3,0,3,0,0,0,2,0,0,-1 },
                    {-1,3,3,3,3,3,0,0,0,0,0,-1 },
                    {-1,0,3,3,3,0,0,0,0,0,0,-1 },
                    {-1,0,0,3,0,0,0,4,0,4,0,-1 },
                    {-1,0,0,0,0,0,4,4,4,4,4,-1 },
                    {-1,0,0,0,0,0,0,4,4,4,0,-1 },
                    {-1,0,5,0,5,0,0,0,4,0,0,-1 },
                    {-1,5,5,5,5,5,0,0,0,0,0,-1 },
                    {-1,0,5,5,5,0,0,0,0,0,0,-1 },
                    {-1,0,0,5,0,0,0,6,0,6,0,-1 },
                    {-1,0,0,0,0,0,6,6,6,6,6,-1 },
                    {-1,0,0,0,0,0,0,6,6,6,0,-1 },
                    {-1,0,0,0,0,0,0,0,6,0,0,-1 },
                    {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 }
                };
                for (int i = 0; i < 21; i++)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        _block_pan[i, j] = over[i, j];
                    }
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _block_pan.NotifyBlockChange();
                    });

                    System.Threading.Thread.Sleep(20);
                }
                Is_gaming = false;

            });
        }

        public void Block_down()
        {
            if (Check_Can_Move(Constants.TETRIT_MOVE_DOWN))
            {
                DrawCurrentBlock(Constants.Block_background);
                status_y++;
                DrawCurrentBlock(block_kind);
            }
            else
            {
                int i, j;

                for (i = 0; i < 4; i++) /*   .*/
                {
                    for (j = 0; j < 4; j++)
                    {
                        if (_block_buf[i, j] != 0 && status_y + i >= 0 && status_x + j >= 0)
                            _memory_pan[status_y + i, status_x + j] = block_kind;
                    }
                }

                Cal_Block();
                NewBlock();
            }
        }

        private void NewBlock()
        {
            block_kind = next_block;
            status_x = 4;
            status_y = -3;

            DrawBufBlock(block_kind, _block_buf);

            next_block = (int)rnd.Next(1, 8);
/*
            if (game_level == 1)
            {
                next_block = (int)rnd.Next(1, 9);
                //block_kind = (int)rnd.Next(1, 9);
                if (Check_Can_Move(Constants.TETRIT_MOVE_DOWN))
                {
                    DrawNext();
                    StopTimer();
                    Block_down();
                    StartTimer();
                }
                else
                {
                    DrawCrash();
                }
            }*/
            if (Check_Can_Move(Constants.TETRIT_MOVE_DOWN))
            {
                DrawNext();
                StopTimer();
                Block_down();
                StartTimer();
            }
            else
            {
                DrawCrash();
            }
        }

        private void Cal_Block()
        {
            int i, j, k;

            for (i = 19; i >= 0; i--)
            {
                int line_chk = 0;

                for (j = 1; j < 11; j++)
                {
                    if (_memory_pan[i, j] != 0) line_chk++;
                }

                if (line_chk == 10)
                {
                    double_bonus = double_bonus + 100 + ((game_level - 1) * 2);
                    Game_line++;
                    Game_score = game_score + double_bonus;

                    if ((game_line % 50) == 0)
                    {
                        Game_level++;
                    }

                    for (k = i - 1; k >= 0; k--)
                    {
                        for (j = 1; j < 11; j++) _memory_pan[k + 1, j] = _memory_pan[k, j];
                    }

                    for (k = 0; k < 20; k++)
                    {
                        for (j = 1; j < 11; j++) _block_pan[k, j] = _memory_pan[k, j];
                    }
                    _block_pan.NotifyBlockChange();

                    Cal_Block();
                }
            }

            if (double_bonus > 0)
            {
                double_bonus = 0;
            }
        }

        public void Block_drop()
        {
            DrawCurrentBlock(Constants.Block_background);
            while (Check_Can_Move(Constants.TETRIT_MOVE_DOWN))
            {
                status_y++;
            }
            DrawCurrentBlock(block_kind);
            Block_down();
        }
        public void BlockMoveLeft()
        {
            if (Check_Can_Move(Constants.TETRIT_MOVE_LEFT))
            {
                DrawCurrentBlock(Constants.Block_background);
                status_x--;
                DrawCurrentBlock(block_kind);
            }
        }

        public void BlockMoveRight()
        {
            if (Check_Can_Move(Constants.TETRIT_MOVE_RIGHT))
            {
                DrawCurrentBlock(Constants.Block_background);
                status_x++;
                DrawCurrentBlock(block_kind);
            }

        }

        public void BlockRotate()
        {
            int i, j;
            int[,] l_block = new int[4, 4];

            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < 4; j++)
                {
                    l_block[3 - j, i] = _block_buf[i, j];
                }
            }

            if (Check_Crash(status_x, status_y, l_block))
            {
                DrawCurrentBlock(Constants.Block_background);
                for (i = 0; i < 4; i++) { for (j = 0; j < 4; j++) _block_buf[i, j] = l_block[i, j]; }
                DrawCurrentBlock(block_kind);
            }
        }



        #region notifyproperty
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
