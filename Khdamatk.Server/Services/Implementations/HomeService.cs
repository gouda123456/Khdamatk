using Khdamatk.Server.Contracts.Home;
using System.Linq.Dynamic.Core;

namespace Khdamatk.Server.Services.Implementations
{
    public class HomeService(Database db) : IHomeService
    {
        private readonly Database db = db;

        public async Task<HomeMainResponse> HomeMain()
        {
            var Data = new HomeMainResponse(
                Categories: await db.Categories.Select(c => c.Name).ToListAsync(),

                );

            throw new NotImplementedException();
        }
    }
}
