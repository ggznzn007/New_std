using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_WF
{
    class Board
    {
        internal static Board GameBoard
        {
            get;
            private set;
        }
        static Board()
        {
            GameBoard = new Board();
        }
        Board()
        {

        }
        int[,] board = new int[GameRule.BX, GameRule.BY];
        internal int this[int x,int y]
        {
            get
            {
                return board[x, y];
            }
        }
        internal bool MoveEnable(int bn, int tn, int x, int y)
        {
            for(int xx = 0; xx<4;xx++)
            {
                for(int yy = 0; yy<4;yy++)
                {
                    if(BlockValue.bvals[bn,tn,xx,yy]!=0)
                    {
                        if(board[x+xx,y+yy]!=0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        internal void Store(int bn, int turn,int x,int y)//쌓기
        {
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (((x+xx)>=0)&&(x+xx<GameRule.BX)
                            &&(y+yy>=0)&&(y+yy<GameRule.BY))
                    {
                        board[x + xx, y + yy] += BlockValue.bvals[bn, turn, xx, yy];
                    }
                }
            }
        }
    }
}
