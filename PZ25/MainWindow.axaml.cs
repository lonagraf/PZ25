using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PZ25;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Width = 1000;
        Height = 450;
    }

    private void CloseBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void ProjectBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        MainPanel.Children.Clear();
        ProjectWindow projectWindow = new ProjectWindow();
        MainPanel.Children.Add(projectWindow);
    }
}