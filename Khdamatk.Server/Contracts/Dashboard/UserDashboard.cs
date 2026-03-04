namespace Khdamatk.Server.Contracts.Dashboard;

/// Represents a user row in the User Management data table.
public record UserListItem(
    string UserId,
    string FullName,
    string Email,
    string Role,
    int JobsCount,
    string Status,
    DateTime JoinDate
);

///Request to change user status (Verify, Block, Active). 
public record UpdateUserStatusRequest(string UserId, string NewStatus);

/// Request to modify user platform roles. 
public record UpdateRoleRequest(string UserId, string NewRole);