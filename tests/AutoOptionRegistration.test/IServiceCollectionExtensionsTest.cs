using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using AutoOptionRegistration.test.Core;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using static AutoOptionRegistration.test.IServiceCollectionExtensionsTest.DataSources;

namespace AutoOptionRegistration.test;

[TestFixture]
[TestOf(typeof(IServiceCollectionExtensions))]
[SuppressMessage("ReSharper", "UseCollectionExpression")]
public partial class IServiceCollectionExtensionsTest {
    [Test, TestCaseSource(typeof(DataSources), nameof(RegisterOption_ValidData_DataSource))]
    public void Test_RegisterOption_ValidData<TOption>(XElement xmlConfig) where TOption : GeneralOption {
        // Arrange
        var host = CreateHostThatHasOptionAndConfig<TOption>(xmlConfig);
        host.Start();

        // Act
        var option = host.Services.GetRequiredService<IOptions<TOption>>();

        //Assert
        option.Value.OptionValue.Should().Be(GeneralOption.NonDefaultValidOptionValue);
    }


    [Test, TestCaseSource(typeof(DataSources), nameof(RegisterOption_DataValidationOnStartup_InvalidData_DataSource))]
    public void Test_RegisterOption_DataValidationOnStartup_InvalidData<TOption>(XElement xmlConfig)
        where TOption : GeneralOption {
        // Arrange
        var host = CreateHostThatHasOptionAndConfig<TOption>(xmlConfig);

        // Act
        var action = () => host.Start();

        // Assert
        action.Should().Throw<OptionsValidationException>();
    }


    [Test, TestCaseSource(typeof(DataSources), nameof(RegisterOption_DataValidationWhenAccess_InvalidData_DataSource))]
    public void TestRegisterOptions_DataValidationWhenAccess_InvalidData<TOption>(XElement xmlConfig) where TOption :
        GeneralOption {
        // Arrange
        var host = CreateHostThatHasOptionAndConfig<TOption>(xmlConfig);
        host.Start();

        // Act
        var option = host.Services.GetRequiredService<IOptions<TOption>>();
        var act = () => option.Value;

        // Assert
        act.Should().Throw<OptionsValidationException>();
    }


    [Test, TestCaseSource(typeof(DataSources), nameof(RegisterOption_NoDataValidation_InvalidData_DataSource))]
    public void TestRegisterOptions_NoDataValidation_InvalidData<TOption>(XElement xmlConfig) where TOption :
        GeneralOption {
        // Arrange
        var host = CreateHostThatHasOptionAndConfig<TOption>(xmlConfig);
        host.Start();

        // Act
        var option = host.Services.GetRequiredService<IOptions<TOption>>();

        // Assert
        option.Value.OptionValue.Should().Be(GeneralOption.InvalidOptionValue);
    }

}