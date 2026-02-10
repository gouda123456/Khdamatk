
using Khdamatk.Server.Contracts.Home;
using System.Linq.Dynamic.Core;

public class HomeService(Database db) : IHomeService
{
    private readonly Database db = db;

    {
        

    }

    public async Task<resultBase> JobsPage(string? service, CancellationToken cancellationToken)
    {
       return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message); 
    }


}
