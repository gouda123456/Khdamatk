
namespace Khdamatk.Server.Data.Configurations.OperationsConfig;

public class UserFavoritesConfiguration : IEntityTypeConfiguration<UserFavorites>
{
    public void Configure(EntityTypeBuilder<UserFavorites> builder)
    {
        builder.HasKey(uf => new { uf.UserID, uf.ServiceID });

        builder.Property(uf => uf.AddedTime)
                .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(uf => uf.User)
                .WithMany() // إذا لم يكن هناك قائمة Favorites في كيان User
                .HasForeignKey(uf => uf.UserID)
                .OnDelete(DeleteBehavior.Restrict); // نفضل Restrict أو NoAction لتجنب الحذف المتتالي

        builder.HasOne(uf => uf.Service)
                .WithMany() // إذا لم يكن هناك قائمة FavoriteByUsers في كيان Service
                .HasForeignKey(uf => uf.ServiceID)
                .OnDelete(DeleteBehavior.Restrict);

        builder.Property(uf => uf.UserID)
       .HasColumnName("User_FK_ID");


    }
}
