using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace AutoService1.DB;

public class ServiceRepository
{
    MySqlConnection connection;
    public ServiceRepository(IOptions<DatabaseConnection> connect)
    {
        connection = new MySqlConnection(connect.Value.ConnectionString);
    }

    public List<Service> GetServicesByTest()
    {
        List<Service> result = new List<Service>();
        string sql = "select  * from services";
        try
        {
            connection.Open();
            using (var mc = new MySqlCommand(sql, connection))
            using (var dr = mc.ExecuteReader())
            {
                while (dr.Read())
                {
                    result.Add(new Service
                    {
                        Id = dr.GetInt32("id"),
                        Title = dr.GetString("title"),
                        Description = dr.GetString("description"),
                       
                    });
                }
            }

            connection.Close();
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return result;
    }
}