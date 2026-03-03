using Khdamatk.Server.Contracts.Home;
using Khdamatk.Server.Contracts.Jobs;

namespace Khdamatk.Server.Configurations.Mapping;

public class MapsterConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, User>()
            .Map(dest => dest.UserName, src => src.userName)
            .Map(dest => dest.Email, src => src.Email)
            .Map(src => src.PhoneNumber, dest => dest.PhoneNumber)
            .IgnoreNonMapped(true);

        config.NewConfig<JobPost, JobCard>()
            .Map(dest => dest.Id, src => src.Id.ToString())
            .Map(dest => dest.JobTitle, src => src.Title)
            .Map(dest => dest.JobDescription, src => src.Description)
            .Map(dest => dest.PostedDate, src => src.CreatedAt)
            .Map(dest => dest.BudgetMin, src => src.BudgetMin)
            .Map(dest => dest.BudgetMax, src => src.BudgetMax)
            .IgnoreNonMapped(true);

        config.NewConfig<JobPost, jobDetailed>()
            //Mapping (id , title , offer count , Expert Level , Project length , budget)
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.OffersCount, src => src.Offers != null ? src.Offers.Count() : 0)
            .Map(dest => dest.ExperienceLevel, src => src.ExperienceLevel)
            .Map(dest => dest.ProjectLength, src => src.ProjectLength)
            .Map(dest => dest.BudgetMin, src => src.BudgetMin)
            .Map(dest => dest.BudgetMax, src => src.BudgetMax)

            //Mapping (description , customerId , customerName , categoryId , categoryName , deadline , createdAt  , status , timeCommitment)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.CustomerId, src => src.CustomerId)
            .Map(dest => dest.CustomerName, src => src.Customer.UserName)
            .Map(dest => dest.CategoryId, src => src.CategoryId)
            .Map(dest => dest.CategoryName, src => src.Category.Name)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.Deadline, src => src.Deadline)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.TimeCommitment, src => src.TimeCommitment)

            //Mapping (ImageUrls , RequiredSkills)
            .Map(dest => dest.ImageUrls, src => src.Images != null
                ? src.Images.Select(img => img.FileName)
                : new List<string>())
            .Map(dest => dest.RequiredSkills, src => src.SkillRequirements != null
                ? src.SkillRequirements.Select(s => s.Skill.Name)
                : new List<string>())
            .IgnoreNonMapped(true)
            .TwoWays();

        config.NewConfig<AddJopRequest, JobPost>()
            .Map(dest => dest.CustomerId, src => src.UserId)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.BudgetMin, src => src.BudgetMin)
            .Map(dest => dest.BudgetMax, src => src.BudgetMax)
            .Map(dest => dest.Deadline, src => src.Deadline)
            .Map(dest => dest.ExperienceLevel, src => src.ExperienceLevel)
            .Map(dest => dest.TimeCommitment, src => src.TimeCommitment.ToString())
            // Mapping CategoryName to CategoryId will require a custom resolver or additional logic
            .IgnoreNonMapped(true)
            .TwoWays();


        config.NewConfig<AddJopOfferRequest, JobOffer>()
            .Map(dest => dest.ProviderProfileId, src => src.ProviderServiceId)
            .Map(dest => dest.NetAmount, src => src.OfferAmount)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.TimeCommitment, src => src.TimeCommitment)
            .Map(dest => dest.ExperienceLevel, src => src.ExperienceLevel)
            .Map(dest => dest.Deadline, src => src.Deadline)
            
            .IgnoreNonMapped(true)
            .TwoWays();



    }
}
