using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LifeProjectAvalonia;

public partial class LifePagePresenter : UserControl
{
    private GameController? _controller;

    private PagePresenter _presenter;

    private int _whiteTotal;
    private int _emptyTotal;
    private int _blackTotal;


    public LifePagePresenter(StartData data)
    {
        InitializeComponent();

        _presenter = new(GameCanvas, data.width, data.height, data.cellSize, null);

        FramedCellsButton.IsCheckedChanged += (s, e) =>
        {
            _controller?.ShowEmptyCells_LifePresenter(FramedCellsButton.IsChecked ?? false);
        };

    }

    public void AssignController(GameController controller)
    {
        _controller = controller;
    }

    public void ClearBox(Cell cell)
    {
        _presenter.cells[cell.X, cell.Y].Opacity = 0;
    }
    public void PaintBox(Cell cell)
    {
        _presenter.cells[cell.X, cell.Y].Opacity = 1;
        _presenter.cells[cell.X, cell.Y].Fill = cell.BrushToPaintCell();
    }

    private void StartButton_Click(object? sender, RoutedEventArgs e) =>
        _controller?.ToggleGame();

    public void UpdateTurnTime(TimeSpan turnTime) => TurnTime.Text = turnTime.ToString(@"s\.fff");
    public void UpdateScantime(TimeSpan scanTime) => ScanTime.Text = scanTime.ToString(@"s\.fff");

    public void UpdateStatistics(int genCnt, int whiteTotal, int blackTotal, int emptyTotal)
    {
        int whiteChanged = whiteTotal - _whiteTotal;
        int blackChanged = blackTotal - _blackTotal;
        int emptyChanged = emptyTotal - _emptyTotal;

        double whiteChangedPercent = CalculatePercent(whiteChanged, _whiteTotal);
        double blackChangedPercent = CalculatePercent(blackChanged, _blackTotal);
        double emptyChangedPercent = CalculatePercent(emptyChanged, _emptyTotal);

        UpdateCounts();

        GenerationCount.Text = genCnt.ToString();
        TotalCells.Text = (whiteTotal + blackTotal).ToString();

        SetTotalVals();
        SetChangedVals();
        SetPercentVals();

        return;

        double CalculatePercent(double changed, double total) => total != 0 ? changed / total : 0;

        void UpdateCounts()
        {
            _whiteTotal = whiteTotal;
            _blackTotal = blackTotal;
            _emptyTotal = emptyTotal;
        }

        void SetTotalVals()
        {
            var blocks = new List<TextBlock>(4) { WhiteTotal, BlackTotal, EmptyTotal };
            var vals = new List<int>(4) { _whiteTotal, _blackTotal, _emptyTotal };
            foreach ((TextBlock block, int val) in blocks.Zip(vals))
                block.Text = val.ToString();
        }
        void SetChangedVals()
        {
            var blocks = new List<TextBlock>(4) { WhiteChanged, BlackChanged, EmptyChanged };
            var vals = new List<int>(4) { whiteChanged, blackChanged, emptyChanged };
            foreach ((TextBlock block, int val) in blocks.Zip(vals))
                block.Text = val.ToString();
        }
        void SetPercentVals()
        {
            var blocks = new List<TextBlock>(4) { WhiteChangedPercent, BlackChangedPercent, EmptyChangedPercent };
            var vals = new List<double>(4) { whiteChangedPercent, blackChangedPercent, emptyChangedPercent };
            foreach ((TextBlock block, double val) in blocks.Zip(vals))
            {
                string percent = (val * 100).ToString("F1");
                if (val > 0)
                {
                    percent = '+' + percent;
                    block.Foreground = Brushes.Green;
                }
                else if (val == 0)
                    block.Foreground = Brushes.DarkGray;
                else
                    block.Foreground = Brushes.Red;
                block.Text = percent;
            }
        }
    }
}