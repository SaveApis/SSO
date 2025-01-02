using Vogen;

namespace Backend.Domains.Common.Domain.VO;

[ValueObject<string>(Conversions.EfCoreValueConverter)]
public partial record Description
{
    private static Validation Validate(string input)
    {
        return string.IsNullOrWhiteSpace(input)
            ? Validation.Invalid("Description cannot be empty!")
            : input.Length > 200
                ? Validation.Invalid("Description cannot be longer than 200 characters!")
                : Validation.Ok;
    }

    private static string NormalizeInput(string input)
    {
        return input;
    }
}
