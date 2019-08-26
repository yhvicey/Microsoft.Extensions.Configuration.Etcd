using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Extensions.Configuration.Etcd.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public ValuesController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet("{key}")]
        public ActionResult<string> Get(string key)
        {
            return configuration[key];
        }

        [HttpPut("{key}")]
        public void Put(string key, [FromBody] string value)
        {
            configuration[key] = value;
        }

        [HttpGet("reset")]
        public void Reset()
        {
            EtcdConfigurationManualRefresher.Instance.Refresh();
        }
    }
}
