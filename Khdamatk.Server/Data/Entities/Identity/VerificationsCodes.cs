namespace Khdamatk.Server.Data.Entities.Identity;

public class VerificationsCodes :BaseEntity
{
    


    public VerificationCodeType Type { get; set; }

    [Range(VerificationsCodesConstrains.MinValue, VerificationsCodesConstrains.MaxValue)]
    public int Value { get; private set; }
    public bool IsUsed { get; private set; } = false;

    public bool IsActive => DateTime.UtcNow < Createdat.AddDays(1) && IsUsed && !IsDelete;

    [ForeignKey(nameof(User))]
    public string UserId { get; set; } = string.Empty;
    public User User { get; set; } = null!;



    public VerificationsCodes()
    {
        
    }

    public VerificationsCodes(VerificationCodeType type)
    {
        Type = type;
        Value = new Random().Next(VerificationsCodesConstrains.MinValue, VerificationsCodesConstrains.MinValue);
        
    }



    public void GenerateNewValue()
    {
        Value = new Random().Next(VerificationsCodesConstrains.MinValue, VerificationsCodesConstrains.MinValue);
    }

    

}

public enum VerificationCodeType
{
    changePassword,
    changeEmail,
    changePhone,
}