using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace AutoService1.DB;

public class OrdersItemsRepository
{
    MySqlConnection connection;
    public OrdersItemsRepository(IOptions<DatabaseConnection> connect)
    {
        connection = new MySqlConnection(connect.Value.ConnectionString);
    }

    public List<OrderItem> GetOrderItemsByTest()
    {
        List<OrderItem> result = new List<OrderItem>();
        string sql = "select * from orders_items";
        try
        {
            connection.Open();
            using (var mc = new MySqlCommand(sql, connection))
            using (var dr = mc.ExecuteReader())
            {
                while (dr.Read())
                {
                    result.Add(new OrderItem
                    {
                        Id = dr.GetInt32("id"),
                        OrderId = dr.GetInt32("order_text"),
                        WorkID = dr.GetInt32("work_id"),
                        WorkPrice = dr.GetInt32("work_price"),
                       
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