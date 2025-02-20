# Blazor Learning - Day 2

## 📖 Table of Contents

1. [Blazor Components Overview](#blazor-components-overview)
2. [One-way Binding in Blazor](#one-way-binding-in-blazor)
3. [Two-way Binding in Blazor vs React](#two-way-binding-in-blazor-vs-react)
4. [State Updates in Blazor vs React](#state-updates-in-blazor-vs-react)
5. [Event Handling in Blazor](#event-handling-in-blazor)
6. [Three Ways to Write C# Code in Blazor Components](#three-ways-to-write-c-code-in-blazor-components)

---

## 1️⃣ Blazor Components Overview <a id="blazor-components-overview"></a>
- A Blazor application consists of **components**, which are reusable UI elements.
- A component is typically a **`.razor` file**, containing UI and logic.
- Example of a simple Blazor component:
  ```razor
  <h3>Hello, @Name!</h3>
  
  @code {
      private string Name = "Blazor";
  }
  ```

## 2️⃣ One-way Binding in Blazor <a id="one-way-binding-in-blazor"></a>
- One-way binding means **data flows from the code to the UI**, but not in the other direction.
- Example:
  ```razor
  <p>Current Time: @currentTime</p>
  
  @code {
      private string currentTime = DateTime.Now.ToString("HH:mm:ss");
  }
  ```
  - The value is displayed but not directly editable.

---

## 3️⃣ Two-way Binding in Blazor vs React <a id="two-way-binding-in-blazor-vs-react"></a>

### **React:**
- React **does not have built-in two-way binding** like Angular or Blazor.
- Instead, React uses **one-way data flow** (unidirectional binding) and updates state via `useState()`.
- Example:
  ```jsx
  const [text, setText] = useState("");
  
  return (
    <input value={text} onChange={(e) => setText(e.target.value)} />
  );
  ```
- The state is **updated explicitly** via `setText()`, and React re-renders the component when the state changes.

### **Blazor:**
- Blazor supports **built-in two-way binding** using the `@bind` directive.
- Example:
  ```razor
  <input @bind="message" />
  
  @code {
      private string message = "";
  }
  ```
- Blazor automatically syncs the `message` variable with the input field, eliminating the need for a manual `onChange` event like in React.

---

## 4️⃣ State Updates in Blazor vs React <a id="state-updates-in-blazor-vs-react"></a>

### **React State Updates (Automatic Re-rendering):**
- When calling `setState()` in React, the UI automatically updates:
  ```jsx
  const [count, setCount] = useState(0);
  
  return (
    <button onClick={() => setCount(count + 1)}>Increase</button>
  );
  ```
- React re-renders the component whenever the state changes.

### **Blazor State Updates:**
- In Blazor, state updates **inside an event handler** automatically trigger a UI update:
  ```razor
  <button @onclick="IncreaseCount">Increase</button>
  <p>Count: @count</p>
  
  @code {
      private int count = 0;
      
      private void IncreaseCount()
      {
          count++;
      }
  }
  ```
- However, **if a state update happens outside an event handler (e.g., inside an async function), Blazor does not re-render the UI automatically**.
- Example:
  ```razor
  private async Task StartTimer()
  {
      await Task.Delay(1000); // Background task
      currentTime = DateTime.Now.ToString("HH:mm:ss");
      StateHasChanged(); // 🔥 Required to update UI
  }
  ```
  **StateHasChanged()** is required to manually trigger a UI update.

---

## 5️⃣ Event Handling in Blazor <a id="event-handling-in-blazor"></a>

### **Inside an Event Handler (Auto UI Update)**
- When a state change happens inside an event handler (e.g., `@onclick`), Blazor **automatically re-renders**:
  ```razor
  <button @onclick="UpdateTime">Update Time</button>
  <p>Current Time: @currentTime</p>
  
  @code {
      private string currentTime = DateTime.Now.ToString("HH:mm:ss");
      
      private void UpdateTime()
      {
          currentTime = DateTime.Now.ToString("HH:mm:ss"); // UI auto-updates
      }
  }
  ```

### **Outside an Event Handler (Needs `StateHasChanged()` for UI Update)**
- If a state change occurs **outside an event handler**, UI **does NOT update automatically**.
- Example:
  ```razor
  private async Task StartTimer()
  {
      await Task.Delay(1000); // Background task
      currentTime = DateTime.Now.ToString("HH:mm:ss");
      StateHasChanged(); // ✅ Required for UI update
  }
  ```

---

## 6️⃣ Three Ways to Write C# Code in Blazor Components <a id="three-ways-to-write-c-code-in-blazor-components"></a>

1. **Inline `@code` Block (Most Common)**
   ```razor
   <h3>Hello, @Name!</h3>
   
   @code {
       private string Name = "Blazor";
   }
   ```

2. **Partial Class (Separation of Logic)**
   - `MyComponent.razor`
     ```razor
     <h3>Hello, @Name!</h3>
     ```
   - `MyComponent.razor.cs`
     ```csharp
     public partial class MyComponent : ComponentBase
     {
         private string Name = "Blazor";
     }
     ```

3. **Base Class Approach (Used for Shared Logic)**
   - Create a base class `ComponentBase.cs` and inherit from it.

---

