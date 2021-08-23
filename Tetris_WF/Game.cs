using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_WF
{
    class Game
    {
        Diagram now;
        Board gboard = Board.GameBoard;
        internal Point NowPosition
        {
            get
            {
                if (now == null)
                {
                    return new Point(0, 0);
                }
                return new Point(now.X, now.Y);
            }
        }
        internal int BlockNum
        {
            get
            {
                return now.BlockNum;
            }
        }
        internal int Turn
        {
            get
            {
                return now.Turn;
            }
        }
        #region 단일체
        internal static Game Singleton
        {
            get;
            private set;
        }
        internal int this[int x,int y]
        {
            get
            {
                return gboard[x, y];
            }
        }
        static Game()
        {
            Singleton = new Game();
        }
        Game()
        {
            now = new Diagram();
        }
        #endregion
        internal bool MoveLeft()
        {
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[now.BlockNum, Turn, xx, yy] != 0)
                    {
                        if (now.X + xx <= 0)
                        {
                            return false;
                        }
                    }
                }
            }
            now.MoveLeft();
            return true;
        }
        internal bool MoveRight()
        {
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[now.BlockNum, Turn, xx, yy] != 0)
                    {
                        if (now.X + xx + 1 >= GameRule.BX)
                        {
                            return false;
                        }
                    }
                }
            }
            now.MoveRight();
            return true;
        }
        internal bool MoveDown()
        {
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[now.BlockNum, Turn, xx, yy] != 0)
                    {
                        if (now.Y + yy >= GameRule.BY)
                        {
                            return false;
                        }
                    }
                }
            }
            now.MoveDown();
            return true;
        }
        internal bool MoveTurn()
        {
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[now.BlockNum, (Turn+1)%4, xx, yy] != 0)
                    {
                        if (((now.X+xx)<0)||((now.X+xx)>=GameRule.BX)||
                            ((now.Y+yy)>=GameRule.BY))
                        {
                            return false;
                        }
                    }
                }
            }
            now.MoveTurn();
            return true;
        }
        internal void Next()
        {
            now.Reset();
        }
    }
}
