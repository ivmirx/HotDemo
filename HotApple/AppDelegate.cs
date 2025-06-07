using Tbc.Target;
using Tbc.Target.Config;

namespace HotApple;

[Register ("AppDelegate")]
public class AppDelegate : UIApplicationDelegate {
	public override UIWindow? Window { get; set; }

	public override bool FinishedLaunching(UIApplication app, NSDictionary? launchOptions)
    {
        Window = new UIWindow(UIScreen.MainScreen.Bounds);
        Window.RootViewController = new ViewController();
        Window.MakeKeyAndVisible();
        
        Task.Run(SetupReload);
        
        return true;
    }
    
    async Task SetupReload()
    {
        var reloadManager = new ReloadManager(Window);
        var targetServer = new TargetServer(new TargetConfiguration { ListenPort = 50130 });

        await targetServer.Run(reloadManager);
    }
}
