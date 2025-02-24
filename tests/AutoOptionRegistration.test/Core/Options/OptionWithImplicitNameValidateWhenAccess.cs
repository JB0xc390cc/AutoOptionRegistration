using AutoOptionRegistration.MarkerAttributes;
using static AutoOptionRegistration.MarkerAttributes.RegisterOptionAttribute.ValidationType;

namespace AutoOptionRegistration.test.Core.Options;

[RegisterOption(Validate = ValidateDataAnnotationsWhenAccess)]
public class OptionWithImplicitNameValidateWhenAccess : GeneralOption
{
    static OptionWithImplicitNameValidateWhenAccess() =>
        RegisterOptionName<OptionWithImplicitNameValidateWhenAccess>(OptionName);
    
    public new static string OptionName => nameof(OptionWithImplicitNameValidateWhenAccess);
}