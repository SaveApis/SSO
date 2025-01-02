using Vogen;

namespace Backend.Domains.Project.Domain.VO;

[ValueObject<string>(Conversions.EfCoreValueConverter)]
public partial record ProjectId
{
    private static Validation Validate(string input)
    {
        return string.IsNullOrEmpty(input)
            ? Validation.Invalid("ProjectId cannot be empty!")
            : input.Length > 200
                ? Validation.Invalid("ProjectId cannot be longer than 200 characters!")
                : Validation.Ok;
    }

    private static string NormalizeInput(string input)
    {
        input = input.ToLowerInvariant();
        input = input.Replace(' ', '_');

        return input;
    }
}
