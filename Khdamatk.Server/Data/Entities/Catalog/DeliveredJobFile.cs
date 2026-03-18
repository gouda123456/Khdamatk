namespace Khdamatk.Server.Data.Entities.Catalog;

public class DeliveredJobFile
{
    public int Id { get; set; }
    [ForeignKey(nameof(Job))]
    public int JobId { get; set; }
    [ForeignKey(nameof(MileStone))]
    public int MileStoneId { get; set; }
    [ForeignKey(nameof(Media))]
    public int MediaId { get; set; }

    public DeliveredFileStatues Statues { get; set; } = DeliveredFileStatues.New;


    //Navigations Properties
    public virtual JobPost Job { get; set; }
    public virtual MileStone MileStone { get; set; }
    public virtual Media Media { get; set; }

}

public enum DeliveredFileStatues
{
    New = 1,
    old,
    Approved,
    Rejected
}
