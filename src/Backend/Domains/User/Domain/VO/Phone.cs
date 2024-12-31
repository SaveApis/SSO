using System.Text.RegularExpressions;
using Vogen;

namespace Backend.Domains.User.Domain.VO;

[ValueObject<string>(Conversions.EfCoreValueConverter)]
public partial class Phone
{
    private const string RegexPattern =
        @"\+(9[976]\d|8[987530]\d|6[987]\d|5[90]\d|42\d|3[875]\d|2[98654321]\d|9[8543210]|8[6421]|6[6543210]|5[87654321]|4[987654310]|3[9643210]|2[70]|7|1)\W*\d\W*\d\W*\d\W*\d\W*\d\W*\d\W*\d\W*\d\W*(\d{1,2})$";

    private static string NormalizeInput(string input)
    {
        return input;
    }

    private static Validation Validate(string input)
    {
        var regex = new Regex(RegexPattern);

        return regex.Match(input).Success
            ? Validation.Ok
            : Validation.Invalid("Invalid phone number!");
    }
}
