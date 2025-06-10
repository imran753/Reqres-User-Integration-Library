using ReqresUserLibrary.Models;

namespace ReqresUserLibrary.Clients
{
    public interface IReqresApiClient
    {
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<PaginatedUsersDto?> GetUsersByPageAsync(int page);
    }
}
