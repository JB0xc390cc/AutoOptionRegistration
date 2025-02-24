using System.ComponentModel.DataAnnotations;
using AutoOptionRegistration;
using AutoOptionRegistration.MarkerAttributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SimpleExample;
using static AutoOptionRegistration.MarkerAttributes.RegisterOptionAttribute.ValidationType;

// The XML config from what the options will be parsed.
// I use XML config in this example because its most readable in my opinion, in contrast to other
// configuration sources
string xmlConfig = """
                   <configuration>
                        <Leader>
                            <Name>Peter</Name>
                            <Age> 22 </Age>
                        </Leader>
                        
                        <Supervisor>
                            <Name>George</Name>
                            <Age>21</Age>
                        </Supervisor>
                   
                        <Worker>
                            <Name>Steve</Name>
                            <Age>20</Age>
                        </Worker>
                   </configuration>
                   """;


var builder = new HostApplicationBuilder(args);

// This registers our options that are tagged with the RegisterOption attribute
builder.Services.RegisterOptions(builder.Configuration, typeof(Program).Assembly);

// Clear the configuration sources so the only one will be the XML stream.
builder.Configuration.Sources.Clear();
builder.Configuration.AddXmlStream(xmlConfig.ToStream());

var host = builder.Build();

host.Start();

Console.WriteLine(host.Services.GetRequiredService<IOptions<Leader>>().Value);
Console.WriteLine(host.Services.GetRequiredService<IOptions<SupervisorPerson>>().Value);
Console.WriteLine(host.Services.GetRequiredService<IOptions<WorkerPerson>>().Value);

/******************************************************************************************************
 *
 * The options that are used in the example, they are only records to be asier to print their data...
 *
 *****************************************************************************************************/

[RegisterOption]
public record class Leader {
    [StringLength(7)] public string Name { get; init; } = "Peter";
    public int Age { get; init; } = 19;
}

[RegisterOption(OptionName = "Supervisor", Validate = ValidateDataAnnotationsOnStart)]
public record class SupervisorPerson {
    [StringLength(7)] public string Name { get; init; } = "Peter";
    public int Age { get; init; } = 19;
}

[RegisterOption(OptionName = "Worker", Validate = NoValidation)]
public record class WorkerPerson {
    [StringLength(7)] public string Name { get; init; } = "Peter";
    public int Age { get; init; } = 19;
}