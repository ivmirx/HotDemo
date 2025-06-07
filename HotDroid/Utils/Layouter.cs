using Android.Views;
using HotDroid.Views;

namespace HotDroid.Utils;

public static class Layouter
{
    public static void Setup(MainActivity activity, ReloadableView reloadableView)
    {
        var layout = new LinearLayout(activity)
        {
            Orientation = Orientation.Vertical
        };

        layout.SetGravity(GravityFlags.Center);
        
        layout.LayoutParameters = new LinearLayout.LayoutParams(
            ViewGroup.LayoutParams.MatchParent,
            ViewGroup.LayoutParams.MatchParent);

        layout.AddView(reloadableView, new LinearLayout.LayoutParams(
            ViewGroup.LayoutParams.WrapContent,
            ViewGroup.LayoutParams.WrapContent));

        activity.SetContentView(layout);
    }
}