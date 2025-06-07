using Android.Views;
using Android.Graphics;

namespace HotDroid.Components;

public static class StaticHotComponent
{
    public static void Attach(View view)
    {
        if (view is TextView tv)
        {
            tv.Text = "🔥 Hot Reloaded via Static Component!";
            tv.SetTextColor(Color.Red);
            tv.SetTypeface(null, TypefaceStyle.Bold);
        }
    }
}