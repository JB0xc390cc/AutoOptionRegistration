using AutoOptionRegistration.MarkerAttributes;
using static AutoOptionRegistration.MarkerAttributes.RegisterOptionAttribute.ValidationType;

namespace AutoOptionRegistration.test.Core.Options;

[RegisterOption(Validate = ValidateDataAnnotationsOnStart)]
public class OptionWithImplicitNameValidateOnStartup : GeneralOption
{
    static OptionWithImplicitNameValidateOnStartup() =>
        RegisterOptionName<OptionWithImplicitNameValidateOnStartup>(OptionName);

    public new static string OptionName => nameof(OptionWithImplicitNameValidateOnStartup);
}