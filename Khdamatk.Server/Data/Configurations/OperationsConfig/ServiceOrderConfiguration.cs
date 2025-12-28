using Microsoft.EntityFrameworkCore;

namespace Khdamatk.Server.Data.Configurations.OperationsConfig;

public class ServiceOrderConfiguration : IEntityTypeConfiguration<ServiceOrder>
{
    public void Configure(EntityTypeBuilder<ServiceOrder> builder)
    {
        

        builder.Property(uf => uf.Createdat)
                .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(uf => uf.User)
                .WithMany() // إذا لم يكن هناك قائمة Favorites في كيان User
                .HasForeignKey(uf => uf.UserID)
                .OnDelete(DeleteBehavior.Restrict); // نفضل Restrict أو NoAction لتجنب الحذف المتتالي

        builder.HasOne(uf => uf.Service)
                .WithMany() // إذا لم يكن هناك قائمة FavoriteByUsers في كيان Service
                .HasForeignKey(uf => uf.ServiceID)
                .OnDelete(DeleteBehavior.Restrict);


        // مثال للتكوين الإلزامي في DbContext.OnModelCreating
        builder
            .HasOne(o => o.Review)
            .WithOne(r => r.ServiceOrder) // يجب إضافة Navigation Property عكسية في Review
            .HasForeignKey<Review>(r => r.OrderId); // هنا OrderId هو المفتاح الخارجي


        


    }
}
