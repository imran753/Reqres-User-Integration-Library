using Microsoft.Extensions.Caching.Memory;
using ReqresUserLibrary.Clients;
using ReqresUserLibrary.Models;

namespace ReqresUserLibrary.Services
{
    public class ExternalUserService(IReqresApiClient client, IMemoryCache cache) : IExternalUserService
    {
        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            return await cache.GetOrCreateAsync($"user_{userId}", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return await client.GetUserByIdAsync(userId);
            });
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = new List<UserDto>();
            int page = 1;
            PaginatedUsersDto? result;

            do
            {
                result = await client.GetUsersByPageAsync(page);
                if (result?.Data != null)
                    users.AddRange(result.Data);
                page++;
            } while (result != null && page <= result.Total_Pages);

            return users;
        }
    }
}
