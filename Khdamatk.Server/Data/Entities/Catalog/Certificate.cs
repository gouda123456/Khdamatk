namespace Khdamatk.Server.Data.Entities.Catalog;

public class Certificate : BaseEntity
{
    

    [ForeignKey(nameof(ServiceProviderProfile))]
    public string ServiceProviderProfileId { get; set; } = null!;

    // اسم الشهادة
    [Required]
    [StringLength(50,MinimumLength =2)]
    public string Title { get; set; } = string.Empty;

    // الجهة المانحة للشهادة
    [Required]
    public string Issuer { get; set; } = string.Empty;

    [Required]
    public string Type { get; set; } = string.Empty;

    // سنة الحصول على الشهادة   
    public int YearAcquired { get; set; }

    // مفتاح خارجي لكيان Media (صورة الشهادة)
    [ForeignKey(nameof(Media))]
    public int? MediaId { get; set; }
    public virtual Media? CertificateMedia { get; set; }

    // العلاقة المتبادلة
    public virtual ServiceProviderProfile ServiceProviderProfile { get; set; } = null!;
}
