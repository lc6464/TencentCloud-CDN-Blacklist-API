using System.Security.Cryptography;

namespace TencentCloudCDNBlacklistAPI.Services;
public class ConfirmSign : IConfirmSign {
	private readonly IConfiguration _configuration;
	private readonly IMemoryCache _memoryCache;
	private readonly ILogger<ConfirmSign> _logger;

	public ConfirmSign(IConfiguration configuration, IMemoryCache memoryCache, ILogger<ConfirmSign> logger) {
		_configuration = configuration;
		_memoryCache = memoryCache;
		_logger = logger;
	}

	public bool Confirm(long timestamp, string? sign, string cacheKey) {
		if (sign?.Length != 43) {
			return false;
		}

		var now = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
		if (now - timestamp > 30 || timestamp - now > 2) {
			return false; // 判断时间戳
		}

		var timestampData = BitConverter.GetBytes(timestamp);
		var data = new byte[104];
		Buffer.BlockCopy(Convert.FromBase64String(_configuration["Secrets:A"]), 0, data, 0, 64);
		Buffer.BlockCopy(timestampData, 0, data, 96, 8);
		using var sha256 = SHA256.Create();
		Buffer.BlockCopy(sha256.ComputeHash(Convert.FromBase64String(_configuration["Secrets:B"])), 0, data, 64, 32);
		using var hs256 = HMAC.Create("HMACSHA256")!;
		hs256.Key = timestampData; // 计算签名

		if (Convert.ToBase64String(hs256.ComputeHash(data))[..43].Replace('+', '-').Replace('/', '_') == sign) {
			if (_memoryCache.TryGetValue(cacheKey, out string lastSign)) { // 如果存在上次的签名，则获取
				if (lastSign == sign) {
					_logger.LogInformation("遭遇重放攻击或请求过于频繁！T: {}    S: {}", timestamp, sign);
					return false; // 如一致，则 return false，防止重放攻击
				}
				_ = _memoryCache.Set(cacheKey, sign, TimeSpan.FromSeconds(35)); // 写入签名
				return true;
			}
			return true;
		}
		return false;
	}
}