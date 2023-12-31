using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;
using PZ25.Windows.ProjectEmployees;

namespace PZ25;

public partial class ProjectWindow : UserControl
{
    private Database _database = new Database();
    private List<Project> _projects = new List<Project>();

    private string _fullTable =
        "select project_id, project_name, date_start, date_end, status_name, priority_name, DATEDIFF(date_end, now()) as days_left from project " +
        "join pz25.priority p on project.priority = p.priority_id " +
        "join pz25.status s on project.status = s.status_id;";
    public ProjectWindow()
    {
        
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
                DaysLeft = reader.GetInt32("days_left")
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

    private void EditBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        Project selectedProject = ProjectGrid.SelectedItem as Project;
        if (selectedProject != null)
        {
            EditProjectWindow editProjectWindow = new EditProjectWindow(selectedProject);
            editProjectWindow.Show();
        }
        else
        {
            var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите строку для редактирования",
                ButtonEnum.Ok, Icon.Error);
            var result = error.ShowAsync();
        }
    }

    private void ProjEmployeeBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        MainPanel.Children.Clear();
        ProjectEmployeeWindow projectEmployeeWindow = new ProjectEmployeeWindow();
        MainPanel.Children.Add(projectEmployeeWindow);
    }

    private void SearchTxt_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        List<Project> search = _projects.Where(x => x.Name.ToLower().Contains(SearchTxt.Text.ToLower())).ToList();
        ProjectGrid.ItemsSource = search;
    }

    private void SortAscBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        List<Project> sortAsc = _projects.OrderBy(x => x.Name).ToList();
        ProjectGrid.ItemsSource = sortAsc;
    }

    private void SortDescBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        List<Project> sortDesc = _projects.OrderByDescending(x => x.Name).ToList();
        ProjectGrid.ItemsSource = sortDesc;
    }
}