namespace AutoOptionRegistration.MarkerAttributes;

/// <summary>
///     Tags classes that are used as
///     <see href="https://learn.microsoft.com/en-us/dotnet/core/extensions/options">Options</see>
/// </summary>
/// <remarks>
///     These options can be registered to
///     <see href="https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host?tabs=appbuilder">
///         .NET Generic host
///     </see>
///     using the <see cref="IServiceCollectionExtensions.RegisterOptions">RegisterOptions</see> function
/// </remarks>
[AttributeUsage(AttributeTargets.Class)]
public class RegisterOptionAttribute : Attribute {
    /// <summary>
    ///     Tells how the <see href="https://learn.microsoft.com/en-us/dotnet/core/extensions/options">Option</see> should be
    ///     validated
    /// </summary>
    public enum ValidationType {
        NoValidation,

        /// <summary>
        ///     Validates the Option at the time when the Value is accessed
        /// </summary>
        ValidateDataAnnotationsWhenAccess,

        /// <summary>
        ///     Validates the Option at the time of
        ///     <see href="https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host?tabs=appbuilder">
        ///         .NET Generic host
        ///     </see>
        ///     startup.
        /// </summary>
        ValidateDataAnnotationsOnStart
    }

    /// <summary>
    ///     Optional name for the option, if omitted, then the class name will be used as an OptionName.
    /// </summary>
    public string? OptionName { get; init; }

    /// <summary>
    ///     Tells how the <see href="https://learn.microsoft.com/en-us/dotnet/core/extensions/options">Option</see> should be
    ///     validated
    /// </summary>
    public ValidationType Validate { get; init; } = ValidationType.ValidateDataAnnotationsOnStart;
}