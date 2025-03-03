using Microsoft.AspNetCore.Components;

namespace BlazorWasmDemo.Pages.Components.ChangeValueAtChild;

public partial class ChangeValueAtChildChild : ComponentBase
{
    [Parameter] public string Message { get; set; } = "Default Message Defined at Child Component";
    private void OnClick()
    {
        Message = "Message is updated by the button";
    }
}