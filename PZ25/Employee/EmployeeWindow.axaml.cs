using System.Collections.Generic;
using System.Data;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace PZ25.Employee;

public partial class EmployeeWindow : UserControl
{
    private Database _database = new Database();
    private List<Employee> _employees = new List<Employee>();
    private string _fullTable = "select * from employee";
    public EmployeeWindow()
    {
        InitializeComponent();
        ShowTable(_fullTable);
    }

    private void ShowTable(string sql)
    {
        _database.openConnection();
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentEmployee = new Employee()
            {
                EmployeeID = reader.GetInt32("employee_id"),
                Firstname = reader.GetString("firstname"),
                Surname = reader.GetString("surname"),
                Email = reader.GetString("email"),
                Number = reader.GetString("phone_number")
            };
            _employees.Add(currentEmployee);
        }
        _database.closeConnection();
        EmployeeGrid.ItemsSource = _employees;
    }

    private void AddBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow();
        addEmployeeWindow.Show();
    }

    private void Delete(int id)
    {
        _database.openConnection();
        string sql = "delete from employee where employee_id = @id;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@id", id);
        command.ExecuteNonQuery();
        _database.closeConnection();
    }

    private async void DeleteBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        Employee selectedEmployee = EmployeeGrid.SelectedItem as Employee;

        if (selectedEmployee != null)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Предупреждение",
                "Вы уверены что хотите удалить сотрудника?", ButtonEnum.YesNo, Icon.Warning);
            var result = await box.ShowAsync();
            if (result == ButtonResult.Yes)
            {
                Delete(selectedEmployee.EmployeeID);
                ShowTable(_fullTable);
                var success = MessageBoxManager.GetMessageBoxStandard("Успешно", "Сотрудник успешно удален",
                    ButtonEnum.Ok, Icon.Success);
                var successResult = success.ShowAsync();
            }
            else
            {
                var cancel = MessageBoxManager.GetMessageBoxStandard("Отмена", "Операция удаления отменена!",
                    ButtonEnum.Ok, Icon.Stop);
                var cancelResult = cancel.ShowAsync();
            }
        }
        else
        {
            var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите строку для удаления!",
                ButtonEnum.Ok, Icon.Error);
            var errorResult = error.ShowAsync();
        }
    }
}