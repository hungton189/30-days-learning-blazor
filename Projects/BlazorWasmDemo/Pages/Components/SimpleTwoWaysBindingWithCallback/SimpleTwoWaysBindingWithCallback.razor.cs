using Microsoft.AspNetCore.Components;

namespace BlazorWasmDemo.Pages.Components.SimpleTwoWaysBindingWithCallback;

public partial class SimpleTwoWaysBindingWithCallback : ComponentBase
{
    private string Message { get; set; } = "Message from Parent component";

    private void OnChangeMessage(string message)
    {
        Message = message;
        StateHasChanged();
    }
}