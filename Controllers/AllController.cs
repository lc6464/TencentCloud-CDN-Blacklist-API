namespace TencentCloudCDNBlacklistAPI.Controllers;
[Route("[controller]")]
[ApiController]
public class AllController : ControllerBase {
	private readonly ILogger<AllController> _logger;
	private readonly IConfiguration _configuration;
	private readonly ITencentCloudSDKClient _client;
	private readonly IConfirmSign _sign;

	public AllController(ILogger<AllController> logger, IConfiguration configuration, ITencentCloudSDKClient client, IConfirmSign sign) {
		_logger = logger;
		_configuration = configuration;
		_client = client;
		_sign = sign;
	}

	[HttpGet]
	public Models.GetResult Get(long timestamp = 0, string? sign = "") {
		if (_sign.Confirm(timestamp, sign, "LastSign")) {
			_logger.LogDebug("Get: 校验签名成功。");

			DescribeDomainsConfigRequest req = new() {
				Filters = new[] {
					new DomainFilter {
						Name = "domain",
						Value = new[] { _configuration["Domain"] }
					}
				}
			};
			try {
				var res = _client.Client.DescribeDomainsConfigSync(req);
				_logger.LogDebug("Get: 请求 API 返回：{}", AbstractModel.ToJsonString(res));
				return new() { Success = true, Blacklist = res.Domains[0]!.IpFilter.Filters };
			} catch (Exception e) {
				_logger.LogCritical("Get: 请求 API 过程中发生异常：{}", e);
				return new() { Success = false, Message = e.Message };
			}
		}
		_logger.LogDebug("Get: 校验签名失败！T: {}    S: {}", timestamp, sign);
		return new() { Success = false };
	}

	[HttpPut]
	public Models.PutResult Put(long timestamp = 0, string? sign = "", [FromBody] string[]? blacklist = null) {
		if (blacklist == null) {
			_logger.LogDebug("Put: 未传入黑名单。");
			return new() { Success = false, Message = "No any data." };
		}

		if (blacklist.Length > 200) {
			_logger.LogError("Put: 黑名单内的地址/地址段过多，最多仅支持200个！");
			return new() { Success = false, Message = "Too many blacklist members." };
		}

		if (_sign.Confirm(timestamp, sign, "LastSign")) {
			_logger.LogDebug("Put: 校验签名成功。");

			UpdateDomainConfigRequest req = new() {
				Domain = _configuration["Domain"],
				IpFilter = new() {
					Switch = "on", FilterType = "blacklist", Filters = blacklist
				}
			};

			try {
				var res = _client.Client.UpdateDomainConfigSync(req);
				_logger.LogDebug("Get: 请求 API 返回：{}", AbstractModel.ToJsonString(res));
				return new() { Success = true };
			} catch (Exception e) {
				_logger.LogCritical("Get: 请求 API 过程中发生异常：{}", e);
				return new() { Success = false, Message = e.Message };
			}
		}
		_logger.LogDebug("Put: 校验签名失败！T: {}    S: {}", timestamp, sign);
		return new() { Success = false };
	}
}