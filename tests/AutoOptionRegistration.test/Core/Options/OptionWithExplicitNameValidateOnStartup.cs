using AutoOptionRegistration.MarkerAttributes;
using static AutoOptionRegistration.MarkerAttributes.RegisterOptionAttribute.ValidationType;

namespace AutoOptionRegistration.test.Core.Options;

[RegisterOption(OptionName = ExplicitName, Validate = ValidateDataAnnotationsOnStart)]
public class OptionWithExplicitNameValidateOnStartup : GeneralOption
{
    static OptionWithExplicitNameValidateOnStartup() =>
        RegisterOptionName<OptionWithExplicitNameValidateOnStartup>(OptionName);

    public new static string OptionName => ExplicitName;
    private const string ExplicitName = "ExplicitOptionName2";
}