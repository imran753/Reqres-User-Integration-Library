﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using ReqresUserLibrary.Clients;
using ReqresUserLibrary.Configuration;
using ReqresUserLibrary.Services;

var host = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile("appsettings.json", optional: false);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddOptions<ReqresApiOptions>()
        .Bind(context.Configuration.GetSection("ReqresApi"))
        .Validate(opt => !string.IsNullOrEmpty(opt.BaseUrl), "BaseUrl is required")
        .Validate(opt => !string.IsNullOrEmpty(opt.ApiKey), "ApiKey is required");
        services.Configure<ReqresApiOptions>(context.Configuration.GetSection("ReqresApi"));
        services.AddHttpClient<IReqresApiClient, ReqresApiClient>((sp, client) =>
        {
            var config = sp.GetRequiredService<IOptions<ReqresApiOptions>>().Value;
            client.BaseAddress = new Uri(config.BaseUrl);
            client.DefaultRequestHeaders.Add("x-api-key", config.ApiKey);
        }).AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
        services.AddMemoryCache();
        services.AddScoped<IExternalUserService, ExternalUserService>();
    })
    .Build();

using var scope = host.Services.CreateScope();
var service = scope.ServiceProvider.GetRequiredService<IExternalUserService>();


Console.WriteLine($"Details of all the users");
var users = await service.GetAllUsersAsync();
foreach (var user in users)
{
    Console.WriteLine($"Id - {user.Id}: Name - {user.First_Name} {user.Last_Name}");
}

Console.WriteLine($"Get details of a user enter userid: ");

int userid = int.Parse(Console.ReadLine());

var userdetails = await service.GetUserByIdAsync(userid);
Console.WriteLine($"Id - {userdetails.Id} : Name - {userdetails.First_Name} {userdetails.Last_Name}");
Console.WriteLine($"Email - {userdetails.Email}");
Console.WriteLine($"Avatar - {userdetails.Avatar}");