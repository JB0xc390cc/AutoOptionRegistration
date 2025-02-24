using System.Xml.Linq;
using AutoOptionRegistration.test.Core;
using AutoOptionRegistration.test.Core.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AutoOptionRegistration.test;

public partial class IServiceCollectionExtensionsTest {
    public static class DataSources {
        public static IEnumerable<TestCaseData> RegisterOption_ValidData_DataSource() {
            yield return CreateConfig<OptionWithExplicitNameValidateOnStartup>(true);
            yield return CreateConfig<OptionWithImplicitNameValidateOnStartup>(true);
            yield return CreateConfig<OptionWithExplicitNameValidateWhenAccess>(true);
            yield return CreateConfig<OptionWithImplicitNameValidateWhenAccess>(true);
            yield return CreateConfig<OptionWithExplicitNameNoValidation>(true);
            yield return CreateConfig<OptionWithImplicitNameNoValidation>(true);
        }

        public static IEnumerable<TestCaseData> RegisterOption_DataValidationOnStartup_InvalidData_DataSource() {
            yield return CreateConfig<OptionWithExplicitNameValidateOnStartup>(false);
            yield return CreateConfig<OptionWithImplicitNameValidateOnStartup>(false);
        }

        public static IEnumerable<TestCaseData> RegisterOption_DataValidationWhenAccess_InvalidData_DataSource() {
            yield return CreateConfig<OptionWithExplicitNameValidateWhenAccess>(false);
            yield return CreateConfig<OptionWithImplicitNameValidateWhenAccess>(false);
        }

        public static IEnumerable<TestCaseData> RegisterOption_NoDataValidation_InvalidData_DataSource() {
            yield return CreateConfig<OptionWithExplicitNameNoValidation>(false);
            yield return CreateConfig<OptionWithImplicitNameNoValidation>(false);
        }


        private static TestCaseData CreateConfig<TOption>(bool valid) where TOption : GeneralOption {
            var xmlConfig = new XElement("Configuration",
                                         new XElement(GeneralOption.GetOptionName<TOption>(),
                                                      new XElement(nameof(GeneralOption.OptionValue), valid
                                                                       ? GeneralOption.NonDefaultValidOptionValue
                                                                       : GeneralOption.InvalidOptionValue)
                                         ));

            return new TestCaseData(xmlConfig) { TypeArgs = [typeof(TOption)] };
        }

        public static IHost CreateHostThatHasOptionAndConfig<TOption>(XElement xmlConfig) {
            var builder = new HostApplicationBuilder();
            builder.Configuration.AddXmlStream(xmlConfig.ToString().ToStream());
            builder.Services.RegisterOptions(builder.Configuration, typeof(TOption).Assembly);
            var host = builder.Build();

            return host;
        }
    }
}