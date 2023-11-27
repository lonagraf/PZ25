using System;

namespace PZ25;

public class Project
{
    public int ProjectID { get; set; }
    public string Name { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }
    public decimal Budget { get; set; }
}