var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSingleton<ITencentCloudSDKCredential, TencentCloudSDKCredential>()
    .AddScoped<ITencentCloudSDKClient, TencentCloudSDKClient>()
    .AddControllers();


var app = builder.Build();

app.MapControllers();

app.Run();