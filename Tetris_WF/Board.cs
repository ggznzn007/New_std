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

    }
}
