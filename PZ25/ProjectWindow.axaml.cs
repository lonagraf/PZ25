using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace PZ25;

public partial class ProjectWindow : UserControl
{
    private Database _database = new Database();
    private List<Project> _projects = new List<Project>();

    private string _fullTable =
        "select project_id, project_name, date_start, date_end, status_name, priority_name, budget from project " +
        "join pro1_4.priority p on project.priority = p.priority_id " +
        "join pro1_4.status s on project.status = s.status_id;";
    public ProjectWindow()
    {
        Width = 1000;
        Height = 405;
        InitializeComponent();
        ShowTable(_fullTable);
    }

    public void ShowTable(string sql)
    {
        _database.openConnection();
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentProject = new Project()
            {
                ProjectID = reader.GetInt32("project_id"),
                Name = reader.GetString("project_name"),
                DateStart = reader.GetDateTime("date_start"),
                DateEnd = reader.GetDateTime("date_end"),
                Status = reader.GetString("status_name"),
                Priority = reader.GetString("priority_name"),
                Budget = reader.GetDecimal("budget")
            };
            _projects.Add(currentProject);
        }
        _database.closeConnection();
        ProjectGrid.ItemsSource = _projects;
    }

    private void AddBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        AddProjectWindow addProjectWindow = new AddProjectWindow();
        addProjectWindow.Show();
    }

    public void Delete(int id)
    {
        _database.openConnection();
        string sql = "delete from project where project_id = @id";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@id", id);
        command.ExecuteNonQuery();
    }

    private async void DeleteBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        Project selectedProject = ProjectGrid.SelectedItem as Project;

        if (selectedProject != null)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Предупреждение", "Вы уверены что хотите удалить?", ButtonEnum.YesNo);
            var result = await box.ShowAsync();
            if (result == ButtonResult.Yes)
            {
                Delete(selectedProject.ProjectID);
                ShowTable(_fullTable);
                var success = MessageBoxManager.GetMessageBoxStandard("Успешно", "Данные успешно удалены!", ButtonEnum.Ok);
                var result1 = success.ShowAsync();
            }
            else
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Отмена", "Операция удаления отменена!", ButtonEnum.Ok);
                var result1 = error.ShowAsync();
            }
        }
        else
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите проект для удаления!", ButtonEnum.Ok);
            var result = box.ShowAsync();
        }
    }
}