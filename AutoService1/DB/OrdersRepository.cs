using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace AutoService1.DB;

public class OrderRepository
{
    MySqlConnection connection;
    public OrderRepository(IOptions<DatabaseConnection> connect)
    {
        connection = new MySqlConnection(connect.Value.ConnectionString);
    }

    public void InsertOrder(Order order, List<Work> works)
    {
        var sql1 = "INSERT INTO Frolof_and_Snigirev.orders (id, client_name, car_model, service_id, total_amount, discount_percent, order_date) VALUES (0, @client_name, @car_model, @service_id, @total_amount, @discount_percent, @order_date); ";
        var sql2 = "SELECT max(id) as id FROM Frolof_and_Snigirev.orders;";
        var sql3 = "INSERT INTO Frolof_and_Snigirev.order_items (order_id, work_id, work_price) VALUES (@order_id, @work_id, @work_price); ";

        connection.Open();
        using var transaction = connection.BeginTransaction();
        try
        {

            using (var mc = new MySqlCommand(sql1, connection, transaction))
            {
                mc.Parameters.AddWithValue("@order_id", order.Id);
                mc.Parameters.AddWithValue("@client_name", order.ClientName);
                mc.Parameters.AddWithValue("@car_model", order.CarModel);
                mc.Parameters.AddWithValue("@service_id", order.ServiceId);
                mc.Parameters.AddWithValue("@total_amount", order.TotalAmount);
                mc.Parameters.AddWithValue("@discount_percent", order.DiscountPercent);
                mc.Parameters.AddWithValue("@order_date", order.OrderTime);
                 mc.ExecuteNonQuery();
            }

            int id = 0;
            using (var mc = new MySqlCommand(sql2, connection, transaction))
            {
                using (var dr = mc.ExecuteReader())
                {
                    while (dr.Read()) 
                    {
                        id = dr.GetInt32("id");
                    }
                }
            }

            foreach (var work in works)
            {

                using (var mc = new MySqlCommand(sql3, connection, transaction))
                {
                    mc.Parameters.AddWithValue("@order_id", id);
                    mc.Parameters.AddWithValue("@work_id", work.Id);
                    mc.Parameters.AddWithValue("@work_price", work.Price);
                    mc.ExecuteNonQuery();
                }
            }
            transaction.Commit();
            connection.Close();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            connection.Close();
        }
    }
}