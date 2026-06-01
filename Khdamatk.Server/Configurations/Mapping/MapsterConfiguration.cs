using System.Data;
using Khdamatk.Server.Contracts.Fawaterak;
using Khdamatk.Server.Contracts.Home;
using Khdamatk.Server.Contracts.Jobs;
using Khdamatk.Server.Contracts.orders;
using Khdamatk.Server.Contracts.Service;

namespace Khdamatk.Server.Configurations.Mapping;

public class MapsterConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {

        config.NewConfig<RegisterRequest, User>()
            .Map(dest => dest.UserName, src => src.userName)
            .Map(dest => dest.Email, src => src.Email)
            .Map(src => src.PhoneNumber, dest => dest.PhoneNumber)
            .TwoWays()
            .IgnoreNonMapped(true)
            .IgnoreNullValues(true);




        config.NewConfig<JobPost, JobCard>()
            .Map(dest => dest.Id, src => src.Id.ToString())
            .Map(dest => dest.JobTitle, src => src.Title)
            .Map(dest => dest.JobDescription, src => src.Description)
            .Map(dest => dest.PostedDate, src => src.CreatedAt)
            .Map(dest => dest.BudgetMin, src => src.BudgetMin)
            .Map(dest => dest.BudgetMax, src => src.BudgetMax)
            .TwoWays()
            .IgnoreNonMapped(true)
            .IgnoreNullValues(true);





