using AutoOptionRegistration.MarkerAttributes;
using static AutoOptionRegistration.MarkerAttributes.RegisterOptionAttribute.ValidationType;

namespace AutoOptionRegistration.test.Core.Options;

[RegisterOption(Validate = NoValidation)]
public class OptionWithImplicitNameNoValidation : GeneralOption
{
    static OptionWithImplicitNameNoValidation() =>
        RegisterOptionName<OptionWithImplicitNameNoValidation>(OptionName);
    
    public new static string OptionName => nameof(OptionWithImplicitNameNoValidation);
}