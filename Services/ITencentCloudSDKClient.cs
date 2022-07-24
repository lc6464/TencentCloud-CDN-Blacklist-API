namespace TencentCloudCDNBlacklistAPI.Services;
public interface ITencentCloudSDKClient
{
    CdnClient Client { get; init; }
}