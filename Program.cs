var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddMemoryCache()
	.AddSingleton<ITencentCloudSDKCredential, TencentCloudSDKCredential>()
	.AddScoped<ITencentCloudSDKClient, TencentCloudSDKClient>()
	.AddSingleton<IConfirmSign, ConfirmSign>()
	.AddControllers();


var app = builder.Build();

app.MapControllers();

app.Run();