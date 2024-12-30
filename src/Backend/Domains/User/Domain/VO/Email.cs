using System.Net.Mail;
using Vogen;

namespace Backend.Domains.User.Domain.VO;

[ValueObject<string>(Conversions.EfCoreValueConverter)]
public partial record Email
{
    private static string NormalizeInput(string input)
    {
        return input;
    }

    private static Validation Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return Validation.Invalid("Email cannot be empty!");
        }

        if (input.Length > 150)
        {
            return Validation.Invalid("Email cannot be longer than 150 characters!");
        }

        try
        {
            _ = new MailAddress(input);

            return Validation.Ok;
        }
        catch (Exception e)
        {
            return Validation.Invalid($"Email is not valid! {e.Message}");
        }
    }
}
