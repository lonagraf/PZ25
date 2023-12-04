using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace PZ25.Employee;

public partial class AddEmployeeWindow : Window
{
    private Database _database = new Database();
    public AddEmployeeWindow()
    {
        InitializeComponent();
        Icon = new WindowIcon("Icons/square-plus.png");
    }

    private void AddBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        _database.openConnection();
        string sql = "insert into employee (firstname, surname, email, phone_number) " +
                     "values (@name, @surname, @email, @number);";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@name", NameTxt.Text);
        command.Parameters.AddWithValue("@surname", SurnameTxt.Text);
        command.Parameters.AddWithValue("@email", EmailTxt.Text);
        command.Parameters.AddWithValue("@number", NumberTxt.Text);
        command.ExecuteNonQuery();
        _database.closeConnection();
        var box = MessageBoxManager.GetMessageBoxStandard("Успешно", "Данные успешно добавлены!", ButtonEnum.Ok,
            MsBox.Avalonia.Enums.Icon.Success);
        var result = box.ShowAsync();
        this.Close();
    }

    private void BackBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}