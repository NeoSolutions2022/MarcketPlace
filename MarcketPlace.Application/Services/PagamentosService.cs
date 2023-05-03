using System.Net;
using AutoMapper;
using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Notification;
using MarcketPlace.Domain.Contracts.Repositories;
using MercadoPago;
using MercadoPago.Client;
using MercadoPago.Client.Payment;
using MercadoPago.Client.PaymentMethod;
using MercadoPago.Config;
using MercadoPago.Resource.Payment;
using Microsoft.AspNetCore.Http;

namespace MarcketPlace.Application.Services;

public class PagamentosService : BaseService, IPagamentosService

{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IClienteRepository _clienteRepository;
    public PagamentosService(IMapper mapper, INotificator notificator, IHttpContextAccessor httpContextAccessor, IClienteRepository clienteRepository) : base(mapper, notificator)
    {
        _httpContextAccessor = httpContextAccessor;
        _clienteRepository = clienteRepository;
        MercadoPagoConfig.AccessToken = "YOUR_ACCESS_TOKEN";
    }

    public async Task PagarComCartao(string token)
    {
        var claimEmail =  _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Email");
        if (claimEmail.Value == null)
        {
            return;
        }

        var cliente = await _clienteRepository.ObterPorEmail(claimEmail.Value);
        if (cliente == null)
        {
            return;
        }
        
        var request = new PaymentCreateRequest
        {
            TransactionAmount = (decimal)1.0,
            Token = token,
            Description = "Assinatura Mundo Web",
            Installments = 1,
            PaymentMethodId = "visa",
            Payer = new PaymentPayerRequest
            {
                Email = claimEmail.Value,
            }
        };
        
        var requestOptions = new RequestOptions();
        requestOptions.AccessToken = "YOUR_ACCESS_TOKEN";

        var client = new PaymentClient();
        Payment payment = await client.CreateAsync(request, requestOptions);

        if (payment.Status == "approved")
        {
            cliente.Inadiplente = false;
            cliente.DataPagamento = DateTime.Now.AddMonths(1);
            return;
        }
        
        Notificator.Handle(payment.Status);
    }
    
}