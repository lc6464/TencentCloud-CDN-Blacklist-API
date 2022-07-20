namespace TencentCloudCDNBlacklistAPI.Controllers;
[Route("[controller]")]
[ApiController]
public class AllController : ControllerBase {
	private readonly ILogger<AllController> _logger;
	private readonly IConfiguration _configuration;

	public AllController(ILogger<AllController> logger, IConfiguration configuration) {
		_logger = logger;
		_configuration = configuration;
	}

	[HttpGet]
	public Models.GetResult Get() {
		return new();
	}

	[HttpPut]
	public Models.PutResult Put() {
		return new();
	}
}