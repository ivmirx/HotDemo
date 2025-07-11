using Tbc.Core.Models;
using Tbc.Target;
using Tbc.Target.Requests;

namespace HotApple;

public class ReloadManager : ReloadManagerBase
{
    readonly UIWindow _window;

    public ReloadManager(UIWindow window)
    {
        _window = window;
    }

    public override async Task<Outcome> ProcessNewAssembly(ProcessNewAssemblyRequest req)
    {
        // this just reloads view controllers, but you can add whatever you want here
        var primaryType = new[] { req.PrimaryType }.Concat(req.Assembly.GetTypes())
            .FirstOrDefault(x => x?.IsSubclassOf(typeof(UIViewController)) ?? false);

        if (primaryType is null)
            return CanOnlyReloadViewControllers();

        // we will just instantiate it using reflection, but you could register and then resolve it using DI
        // see the prism sample in TBC repository for more complicated examples
        var result = new TaskCompletionSource<Exception?>();
        _window.InvokeOnMainThread(() =>
        {
            try
            {
                _window.RootViewController = Activator.CreateInstance(primaryType) as UIViewController;
                result.TrySetResult(null);
            }
            catch (Exception ex) { result.TrySetResult(ex); }
        });

        return await result.Task is { } error
            ? FailedWithError(error)
            : Success();
    }

    public override async Task<Outcome> ExecuteCommand(CommandRequest req)
        => Success();

    static Outcome CanOnlyReloadViewControllers() =>
        new() { Success = false, Messages = { new OutcomeMessage {
                    Message = "This reload manager only knows how to reload view controllers " +
                              "and there aren't any in the incremental :'("
                }
            }
        };

    static Outcome FailedWithError(Exception ex) =>
        new() { Success = false, Messages = { new OutcomeMessage { Message = ex.ToString() } } };

    static Outcome Success() =>
        new() { Success = true };
}