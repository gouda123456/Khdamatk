namespace Khdamatk.Server.Data.Entities.Interaction;

public class Review : BaseEntity
{
    [Required]
    [StringLength(30, MinimumLength = 2)] 
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(500, MinimumLength = 2)] 
    public string Content { get; set; } = string.Empty;

    [Required]
    [Range(1, 5)]
    public double Rating { get; set; }

    // === Foreign Keys ===

    [Required]
    [ForeignKey(nameof(ServiceOrder))]
    public int OrderId { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    // تم تغيير الاسم لزيادة الوضوح
    public string ReviewerId { get; set; }

    [Required]
    [ForeignKey(nameof(ServiceProvider))]
    public string ServiceProviderId { get; set; }

    // === Navigation Properties ===
    public virtual ServiceOrder ServiceOrder { get; set; } = null!;
    // يفضل أن يكون نوع User هنا هو الـ User Entity الخاص بنطاق الهوية
    public virtual User Reviewer { get; set; } = null!;
    //مقدم الخدمة
    public virtual ServiceProviderProfile ServiceProvider { get; set; } = null!;
}
