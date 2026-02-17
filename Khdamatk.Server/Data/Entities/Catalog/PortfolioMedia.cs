namespace Khdamatk.Server.Data.Entities.Catalog;

public class PortfolioMedia
{
    // 1. المفاتيح الأجنبية التي تشكل المفتاح المركب
    public int PortfolioItemId { get; set; }
    public int MediaId { get; set; }

    // 2. خصائص التنقل
    public virtual PortfolioItem PortfolioItem { get; set; } = null!;
    public virtual Media Media { get; set; } = null!;
}