namespace TencentCloudCDNBlacklistAPI.Services;
public class TencentCloudSDKClient : ITencentCloudSDKClient {
	public TencentCloudSDKClient(ITencentCloudSDKCredential credential) {
		Client = new(credential.Credential, "");
	}

	public CdnClient Client { get; init; }
}