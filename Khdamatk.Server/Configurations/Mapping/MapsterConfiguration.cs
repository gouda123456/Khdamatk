using Khdamatk.Server.Contracts.Home;

namespace Khdamatk.Server.Configurations.Mapping;

public class MapsterConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, User>()
            .Map(dest => dest.UserName, src => src.userName)
            .Map(dest => dest.Email, src => src.Email)
            .Map(src => src.PhoneNumber , dest => dest.PhoneNumber)
            .IgnoreNonMapped(true);
        
        config.NewConfig<JobPost, JobCard>()
            .Map(dest => dest.Id, src => src.Id.ToString())
            .Map(dest => dest.JobTitle, src => src.Title)
            .Map(dest => dest.JobDescription, src => src.Description)
            .Map(dest => dest.PostedDate, src => src.CreatedAt)
            .Map(dest => dest.BudgetMin, src => src.BudgetMin)
            .Map(dest => dest.BudgetMax, src => src.BudgetMax)
            .IgnoreNonMapped(true);
    }
}
