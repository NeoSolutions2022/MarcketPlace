using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using MarcketPlace.Infra;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Newtonsoft.Json;

namespace MarcketPlace.Api.Configuration;

public static class ApiConfiguration
{
    public static void AddApiConfiguration(this IServiceCollection services)
    {
        services.AddResponseCaching();
        
        services
            .AddRouting(options => options.LowercaseUrls = true);
        services
            .Configure<RouteOptions>(options => options.LowercaseUrls = true)
            .Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        
        services
            .AddControllers(conf =>
            {
                conf.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
                conf.UseDateOnlyTimeOnlyStringConverters();
            })  
            .AddDataAnnotationsLocalization()
            .AddJsonOptions(options =>
            {
                options.UseDateOnlyTimeOnlyStringConverters();
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.MaxDepth = 3;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

         services
            .AddCors(o => 
            {
                o.AddPolicy("default", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        
        services
            .Configure<RequestLocalizationOptions>(o => 
            {
                var supportedCultures = new[] { new CultureInfo("pt-BR") };
                o.DefaultRequestCulture = new RequestCulture("pt-BR", "pt-BR");
                o.SupportedCultures = supportedCultures;
                o.SupportedUICultures = supportedCultures;
            });
    }

    public static void UseApiConfiguration(this IApplicationBuilder app, IServiceProvider services, IHostEnvironment env)
    {
        if (!env.IsDevelopment())
            app.UseMigrations(services);
        
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
        
        var supportedCultures = new[] { new CultureInfo("pt-BR") };
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });

        app.UseCors("default");
    }

    private sealed class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value) =>
            value == null 
                ? null 
                : Regex.Replace(value.ToString() ?? string.Empty, "([a-z])([A-Z])", "$1-$2").ToLower();
    }
}