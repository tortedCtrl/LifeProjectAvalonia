using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LifeProjectAvalonia;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();


        Height = 825;
        Width = 1050;

        Position = new Avalonia.PixelPoint(10, 100);


        StartButton.Click += StartButton_Click;
    }

    private void StartButton_Click(object? sender, RoutedEventArgs e)
    {
        var data = GetData();

        var lifePresenter = InitPresenters();


        InitController();

        return;

        StartData GetData()
        {
            int width = int.TryParse(WidthTextBox.Text, out var w) ? w : 30;
            int height = int.TryParse(HeightTextBox.Text, out var h) ? h : 30;

            bool randomize = WrapCheckBox.IsChecked ?? false;
            int timeDelay = (int)SpeedSlider.Value;

            return new StartData(width, height, randomize, timeDelay);
        }

        LifePagePresenter InitPresenters()
        {
            var lifePresenter = new LifePagePresenter(data.width, data.height, data.cellSize);
            Content = lifePresenter;

            return lifePresenter;
        }

        void InitController()
        {
            var controller = new GameController(data.width, data.height, data.timeDelay, lifePresenter, data);
            lifePresenter.AssignController(controller);
            if (data.randomize) controller.Randomize();
        }
    }
}
public struct StartData
{
    public int width;
    public int height;
    public double cellSize;
    private int maxSide => Math.Max(width, height);

    public bool randomize;

    public int timeDelay;

    public StartData(int width, int height, bool randomize, int timeDelay, double fieldSize = 800.0)
    {
        this.width = Math.Clamp(width, 1, 200);
        this.height = Math.Clamp(height, 1, 200);
        this.randomize = randomize;
        this.timeDelay = timeDelay;
        cellSize = fieldSize / maxSide;
    }
}