using Backend.Domains.Mail.Application.Hangfire.Events;
using Backend.Domains.Mail.Infrastructure.Clients;
using Backend.Domains.User.Domain.VO;
using SaveApis.Core.Infrastructure.Hangfire.Attributes;
using SaveApis.Core.Infrastructure.Hangfire.Jobs;
using ILogger = Serilog.ILogger;

namespace Backend.Domains.Mail.Application.Hangfire.Jobs;

[HangfireQueue]
public class SendEmailJob(ILogger logger, IMailClientFactory factory, IConfiguration configuration) : BaseJob<SendEmailEvent>(logger)
{
    public override Task<bool> CanExecute(SendEmailEvent @event)
    {
        var smtpEnabled = bool.TryParse(configuration["smtp_enabled"] ?? "false", out var b) && b;

        return Task.FromResult(smtpEnabled);
    }

    [HangfireJobName("{0}: Send email")]
    public override async Task RunAsync(SendEmailEvent @event, CancellationToken cancellationToken)
    {
        var client = factory.CreateClient();

        await client.SendTemplate(@event.Template, Email.From(@event.Email), @event.Properties, cancellationToken).ConfigureAwait(false);
    }
}
