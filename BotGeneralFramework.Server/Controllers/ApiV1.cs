using BotGeneralFramework.Interfaces.Core;
using BotGeneralFramework.Records.CLI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
  [Route("api/v1")]
  [ApiController]
  public class ApiV1 : ControllerBase
  {
    private readonly IApp _app;

    [HttpGet("test")]
    public bool Test() {
      _app.trigger("api.v1.test");
      return true;
    }

    public ApiV1(IApp app) {
      _app = app;
    }
  }
}
