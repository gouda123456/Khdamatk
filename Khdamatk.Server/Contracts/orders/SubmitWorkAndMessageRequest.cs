namespace Khdamatk.Server.Contracts.orders;

public record SubmitWorkAndMessageRequest(
    string Message,
    List<IFormFile>? Attachments
    );
