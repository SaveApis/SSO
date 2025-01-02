using Vogen;

namespace Backend.Domains.Common.Domain.VO;

[ValueObject<string>(Conversions.EfCoreValueConverter)]
public partial record Name
{
    private static string NormalizeInput(string input)
    {
        return input;
    }

    private static Validation Validate(string input)
    {
        return string.IsNullOrWhiteSpace(input)
            ? Validation.Invalid("Name cannot be empty!")
            : input.Length > 50
                ? Validation.Invalid("Name cannot be longer than 50 characters!")
                : Validation.Ok;
    }
}
