# Day 5: Dependency Injection in Blazor

## 📖 Table of Contents

1. [Overview](#overview)
2. [DI Lifetimes in Blazor](#di-lifetimes)
3. [Injecting Services into Components](#inject-services-into-components)
4. [Practice: Creating and Injecting a Logging Service](#practice)

---

## <a id="overview">1. Overview</a>

Dependency Injection (DI) in Blazor allows components and services to receive dependencies efficiently, making applications more modular and maintainable.

---

## <a id="di-lifetimes">2. DI Lifetimes in Blazor</a>

Blazor supports three types of service lifetimes:

1. **Singleton**: The service is created once and shared across the entire application.
2. **Scoped**: The service is created per user session in Blazor Server, but behaves like a singleton in Blazor WebAssembly.
3. **Transient**: A new instance of the service is created every time it is injected.

```csharp
// Registering services in Program.cs
builder.Services.AddSingleton<IMyService, MyService>(); // Singleton
builder.Services.AddScoped<IMyService, MyService>();   // Scoped
builder.Services.AddTransient<IMyService, MyService>(); // Transient
```

---

## <a id="inject-services-into-components">3. Injecting Services into Components</a>

To inject a service into a Blazor component, use the `[Inject]` attribute:

```razor
@inject IMyService MyService

<p>Service Message: @MyService.GetMessage()</p>
```

Or inject using the constructor:

```razor
@code {
    private readonly IMyService _myService;

    public MyComponent(IMyService myService)
    {
        _myService = myService;
    }
}
```

---

## <a id="practice">4. Practice: Creating and Injecting a Logging Service</a>

### **Step 1: Define the Logging Service**

```csharp
public interface ILoggerService
{
    void Log(string message);
}

public class LoggerService : ILoggerService
{
    public void Log(string message)
    {
        Console.WriteLine($"[LOG]: {message}");
    }
}
```

### **Step 2: Register the Service in `Program.cs`**

```csharp
builder.Services.AddSingleton<ILoggerService, LoggerService>();
```

### **Step 3: Inject and Use the Service in a Component**

```razor
@inject ILoggerService LoggerService

<button @onclick="LogMessage">Log Message</button>

@code {
    private void LogMessage()
    {
        LoggerService.Log("Button Clicked!");
    }
}
```

---

## Summary

- Dependency Injection helps manage dependencies efficiently.
- Blazor supports Singleton, Scoped, and Transient lifetimes.
- Services can be injected using `[Inject]` or constructor injection.
- Practice: Implementing a simple logging service to understand DI better.
