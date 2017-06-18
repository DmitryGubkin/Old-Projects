

using System;

namespace TicTacToe
{
    [Flags]
    public enum MarkType
    {
        /// <summary>
        /// The cell hasn't cliked yet
        /// </summary>
        Free,
        /// <summary>
        /// The cell is a o
        /// </summary>
        Nought,
        /// <summary>
        /// The cell is a X
        /// </summary>
        Cross
    }
}
