using Microsoft.Identity.Client;
using Xamarin.Forms;

namespace AzureADTestApp
{
	public partial class App : Application
	{
		public static IPublicClientApplication PCA = null;

		public static string ClientID = "SOME_CLIENT_ID";
		public static string[] DefaultScopes = { "User.Read" };
		public static string Username = string.Empty;
		public static string HomeTenant = "SOME_TENANT_ID";
		public const string DomainHint = "DOMAIN_HINT_HERE";
		public static string redirectURI = "SOME_REDIRECT_URI";
		public static object UIParent { get; set; }



		public App()
		{
			InitializeComponent();

			PCA = PublicClientApplicationBuilder.Create(ClientID)
				.WithRedirectUri(redirectURI)
				.WithAuthority(string.Format("https://login.microsoftonline.com/{0}", HomeTenant))
				.WithExtraQueryParameters(DomainHint)
				.WithIosKeychainSecurityGroup("com.microsoft.msalrocks")
				.Build();

			MainPage = new MainPage();
		}

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
