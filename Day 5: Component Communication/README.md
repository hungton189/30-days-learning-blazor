# Day 5: Component Communication

## 📖 Table of Contents

1. [Component Parameters in Blazor](#component-parameters)
2. [Event Callback in Blazor](#event-callback)

---

## <a id="component-parameters">1. Component Parameters in Blazor</a>

### a. Introduction

Component Parameters in Blazor allow data to be passed from a parent component to a child component. There are three main ways to pass data:

- **One-way Binding (`[Parameter]`)**
- **Two-way Binding (`@bind-Parameter`)**
- **EventCallback (Passing events from Child to Parent)**

---

### b. One-way Binding (`[Parameter]`)

#### **How it works:**

- The parent component passes data to the child using the `[Parameter]` attribute.
- The Child can modify the value, but the Parent will not be updated if the callback is not applied.

#### **Example:**

##### **Parent Component (Index.razor)**

```razor
<ChildComponent Message="Hello from Parent!" />
```

#### **Child Component (ChildComponent.razor)**

```razor
@code {
    [Parameter] public string Message { get; set; } = "Default Message";
}
<h3>Message from Parent: @Message</h3>
```

**🔍 Result:**  
The child displays "Hello from Parent!", but if the child changes `Message`, the parent remains unchanged.

---

### C. Passing Data from Child to Parent (`EventCallback`)

When a child wants to notify the parent of changes, `EventCallback<T>` is used.

#### **Example:**

#### **Parent Component (Index.razor)**

```razor
<ChildComponent Name="Alice" OnNameChange="HandleNameChange" />
<p>Parent Name: @UserName</p>

@code {
    private string UserName = "Alice";

    private void HandleNameChange(string newName) {
        UserName = newName;
    }
}
```

##### **Child Component (ChildComponent.razor)**

```razor
@code {
    [Parameter] public string Name { get; set; }
    [Parameter] public EventCallback<string> OnNameChange { get; set; }

    private async Task ChangeName() {
        await Task.Delay(2000);
        await OnNameChange.InvokeAsync("Bob");
    }
}
<h3>Child Name: @Name</h3>
<button @onclick="ChangeName">Change Name</button>
```

**🔍 Result:**  
After clicking "Change Name", "Alice" changes to "Bob" in both parent and child.

---

### f. Summary

| **Binding Type**  | **Data Flow**            | **When Child Changes** | **When Parent Changes**  |
| ----------------- | ------------------------ | ---------------------- | ------------------------ |
| `[Parameter]`     | One-way (Parent → Child) | Does not affect Parent | Child receives new value |
| `@bind-Parameter` | Two-way (Parent ↔ Child) | Updates Parent         | Updates Child            |
| `EventCallback`   | Child → Parent           | Parent is updated      | Does not affect Child    |

---

## <a id="event-callback">2. Event Callback in Blazor</a>

### a. Overview

Event callbacks in Blazor enable communication from child components to parent components. They are used to notify parents when a specific action occurs in the child component.

---

### b. Using `EventCallback`

#### Child Component

```razor
<button @onclick="HandleClick">Click Me</button>

@code {
    [Parameter]
    public EventCallback OnButtonClick { get; set; }

    private async Task HandleClick()
    {
        await OnButtonClick.InvokeAsync();
    }
}
```

#### Parent Component

```razor
<ChildComponent OnButtonClick="HandleChildClick" />

@code {
    private void HandleChildClick()
    {
        Console.WriteLine("Button clicked in child component");
    }
}
```

In this example, when the button in the child component is clicked, it triggers `OnButtonClick`, which calls `HandleChildClick` in the parent component.

---

### c. Using `EventCallback<T>` to Pass Data

#### Child Component

```razor
<input @bind="UserName" />
<button @onclick="SendName">Send Name</button>

@code {
    [Parameter]
    public EventCallback<string> OnNameChanged { get; set; }

    private string UserName { get; set; }

    private async Task SendName()
    {
        await OnNameChanged.InvokeAsync(UserName);
    }
}
```

#### Parent Component

```razor
<ChildComponent OnNameChanged="HandleNameChange" />
<p>Received Name: @Name</p>

@code {
    private string Name;

    private void HandleNameChange(string newName)
    {
        Name = newName;
    }
}
```

Here, the child component sends the updated `UserName` value to the parent using `EventCallback<string>`.

---

### d. `EventCallback` vs `Action`

| Feature                | EventCallback | Action                            |
| ---------------------- | ------------- | --------------------------------- |
| Supports async         | ✅ Yes        | ❌ No                             |
| Works with UI events   | ✅ Yes        | ❌ No                             |
| Recommended for Blazor | ✅ Yes        | ✔️ Can be used, but less flexible |

### e. Why Use `InvokeAsync`?

The key difference between calling `OnButtonClick()` directly and using `OnButtonClick.InvokeAsync()` lies in **asynchronous execution** and **Blazor's event handling mechanism**.

#### Ensuring Proper Execution

- `InvokeAsync` ensures **correct execution of asynchronous event handlers**.
- Blazor **optimizes rendering and state updates** when using `InvokeAsync`.
- If `OnButtonClick` is not set, calling `OnButtonClick.InvokeAsync()` will **not throw an exception**, whereas `OnButtonClick()` might.

#### Example Comparison

##### ❌ **Without `InvokeAsync` (Incorrect)**

```razor
private void HandleClick()
{
    OnButtonClick(); // This may not work as expected for async methods
}
```

- If `OnButtonClick` is an **async delegate**, this can cause issues, such as **ignoring exceptions** or **blocking UI updates**.

##### ✅ **With `InvokeAsync` (Correct)**

```razor
private async Task HandleClick()
{
    await OnButtonClick.InvokeAsync();
}
```

- This ensures proper **awaiting** of any async operations in the parent component.

#### When to Use Which?

| Scenario                           | Use `InvokeAsync` | Use `OnButtonClick()` |
| ---------------------------------- | ----------------- | --------------------- |
| Handling async methods             | ✅ Yes            | ❌ No                 |
| Ensuring proper exception handling | ✅ Yes            | ❌ No                 |
| Simple synchronous event           | ❌ No             | ✅ Yes                |

##### 👉 **Conclusion**: Always use `InvokeAsync` when handling Blazor events, especially if they involve async code, to prevent unexpected behavior.

---

## <a id="cascading-parameters">3. Cascading Parameters in Blazor</a>

### a. Overview

Cascading Parameters allow you to pass data from a parent component to multiple nested child components without explicitly passing it through `[Parameter]` at each level. This simplifies state management in deeply nested component structures.

---

### b. Using Cascading Parameters

#### **Step 1: Define `CascadingValue` in the Parent Component**

```razor
<CascadingValue Value="Theme">
    <ChildComponent />
</CascadingValue>

@code {
    private string Theme = "Dark";
}
```

Here, `Theme` is made available to `ChildComponent` without needing `[Parameter]`.

#### **Step 2: Receive the Value in the Child Component with `[CascadingParameter]`**

```razor
<p>Current Theme: @Theme</p>

@code {
    [CascadingParameter]
    public string Theme { get; set; }
}
```

The `Theme` value will automatically be received as "Dark".

---

### c. Cascading Parameters in Multi-Level Components

You can pass values from the root component down multiple levels without explicitly defining `[Parameter]` at each step.

### **Parent Component**

```razor
<CascadingValue Value="UserName">
    <ParentComponent />
</CascadingValue>

@code {
    private string UserName = "Alice";
}
```

#### **Intermediate Component (`ParentComponent`)**

```razor
<ChildComponent />
```

#### **Child Component (`ChildComponent`)**

```razor
<p>Current User: @User</p>

@code {
    [CascadingParameter]
    public string User { get; set; }
}
```

👉 `ChildComponent` will receive `UserName = "Alice"` even though it is not a direct child of the root component.

---

### d. Cascading Parameters with Complex Data Types

You can pass complex objects instead of simple strings.

#### **Define a `UserInfo` Class**

```csharp
public class UserInfo
{
    public string Name { get; set; }
    public string Role { get; set; }
}
```

#### **Pass an Object from the Parent Component**

```razor
<CascadingValue Value="User">
    <ChildComponent />
</CascadingValue>

@code {
    private UserInfo User = new UserInfo { Name = "Alice", Role = "Admin" };
}
```

### **Receive the Object in the Child Component**

```razor
<p>Current User: @User.Name (@User.Role)</p>

@code {
    [CascadingParameter]
    public UserInfo User { get; set; }
}
```

The `ChildComponent` now has access to both `User.Name` and `User.Role`.

---

### e. When Should You Use Cascading Parameters?

| **Scenario**                                            | **Use Cascading Parameters?** |
| ------------------------------------------------------- | ----------------------------- |
| Passing data to **multiple child components**           | ✅ Yes                        |
| Passing data between **direct parent-child** components | ❌ Not necessary              |
| Passing **static data** (e.g., theme, user info)        | ✅ Suitable                   |
| Passing **frequently changing data**                    | ⚠️ May impact performance     |

---

### f. Difference Between `[Parameter]` and `[CascadingParameter]`

|                | `[Parameter]`                          | `[CascadingParameter]`           |
| -------------- | -------------------------------------- | -------------------------------- |
| Scope          | Direct **parent → child**              | **Parent → multiple children**   |
| Passing method | Explicitly defined using `[Parameter]` | Implicitly available to children |
| Performance    | More efficient for direct passing      | Less efficient if overused       |

---
