using Android.App;
using Android.OS;

namespace PercentLib
{
	[Activity(Label = "Android Percent Library", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{ 
		protected override void OnCreate(Bundle savedInstanceState)
		{
			RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
			base.OnCreate(savedInstanceState); 
			SetContentView(Resource.Layout.Main); 
		}
	}
}