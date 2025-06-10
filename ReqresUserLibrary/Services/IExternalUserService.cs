using ReqresUserLibrary.Models;

namespace ReqresUserLibrary.Services
{
    public interface IExternalUserService
    {
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
    }
}
