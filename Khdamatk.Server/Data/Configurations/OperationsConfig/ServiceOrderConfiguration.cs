using Microsoft.EntityFrameworkCore;

namespace Khdamatk.Server.Data.Configurations.OperationsConfig;

public class ServiceOrderConfiguration : IEntityTypeConfiguration<ServiceOrder>
{
    public void Configure(EntityTypeBuilder<ServiceOrder> builder)
    {
        

        builder.Property(uf => uf.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(uf => uf.Customer)
                .WithMany() // إذا لم يكن هناك قائمة Favorites في كيان User
                .HasForeignKey(uf => uf.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); // نفضل Restrict أو NoAction لتجنب الحذف المتتالي

       

        


       


        


    }
}
