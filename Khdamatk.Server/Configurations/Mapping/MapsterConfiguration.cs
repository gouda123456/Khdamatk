namespace Khdamatk.Server.Configurations.Mapping;

public class MapsterConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, User>()
            .Map(dest => dest.UserName, src => src.userName)
            .Map(dest => dest.Email, src => src.Email)
            .IgnoreNonMapped(true);
    }
}
