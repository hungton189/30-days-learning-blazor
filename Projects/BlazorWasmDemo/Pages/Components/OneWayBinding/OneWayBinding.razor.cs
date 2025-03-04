using Microsoft.AspNetCore.Components;

namespace BlazorWasmDemo.Pages.Components.OneWayBinding;

public partial class OneWayBinding : ComponentBase
{
    private string Message { get; set; } = "Message from Parent component";
}