using Microsoft.AspNetCore.Components;

namespace BlazorWasmDemo.Pages.Components.OneWayBinding;

public partial class OneWayBindingChild : ComponentBase
{
    [Parameter] public string Message { get; set; } = "Default Message Defined at Child Component";
}