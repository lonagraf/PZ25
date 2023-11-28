using System.Data;
using MySql.Data.MySqlClient;

namespace PZ25;

public class Database
{
    private MySqlConnection _connection = new MySqlConnection(@"server=localhost;database=pz25;port=3306;User Id=root;password=IGraf123*");

    public void openConnection()
    {
        if (_connection.State == ConnectionState.Closed)
        {
            _connection.Open();
        }
    }

    public void closeConnection()
    {
        if (_connection.State == ConnectionState.Open)
        {
            _connection.Close();
        }
    }

    public MySqlConnection getConnection()
    {
        return _connection;
    }
}