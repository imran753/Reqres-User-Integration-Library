using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReqresUserLibrary.Configuration;
using ReqresUserLibrary.Models;
using ReqresUserLibrary.Services;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

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
            catch (JsonException ex)
            {
                logger.LogError(ex, "Deserialization failed for userId {UserId}", userId);
                throw new ExternalServiceException("Deserialization failed", ex);
            }
            catch (TaskCanceledException ex) when (!ex.CancellationToken.IsCancellationRequested)
            {
                logger.LogError(ex, "Request timed out");
                throw new ExternalServiceException("Request timed out", ex);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception in GetUserByIdAsync");
                throw new ExternalServiceException("Unexpected failure", ex);
            }
        }

        public async Task<PaginatedUsersDto?> GetUsersByPageAsync(int page)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<PaginatedUsersDto>($"{_baseUrl}/users?page={page}");
            }
            catch (JsonException ex)
            {
                logger.LogError(ex, "Deserialization failed for users");
                throw new ExternalServiceException("Deserialization failed", ex);
            }
            catch (TaskCanceledException ex) when (!ex.CancellationToken.IsCancellationRequested)
            {
                logger.LogError(ex, "Request timed out");
                throw new ExternalServiceException("Request timed out", ex);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception in GetUsersByPageAsync");
                throw new ExternalServiceException("Unexpected failure", ex);
            }
        }

        private class ApiUserResponse
        {
            public UserDto? Data { get; set; }
        }
    }
}
