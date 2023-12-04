using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace PZ25.Windows.ProjectEmployees;

public partial class AddProjectEmployee : Window
{
    private Database _database = new Database();
    public AddProjectEmployee()
    {
        InitializeComponent();
        LoadDataProjectCmb();
        LoadDataEmployeeCmb();
        LoadDataRoleCmb();
        Icon = new WindowIcon("Icons/square-plus.png");
    }

    private void AddBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        _database.openConnection();
        string sql = "insert into project_employees (project, employee, role) values (@project, @employee, @role);";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        int selectedProjectId = GetSelectedProjectId(ProjectCmb.SelectedItem.ToString());
        command.Parameters.AddWithValue("@project", selectedProjectId);
        int selectedEmployeeId = GetSelectedEmployeeId(EmployeeCmb.SelectedItem.ToString());
        command.Parameters.AddWithValue("@employee", selectedEmployeeId);
        int selectedRoleId = GetSelectedRoleId(RoleCmb.SelectedItem.ToString());
        command.Parameters.AddWithValue("@role", selectedRoleId);
        command.ExecuteNonQuery();
        var success = MessageBoxManager.GetMessageBoxStandard("Успешно", "Данные успешно добавлены", ButtonEnum.Ok,
            MsBox.Avalonia.Enums.Icon.Success);
        var result = success.ShowAsync();
        this.Close();
    }

    private void LoadDataProjectCmb()
    {
        _database.openConnection();
        string sql = "select project_name from project;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            ProjectCmb.Items.Add(reader["project_name"]).ToString();
        }
        _database.closeConnection();
    }

    private int GetSelectedProjectId(string selectedProject)
    {
        _database.openConnection();
        string sql = "select project_id from project where project_name = @selectedProject;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@selectedProject", selectedProject);
        int selectedId = Convert.ToInt32(command.ExecuteScalar());
        return selectedId;
    }
    
    private void LoadDataEmployeeCmb()
    {
        _database.openConnection();
        string sql = "select concat(firstname, ' ', surname) as employee from employee;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            EmployeeCmb.Items.Add(reader["employee"]).ToString();
        }
        _database.closeConnection();
    }

    private int GetSelectedEmployeeId(string selectedEmployee)
    {
        _database.openConnection();
        string sql = "select employee_id from employee where concat(firstname, ' ', surname) = @selectedEmployee;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@selectedEmployee", selectedEmployee);
        int selectedId = Convert.ToInt32(command.ExecuteScalar());
        return selectedId;
    }
    
    private void LoadDataRoleCmb()
    {
        _database.openConnection();
        string sql = "select role_name from role;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            RoleCmb.Items.Add(reader["role_name"]).ToString();
        }
        _database.closeConnection();
    }

    private int GetSelectedRoleId(string selectedRole)
    {
        _database.openConnection();
        string sql = "select role_id from role where role_name = @selectedRole;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@selectedRole", selectedRole);
        int selectedId = Convert.ToInt32(command.ExecuteScalar());
        return selectedId;
    }

    private void BackBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}