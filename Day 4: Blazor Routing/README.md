# Day 4: Routing and Navigation

## 📖 Table of Contents

1. [Blazor Routing & Navigation](#blazor-routing-and-navigation)
2. [Blazor Routing vs. React Router](#blazor-routing-vs-react-router)
3. [`+=` vs `-=` in Blazor (Event Subscription & Unsubscription)](#event-subcription-and-unsubcription)

---

## <a id="blazor-routing-and-navigation">1. Blazor Routing & Navigation</a>

### a. Basic Routing Structure in Blazor

Blazor uses the **Router component** to handle navigation between pages in the application.

#### Basic structure of `App.razor`

```razor
<Router AppAssembly="@typeof(Program).Assembly">
    <Found Context="routeData">
        <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
    </Found>
    <NotFound>
        <h3>Page not found!</h3>
    </NotFound>
</Router>
```

- **Router**: Defines the routing system.
- **RouteView**: Renders the component corresponding to the URL.
- **NotFound**: Displays when no matching route is found.

---

### b. Defining Routes in Blazor

Each component in Blazor can represent a page by using the `@page` directive at the top of the file.

#### Example: Defining Home & About pages

```razor
// Home.razor
@page "/"
<h3>Home Page</h3>

// About.razor
@page "/about"
<h3>About Page</h3>
```

- `/` maps to the **Home** page.
- `/about` maps to the **About** page.

---

### c. Navigating Between Pages

There are two main ways to navigate in Blazor:

1. **Using `<NavLink>`** (Similar to `<Link>` in React)
2. **Using `NavigationManager` in C# code**

### Using `<NavLink>`

```razor
<NavLink href="/" class="nav-link">Home</NavLink>
<NavLink href="/about" class="nav-link">About</NavLink>
```

#### Navigating in C# with `NavigationManager`

```razor
@inject NavigationManager Navigation

<button @onclick="NavigateToAbout">Go to About</button>

@code {
    void NavigateToAbout() {
        Navigation.NavigateTo("/about");
    }
}
```

- `Navigation.NavigateTo("/about")` navigates programmatically.
- Adding `forceLoad: true` forces a full-page reload.

---

### d. Passing Parameters via URL

Blazor supports **passing parameters via the URL** using `{parameter}` in `@page`.

### Example: Receiving `id` from the URL

```razor
@page "/user/{id}"
<h3>User ID: @Id</h3>

@code {
    [Parameter] public string Id { get; set; }
}
```

- Visiting `/user/123` will set `Id = "123"`.
- The `{id}` value is automatically bound to the `[Parameter]` property.

#### Receiving Multiple Parameters

```razor
@page "/user/{id}/{name}"
<h3>ID: @Id, Name: @Name</h3>

@code {
    [Parameter] public string Id { get; set; }
    [Parameter] public string Name { get; set; }
}
```

- Visiting `/user/1/Alice` will set `Id = "1"` and `Name = "Alice"`.

---

### e. Listening to URL Changes

Blazor allows listening to URL changes using the `LocationChanged` event.

### Example: Listening for URL Changes

```razor
@inject NavigationManager Navigation

<h3>Current URL: @currentUrl</h3>

@code {
    private string currentUrl;

    protected override void OnInitialized() {
        currentUrl = Navigation.Uri;
        Navigation.LocationChanged += HandleLocationChanged;
    }

    private void HandleLocationChanged(object sender, LocationChangedEventArgs e) {
        currentUrl = e.Location;  // Update new URL
        StateHasChanged();  // Refresh UI
    }

    public void Dispose() {
        Navigation.LocationChanged -= HandleLocationChanged;
    }
}
```

- When the URL changes, `HandleLocationChanged` updates `currentUrl`.
- `StateHasChanged()` triggers a UI refresh.

---

### f. Handling 404 Not Found Pages

When a user navigates to an undefined route, Blazor allows displaying a **custom error page**.

### Handling Page Not Found

```razor
<NotFound>
    <h3>Oops! Page not found.</h3>
</NotFound>
```

- This appears when a user enters an unknown route.

---

## <a id="blazor-routing-vs-react-router">2. Blazor Routing vs. React Router</a>

### a. Overview

Blazor and React use different routing mechanisms, each optimized for their respective frameworks:

- **Blazor Routing**: Built-in routing with the `@page` directive and `Router` component.
- **React Router**: A third-party library (`react-router-dom`) for handling navigation in React apps.

---

### b. Defining Routes

#### Blazor

Blazor uses the `@page` directive in each component to define routes.

```razor
// Home.razor
@page "/"
<h3>Home Page</h3>
```

#### React

React uses `react-router-dom` to define routes in a central `Routes` component.

```jsx
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Home from "./Home";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />
      </Routes>
    </BrowserRouter>
  );
}
```

---

### c. Navigation Methods

#### Blazor

Navigation is done using `<NavLink>` or `NavigationManager.NavigateTo()`.

```razor
<NavLink href="/about">About</NavLink>
@inject NavigationManager Navigation
<button @onclick="() => Navigation.NavigateTo('/about')">Go to About</button>
```

#### React

React uses `<Link>` or `useNavigate` hook.

```jsx
import { Link, useNavigate } from "react-router-dom";

<Link to="/about">About</Link>;

const navigate = useNavigate();
<button onClick={() => navigate("/about")}>Go to About</button>;
```

---

### d. Passing URL Parameters

#### Blazor

Blazor uses `{}` inside `@page` and `[Parameter]` to retrieve values.

```razor
@page "/user/{id}"
<h3>User ID: @Id</h3>
@code {
    [Parameter] public string Id { get; set; }
}
```

#### React

React uses `:param` in route paths and `useParams` to retrieve values.

```jsx
import { useParams } from "react-router-dom";
const { id } = useParams();
<h3>User ID: {id}</h3>;
```

---

### e. Listening to Route Changes

#### Blazor

Uses `Navigation.LocationChanged` event.

```razor
@inject NavigationManager Navigation

@code {
    protected override void OnInitialized() {
        Navigation.LocationChanged += HandleLocationChanged;
    }

    private void HandleLocationChanged(object sender, LocationChangedEventArgs e) {
        Console.WriteLine($"Navigated to: {e.Location}");
    }
}
```

#### React

Uses the `useLocation` hook.

```jsx
import { useLocation } from "react-router-dom";
const location = useLocation();
console.log("Navigated to:", location.pathname);
```

---

### f. Handling 404 Pages

#### Blazor

```razor
<NotFound>
    <h3>Page not found!</h3>
</NotFound>
```

#### React

```jsx
<Route path="*" element={<h3>Page not found!</h3>} />
```

---

### g. Summary

| Feature          | Blazor Routing               | React Router            |
| ---------------- | ---------------------------- | ----------------------- |
| Route Definition | `@page` directive            | `<Route>` component     |
| Navigation       | `<NavLink>`, `NavigateTo()`  | `<Link>`, `useNavigate` |
| URL Parameters   | `{param}`, `[Parameter]`     | `:param`, `useParams`   |
| Route Listening  | `Navigation.LocationChanged` | `useLocation()`         |
| 404 Handling     | `<NotFound>`                 | `<Route path="*" />`    |

---

## <a id="event-subcription-and-unsubcription">3. `+=` vs `-=` in Blazor (Event Subscription & Unsubscription)</a>

### a. Understanding `+=` (Event Subscription)

In Blazor (and C# in general), `+=` is used to subscribe a method to an event.

```razor
@code {
    protected override void OnInitialized()
    {
        Navigation.LocationChanged += HandleLocationChanged;
    }

    private void HandleLocationChanged(object sender, LocationChangedEventArgs e)
    {
        Console.WriteLine($"Navigated to: {e.Location}");
    }
}
```

➡ **What happens?**

- Every time `Navigation.LocationChanged` is triggered, `HandleLocationChanged` will be executed.

---

### b. Understanding `-=` (Event Unsubscription)

To prevent memory leaks and unintended behavior, you should **unsubscribe** from events when the component is disposed.

```razor
@code {
    protected override void OnInitialized()
    {
        Navigation.LocationChanged += HandleLocationChanged;
    }

    public void Dispose()
    {
        Navigation.LocationChanged -= HandleLocationChanged;
    }

    private void HandleLocationChanged(object sender, LocationChangedEventArgs e)
    {
        Console.WriteLine($"Navigated to: {e.Location}");
    }
}
```

➡ **Why unsubscribe?**  
If you don't call `-=` in `Dispose()`, the event handler may still be active **even after the component is removed**, leading to potential issues like:

- **Memory leaks** (Handlers still in memory but unused).
- **Multiple event executions** (If the component is reloaded, it may subscribe again, causing duplicate event triggers).

---

### c. What Happens If You Forget to Unsubscribe?

Imagine we navigate back and forth between pages that **subscribe** but don’t **unsubscribe**.  
Each time we revisit, we **add another** `HandleLocationChanged` to the event.

- On the **first visit**, `HandleLocationChanged` runs **once**.
- On the **second visit**, it runs **twice**.
- On the **third visit**, it runs **three times**!
- ...and so on.

This can cause performance issues and unintended behavior.

---

### d. Best Practices for Using Events in Blazor

✅ **Always unsubscribe (`-=`) in `Dispose()`**  
✅ **Use `+=` in `OnInitialized` or `OnInitializedAsync`**  
✅ **Avoid subscribing inside render logic (e.g., inside `OnAfterRender`)**
