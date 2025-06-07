using HotDroid.Utils;
using HotDroid.Views;
using Tbc.Target;
using Tbc.Target.Config;

namespace HotDroid;

[Activity(Label = "HotDroid", MainLauncher = true)]
public class MainActivity : Activity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        
        Layouter.Setup(this, new ReloadableView(this));
        Task.Run(SetupReload);
    }

    async Task SetupReload()
    {
        var reloadManager = new DroidReloadManager(this);
        var server = new TargetServer(new TargetConfiguration { ListenPort = 50125 });
        await server.Run(reloadManager);
    }
}