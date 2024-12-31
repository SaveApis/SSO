using Vogen;

namespace Backend.Domains.User.Domain.VO;

[ValueObject<Guid>(Conversions.EfCoreValueConverter)]
public partial record UserId
{
    private static Validation Validate(Guid input)
    {
        return input == Guid.Empty
            ? Validation.Invalid("UserId cannot be empty!")
            : Validation.Ok;
    }

    private static Guid NormalizeInput(Guid input)
    {
        return input;
    }
}
