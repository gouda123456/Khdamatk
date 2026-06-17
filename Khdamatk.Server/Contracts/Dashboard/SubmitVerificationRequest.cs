using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Khdamatk.Server.Contracts.Verification;

public class SubmitVerificationRequest
{
    [Required(ErrorMessage = "برجاء إدخال الرقم القومي")]
    public string NationalNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "برجاء إدخال الدولة")]
    public string Country { get; set; } = string.Empty;

    [Required(ErrorMessage = "برجاء إدخال المدينة")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "برجاء رفع صورة وجه البطاقة")]
    public IFormFile IdFront { get; set; }

    [Required(ErrorMessage = "برجاء رفع صورة ظهر البطاقة")]
    public IFormFile IdBack { get; set; }

    [Required(ErrorMessage = "برجاء رفع الصورة الشخصية مع البطاقة")]
    public IFormFile SelfieWithId { get; set; }
}