namespace TencentCloudCDNBlacklistAPI.Models;
public struct GetResult {
	public bool Success { get; set; }
	public string? Message { get; set; }
	public string[]? Blacklist { get; set; }
}