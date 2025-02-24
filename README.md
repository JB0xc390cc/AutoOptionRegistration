# AutoOptionsRegistration for .NET

This library helps in registering the Option classes dynamically to .NET generic host
in your application.

## What this is trying to solve

Normally you would register your options manually by writing lines of code like this:

```csharp

public record class Leader {
    [StringLength(7)] public string Name { get; init; } = "Peter";
    public int Age { get; init; } = 19;
}

builder.Services.AddOptions<TOption>()
                 .Bind(configuration.GetSection("OptionName"))
                 .ValidateDataAnnotations()
                 .ValidateOnStart();
```

In my opinion, this is too much verbose, and if you have more options, it's really a lot of lines of code
also when you're creating an Option class, you already know that it'll be used as an option,
but you must not forget to register it with the code given above.
That's why I came up with the marker attribute solution

```csharp
[RegisterOption(OptionName = "OptionName")]
public record class Leader {
    [StringLength(7)] public string Name { get; init; } = "Peter";
    public int Age { get; init; } = 19;
}

builder.Services.RegisterOptions(builder.Configuration, typeof(Program).Assembly);
```

Now you only have to call the RegisterOptions function; it does not matter how many Options you have.

## Features

- DataAnnotation validation at startup
- DataAnnotation validation when access
- No validation
- Explicitly specified OptionName
- Implicitly specified OptionName(same as the name of class that is tagged with the RegisterOption Attribute)
