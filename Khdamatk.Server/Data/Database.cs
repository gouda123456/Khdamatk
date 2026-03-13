using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Khdamatk.Server.Data;

public class Database(DbContextOptions<Database> options,IHttpContextAccessor contextAccessor, IConfiguration configuration) : IdentityDbContext<User,Role,string>(options)
{
    
    
    private readonly HttpContext httpContextAccessor = contextAccessor?.HttpContext!;
    private readonly IConfiguration configuration = configuration;

    protected override void OnModelCreating(ModelBuilder builder)
    {

        var foreKeys = builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
        foreach (var key in foreKeys)
        {
            key.DeleteBehavior = DeleteBehavior.Restrict;
        }

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // سيتم تحويل أي Enum في أي Entity إلى String في قاعدة البيانات تلقائياً
        configurationBuilder
            .Properties<Enum>()
            .HaveConversion<string>();
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //var userid = httpContextAccessor.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userid = httpContextAccessor.User.FindFirst(JWTClaimsDefault.UserId)?.Value;
        if (userid is { })
        {
            var entrirs = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entrirs)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(p => p.CreatedBy).CurrentValue = userid;
                    entry.Property(p => p.Createdat).CurrentValue = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(p => p.UpdatedBy).CurrentValue = userid;
                    entry.Property(p => p.Updatedat).CurrentValue = DateTime.UtcNow;

                    // إضافة سطرين لضمان عدم تغيير بيانات الإنشاء
                    entry.Property(p => p.CreatedBy).IsModified = false;
                    entry.Property(p => p.Createdat).IsModified = false;
                }
            }
        }


        return base.SaveChangesAsync(cancellationToken);
    }




    #region Catalog DbSets
    public DbSet<Category> Categories { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<ServiceMedia> ServiceMedia { get; set; }
    public DbSet<Media> Medias { get; set; }
    public DbSet<Certificate> Certificates { get; set; }
    public DbSet<PortfolioItem> PortfolioItems { get; set; }
    public DbSet<PortfolioMedia> PortfolioMedia { get; set; }
    public DbSet<ProviderSkill> ProviderSkills { get; set; }
    public DbSet<JobPost> JobPosts { get; set; }
    public DbSet<JobOffer> jobOffers { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<JobSkillRequirement> JobSkillRequirements { get; set; }
    public DbSet<MileStone> MileStones { get; set; }

    #endregion

    #region Financial DbSets
    public DbSet<CreditCard> CreditCards { get; set; }
    public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
    #endregion

    #region Identity DbSets
    public DbSet<RefreshTokens> RefreshTokens { get; set; }
    public DbSet<ServiceProviderProfile> ServiceProviderProfiles { get; set; }
    public DbSet<VerificationData> VerificationData { get; set; }

    public DbSet<VerificationsCodes> VerificationsCodes { get; set; }
    #endregion

    #region Interactions DbSets
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<Dispute> Disputes { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Review> Reviews { get; set; }
    #endregion

    #region operations DbSets
    public DbSet<ServiceOrder> ServiceOrders { get; set; }
    public DbSet<JobOrder> jobOrders { get; set; }
    public DbSet<UserFavorites> UserFavorites { get; set; }
    #endregion


}
