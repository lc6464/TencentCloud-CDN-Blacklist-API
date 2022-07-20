namespace TencentCloudCDNBlacklistAPI.Services;
public class TencentCloudSDKCredential: ITencentCloudSDKCredential {
	public TencentCloudSDKCredential(IConfiguration configuration) {
		Credential = new() { SecretId = configuration["Secrets:Id"], SecretKey = configuration["Secrets:Key"] };
	}

	public Credential Credential { get; init; }
}