using System.Linq;

namespace AutoOptionRegistration.test.Core;

public static class OptionStaticInitializer {
    public static void InitializeAllOptions() {
        var optionTypes = typeof(GeneralOption).Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(GeneralOption)) && !t.IsAbstract);

        foreach (var type in optionTypes) {
            // Force the static constructor to run
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);
        }
    }
}