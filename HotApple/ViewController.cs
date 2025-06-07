namespace HotApple;

public class ViewController : UIViewController
{
    UILabel? label;
    
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        View.BackgroundColor = UIColor.SystemBackground;

        label = new UILabel(View.Bounds)
        {
            TextAlignment = UITextAlignment.Center,
            AutoresizingMask = UIViewAutoresizing.All,
            Text = "This text can be directly edited for reloading",
            Font = UIFont.PreferredHeadline
        };

        View.AddSubview(label);

    }
}
