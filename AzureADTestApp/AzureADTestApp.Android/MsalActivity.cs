
using Android.App;
using Android.Content;
using Microsoft.Identity.Client;

namespace AzureADTestApp.Droid
{
	[Activity]
	[IntentFilter(new[] { Intent.ActionView },
		Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
		DataScheme = "msauth",
		DataHost = "COM.SOMETHING.SOMETHING.APPNAMEHERE")]
	public class MsalActivity : BrowserTabActivity
	{
	}
}