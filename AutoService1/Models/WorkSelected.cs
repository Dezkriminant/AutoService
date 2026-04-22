namespace AutoService1.DB;

public class WorkSelected
{
    public Work Work { get; set; }
    
    public bool IsSelected { get; set; }

    public WorkSelected(Work work)
    {
        Work = work;
    }

}