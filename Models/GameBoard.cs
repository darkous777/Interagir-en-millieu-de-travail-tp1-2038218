namespace TicTacToe.Models;

public class GameBoard
{
    private readonly char[] _board = new char[9];

    public GameBoard()
    {
        Reset();
    }

    public char this[int index] => _board[index];

    public bool MakeMove(int index, char player)
    {
        if (index < 0 || index > 8 || _board[index] != ' ')
            return false;

        _board[index] = player;
        return true;
    }

    /// <summary>
    /// Returns 'X', 'O' if there's a winner, 'D' for draw, or ' ' if game is ongoing.
    /// </summary>
    public char CheckWinner()
    {
        int[][] lines =
        [
            [0, 1, 2], [3, 4, 5], [6, 7, 8], // rows
            [0, 3, 6], [1, 4, 7], [2, 5, 8], // cols
            [0, 4, 8], [2, 4, 6]              // diagonals
        ];

        foreach (var line in lines)
        {
            char a = _board[line[0]], b = _board[line[1]], c = _board[line[2]];
            if (a != ' ' && a == b && b == c)
                return a;
        }

        return IsFull() ? 'D' : ' ';
    }

    public bool IsFull()
    {
        foreach (char c in _board)
            if (c == ' ') return false;
        return true;
    }

    public void Reset()
    {
        for (int i = 0; i < 9; i++)
            _board[i] = ' ';
    }

    public char[] GetBoardCopy()
    {
        return (char[])_board.Clone();
    }
}
