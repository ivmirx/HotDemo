using System.Reflection;
using HotDroid.Views;
using Tbc.Core.Models;
using Tbc.Target;
using Tbc.Target.Requests;

namespace HotDroid.Utils;

public class DroidReloadManager : ReloadManagerBase
{
    readonly MainActivity _activity;

    public DroidReloadManager(MainActivity activity)
    {
        _activity = activity;
    }

    public override async Task<Outcome> ProcessNewAssembly(ProcessNewAssemblyRequest req)
    {
        var result = new TaskCompletionSource<Exception?>();

        try
        {
            var types = req.Assembly.GetTypes();
            var match = types.Where(t => t.Name.Contains("Component")).ToArray();
            foreach (var t in match)
            {
                Console.WriteLine($"Found type: {t.FullName}, Base types: {t.BaseType}, Implements: {string.Join(", ", t.GetInterfaces().Select(i => i.FullName))}");
            }
            
            var instanceComponentType = req.Assembly.GetTypes()
                .FirstOrDefault(t =>
                    t.Name.EndsWith("Component", StringComparison.OrdinalIgnoreCase) &&
                    t is { IsInterface: false, IsAbstract: false } &&
                    t.GetMethod("Attach") is not null);

            var staticComponentType = req.Assembly.GetTypes()
                .FirstOrDefault(t =>
                    t.Name.EndsWith("Component", StringComparison.OrdinalIgnoreCase) &&
                    t.IsAbstract && t.IsSealed &&
                    t.GetMethod("Attach", BindingFlags.Static | BindingFlags.Public) is not null);
            
            var selectedComponentType = instanceComponentType ?? staticComponentType;
            bool isStaticComponent = selectedComponentType == staticComponentType && instanceComponentType == null;
            
            if (instanceComponentType == null && staticComponentType == null)
                return FailedWithError(new Exception("No component found in assembly"));
            
            _activity.RunOnUiThread(() =>
            {
                try
                {
                    var reloadableView = new ReloadableView(_activity);
        
                    if (isStaticComponent)
                    {
                        reloadableView.StaticComponentType = selectedComponentType;
                    }
                    else
                    {
                        var reloadedComponent = Activator.CreateInstance(selectedComponentType);
                        reloadableView.Component = reloadedComponent;
                    }
        
                    Layouter.Setup(_activity, reloadableView);
                    result.TrySetResult(null);
                }
                catch (Exception ex)
                {
                    result.TrySetResult(ex);
                }

            });
        }
        catch (Exception ex)
        {
            return FailedWithError(ex);
        }

        var error = await result.Task;
        return error != null ? FailedWithError(error) : Success();
    }


    public override Task<Outcome> ExecuteCommand(CommandRequest req) => Task.FromResult(Success());

    static Outcome FailedWithError(Exception ex) =>
        new() { Success = false, Messages = { new OutcomeMessage { Message = ex.ToString() } } };

    static Outcome Success() =>
        new() { Success = true };
}