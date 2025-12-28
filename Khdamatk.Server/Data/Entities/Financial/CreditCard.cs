namespace Khdamatk.Server.Data.Entities.Financial;


public class CreditCard
{
    
    [Key]
    public int Id { get; set; }

    [Required]
    public string Tokenized { get; set; } = null!;

    [Required]
    public string Last4Digits { get; set; } = null!;

    [Required]
    public DateTime ExpirationDate { get; set; }

    [Required]
    public string CardType { get; set; } = null!;

    [ForeignKey(nameof(User))]
    public string UserId { get; set; } = null!;

    public User User { get; set; } = null!;


    public List<PaymentTransaction> Transactions { get; set; } = [];

}
