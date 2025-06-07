using Android.Content;
using Android.Views;
using HotDroid.Utils;

namespace HotDroid.Views;

public class ReloadableView : TextView
{
    public dynamic? Component { get; set; }
    
    public Type? StaticComponentType { get; set; }


    public ReloadableView(Context context) : base(context)
    {
        // can't be reloaded here, native views are not reloadable!!
        Text = "Initial text, do not edit here";
        SetTextColor(Android.Graphics.Color.Black);
        TextSize = 20;
    }

    protected override void OnAttachedToWindow()
    {
        base.OnAttachedToWindow();
    
        if (Component != null)
        {
            var method = Component.GetType().GetMethod("Attach", new[] { typeof(View) });
            method?.Invoke(Component, new dynamic[] { this });
        }
        
        if (StaticComponentType != null)
        {
            try
            {
                StaticMethodInvoker.InvokeStaticMethod(StaticComponentType, "Attach", this);
            }
            catch (Exception ex)
            {
                Text = $"Error invoking static method: {ex.Message}";
            }
        }

    }
}

