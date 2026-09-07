namespace TradeSphere.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase(ISender sender) : ControllerBase
{
    protected ISender Mediator { get; } = sender;

    protected ActionResult HandleResult(Result result) =>
        result.IsSuccess ? Ok() : BadRequest(new { errors = result.Errors });

    protected ActionResult<T> HandleResult<T>(Result<T> result) =>
        result.IsSuccess ? Ok(result.Value) : BadRequest(new { errors = result.Errors });
}