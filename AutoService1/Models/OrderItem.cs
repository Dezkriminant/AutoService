namespace AutoService1.DB;

public class OrderItem
{
    public int Id { get; set; }
    
    public int OrderId { get; set; }
    
    public int WorkID { get; set; }
    
    public double WorkPrice { get; set; }
}