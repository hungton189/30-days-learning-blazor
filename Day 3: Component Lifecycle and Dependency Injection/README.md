# Blazor Learning - Day 3

## 📖 Table of Contents

1. [Component Lifecycle in Blazor](#component-lifecycle-in-blazor)
2. [Dependency Injection in Blazor](#dependency-injection-in-blazor)

---

## <a id="component-lifecycle-in-blazor">1. Component Lifecycle in Blazor</a>

---

## 🔄 Comparison: Blazor vs React Lifecycle

| **Lifecycle Stage**          | **Blazor Method**                              | **React Equivalent**                                      | **Description**                                 |
| ---------------------------- | ---------------------------------------------- | --------------------------------------------------------- | ----------------------------------------------- |
| **Component Initialization** | `OnInitialized()` / `OnInitializedAsync()`     | `componentDidMount()` / `useEffect(()=>{}, [])`           | Runs when the component is first created.       |
| **Receiving New Props**      | `OnParametersSet()` / `OnParametersSetAsync()` | `componentDidUpdate()` / `useEffect(()=>{}, [props])`     | Runs when component parameters (props) change.  |
| **After Rendering**          | `OnAfterRender()` / `OnAfterRenderAsync()`     | `useEffect(()=>{...})` (with dependencies)                | Runs after the component has been rendered.     |
| **Before Rendering**         | `ShouldRender()`                               | `shouldComponentUpdate()` / `React.memo()`                | Determines whether re-rendering should happen.  |
| **Component Destruction**    | `Dispose()`                                    | `componentWillUnmount()` / `useEffect(()=>{}, return fn)` | Runs when the component is removed from the UI. |

---

## 🆚 Key Differences Between Blazor and React Lifecycle

### ✅ 1. Initialization

- **Blazor**: Uses `OnInitialized()` (sync) and `OnInitializedAsync()` (async) to set up initial state and fetch data.
- **React**: Uses `useEffect(() => {...}, [])` (or `componentDidMount` in class components).

#### Example: Fetching Data on Initialization

**Blazor**

```razor
@code {
    protected override async Task OnInitializedAsync()
    {
        await FetchData();
    }
}
```

**React**

```jsx
useEffect(() => {
  fetchData();
}, []);
```

---

### ✅ 2. Handling Prop Changes

- **Blazor**: `OnParametersSet()` runs when component parameters (props) change.
- **React**: Uses `useEffect(() => {...}, [props])` to watch for prop changes.

#### Example: Watching for Prop Changes

**Blazor**

```razor
@code {
    [Parameter] public string Message { get; set; }

    protected override void OnParametersSet()
    {
        Console.WriteLine($"New prop value: {Message}");
    }
}
```

**React**

```jsx
useEffect(() => {
  console.log(`New prop value: ${message}`);
}, [message]);
```

---

### ✅ 3. After Component Rendering

- **Blazor**: `OnAfterRender()` runs **after** the component has been rendered.
- **React**: `useEffect(() => {...})` without dependencies runs after every render.

#### Example: Interacting with JavaScript after render

**Blazor**

```razor
@inject IJSRuntime JS

protected override async Task OnAfterRenderAsync(bool firstRender)
{
    if (firstRender)
    {
        await JS.InvokeVoidAsync("console.log", "Component rendered!");
    }
}
```

**React**

```jsx
useEffect(() => {
  console.log("Component rendered!");
});
```

---

### ✅ 4. Controlling Re-renders

- **Blazor**: Uses `ShouldRender()` to prevent unnecessary re-renders.
- **React**: Uses `React.memo()` or `shouldComponentUpdate()` in class components.

#### Example: Preventing Unnecessary Renders

**Blazor**

```razor
@code {
    private int count = 0;

    protected override bool ShouldRender()
    {
        return count % 2 == 0; // Re-render only on even counts
    }
}
```

**React**

```jsx
const MyComponent = React.memo(({ count }) => {
  console.log("Rendered");
  return <div>{count}</div>;
});
```

---

### ✅ 5. Component Cleanup

- **Blazor**: Implements `IDisposable` and uses `Dispose()` to clean up resources.
- **React**: Uses `useEffect(() => {... return cleanup})` or `componentWillUnmount()` (class components).

#### Example: Cleaning Up Event Listeners

**Blazor**

```razor
@implements IDisposable

@code {
    private Timer? timer;

    protected override void OnInitialized()
    {
        timer = new Timer(_ => Console.WriteLine("Tick"), null, 0, 1000);
    }

    public void Dispose()
    {
        timer?.Dispose();
        Console.WriteLine("Component disposed");
    }
}
```

**React**

```jsx
useEffect(() => {
  const interval = setInterval(() => console.log("Tick"), 1000);
  return () => {
    clearInterval(interval);
    console.log("Component disposed");
  };
}, []);
```

---

## <a id="dependency-injection-in-blazor">2. Dependency Injection (DI) in Blazor</a>

### ✅ 1. What is Dependency Injection (DI)?

- **DI (Dependency Injection)** is a design pattern used to manage dependencies in an application.
- Instead of creating instances manually, dependencies are provided by a central system (DI container).
- Benefits:
  - Improves **code reusability** and **maintainability**.
  - **Reduces coupling** between components and services.
  - Enhances **testability** (can easily mock dependencies).

---

### ✅ 2. Types of DI in Blazor

| DI Type       | Description                                                                                            |
| ------------- | ------------------------------------------------------------------------------------------------------ |
| **Singleton** | One instance is shared across the entire application. The service lives as long as the app is running. |
| **Scoped**    | One instance is created per **request** (Blazor Server) or per **tab/session** (Blazor WebAssembly).   |
| **Transient** | A new instance is created every time the service is requested.                                         |

### Registering DI Services in `Program.cs`

```csharp
builder.Services.AddSingleton<MySingletonService>();
builder.Services.AddScoped<MyScopedService>();
builder.Services.AddTransient<MyTransientService>();
```

---

### ✅ 3. Comparison and Real-World Examples of DI Types

#### 1. Singleton

- **Description**: A **single instance** is shared across the entire application.
- **Use Case**: Application-wide settings, Logging services.

**Implementation:**

```csharp
public class AppConfigService
{
    public string Theme { get; set; } = "Light";
}

builder.Services.AddSingleton<AppConfigService>();
```

**Usage in Component:**

```razor
@inject AppConfigService ConfigService

<h3>Current Theme: @ConfigService.Theme</h3>
<button @onclick="ToggleTheme">Toggle Theme</button>

@code {
    private void ToggleTheme()
    {
        ConfigService.Theme = ConfigService.Theme == "Light" ? "Dark" : "Light";
    }
}
```

---

#### 2. Scoped

- **Description**: A new instance is created **per request/session**.
- **Use Case**: Shopping cart, User authentication service.

**Implementation:**

```csharp
public class ShoppingCartService
{
    public List<string> Items { get; set; } = new();
    public void AddItem(string item) => Items.Add(item);
}

builder.Services.AddScoped<ShoppingCartService>();
```

**Usage in Component:**

```razor
@inject ShoppingCartService CartService

<h3>Shopping Cart</h3>
<button @onclick="AddToCart">Add Item</button>
<p>Cart Items: @CartService.Items.Count</p>

@code {
    private void AddToCart()
    {
        CartService.AddItem("New Product");
    }
}
```

---

#### 3. Transient

- **Description**: A new instance is created **every time** it is requested.
- **Use Case**: Generating random numbers, Utility services.

**Implementation:**

```csharp
public class RandomGeneratorService
{
    public int GetRandomNumber() => new Random().Next(1, 100);
}

builder.Services.AddTransient<RandomGeneratorService>();
```

**Usage in Component:**

```razor
@inject RandomGeneratorService RandomService

<h3>Random Number: @RandomNumber</h3>
<button @onclick="GenerateNumber">Generate</button>

@code {
    private int RandomNumber;

    private void GenerateNumber()
    {
        RandomNumber = RandomService.GetRandomNumber();
    }
}
```

---

### ✅ 4. Comparison Between Singleton, Scoped, and Transient

| Feature          | Singleton               | Scoped                          | Transient                      |
| ---------------- | ----------------------- | ------------------------------- | ------------------------------ |
| Instance Sharing | Shared across app       | Shared within a request/session | New instance every request     |
| Suitable For     | Caching, Configurations | User-specific data              | Lightweight operations         |
| Memory Usage     | Low (but can grow)      | Moderate                        | Higher (many instances)        |
| Performance      | High (since reused)     | Moderate                        | Lower (more creation overhead) |

---

### ✅ 5. DI in Blazor vs State Management in React

| Feature           | Blazor DI                     | React (Jotai / Context API)         |
| ----------------- | ----------------------------- | ----------------------------------- |
| **Usage**         | Centralized service injection | Decentralized state management      |
| **Lifetime**      | Singleton, Scoped, Transient  | Scoped per component tree           |
| **Performance**   | Efficient with caching        | Efficient with fine-grained updates |
| **State Sharing** | Available globally            | Available within provider scope     |

### Equivalent Example in React (Jotai)

```tsx
const cartAtom = atom<string[]>([]);
const CartComponent = () => {
  const [cart, setCart] = useAtom(cartAtom);
  return (
    <div>
      <button onClick={() => setCart([...cart, "New Product"])}>
        Add Item
      </button>
      <p>Cart Items: {cart.length}</p>
    </div>
  );
};
```

---

### ✅ 🔥 Summary

- **DI in Blazor** helps manage dependencies centrally, promoting reusability.
- **Three DI types**:
  - **Singleton**: Shared globally (e.g., config settings).
  - **Scoped**: Per user/session (e.g., shopping cart).
  - **Transient**: New instance every request (e.g., random number generator).
- **Blazor DI vs React State Management**:
  - Blazor DI is more structured and centralized.
  - React's Context API/Jotai provides finer control but requires explicit providers.
