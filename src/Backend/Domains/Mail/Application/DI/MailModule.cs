using Autofac;
using Backend.Domains.Mail.Application.Clients;
using Backend.Domains.Mail.Infrastructure.Clients;
using SaveApis.Core.Infrastructure.DI;

namespace Backend.Domains.Mail.Application.DI;

public class MailModule(IConfiguration configuration) : BaseModule(configuration)
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MailClientFactory>().As<IMailClientFactory>();
    }
}
