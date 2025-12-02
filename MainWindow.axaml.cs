using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Runtime.CompilerServices;

namespace LifeProjectAvalonia;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();


        Height = 900;
        Width = 900;

        Position = new Avalonia.PixelPoint(50, 50);


        StartButton.Click += StartButton_Click;

        
    }

    private void StartButton_Click(object? sender, RoutedEventArgs e)
    {
        int width = int.TryParse(WidthTextBox.Text, out var w) ? w : 30;
        int height = int.TryParse(HeightTextBox.Text, out var h) ? h : 30;
        bool randomize = WrapCheckBox.IsChecked ?? false;
        int timeDelay = (int)SpeedSlider.Value;

        var controller = new GameController(width, height, timeDelay);

        this.Content = new LifePagePresenter(controller, width, height, 900 / width);

        Window favorites = new FavoritesWindow(controller.scanner, width, height, 900 / width);
        favorites.Show();

        Closing += (_, _) => favorites.Close();

        if (randomize) controller.terrain.Randomize();
    }
}