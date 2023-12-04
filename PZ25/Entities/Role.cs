namespace PZ25.Entities;

public class Role
{
    public int RoleID { get; set; }
    public string RoleName { get; set; }
    public override string ToString()
    {
        return RoleName;
    }
}