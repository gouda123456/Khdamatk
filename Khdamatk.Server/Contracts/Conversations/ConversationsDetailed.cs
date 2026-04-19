namespace Khdamatk.Server.Contracts.Conversations;

public record ConversationsDetailed(
    int Id,
    string Title,
    string userId,
    string userName,
    string userImageUrl,
    string ClientId,
    string ClientName,
    string ClientImageUrl,
    List<ConversationMessageResponse> Messages
);
