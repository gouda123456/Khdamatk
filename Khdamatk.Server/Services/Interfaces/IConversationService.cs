namespace Khdamatk.Server.Services.Interfaces;

public interface IConversationService
{
    Task<resultBase> GetJoborderConversations(string userId);
    Task<resultBase> GetJoborderConversation(string userId, int Joborder); 
    Task<resultBase> GetServiceorderConversations(string userId);
    Task<resultBase> GetServiceorderConversation(string userId, int Joborder);


}
