using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Notification;
using MercadoPago;
using MercadoPago.Config;
using MercadoPago.Resource.MerchantOrder;
using MercadoPago.Resource.Payment;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;

namespace MarcketPlace.Api.Controllers.V1.Cliente;

[Route("v{version:apiVersion}/Cliente/[controller]")]
public class PagamentosController : MainController
{
    private readonly IPagamentosService _pagamentos;
    protected PagamentosController(INotificator notificator, IPagamentosService pagamentos) : base(notificator)
    {
        _pagamentos = pagamentos;
    }
    
    [HttpPost("/Cartao")]
    [SwaggerOperation(Summary = "Realizar pagamento.", Tags = new[] { "Usuario - Cliente" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RealizarPagamento(string tokenDoCartao)
    {
        await _pagamentos.PagarComCartao(tokenDoCartao);
        return OkResponse();
    }
    
    // [HttpPost]
    // public async Task<ActionResult> ReceiveNotification()
    // {
    //     using (var reader = new StreamReader(Request.Body))
    //     {
    //         string json = reader.ReadToEnd();
    //         JObject notification = JObject.Parse(json);
    //         string clientSecret = "SEU_CLIENT_SECRET"; 
    //         // MPWebhook webhook = new MPWebhook();
    //         bool isNotificationValid = true;
    //             // WebhookUtils.Validate(notification, clientSecret);
    //         string paymentId = notification["data"]["id"].ToString();
    //         var payment = ;
    //         if (isNotificationValid)
    //         {
    //             MercadoPagoConfig.AccessToken = "SEU_ACCESS_TOKEN"; // Substitua pelo seu próprio token de acesso
    //             
    //             if (payment != null)
    //             {
    //                 switch (payment.Status)
    //                 {
    //                     case PaymentStatus.approved:
    //                         // Atualize o status da transação em seu banco de dados para "aprovado"
    //                         break;
    //                     case PaymentStatus.pending:
    //                         // Atualize o status da transação em seu banco de dados para "pendente"
    //                         break;
    //                     case PaymentStatus.rejected:
    //                         // Atualize o status da transação em seu banco de dados para "rejeitado"
    //                         break;
    //                     default:
    //                         break;
    //                 }
    //             }
    //         }
    //     }
    //     return Ok();
    // }
    
}