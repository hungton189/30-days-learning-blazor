# Day 4: Routing and Navigation

## 📖 Table of Contents

1. [Component Parameters in Blazor](#component-parameters)

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
