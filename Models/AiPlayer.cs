using System;

namespace TicTacToe.Models;

public static class AiPlayer
{
    public static int GetBestMove(char[] board, char aiPlayer)
    {
        char human = aiPlayer == 'X' ? 'O' : 'X';
        int bestScore = int.MinValue;
        int bestMove = -1;

        for (int i = 0; i < 9; i++)
        {
            if (board[i] != ' ') continue;

            board[i] = aiPlayer;
            int score = Minimax(board, false, aiPlayer, human);
            board[i] = ' ';

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = i;
            }
        }

        return bestMove;
    }

    private static int Minimax(char[] board, bool isMaximizing, char aiPlayer, char human)
    {
        char winner = EvaluateWinner(board);

        if (winner == aiPlayer) return 10;
        if (winner == human) return -10;
        if (IsFull(board)) return 0;

        if (isMaximizing)
        {
            int best = int.MinValue;
            for (int i = 0; i < 9; i++)
            {
                if (board[i] != ' ') continue;
                board[i] = aiPlayer;
                best = Math.Max(best, Minimax(board, false, aiPlayer, human));
                board[i] = ' ';
            }
            return best;
        }
        else
        {
            int best = int.MaxValue;
            for (int i = 0; i < 9; i++)
            {
                if (board[i] != ' ') continue;
                board[i] = human;
                best = Math.Min(best, Minimax(board, true, aiPlayer, human));
                board[i] = ' ';
            }
            return best;
        }
    }

    private static char EvaluateWinner(char[] board)
    {
        int[][] lines =
        [
            [0, 1, 2], [3, 4, 5], [6, 7, 8],
            [0, 3, 6], [1, 4, 7], [2, 5, 8],
            [0, 4, 8], [2, 4, 6]
        ];

        foreach (var line in lines)
        {
            char a = board[line[0]], b = board[line[1]], c = board[line[2]];
            if (a != ' ' && a == b && b == c)
                return a;
        }

        return ' ';
    }

    private static bool IsFull(char[] board)
    {
        foreach (char c in board)
            if (c == ' ') return false;
        return true;
    }
}
