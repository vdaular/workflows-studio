using System.Text.Json.Nodes;
using Elsa.Api.Client.Extensions;
using Elsa.Api.Client.Resources.ActivityDescriptors.Enums;
using Elsa.Api.Client.Resources.ActivityDescriptors.Models;
using Elsa.Api.Client.Shared.Models;
using Elsa.Studio.Contracts;
using Elsa.Studio.Workflows.Args;
using Elsa.Studio.Workflows.Designer.Components;
using Elsa.Studio.Workflows.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Elsa.Studio.Workflows.DiagramDesigners.Flowcharts;

public partial class FlowchartDesignerWrapper
{
    [Parameter] public JsonObject Flowchart { get; set; } = default!;
    [Parameter] public bool IsReadOnly { get; set; }
    [Parameter] public Func<JsonObject, Task>? ActivitySelected { get; set; }
    [Parameter] public Func<ActivityEmbeddedPortSelectedArgs, Task>? ActivityEmbeddedPortSelected { get; set; }
    [Parameter] public Func<Task>? GraphUpdated { get; set; }
    [CascadingParameter] public DragDropManager DragDropManager { get; set; } = default!;
    [Inject] private IActivityIdGenerator ActivityIdGenerator { get; set; } = default!;
    internal FlowchartDesigner Designer { get; set; } = default!;

    public async Task LoadFlowchartAsync(JsonObject flowchart)
    {
        Flowchart = flowchart;
        await Designer.LoadFlowchartAsync(flowchart);
    }
    
    public async Task UpdateActivityAsync(string id, JsonObject activity)
    {
        if (IsReadOnly)
            throw new InvalidOperationException("Cannot update activity because the designer is read-only.");
        
        if (activity != Flowchart)
            await Designer.UpdateActivityAsync(id, activity);
    }

    public async Task<JsonObject> ReadRootActivityAsync() => await Designer.ReadFlowchartAsync();

    private bool GetNameExists(string name) => Flowchart.GetActivities().Any(x => x.GetId() == name);

    private string GenerateNextName(ActivityDescriptor activityDescriptor)
    {
        var max = 100;
        var count = GetNextNumber(activityDescriptor);

        while (count++ < max)
        {
            var nextId = $"{activityDescriptor.Name}{count}";
            if (!GetNameExists(nextId))
                return nextId;
        }

        throw new Exception("Could not generate a unique name.");
    }
    
    private int GetNextNumber(ActivityDescriptor activityDescriptor)
    {
        return Flowchart.GetActivities().Count(x => x.GetTypeName() == activityDescriptor.TypeName);
    }

    private async Task AddNewActivityAsync(ActivityDescriptor activityDescriptor, double x, double y)
    {
        var newActivity = new JsonObject(new Dictionary<string, JsonNode?>
        {
            ["id"] = ActivityIdGenerator.GenerateId(),
            ["name"] = GenerateNextName(activityDescriptor),
            ["type"] = activityDescriptor.TypeName,
            ["version"] = activityDescriptor.Version,
        });
        
        newActivity.SetDesignerMetadata(new ActivityDesignerMetadata
        {
            Position = new Position(x, y)
        });
        
        // If the activity is a trigger and it's the first trigger on the flowchart, set the trigger property to true.
        if (activityDescriptor.Kind == ActivityKind.Trigger && Flowchart.GetActivities().All(activity => activity.GetCanStartWorkflow() != true))
            newActivity.SetCanStartWorkflow(true);

        await Designer.AddActivityAsync(newActivity);
        
        ActivitySelected?.Invoke(newActivity);
    }

    private void OnDragOver(DragEventArgs e)
    {
        if (DragDropManager.Payload is not ActivityDescriptor)
        {
            e.DataTransfer.DropEffect = "none";
            return;
        }

        e.DataTransfer.DropEffect = "move";
    }

    private async Task OnDrop(DragEventArgs e)
    {
        if (DragDropManager.Payload is not ActivityDescriptor activityDescriptor)
            return;

        var x = e.PageX;
        var y = e.PageY;

        await AddNewActivityAsync(activityDescriptor, x, y);
    }

    private async Task OnCanvasSelected()
    {
        if (ActivitySelected != null)
            await ActivitySelected(Flowchart);
    }

    private async Task OnZoomToFitClick() => await Designer.ZoomToFitAsync();
    private async Task OnCenterContentClick() => await Designer.CenterContentAsync();
}