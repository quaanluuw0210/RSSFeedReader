using Microsoft.AspNetCore.Http;
using RSSFeedReader.Api.Models;
using RSSFeedReader.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<SubscriptionStore>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalFrontend", policy =>
        policy.WithOrigins("http://localhost:5213", "https://localhost:7025")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("LocalFrontend");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/api/subscriptions", (SubscriptionStore store) =>
    Results.Ok(store.GetAll()));

app.MapPost("/api/subscriptions", IResult (AddSubscriptionRequest? request, SubscriptionStore store) =>
{
    if (request is null || !store.TryAdd(request.Url, out var subscription))
    {
        return Results.BadRequest();
    }

    return Results.Ok(subscription);
});

app.Run();

public partial class Program
{
}

public sealed record AddSubscriptionRequest(string? Url);
