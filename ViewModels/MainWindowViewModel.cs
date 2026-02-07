using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicTacToe.Models;

namespace TicTacToe.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly GameBoard _board = new();
    private char _currentPlayer = 'X';
    private bool _gameOver;

    [ObservableProperty]
    private ObservableCollection<string> _cells = new(new string[9]);

    [ObservableProperty]
    private string _statusMessage = "X's turn";

    [ObservableProperty]
    private bool _isVsAi;

    [ObservableProperty]
    private string _modeButtonText = "Mode: PvP";

    [ObservableProperty]
    private int _scoreX;

    [ObservableProperty]
    private int _scoreO;

    [ObservableProperty]
    private int _scoreDraw;

    public MainWindowViewModel()
    {
        InitCells();
    }

    private void InitCells()
    {
        for (int i = 0; i < 9; i++)
            Cells[i] = "";
    }

    [RelayCommand]
    private void CellClick(string parameter)
    {
        if (_gameOver) return;
        if (!int.TryParse(parameter, out int index)) return;

        if (!_board.MakeMove(index, _currentPlayer)) return;

        Cells[index] = _currentPlayer.ToString();

        char result = _board.CheckWinner();
        if (result != ' ')
        {
            EndGame(result);
            return;
        }

        SwitchPlayer();

        if (IsVsAi && _currentPlayer == 'O' && !_gameOver)
        {
            PlayAi();
        }
    }

    private void PlayAi()
    {
        char[] boardCopy = _board.GetBoardCopy();
        int move = AiPlayer.GetBestMove(boardCopy, 'O');

        if (move < 0) return;

        _board.MakeMove(move, 'O');
        Cells[move] = "O";

        char result = _board.CheckWinner();
        if (result != ' ')
        {
            EndGame(result);
            return;
        }

        SwitchPlayer();
    }

    private void SwitchPlayer()
    {
        _currentPlayer = _currentPlayer == 'X' ? 'O' : 'X';
        StatusMessage = $"{_currentPlayer}'s turn";
    }

    private void EndGame(char result)
    {
        _gameOver = true;

        switch (result)
        {
            case 'X':
                ScoreX++;
                StatusMessage = "X wins!";
                break;
            case 'O':
                ScoreO++;
                StatusMessage = "O wins!";
                break;
            case 'D':
                ScoreDraw++;
                StatusMessage = "It's a draw!";
                break;
        }
    }

    [RelayCommand]
    private void NewGame()
    {
        _board.Reset();
        _currentPlayer = 'X';
        _gameOver = false;
        InitCells();
        StatusMessage = "X's turn";
    }

    [RelayCommand]
    private void ToggleMode()
    {
        IsVsAi = !IsVsAi;
        ModeButtonText = IsVsAi ? "Mode: PvAI" : "Mode: PvP";
        NewGame();
    }
}
