First, install [tbc](https://github.com/rdavisau/tbc) following its instructions.

### Both platforms

* After starting `tbc <config>`, wait 5-15 seconds for system assemblies to load from the app before attempting a hot reload. You'll see lots of activity in the terminal when they're arriving to the host.
* Your projects must have shared Mono runtime and fast assembly deployment enabled.
* Use different ports for different platforms.
* If it refuses to work for some reason, close the solution, delete `bin/` and `obj/` directories, rebuild-restart the app, and then run `tbc` again.

### iOS
Very close to the sample you can find in `tbc` repository.

1. Build and run your app in Debug mode on the iOS simulator.
2. Run `tbc ./reload-ios.json` from the solution's root (for some reason it does not work from the project's root).
3. Wait for assemblies to load.
4. That's it, you can edit your view controllers' content directly.

This sample's `ReloadManager` only reloads changes in `UIViewController` subclasses. So if you want to edit custom subviews, either edit something in the controller first or make `tbc` watch the directory recursively (see its Readme). In real-life apps, you may want to only reload the currently showing tab/screen because it's more straightforward.

### Android

I've managed to get `tbc` running on Android but, unfortunately, it comes with some significant limitations because you can't reload native types (everything derived from Java classes). JNI registers all types at compile time and then refuses to accept new ones.

So, the only way is to use plain C# classes for any UI-related work. Android native `View` becomes just a shell, while there are at least three separate classes: one for creating child views (can be static), one for layouting them (also can be static), and one for holding references to the created views (this can be done in the parent view, but then you won't be able to add new UI elements on the fly).

1. Launch Android emulator.
2. Run `adb forward tcp:50125 tcp:50125` in the terminal.
3. Run Debug build of your app in emulator.
4. Run `tbc ./reload-droid.json`.
5. Wait for assemblies, it takes longer than on iOS.
6. Proceed with hot reloading regular or static component, but do not edit `ReloadableView` directly (it's not so reloadable, after all).

Watch out: reloadable parts must be stored in a subdirectory because `tbc` recompiles entire directory, so all native views should be isolated outside of it.
