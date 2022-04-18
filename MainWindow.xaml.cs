using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GameOfLife;

public partial class MainWindow : Window
{
    private Brush AliveBrush = Brushes.Black;
    private Brush DeadBrush = Brushes.Transparent;
    private GameModel _game;
    private Timer _generationTimer;
    private Border[] _cells = Array.Empty<Border>();
    public MainWindow()
    {
        InitializeComponent();

        Loaded += OnLoaded;

        _game = new GameModel(100, 100);

        _generationTimer = new Timer(TimeSpan.FromSeconds(0.1).TotalMilliseconds);
        _generationTimer.Elapsed += OnNewGeneration;
    }

    void OnNewGeneration(object? sender, ElapsedEventArgs args)
    {
        _game.NextGeneration();
        UpdateUi();
    }

    void OnLoaded(object sender, RoutedEventArgs args)
    {
        _game.Init(50);

        InitUi();

        _generationTimer.Start();
    }

    void InitUi()
    {
        _cells = new Border[_game.Size];

        UiGrid.Children.Clear();
        UiGrid.RowDefinitions.Clear();
        UiGrid.ColumnDefinitions.Clear();

        for (int y = 0; y < _game.Height; y++)
        {
            UiGrid.RowDefinitions.Add(new RowDefinition());
        }
        for (int x = 0; x < _game.Width; x++)
        {
            UiGrid.ColumnDefinitions.Add(new ColumnDefinition());
        }

        int pos = -1;
        for (int y = 0; y < _game.Height; y++)
        {
            for (int x = 0; x < _game.Width; x++)
            {
                pos++;
                var cell = new Border()
                {
                    Background = DeadBrush,
                    BorderThickness = new Thickness(0)
                };
                _cells[pos] = cell;

                Grid.SetRow(cell, y);
                Grid.SetColumn(cell, x);

                UiGrid.Children.Add(cell);
            }
        }
    }

    void UpdateUi()
    {
        Application.Current?.Dispatcher?.Invoke(new Action(() =>
        {
            int pos = -1;
            for (int y = 0; y < _game.Height; y++)
            {
                for (int x = 0; x < _game.Width; x++)
                {
                    pos++;
                    var cell = _cells[pos];

                    Brush newBrush = _game.IsAlive(x, y)
                        ? AliveBrush
                        : DeadBrush;


                    if (cell.Background != newBrush)
                    {
                        cell.Background = newBrush;
                    }
                }
            }
        }));
    }
}

