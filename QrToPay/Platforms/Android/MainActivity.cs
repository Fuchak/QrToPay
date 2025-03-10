﻿using Android.App;
using Android.Content.PM;

namespace QrToPay
{
    [Activity(Theme = "@style/Maui.SplashTheme", 
        ScreenOrientation = ScreenOrientation.Portrait, 
        MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | 
                               ConfigChanges.Orientation | 
                               ConfigChanges.UiMode | 
                               ConfigChanges.ScreenLayout | 
                               ConfigChanges.SmallestScreenSize | 
                               ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
    }
}