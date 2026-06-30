using Khdamatk.Server.Contracts.Fawaterak;
using Khdamatk.Server.Helper.Validations;

namespace Khdamatk.Server.Contracts.orders;

public record StartServiceOrderPaymentRequest
    (
    EInvoiceRequestModel Order,
    string? AdditionalDetails,
    List<IFormFile> Attachments,
    int ServiceId);
    