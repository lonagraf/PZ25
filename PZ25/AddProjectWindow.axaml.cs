using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace PZ25;

public partial class AddProjectWindow : Window
{
    private Database _database = new Database();
    public AddProjectWindow()
    {
        InitializeComponent();
        Width = 400;
        Height = 450;
        LoadDataStatusCmb();
        LoadDataPriorityCmb();
    }

    private void AddBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        _database.openConnection();
        string sql = "insert into project (project_name, date_start, date_end, status, priority, budget) " +
                     "values (@name, @start, @end, @status, @priority, @budget);";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@name", NameTxt.Text);
        command.Parameters.AddWithValue("@start", StartDate.SelectedDate.GetValueOrDefault());
        command.Parameters.AddWithValue("@end", EndDate.SelectedDate.GetValueOrDefault());
        int selectedStatusId = GetSelectedStatusId(StatusCmb.SelectedItem.ToString());
        command.Parameters.AddWithValue("@status", selectedStatusId);
        int selectedPriorityId = GetSelectedPriorityId(PriorityCmb.SelectedItem.ToString());
        command.Parameters.AddWithValue("@priority", selectedPriorityId);
        command.Parameters.AddWithValue("@budget", BudgetTxt.Text);
        command.ExecuteNonQuery();
        _database.closeConnection();
        var box = MessageBoxManager.GetMessageBoxStandard("Успешно", "Данные успешно добавлены!", ButtonEnum.Ok);
        var result = box.ShowAsync();
    }

    public void LoadDataStatusCmb()
    {
        _database.openConnection();
        string sql = "select status_name from status;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            StatusCmb.Items.Add(reader["status_name"].ToString());
        }
        _database.closeConnection();
    }

    public int GetSelectedStatusId(string selectedStatus)
    {
        _database.openConnection();
        string sql = "select status_id from status where status_name = @selectedStatus;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@selectedStatus", selectedStatus);
        int selectedId = Convert.ToInt32(command.ExecuteScalar());
        return selectedId;
    }

    public void LoadDataPriorityCmb()
    {
        _database.openConnection();
        string sql = "select priority_name from priority;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            PriorityCmb.Items.Add(reader["priority_name"].ToString());
        }
        _database.closeConnection();
    }

    public int GetSelectedPriorityId(string selectedPriority)
    {
        _database.openConnection();
        string sql = "select priority_id from priority where priority_name = @selectedPriority;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@selectedPriority", selectedPriority);
        int selectedId = Convert.ToInt32(command.ExecuteScalar());
        return selectedId;
    }
}