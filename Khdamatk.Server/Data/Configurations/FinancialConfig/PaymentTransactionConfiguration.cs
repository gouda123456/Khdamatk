namespace Khdamatk.Server.Data.Configurations.FinancialConfig;

public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
    {
        // 1. المفتاح الأساسي
        builder.HasKey(t => t.Id);

        // 2. ضمان الدقة المالية (تأكيد على Data Annotation)
        builder.Property(t => t.Amount)
               .HasColumnType("decimal(18, 2)")
               .IsRequired();

        builder.Property(t => t.PlatformFee)
               .HasColumnType("decimal(18, 2)")
               .IsRequired();

        builder.Property(t => t.NetPayout)
               .HasColumnType("decimal(18, 2)")
               .IsRequired();

        // 3. العلاقة (1-1) مع ServiceOrder
        // كل طلب يجب أن يكون له معاملة دفع واحدة على الأكثر (والمعاملة تنتمي لطلب واحد)
        builder.HasOne(t => t.ServiceOrder)
               .WithOne(o => o.PaymentTransaction) // افترضنا أن ServiceOrder يحتوي على خاصية PaymentTransaction
               .HasForeignKey<ServiceOrder>(t => t.PaymentTransactionId) 
               // المفتاح الخارجي في هذا الكيان
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.JobOrder)
               .WithOne(j => j.PaymentTransaction) // افترضنا أن JobOrder يحتوي على خاصية PaymentTransaction
               .HasForeignKey<JobOrder>(fk => fk.PaymentTransactionId) // المفتاح الخارجي في هذا الكيان
               .IsRequired(false) // المعاملة قد لا تكون مرتبطة بـ JobOrder
               .OnDelete(DeleteBehavior.Restrict); // منع حذف الطلب إذا كانت هناك معاملة مرتبطة به

        // 4. العلاقة مع CreditCard (TokenizedCard)
        builder.HasOne(t => t.TokenizedCard)
               .WithMany() // إذا كان CreditCard لا يحتوي على قائمة معاملات
               .HasForeignKey(t => t.TokenizedCreditCardId)
               .OnDelete(DeleteBehavior.Restrict);

        
    }
}
