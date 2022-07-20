namespace TencentCloudCDNBlacklistAPI.Services;
public interface IConfirmSign {
	bool Confirm(long timestamp, string? sign, string cacheKey);
}