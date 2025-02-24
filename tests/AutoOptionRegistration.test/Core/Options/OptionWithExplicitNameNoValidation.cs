using AutoOptionRegistration.MarkerAttributes;

namespace AutoOptionRegistration.test.Core.Options;

using static MarkerAttributes.RegisterOptionAttribute.ValidationType;

[RegisterOption(Validate = NoValidation, OptionName = ExplicitName)]
public class OptionWithExplicitNameNoValidation : GeneralOption
{
    static OptionWithExplicitNameNoValidation() =>
        RegisterOptionName<OptionWithExplicitNameNoValidation>(OptionName);

    public new static string OptionName => ExplicitName;
    private const string ExplicitName = "ExplicitOptionName1";
}