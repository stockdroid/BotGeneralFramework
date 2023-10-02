using BotGeneralFramework.Interfaces.Core;
using BotGeneralFramework.Records.CLI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BotGeneralFramework.Server
{
  [Route("api/v1")]
  [ApiController]
  public class ApiV1 : ControllerBase
  {
    private readonly IApp _app;

    [HttpGet("{request}/{**route}")]
    public dynamic V1Request(string request, string? route = null) {
      dynamic? ctx = _app.trigger($"api.v1.{request}", new() {
        { "path", $"/{route}" }
      });

      if (ctx?.res is null) {
        Response.StatusCode = 404;
        return "Request not found";
      } else if (ctx?.code is not null) {
        Response.StatusCode = (int)ctx.code;
        return ctx.res!;
      }

      Response.StatusCode = 200;
      return ctx?.res!;
    }

    public ApiV1(IApp app) {
      _app = app;
    }
  }
}
