using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;
using PZ25.Entities;

namespace PZ25.Windows.ProjectEmployees;

public partial class ProjectEmployeeWindow : UserControl
{
    private Database _database = new Database();
    private List<ProjectEmployee> _projectEmployees = new List<ProjectEmployee>();
    private List<Role> _roles = new List<Role>();

    private string _fullTable =
        "select project_employees_id, project_name, concat(firstname, ' ', surname) as employee, role_name from project_employees " +
        "join pz25.employee e on project_employees.employee = e.employee_id " +
        "join pz25.project p on p.project_id = project_employees.project " +
        "join pz25.role r on r.role_id = project_employees.role";
    public ProjectEmployeeWindow()
    {
        InitializeComponent();
        ShowTable(_fullTable);
        LoadDataRoleCmb();
    }

    private void ShowTable(string sql)
    {
        _database.openConnection();
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentProjEmployee = new ProjectEmployee()
            {
                ProjectEmployeeID = reader.GetInt32("project_employees_id"),
                Project = reader.GetString("project_name"),
                Employee = reader.GetString("employee"),
                Role = reader.GetString("role_name")
            };
            _projectEmployees.Add(currentProjEmployee);
        }
        _database.closeConnection();
        ProjEmployeeGrid.ItemsSource = _projectEmployees;
    }

    private void AddBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        AddProjectEmployee addProjectEmployee = new AddProjectEmployee();
        addProjectEmployee.Show();
    }

    private void Delete(int id)
    {
        _database.openConnection();
        string sql = "delete from project_employees where project_employees_id = @id";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@id", id);
        command.ExecuteNonQuery();
        _database.closeConnection();
    }

    private async void DeleteBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        ProjectEmployee selectedItem = ProjEmployeeGrid.SelectedItem as ProjectEmployee;

        if (selectedItem != null)
        {
            var warning = MessageBoxManager.GetMessageBoxStandard("Предупреждение", "Вы уверены что хотите удалить?", ButtonEnum.YesNo, Icon.Warning);
            var result = await warning.ShowAsync();
            if (result == ButtonResult.Yes)
            {
                Delete(selectedItem.ProjectEmployeeID);
                var success = MessageBoxManager.GetMessageBoxStandard("Успешно", "Данные успешно удалены", ButtonEnum.Ok, Icon.Success);
                var resultSuccess = success.ShowAsync();
            }
            else
            {
                var cancel = MessageBoxManager.GetMessageBoxStandard("Отмена", "Операция удаления отменена", ButtonEnum.Ok, Icon.Stop);
                var resultCancel = cancel.ShowAsync();
            }
        }
        else
        {
            var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите строку для удаления", ButtonEnum.Ok, Icon.Error);
            var resultError = error.ShowAsync();
        }
    }

    private void LoadDataRoleCmb()
    {
        _database.openConnection();
        string sql = "select role_name from role";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.HasRows && reader.Read())
        {
            var currentRole = new Role()
            {
                RoleName = reader.GetString("role_name")
            };
            _roles.Add(currentRole);
        }
        _database.closeConnection();
        RoleCmb.ItemsSource = _roles;
    }

    private void RoleCmb_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var currentRole = RoleCmb.SelectedItem as Role;
        var filtered = _projectEmployees.Where(x => x.Role == currentRole.RoleName).ToList();
        ProjEmployeeGrid.ItemsSource = filtered;
    }
}