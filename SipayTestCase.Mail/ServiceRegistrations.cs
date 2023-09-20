using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SipayTestCase.Contract.Commons;
using SipayTestCase.Mail.Consumers;

namespace SipayTestCase.Mail;

public static class ServiceRegistrations
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqConfig = configuration.GetSection("RabbitMqConfig").Get<RabbitMqConfig>();
        
        services.AddMassTransit(config =>
        {
            config.AddConsumer<RegisterMailConsumer>();
            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(rabbitMqConfig!.Host, "/",
                    c =>
                    {
                        c.Username(rabbitMqConfig.UserName);
                        c.Password(rabbitMqConfig.Password);
                    });


                cfg.UseRetry(r => r.Interval(3, TimeSpan.FromSeconds(2)));
                cfg.ReceiveEndpoint("register-mail-queue", c => { c.ConfigureConsumer<RegisterMailConsumer>(ctx); });
            });
        });

        services.AddMassTransitHostedService();
    }
}