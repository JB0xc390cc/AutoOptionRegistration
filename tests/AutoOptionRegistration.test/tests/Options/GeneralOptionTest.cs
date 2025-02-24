using System.Diagnostics;
using System.Reflection;
using AutoOptionRegistration.test.Core;
using FluentAssertions;

namespace AutoOptionRegistration.test.tests.Options;

[TestOf(typeof(GeneralOption))]
public class GeneralOptionTest {
    [Test]
    public void TestCreateNonDefaultValidOptionValue_NotDefault() {
        var optionValue = GeneralOption.NonDefaultValidOptionValue;
        optionValue.Should().NotBe(new GeneralOption().OptionValue);
    }


    [Test, Order(1)]
    public void Test_AllGeneralOptions_RegisteredNames() {
        var options = GetAllOptionTypeNamePair(typeof(GeneralOption).Assembly);

        foreach (var option in options) {
            GeneralOption.GetOptionName(option.OptionType).Should().Be(option.OptionName);
        }
    }

    [Test, Order(2)]
    public void Test_OptionName_UniqueAcrossChild() {
        var optionNames = GetAllOptionTypeNamePair(typeof(GeneralOption).Assembly);

        // Group by OptionName and find duplicates
        var duplicateOptionNames = optionNames
            .GroupBy(o => o.OptionName)
            .Where(g => g.Count() > 1)
            .Select(g => new { OptionName = g.Key, Types = g.Select(o => o.OptionType).ToList() })
            .ToList();

        // Assert that there are no duplicates
        duplicateOptionNames.Should().BeEmpty(
            "Option names across GeneralOption child classes must be unique. Duplicates found: " +
            string.Join(", ",
                        duplicateOptionNames.Select(d =>
                                                        $"'{d.OptionName}' in {string.Join(" and ", d.Types.Select(t => t.Name))}")));
    }


    /// <summary>
    ///     Gets all the child types of <see cref="GeneralOption" /> and the value of their static OptionName field too.
    /// </summary>
    /// <returns>The OptionType, OptionName pairs.</returns>
    private static IEnumerable<(Type OptionType, string OptionName )> GetAllOptionTypeNamePair(Assembly assembly) {
        var optionTypes = assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(GeneralOption)) && !t.IsAbstract);

        var optionNames = new List<(Type, string)>();

        foreach (var type in optionTypes) {
            var propertyInfo =
                type.GetProperty(nameof(GeneralOption.OptionName), BindingFlags.Static | BindingFlags.Public);

            Debug.Assert(propertyInfo != null,
                         $"Option {type.Name} has no {nameof(GeneralOption.OptionName)} property.");
            var optionName = (string)propertyInfo.GetValue(null)!;

            optionNames.Add((type, optionName));
        }

        return optionNames;
    }
}