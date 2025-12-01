using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;

namespace LifeProjectAvalonia;

public partial class LifePagePresenter : UserControl
{
    private GameController _controller;
    // Размеры сетки
    private int _gridWidth;
    private int _gridHeight;

    // Двумерный массив клеток
    private Rectangle[,] _cells;

    // Размер одной клетки в пикселях
    private const int CellSize = 20;

    // Цвета
    private readonly SolidColorBrush _deadColor = new SolidColorBrush(Colors.White);
    private readonly SolidColorBrush _aliveColor = new SolidColorBrush(Colors.Black);
    private readonly SolidColorBrush _gridColor = new SolidColorBrush(Colors.LightGray);

    public LifePagePresenter(GameController controller, int width, int height)
    {
        InitializeComponent();

        _controller = controller;

        _controller._terrain.TurnFinished += PaintField;

        _gridWidth = width;
        _gridHeight = height;

        

        _cells = new Rectangle[width, height];

        InitializeGrid();
    }

    private void InitializeGrid()
    {
        GameCanvas.Children.Clear();

        var totalWidth = _gridWidth * CellSize;
        var totalHeight = _gridHeight * CellSize;


        for (int row = 0; row < _gridHeight; row++)
        {
            for (int col = 0; col < _gridWidth; col++)
            {
                var cell = new Rectangle
                {
                    Width = CellSize - 1, // -1 для границ
                    Height = CellSize - 1,
                    Fill = _deadColor,
                    Stroke = _gridColor,
                    StrokeThickness = 1
                };


                Canvas.SetLeft(cell, col * CellSize);
                Canvas.SetTop(cell, row * CellSize);

                //cell.PointerPressed += (sender, e) => OnCellClicked(row, col);

                _cells[col, row] = cell;

                GameCanvas.Children.Add(cell);
            }
        }

        GameCanvas.Width = totalWidth;
        GameCanvas.Height = totalHeight;
    }

    // Обработчик клика по клетке
    private void OnCellClicked(int row, int col)
    {
        ToggleCell(row, col);
    }

    // Переключение состояния клетки
    public void ToggleCell(int row, int col)
    {

    }

    private void PaintField(CellField field)
    {
        foreach (Cell cell in field)
        {
            _cells[cell.X, cell.Y].Fill = cell.Brush();
        }
    }

    // Обработчик кнопки "Начать"
    private void StartButton_Click(object? sender, RoutedEventArgs e) =>
        _controller.ToggleGame();
            
}