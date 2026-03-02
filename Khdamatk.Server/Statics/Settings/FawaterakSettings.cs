namespace Khdamatk.Server.Statics.Settings;

public class FawaterakSettings
{
    [Required]
    public string ApiKey { get; set; } = string.Empty;
    [Required]
    public string BaseUrl { get; set; } = string.Empty;
    [Required]
    public string ProviderKey { get; set; } = string.Empty;
}
