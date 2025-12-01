using Avalonia.Controls;
using Avalonia.Interactivity;

namespace LifeProjectAvalonia;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        StartButton.Click += StartButton_Click;
    }

    private void StartButton_Click(object? sender, RoutedEventArgs e)
    {
        int width = int.TryParse(WidthTextBox.Text, out var w) ? w : 30;
        int height = int.TryParse(HeightTextBox.Text, out var h) ? h : 30;
        bool randomize = WrapCheckBox.IsChecked ?? false;
        int timeDelay = (int)SpeedSlider.Value;

        var controller = new GameController(width, height, timeDelay);

        Content = new LifePagePresenter(controller, width, height);

        Width = 1000;
        Height = 1000;
        

        if (randomize) controller._terrain.Randomize();
    }
}