namespace Khdamatk.Server.Contracts.Conversations;

public record ConversationsSummaryResponse(
    string userId,
    string userName,
    string userImageUrl,
    string Title,
    string LastMessageText,
    DateTime SentAt,
    bool IsRead,
    int orderId
    );

    
public record ConversationMessageResponse(
    int messageId,
    string messageText,
    string senderId,
    DateTime sentAt
    );