using Microsoft.AspNetCore.Components;

namespace BlazorWasmDemo.Pages.Components.SimpleTwoWaysBindingWithCallback;

public partial class SimpleTwoWaysBindingWithCallbackChild : ComponentBase
{
    [Parameter] public string Message { get; set; } = "Default Message Defined at Child Component";

    [Parameter] public EventCallback<string> OnChangeMessage { get; set; }
    private void OnClick()
    {
        var newMessage = "Message is updated by the button";
        OnChangeMessage.InvokeAsync(newMessage);
    }
}