using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;

namespace SnakeGame.Android;

[Activity(
    Label = "@string/app_name",
    MainLauncher = true,
    Icon = "@drawable/icon",
    AlwaysRetainTaskState = true,
    LaunchMode = LaunchMode.SingleInstance,
    ScreenOrientation = ScreenOrientation.SensorLandscape,
    ConfigurationChanges = ConfigChanges.Orientation
                           | ConfigChanges.KeyboardHidden
                           | ConfigChanges.ScreenSize,
    ResizeableActivity = false,
    UiOptions = UiOptions.None,
    Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
    HardwareAccelerated = true
    )]
public class Activity1 : AndroidGameActivity
{
    private SnakeGame _game;
    private View _view;

    protected override void OnCreate(Bundle bundle)
    {
        base.OnCreate(bundle);
            
        _game = new SnakeGame();
        _view = _game.Services.GetService(typeof(View)) as View;

        SetContentView(_view);
        _game.Run();
    }
}