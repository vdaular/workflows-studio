using Elsa.Api.Client.Resources.ActivityDescriptors.Models;
using Microsoft.AspNetCore.Components;

namespace Elsa.Studio.Workflows.Components.WorkflowDefinitionEditor.Components.ActivityProperties.Tabs;

/// <summary>
/// The info tab for an activity.
/// </summary>
public partial class InfoTab
{
    /// <summary>
    /// The activity descriptor.
    /// </summary>
    [Parameter] public ActivityDescriptor ActivityDescriptor { get; set; } = default!;

    private IDictionary<string, string?> ActivityInfo { get; } = new Dictionary<string, string?>();

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        ActivityInfo["Type"] = ActivityDescriptor.TypeName;
        ActivityInfo["Description"] = ActivityDescriptor.Description;
    }
}