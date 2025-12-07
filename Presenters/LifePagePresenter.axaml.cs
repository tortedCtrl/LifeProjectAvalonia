using Avalonia.Controls;
using Avalonia.Interactivity;

namespace LifeProjectAvalonia;

public partial class LifePagePresenter : UserControl
{
    private GameController? _controller;

    private PagePresenter _presenter;

    public LifePagePresenter(int width, int height, double cellSize)
    {
        InitializeComponent();

        _presenter = new(GameCanvas, width, height, cellSize, null);
    }

    public void AssignController(GameController controller)
    {
        _controller = controller;
    }

    public void PaintBox(Cell cell) =>
        _presenter.cells[cell.X, cell.Y].Fill = cell.BrushToPaintCell();

    private void StartButton_Click(object? sender, RoutedEventArgs e) =>
        _controller?.ToggleGame();
}