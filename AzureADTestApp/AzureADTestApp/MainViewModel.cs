using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AzureADTestApp
{
	public class MainViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public string ButtonText
		{
			get => _buttonText;
			set
			{
				_buttonText = value;
				RaisePropertyChanged("ButtonText");
			}
		}

		/// <summary>
		/// Sign In button click handler
		/// </summary>
		public Command ButtonClickCommand => new Command(async () =>
		{
			AuthenticationResult authResult = null;
			IEnumerable<IAccount> accounts = await App.PCA.GetAccountsAsync();

			try
			{
				if (ButtonText == "Sign in")
				{
					try
					{
						IAccount firstAccount = accounts.FirstOrDefault();
						authResult = await App.PCA.AcquireTokenSilent(App.DefaultScopes, firstAccount).ExecuteAsync();
					}
					catch (MsalUiRequiredException)
					{
						try
						{
							authResult = await App.PCA.AcquireTokenInteractive(App.DefaultScopes)
							.WithParentActivityOrWindow(App.UIParent)
							//.WithPrompt(Prompt.ForceLogin)
							.WithExtraQueryParameters(App.DomainHint)
							.ExecuteAsync();
						}
						catch (Exception ex2)
						{
							Debug.WriteLine($"Token aquistion failed. Exception details: {ex2.Message}");
						}
					}

					if (authResult != null)
					{
						var content = await GetUserDataAsync(authResult.AccessToken);
						ParseUserData(content);
						Device.BeginInvokeOnMainThread(() => { ButtonText = "Sign out"; });
					}
				}
				else
				{
					while (accounts.Any())
					{
						await App.PCA.RemoveAsync(accounts.FirstOrDefault());
						accounts = await App.PCA.GetAccountsAsync();
					}

					Device.BeginInvokeOnMainThread(() => { ButtonText = "Sign in"; });
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Authentication failed. Exception details: {ex.Message}");
			}
		});


		/// <summary>
		/// Parse json data from graph api
		/// </summary>
		/// <param name="content"></param>
		private void ParseUserData(string content)
		{
			if (!string.IsNullOrEmpty(content))
			{
				JObject user = JObject.Parse(content);

				Device.BeginInvokeOnMainThread(() =>
				{
					var strName = user["displayName"].ToString();
				});
			}
		}

		/// <summary>
		/// Gets user data from Graph API
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<string> GetUserDataAsync(string token)
		{
			try
			{
				//get data from API
				var client = new HttpClient();
				var message = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me");
				message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
				var response = await client.SendAsync(message);
				return await response.Content.ReadAsStringAsync();
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error in API call to graph: {ex.Message}");
				return ex.ToString();
			}
		}

		private void RaisePropertyChanged(string v)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
		}

		private string _buttonText = "Sign in";

	}
}
