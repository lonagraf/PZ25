using Avalonia.Controls;
using Avalonia.Interactivity;
using PZ25.Employee;

namespace PZ25;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
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
    

    private void EmployeeBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        MainPanel.Children.Clear();
        EmployeeWindow employeeWindow = new EmployeeWindow();
        MainPanel.Children.Add(employeeWindow);
    }
}