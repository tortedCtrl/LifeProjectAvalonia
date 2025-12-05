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

    private PagePresenter _presenter;

    public LifePagePresenter(GameController controller, int width, int height, double cellSize)
    {
        InitializeComponent();

        _presenter = new(GameCanvas, width, height, cellSize, null);

        _controller = controller;

        _controller.terrain.TurnFinished += PaintField;
    }

    private void PaintField(CellField field)
    {
        foreach (Cell cell in field)
        {
            _presenter.cells[cell.X, cell.Y].Fill = cell.BrushToPaintCell();
        }
    }

    private void StartButton_Click(object? sender, RoutedEventArgs e) =>
        _controller.ToggleGame();
            
}