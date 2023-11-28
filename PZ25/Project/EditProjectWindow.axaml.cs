using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace PZ25;

public partial class EditProjectWindow : Window
{
    private Database _database = new Database();
    private Project _project;
    public EditProjectWindow(Project project)
    {
        InitializeComponent();
        _project = project;
        NameTxt.Text = _project.Name;
        StartDate.SelectedDate = _project.DateStart;
        EndDate.SelectedDate = _project.DateEnd;
        StatusCmb.SelectedItem = _project.Status;
        PriorityCmb.SelectedItem = _project.Priority;
        LoadDataStatusCmb();
        LoadDataPriorityCmb();
    }

    private void EditBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        int id = _project.ProjectID;
        _database.openConnection();
        string sql =
            "update project set project_name = @name, date_start = @start, date_end = @end, status = @status, priority = @priority where project_id = @id";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@name", NameTxt.Text);
        command.Parameters.AddWithValue("@start", StartDate.SelectedDate.GetValueOrDefault());
        command.Parameters.AddWithValue("@end", EndDate.SelectedDate.GetValueOrDefault());
        int selectedStatusId = GetSelectedStatusId(StatusCmb.SelectedItem.ToString());
        command.Parameters.AddWithValue("@status", selectedStatusId);
        int selectedPriorityId = GetSelectedPriorityId(PriorityCmb.SelectedItem.ToString());
        command.Parameters.AddWithValue("@priority", selectedPriorityId);
        command.Parameters.AddWithValue("@id", id);
        command.ExecuteNonQuery();
        var success = MessageBoxManager.GetMessageBoxStandard("Успешно", "Данные успешно изменены", ButtonEnum.Ok,
            MsBox.Avalonia.Enums.Icon.Success);
        var result = success.ShowAsync();
        this.Close();
    }

    private void LoadDataStatusCmb()
    {
        _database.openConnection();
        string sql = "select status_name from status";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            StatusCmb.Items.Add(reader["status_name"]).ToString();
        }
        _database.closeConnection();
    }

    private int GetSelectedStatusId(string selectedName)
    {
        _database.openConnection();
        string sql = "select status_id from status where status_name = @name";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@name", selectedName);
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