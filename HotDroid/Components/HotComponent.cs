using Android.Views;

namespace HotDroid.Components;

public class HotComponent
{
    public void Attach(View view)
    {
        if (view is TextView tv)
        {
            tv.Text = "🧩 Hot Reloaded via Component!!";
        }
    }
}