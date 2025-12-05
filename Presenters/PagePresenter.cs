using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using System;

namespace LifeProjectAvalonia;

public class PagePresenter
{
    private Canvas _gameCanvas;

    private EventHandler<PointerPressedEventArgs>? _cellClickedHandler;

    private int _gridWidth;
    private int _gridHeight;

    public Rectangle[,] cells;

    private readonly double _cellSize;

    private readonly SolidColorBrush _emptyColor = new SolidColorBrush(Colors.White);
    private readonly SolidColorBrush _gridColor = new SolidColorBrush(Colors.DarkGray);

    public PagePresenter(Canvas canvas, int width, int height, double cellSize,
        EventHandler<PointerPressedEventArgs>? cellClickedHandler = null)
    {
        _cellClickedHandler = cellClickedHandler;

        _gameCanvas = canvas ?? throw new NullReferenceException(nameof(canvas));

        _gridWidth = width > 1 ? width : 10;
        _gridHeight = height > 1 ? height : 10;

        _cellSize = cellSize > 0 && cellSize < 100 ? cellSize : 30;

        cells = new Rectangle[width, height];

        InitializeGrid();
    }

    private void InitializeGrid()
    {
        _gameCanvas.Children.Clear();

        var totalWidth = _gridWidth * _cellSize;
        var totalHeight = _gridHeight * _cellSize;


        for (int row = 0; row < _gridHeight; row++)
        {
            for (int col = 0; col < _gridWidth; col++)
            {
                var cell = new Rectangle
                {
                    Width = _cellSize - _cellSize / 10, // -1 для границ
                    Height = _cellSize - _cellSize / 10,
                    Fill = _emptyColor,
                    Stroke = _gridColor,
                    StrokeThickness = 1
                };


                Canvas.SetLeft(cell, col * _cellSize);
                Canvas.SetTop(cell, row * _cellSize);

                cell.PointerPressed += _cellClickedHandler;

                cells[col, row] = cell;

                _gameCanvas.Children.Add(cell);
            }
        }

        _gameCanvas.Width = totalWidth;
        _gameCanvas.Height = totalHeight;
    }
}

