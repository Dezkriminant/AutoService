using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace AutoService1.DB;

public class WorkRepository
{
    MySqlConnection connection;

    public WorkRepository(IOptions<DatabaseConnection> connect)
    {
        connection = new MySqlConnection(connect.Value.ConnectionString);
    }

    public List<Work> GetWorksByService(Service service)
    {
        List<Work> result = new List<Work>();
        string sql = "select * from works";
        try
        {
            connection.Open();
            using (var mc = new MySqlCommand(sql, connection))
            using (var dr = mc.ExecuteReader())
            {
                while (dr.Read())
                {
                    result.Add(new Work
                    {
                        Id = dr.GetInt32("id"),
                        ServiceId = dr.GetInt32("service_id"),
                        WorkName = dr.GetString("work_name"),
                        Price = dr.GetDecimal("price"),
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



    public List<Work> GetWorksByServices(Service selectedService)
    {
        List<Work> s = new List<Work>();
        string sql = "select * from works where service_id = " + selectedService.Id; 
        try
        {

            connection.Open();
            using (var mc = new MySqlCommand(sql, connection))
            using (var dr = mc.ExecuteReader())
            {
                while (dr.Read())
                {
                    s.Add(new Work
                    {
                        Id = dr.GetInt32("id"),
                        ServiceId = dr.GetInt32("service_id"),
                        WorkName = dr.GetString("work_name"),
                        Price = dr.GetDecimal("price"),
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

    
    return s;
}

}