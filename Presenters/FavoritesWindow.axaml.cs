using Avalonia.Controls;
using Avalonia.Media;
using System.Threading;
using Avalonia;
using Avalonia.Layout;
using System;

namespace LifeProjectAvalonia;

public partial class FavoritesWindow : Window
{
    private PagePresenter _presenter;
    public FavoritesWindow(int width, int height, double cellSize, Action<bool>? showEmptyCells)
    {
        InitializeComponent();

        Height = 800;
        Width = 800;

        Position = new PixelPoint(1100, 100);

        _presenter = CreatePresenter(out UserControl control, out Canvas canvas);

        AddCheckBox(control, canvas);

        Content = control;

        return;

        PagePresenter CreatePresenter(out UserControl control, out Canvas canvas)
        {
            control = new UserControl();
            canvas = new Canvas()
            {
                Name = "GameCanvas",
                Background = Brushes.White
            };

            control.Content = canvas;
            return new PagePresenter(canvas, width, height, cellSize);
        }

        void AddCheckBox(UserControl control, Canvas canvas)
        {
            var framedCellsBox = new CheckBox()
            {
                Name = "StartButton",

                Width = 40,
                Height = 40,
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                IsChecked = true,
                BackgroundSizing = BackgroundSizing.CenterBorder,
                Background = Brushes.Aqua,
            };

            framedCellsBox.IsCheckedChanged += (s, e) =>
            {
                showEmptyCells?.Invoke(framedCellsBox.IsChecked ?? false);
            };

            canvas.Children.Add(framedCellsBox);
        }
    }
    public void Clear(Cell cell) =>
        _presenter.cells[cell.X, cell.Y].Opacity = 0;
    public void PaintBox(Cell cell)
    {
        _presenter.cells[cell.X, cell.Y].Opacity = 1;
        _presenter.cells[cell.X, cell.Y].Fill = cell.BrushToPaintCell();
    }
}