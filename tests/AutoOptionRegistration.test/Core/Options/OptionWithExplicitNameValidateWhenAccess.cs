using AutoOptionRegistration.MarkerAttributes;

namespace AutoOptionRegistration.test.Core.Options;

using static MarkerAttributes.RegisterOptionAttribute.ValidationType;

[RegisterOption(OptionName = ExplicitName, Validate = ValidateDataAnnotationsWhenAccess)]
public class OptionWithExplicitNameValidateWhenAccess : GeneralOption
{
      static OptionWithExplicitNameValidateWhenAccess() =>
            RegisterOptionName<OptionWithExplicitNameValidateWhenAccess>(OptionName);
      
    public new static string OptionName => ExplicitName;
    private const string ExplicitName = "ExplicitOptionName3";
}