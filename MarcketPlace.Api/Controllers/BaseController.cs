using FluentValidation.Results;
using MarcketPlace.Api.Responses;
using MarcketPlace.Application.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MarcketPlace.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
public class BaseController : Controller
{
    private readonly INotificator _notificator;

    public BaseController(INotificator notificator)
    {
        _notificator = notificator;
    }
    
    protected IActionResult NoContentResponse() 
        => CustomResponse(NoContent());

    protected IActionResult CreatedResponse(string uri = "", object? result = null) 
        => CustomResponse(Created(uri, result));

    protected IActionResult OkResponse(object? result = null) 
        => CustomResponse(Ok(result));
    
    private IActionResult CustomResponse(IActionResult objectResult)
    {
        if (OperacaoValida)
        {
            return objectResult;
        }
        
        if (_notificator.IsNotFoundResource)
        {
            return NotFound();
        }

        var response = new BadRequestResponse(_notificator.GetNotifications().ToList());
        return BadRequest(response);
    }
    
    protected IActionResult CustomResponse(ModelStateDictionary modelState)
    {
        var erros = modelState.Values.SelectMany(e => e.Errors);
        foreach (var erro in erros)
        {
            AdicionarErroProcessamento(erro.ErrorMessage);
        }

        return CustomResponse(Ok(null));
    }

    protected IActionResult CustomResponse(ValidationResult validationResult)
    {
        foreach (var erro in validationResult.Errors)
        {
            AdicionarErroProcessamento(erro.ErrorMessage);
        }

        return CustomResponse(Ok(null));
    }

    private bool OperacaoValida => !(_notificator.HasNotification || _notificator.IsNotFoundResource);

    private void AdicionarErroProcessamento(string erro) => _notificator.Handle(erro);
}