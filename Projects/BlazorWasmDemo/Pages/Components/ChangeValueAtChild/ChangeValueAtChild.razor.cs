using Microsoft.AspNetCore.Components;

namespace BlazorWasmDemo.Pages.Components.ChangeValueAtChild;

public partial class ChangeValueAtChild : ComponentBase
{
    private string Message { get; set; } = "Message from Parent component";
}