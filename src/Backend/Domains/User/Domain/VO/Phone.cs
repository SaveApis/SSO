using Vogen;

namespace Backend.Domains.User.Domain.VO;

[ValueObject<string>(Conversions.EfCoreValueConverter)]
public partial class Phone
{
    private static string NormalizeInput(string input)
    {
        return input;
    }

    private static Validation Validate(string _)
    {
        return Validation.Ok;
    }
}
