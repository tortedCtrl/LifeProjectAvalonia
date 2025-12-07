using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace LifeProjectAvalonia;

public partial class FavoritesWindow : Window
{
    private PagePresenter _presenter;
    public FavoritesWindow(int width, int height, double cellSize)
    {
        InitializeComponent();

        Height = 900;
        Width = 900;

        Position = new Avalonia.PixelPoint(1000, 50);

        UserControl control = new UserControl();
        var canvas = new Canvas()
        {
            Name = "GameCanvas",
            Background = Brushes.White
        };

        control.Content = canvas;
        _presenter = new PagePresenter(canvas, width, height, cellSize);
        Content = control;
    }

    public void PaintField(CellField field)
    {
        foreach (Cell cell in field)
        {
            _presenter.cells[cell.X, cell.Y].Fill = cell.BrushToPaintCell();
        }
    }
}