        config.NewConfig<JobPost, JobDetailed>()
            //Mapping (id , title , offer count , Expert Level , Project length , budget)
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.OffersCount, src => (src.Offers != null) ? src.Offers.Count() : 0)
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
            .Map(dest => dest.ImageUrls, src => src.Media != null
                ? src.Media.Select(img => img.FileName)
                : new List<string>())
            .Map(dest => dest.RequiredSkills, src => src.SkillRequirements != null
                ? src.SkillRequirements.Select(s => s.Skill.Name)
                : new List<string>())
            .TwoWays()
            .IgnoreNonMapped(true)
            .IgnoreNullValues(true);



        config.NewConfig<AddJobRequest, JobPost>()
            .Map(dest => dest.CustomerId, src => src.UserId)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.BudgetMin, src => src.BudgetMin)
            .Map(dest => dest.BudgetMax, src => src.BudgetMax)
            .Map(dest => dest.Deadline, src => src.Deadline)
            .Map(dest => dest.ExperienceLevel, src => src.ExperienceLevel)
            .Map(dest => dest.TimeCommitment, src => src.TimeCommitment)
            
            // Mapping CategoryName to CategoryId will require a custom resolver or additional logic
            .TwoWays()
            .IgnoreNonMapped(true)
            .IgnoreNullValues(true);



        config.NewConfig<AddJopOfferRequest, JobOffer>()
            .Map(dest => dest.ProviderProfileId, src => src.ProviderServiceId)
            .Map(dest => dest.Amount, src => src.OfferAmount)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.SimilarWorkExamplesURL, src => src.SimilarWorkExamplesURL)
            .Map(dest => dest.TimeCommitment, src => src.TimeCommitment)
            .Map(dest => dest.ExperienceLevel, src => src.ExperienceLevel)

            .Map(dest => dest.Attachments, src => src.Attachment != null
                ? src.Attachment.Select(m =>
                
                    new Media
                    {
                        FileName = m.FileName,
                        
                        ContentType = m.ContentType,
                        FileExtension = Path.GetExtension(m.FileName),
                        Size = m.Length
                    }
                )
                : new List<Media>())
            .TwoWays()
            .IgnoreNonMapped(true)
            .IgnoreNullValues(true);





        config.NewConfig<Service,AddServiceRequest>()
            .Map(dest => dest.ProviderProfileId, src => src.ServiceProviderProfileId)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.ShortDescription, src => src.ShortDescription)
            .Map(dest => dest.DetailedDescription, src => src.DetailedDescription)
            .Map(dest => dest.Concepts, src => src.Concepts)
            .Map(dest => dest.RevisionCount, src => src.RevisionCount)
            .Map(dest => dest.Price, src => src.Price)
            .Map(dest => dest.DeliverTimeInDays, src => src.DeliveryTimeInDays)
            .Map(dest => dest.ServiceEnvelope, src => src.MainImage != null
                ?   new Media
                    {
                        FileName = src.MainImage.FileName ?? string.Empty,
                        
                        ContentType = src.MainImage.ContentType ?? string.Empty,
                        FileExtension = Path.GetExtension(src.MainImage.FileName ?? string.Empty),
                        Size = src.MainImage.Size
                }
                
                : null)
            .TwoWays()
            .IgnoreNonMapped(true)
            .IgnoreNullValues(true);



        config.NewConfig<Service, ServiceDetailsResponse>()
            .Map(dest => dest.ServiceId, src => src.Id)
            .Map(dest => dest.ServiceTitle, src => src.Title)
            .Map(dest => dest.ShortDescription, src => src.ShortDescription)
            .Map(dest => dest.DetailDescription, src => src.DetailedDescription)
            .Map(dest => dest.Price, src => src.Price)
            .Map(dest => dest.RevisionCount, src => src.RevisionCount)
            .Map(dest => dest.DeliveryTimeInDays, src => src.DeliveryTimeInDays)
            .Map(dest => dest.ExperienceLevel, src => ExperienceLevel.Intermediate) // Default value until Service entity is updated
            .Map(dest => dest.Concepts, src => src.Concepts)
            .Map(dest => dest.MainImage, src => src.ServiceProviderProfile.User.ProfilePicture != null ? System.IO.File.ReadAllBytes(src.ServiceProviderProfile.User.ProfilePicture.FullPath) : null)
            .Map(dest => dest.ServiceImages, src => new List<byte[]>())
            .Map(dest => dest.OrdersCount, src => src.Orders.Count)
            .Map(dest => dest.AverageRating, src => src.ServiceProviderProfile.AverageRating)
            .Map(dest => dest.ProviderServiceInfo, src => new ProviderServiceInfo(
                src.ServiceProviderProfileId.ToString(),
                src.ServiceProviderProfile.User.UserName,
                src.ServiceProviderProfile.JobTitle,
                System.IO.File.ReadAllBytes(src.ServiceProviderProfile.User.ProfilePicture.FullPath),
                src.ServiceProviderProfile.AverageRating,
                (int)src.ServiceProviderProfile.AverageResponseTime,
                (int)src.Orders.Where(s => s.Status == OrderStatus.Active).Count(),
                (int)src.Orders.Where(s => s.Status == OrderStatus.Pending || s.Status == OrderStatus.PendingPayment).Count(),
                (int)src.Orders.Where(s => s.Status == OrderStatus.Completed).Count()
            ))
            .TwoWays()
            .IgnoreNonMapped(true)
            .IgnoreNullValues(true);



        
        config.NewConfig<JobOffer, OfferForServiceResponse>()
            .Map(dest => dest.ProviderOfferInfo.ProviderId, src => src.ProviderProfileId)
            .Map(dest => dest.ProviderOfferInfo.ProviderName, src => src.ProviderProfile.User.UserName)
            .Map(dest => dest.ProviderOfferInfo.ProviderJobTitle, src => src.ProviderProfile.JobTitle)
            .Map(dest => dest.ProviderOfferInfo.ProviderRate, src => src.ProviderProfile.AverageRating)
            .Map(dest => dest.OfferPrice, src => src.Amount)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.DeliverAt, src => DateTime.UtcNow.AddDays(src.DeliveryTimeInDays))
            .TwoWays()
            .IgnoreNonMapped(true)
            .IgnoreNullValues(true);

        
        config.NewConfig<JobOffer, OfferDetailedForServiceResponse>()

            //Provider Info
            .Map(dest => dest.ProviderOfferInfo.Id, src => src.ProviderProfile.UserId)
            .Map(dest => dest.ProviderOfferInfo.Name, src => src.ProviderProfile.User.UserName)
            .Map(dest => dest.ProviderOfferInfo.JobTitle, src => src.ProviderProfile.JobTitle)
            .Map(dest => dest.ProviderOfferInfo.Address, src => (src.ProviderProfile.User != null && src.ProviderProfile.User!.IsVerified) ? $"{src.ProviderProfile.User!.VerificationData!.Country},{src.ProviderProfile.User.VerificationData.City}" : "Not Have A Address he is Not verified")
            .Map(dest => dest.ProviderOfferInfo.ExperienceInYears, src => src.ProviderProfile.ExperienceYears)
            .Map(dest => dest.ProviderOfferInfo.IsVerified, src => src.ProviderProfile.User!.IsVerified)
            .Map(dest => dest.ProviderOfferInfo.ProviderRate, src => src.ProviderProfile.AverageRating)
            .Map(dest => dest.ProviderOfferInfo.ProviderProfile, src => src.ProviderProfile.User.ProfilePicture)

            //OfferServiceDetailed
            .Map(dest => dest.OfferServiceDetailed.Id, src => src.Id)
            .Map(dest => dest.OfferServiceDetailed.Amount, src => src.Amount)
            .Map(dest => dest.OfferServiceDetailed.DeliversAt, src => DateTime.UtcNow.AddDays(src.DeliveryTimeInDays))
            .Map(dest => dest.OfferServiceDetailed.Description, src => src.Description)
            
            //JobSummary
            .Map(dest => dest.JobSummary.Id, src => src.JobPost.Id)
            .Map(dest => dest.JobSummary.BudgetMin, src => src.JobPost.BudgetMin)
            .Map(dest => dest.JobSummary.BudgetMax, src => src.JobPost.BudgetMax)
            .Map(dest => dest.JobSummary.DeliversInDays, src => (src.JobPost.Deadline - DateTime.UtcNow).Days)
            .Map(dest => dest.JobSummary.Deadline, src => src.JobPost.Deadline )
            .Map(dest => dest.JobSummary.ExperienceLevel, src => src.JobPost.ExperienceLevel)
            .Map(dest => dest.JobSummary.Skills, src => src.JobPost.SkillRequirements.Select(s => s.Skill.Name).Distinct())
            .Map(dest => dest.JobSummary.Description, src => src.JobPost.Description)
            .Map(dest => dest.JobSummary.MileStones, src => src.JobPost.MileStones)

            .TwoWays()
            .IgnoreNonMapped(true)
            .IgnoreNullValues(true);


        config.NewConfig<JobOrder, JobOrderResponse>()
            .Map(dest => dest.OrderId, src => src.Id)
            .Map(dest => dest.OrderType, src => OrderType.Job)
            .Map(dest => dest.FinalPrice, src => src.Amount)

            //customer
            .Map(dest => dest.Customer.Id, src => src.CustomerId)
            .Map(dest => dest.Customer.Name, src => src.Customer.UserName)
            .Map(dest => dest.Customer.Email, src => src.Customer.Email)
            //Profile Picture 

            //Provider
            .Map(dest => dest.Provider.Id, src => src.ServiceProviderId)
            .Map(dest => dest.Provider.Name, src => src.ServiceProviderProfile.User.UserName)
            .Map(dest => dest.Provider.Email, src => src.ServiceProviderProfile.User.Email)
            //Profile Picture 

            //JobSummary
            .Map(dest => dest.JobSummary.Id, src => src.Job.Id)
            .Map(dest => dest.JobSummary.BudgetMin, src => src.Job.BudgetMin)
            .Map(dest => dest.JobSummary.BudgetMax, src => src.Job.BudgetMax)
            .Map(dest => dest.JobSummary.DeliversInDays, src => (src.Job.Deadline - DateTime.UtcNow).Days)
            .Map(dest => dest.JobSummary.Deadline, src => src.Job.Deadline)
            .Map(dest => dest.JobSummary.ExperienceLevel, src => src.Job.ExperienceLevel)
            .Map(dest => dest.JobSummary.Skills, src => src.Job.SkillRequirements.Select(s => s.Skill.Name).Distinct())
            .Map(dest => dest.JobSummary.Description, src => src.Job.Description)
            .Map(dest => dest.JobSummary.MileStones, src => src.Job.MileStones)

            //Chat
            .Map(dest => dest.Chat, src => src.Conversation.Messages.Select(m => new OrderChat
                (
                    m.Id,
                    m.SenderId,
                    m.Content,
                    m.CreatedAt
                )))


            .TwoWays()
            .IgnoreNonMapped(true)
            .IgnoreNullValues(true);


    }
}
