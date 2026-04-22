using System;

namespace AutoService1.DB;

public class Order
{
    public int Id { get; set; }

    public string ClientName { get; set; }

    public string CarModel { get; set; }

    public int ServiceId { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal DiscountPercent { get; set; }

    public DateTime OrderTime { get; set; }
}