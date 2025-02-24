using System.ComponentModel.DataAnnotations;

namespace AutoOptionRegistration.test.Core;

/// <summary>
///     Base class for options that are used for testing the <see cref="IServiceCollectionExtensions.RegisterOptions" />
///     function.
/// </summary>
public class GeneralOption {
    /// <summary>
    ///     Retrieves the static <see cref="OptionName" /> for the specified <typeparamref name="TOption" /> type.
    /// </summary>
    /// <typeparam name="TOption">The </typeparam>
    /// <returns>The <see cref="OptionName" /> for the specified <typeparamref name="TOption" /> </returns>
    public static string GetOptionName<TOption>() where TOption : GeneralOption => GetOptionName(typeof(TOption));

    /// <summary>
    ///     Retrieves the static <see cref="OptionName" /> for the specified <paramref name="optionType" /> type.
    /// </summary>
    /// <param name="optionType"></param>
    /// <returns>The <see cref="OptionName" /> for the specified <paramref name="optionType" /> </returns>
    public static string GetOptionName(Type optionType) => OptionNames[optionType];

    /// <summary>
    ///     The actual value of the option which will be set when the actual config gets parsed.
    /// </summary>
    [MaxLength(MaxLengthOfOptionValue)]
    public string OptionValue { get; init; } = GetDefaultOptionValue();

    public static string OptionName => null!;


    /// <summary>
    ///     The value that should be used to produce invalid <see cref="OptionValue" />
    /// </summary>
    public static string InvalidOptionValue => new('A', MaxLengthOfOptionValue + 1);

    /// <summary>
    ///     The value that should be used to produce valid <see cref="OptionValue" />
    /// </summary>
    /// <remarks>
    ///     You can be sure that this value is different from the default value of the <see cref="OptionValue" />
    ///     This can be important when you assert in the unit tests,
    ///     you'll know that the option actually read from the config, and you didn't just get the default value.
    /// </remarks>
    public static string NonDefaultValidOptionValue => new('B', MaxLengthOfOptionValue);


    /// <summary>
    ///     Make sure that all
    /// </summary>
    static GeneralOption() => OptionStaticInitializer.InitializeAllOptions();

    /// <summary>
    ///     Registers an OptionType OptionName pair.
    /// </summary>
    /// <param name="optionName"></param>
    /// <typeparam name="T"></typeparam>
    /// <remarks>This function must be called from the child <see cref="GeneralOption" /> classes static constructors</remarks>
    protected static void RegisterOptionName<T>(string optionName) where T : GeneralOption =>
        OptionNames[typeof(T)] = optionName;


    private const int MaxLengthOfOptionValue = 5;

    private static readonly Dictionary<Type, string> OptionNames = new();


    private static string GetDefaultOptionValue() => new('A', MaxLengthOfOptionValue);
}