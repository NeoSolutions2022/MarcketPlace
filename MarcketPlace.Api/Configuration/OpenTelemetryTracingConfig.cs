using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace MarcketPlace.Api.Configuration;

public static class OpenTelemetryTracingConfig
{
    public static void AddOpenTelemetryTracingConfig(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<string>("SigNoz:Active").ToLowerInvariant() != "true")
        {
            return;
        }

        services.AddOpenTelemetryTracing(budiler =>
        {
            budiler
                .AddAspNetCoreInstrumentation(opt =>
                {
                    opt.RecordException = true;

                    opt.Filter = context =>
                    {
                        var path = context.Request.Path.Value ?? string.Empty;
                        return !path.Contains("swagger");
                    };

                    opt.EnrichWithHttpRequest = EnrichWithHttpRequest;
                    opt.EnrichWithHttpResponse = EnrichWithHttpResponse;
                    opt.EnrichWithException = EnrichWithException;
                })
                .AddEntityFrameworkCoreInstrumentation(opt =>
                {
                    opt.SetDbStatementForText = true;
                    opt.SetDbStatementForStoredProcedure = true;
                })
                .SetResourceBuilder(ResourceBuilder.CreateDefault()
                    .AddService(configuration.GetValue<string>("SigNoz:ServiceName"))
                    .AddTelemetrySdk()
                )
                .SetErrorStatusOnException()
                .AddOtlpExporter(options =>
                {
                    var endpoint = configuration.GetValue<string>("SigNoz:Endpoint");
                    options.Endpoint = new Uri(endpoint);
                });
        });
    }
    
    private static void EnrichWithHttpRequest(Activity activity, HttpRequest httpRequest)
    {
        activity.SetTag("request.protocol", httpRequest.Protocol);
        
        var token = httpRequest.Headers["authorization"];
        if (string.IsNullOrWhiteSpace(token))
        {
            return;
        }
        
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token.ToString().Replace("Bearer ", ""));
        var tokenS = jsonToken as JwtSecurityToken;

        activity.SetTag("usuario.id",
            tokenS!.Claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier)?.Value);
        activity.SetTag("usuario.nome", 
            tokenS.Claims.FirstOrDefault(t => t.Type == "nome")?.Value);
        activity.SetTag("usuario.email", 
            tokenS.Claims.FirstOrDefault(t => t.Type == "email")?.Value);
        activity.SetTag("usuario.tipo",
            tokenS.Claims.FirstOrDefault(t => t.Type == "TipoUsuario")?.Value);
    }

    private static void EnrichWithHttpResponse(Activity activity, HttpResponse httpResponse)
    {
        activity.SetTag("Response.Length", httpResponse.ContentLength);
        activity.SetTag("Response.StatusCode", httpResponse.StatusCode);
        activity.SetTag("Response.ContentType", httpResponse.ContentType);
    }

    private static void EnrichWithException(Activity activity, Exception exception)
    {
        activity.SetTag("Exception.Type", exception.GetType().ToString());
    }
}