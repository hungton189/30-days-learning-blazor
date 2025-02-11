# 🚀 WebAssembly & Blazor Learning Notes

This repository contains my learning notes on **Blazor Server, Blazor WebAssembly, and WebAssembly fundamentals**. The goal is to master how Blazor and WebAssembly work, their differences, and how to interact with the DOM using WebAssembly.

---

## 📖 Table of Contents

1. [Introduction to Blazor](#introduction-to-blazor)
2. [Creating a Simple Blazor WebAssembly Project](#creating-a-simple-blazor-webassembly-project)
3. [Razor Syntax in Blazor](#razor-syntax-in-blazor)
4. [Blazor Server vs. Blazor WebAssembly](#blazor-server-vs-blazor-webassembly)
5. [How WebAssembly Works](#how-webassembly-works)
6. [Interop Between WebAssembly and JavaScript](#interop-between-webassembly-and-javascript)
7. [Demo: Calling JavaScript from WebAssembly](#demo-calling-javascript-from-webassembly)
8. [Blazor WebAssembly Demo](#blazor-webassembly-demo)

---

## 📌 Introduction to Blazor

Blazor is a **.NET web framework** that enables developers to build interactive web applications using **C# and Razor** instead of JavaScript.

Blazor provides two hosting models:

1. **Blazor Server** - Runs on the server and updates the UI via **SignalR**.
2. **Blazor WebAssembly (WASM)** - Runs in the browser using **WebAssembly**.

---

## 🔹 Razor Syntax in Blazor

<details>
  <summary>Click to expand</summary>

- Razor is a **markup syntax** used in Blazor to combine **C# code with HTML**.
- Example of Razor syntax in Blazor:

```razor
@page "/"
<h1>Hello, @name!</h1>

@code {
    private string name = "Blazor";
}
```

- **Directives:** `@code`, `@inject`, `@if`, `@foreach`, `@bind`.
- **Component example:**

```razor
<button @onclick="Increment">Click me</button>
<p>Count: @count</p>

@code {
    private int count = 0;
    private void Increment() => count++;
}
```

</details>

## 📌 Creating a Simple Blazor WebAssembly Project

<details>
  <summary>Click to expand</summary>

### **Steps to Create a Blazor WebAssembly Project**

1. Open a terminal and run:

```sh
 dotnet new blazorwasm -o BlazorWasmDemo
```

2. Navigate into the project folder:

```sh
 cd BlazorWasmDemo
```

3. Run the project:

```sh
 dotnet run
```

4. Open the browser and go to `https://localhost:5001` to see the Blazor WebAssembly app running.
</details>

---

## 🔥 Blazor Server vs. Blazor WebAssembly

<details>
  <summary>Click to expand</summary>

### **1️⃣ Blazor Server**

- The application runs **on the server**.
- UI updates are handled over **WebSockets (SignalR)**.
- Requires a **persistent connection** to function.
- **Fast initial load** because only HTML/JS is sent to the client.
- **Downside:** Higher **latency**, and requires a stable connection.

### **2️⃣ Blazor WebAssembly**

- Runs **entirely in the browser** using **WebAssembly**.
- **No server connection needed** after initial load.
- **Slower initial load** due to downloading the .NET runtime.
- **Upside:** Can work **offline** after loading.

### **Comparison Table**

| Feature         | Blazor Server   | Blazor WebAssembly    |
| --------------- | --------------- | --------------------- |
| Execution       | Server-side     | Client-side (Browser) |
| UI Updates      | Via WebSockets  | Directly in Browser   |
| Offline Support | ❌ No           | ✅ Yes                |
| Load Time       | ✅ Fast         | ❌ Slower (initially) |
| Dependency      | Requires server | Runs independently    |

</details>

---

## ⚙️ How WebAssembly Works

<details>
  <summary>Click to expand</summary>

- **WebAssembly (WASM)** is a binary instruction format that runs in modern web browsers **alongside JavaScript**.
- Allows running **high-performance code** written in languages like **C, C++, Rust, and C#**.
- The browser doesn’t understand **C# or .NET**, but:
  - Blazor WebAssembly compiles C# into **WebAssembly bytecode**.
  - The **.NET runtime** runs inside WebAssembly and executes C# code.

### **Why WebAssembly?**

✅ **Faster** than JavaScript for compute-heavy tasks.  
✅ **Runs securely** inside a sandboxed environment.  
✅ **Compatible** with existing web technologies.

</details>

---

## 🔄 Interop Between WebAssembly and JavaScript

<details>
  <summary>Click to expand</summary>

### **Can WebAssembly Access the DOM?**

- **No**, WebAssembly does **not** have built-in DOM APIs like JavaScript.
- Runs in a **sandboxed environment** and cannot manipulate the DOM **directly**.

### **How Can WebAssembly Interact with the DOM?**

- **By calling JavaScript functions from WebAssembly** using **WebAssembly-JavaScript Interop**.
- JavaScript handles DOM updates, while WebAssembly performs the heavy logic.

### **Example Workflow**

1. **WebAssembly calculates something** (e.g., `42`).
2. **WebAssembly calls a JavaScript function**.
3. **JavaScript updates the DOM**.

</details>

---

## 🛠 Demo: Calling JavaScript from WebAssembly

<details>
  <summary>Click to expand</summary>

### **Steps to Run the Demo**

1. Create a **WebAssembly module (`demo.wat`)**:

```wat
(module
  (import "env" "updateDOM" (func $updateDOM (param i32)))

  (func $calculate (export "calculate")
    i32.const 42
    call $updateDOM
  )
)
```

2. Convert `.wat` to `.wasm`:

```sh
wat2wasm demo.wat -o demo.wasm
```

3. Create an **HTML page (`index.html`)**:

```html
<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>WebAssembly DOM Demo</title>
  </head>
  <body>
    <h1>WebAssembly DOM Interaction</h1>
    <p id="output">Waiting for WebAssembly...</p>
    <button id="run">Run WebAssembly</button>

    <script>
      function updateDOM(value) {
        document.getElementById(
          "output"
        ).innerText = `Result from WASM: ${value}`;
      }

      async function loadWasm() {
        const response = await fetch("demo.wasm");
        const bytes = await response.arrayBuffer();
        const wasmModule = await WebAssembly.instantiate(bytes, {
          env: { updateDOM },
        });

        document.getElementById("run").addEventListener("click", () => {
          wasmModule.instance.exports.calculate();
        });
      }

      loadWasm();
    </script>
  </body>
</html>
```

</details>

---
