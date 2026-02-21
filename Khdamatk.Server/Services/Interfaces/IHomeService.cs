
﻿using Khdamatk.Server.Contracts.Home;

namespace Khdamatk.Server.Services.Interfaces;

public interface IHomeService : IService
{
    Task<resultBase> MainPage(CancellationToken cancellationToken);
    Task<resultBase> JobsPage(string? service, CancellationToken cancellationToken);
    Task<resultBase> FreelancersPage(FreelancerRequest freelancerRequest, CancellationToken cancellationToken);
    Task<resultBase> FreelancerProfile(string userId, CancellationToken cancellationToken);
}