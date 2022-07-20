namespace TencentCloudCDNBlacklistAPI.Services;
public class TencentCloudSDKCredential: ITencentCloudSDKCredential {
	public TencentCloudSDKCredential(IConfiguration configuration) {
		Credential = new() { SecretId = configuration["Secret:Id"], SecretKey = configuration["Secret:Key"] };
	}

	public Credential Credential { get; init; }
}