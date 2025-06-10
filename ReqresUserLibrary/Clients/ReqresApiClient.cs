using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReqresUserLibrary.Configuration;
using ReqresUserLibrary.Models;
using System.Net.Http.Json;

namespace ReqresUserLibrary.Clients
{
    public class ReqresApiClient(HttpClient httpClient, IOptions<ReqresApiOptions> options, ILogger<ReqresApiClient> logger) : IReqresApiClient
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly string _baseUrl = options.Value.BaseUrl.TrimEnd('/');

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiUserResponse>($"{_baseUrl}/users/{userId}");
                return response?.Data;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get user by ID");
                return null;
            }
        }

        public async Task<PaginatedUsersDto?> GetUsersByPageAsync(int page)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<PaginatedUsersDto>($"{_baseUrl}/users?page={page}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get users by page");
                return null;
            }
        }

        private class ApiUserResponse
        {
            public UserDto? Data { get; set; }
        }
    }
}
