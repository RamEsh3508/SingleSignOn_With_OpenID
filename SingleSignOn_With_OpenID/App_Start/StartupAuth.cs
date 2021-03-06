﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;

namespace SingleSignOn_With_OpenID
{
	public partial class Startup
	{
		#region Constants
		private const string strClientId = "9aabbb3a-6a68-44fa-a143-abefc7f299c2";
		private const string strInstance = "https://login.microsoftonline.com/";
		private const string strTenantId = "8b67b292-ebf3-4d29-89a6-47f7971c2e16";
		private const string strRedirectUri = "https://localhost:44363/About.aspx";
		#endregion

		#region Fields
		private readonly string strAuthority = strInstance + strTenantId;
		private ISecurityTokenValidator _tokenValidator;
		#endregion

		#region Publics
		public void ConfigureAuth(IAppBuilder app)
		{
			app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

			app.UseCookieAuthentication(new CookieAuthenticationOptions());

			app.UseOpenIdConnectAuthentication(
			                                   new OpenIdConnectAuthenticationOptions
			                                   {
				                                   ClientId = strClientId,
				                                   Authority = strAuthority,
				                                   PostLogoutRedirectUri = strRedirectUri,
				                                   Scope = "openid",
				                                   RedirectUri = strRedirectUri,

				                                   TokenValidationParameters = new TokenValidationParameters
				                                                               {
					                                                               ValidateIssuer = true,
					                                                               IssuerValidator = (issuer, token, tvp) => { return issuer; }
				                                                               },

				                                   Notifications = new OpenIdConnectAuthenticationNotifications
				                                                   {
					                                                   AuthorizationCodeReceived = OnAuthorizationCodeReceived,
					                                                   AuthenticationFailed = OnAuthenticationFailed
				                                                   }
			                                   }
			                                  );
			app.UseStageMarker(PipelineStage.Authenticate);
		}
		#endregion

		#region Privates
		private Task OnAuthorizationCodeReceived(AuthorizationCodeReceivedNotification arg)
		{
			string strEmail = arg.JwtSecurityToken.Claims.First(claim => claim.Type == "upn").Value;
			string strName = arg.JwtSecurityToken.Claims.First(claim => claim.Type == "name").Value;

			if(strEmail == "pradeep@msazure01.onmicrosoft.com")
			{
			}

			return Task.FromResult(0);
		}

		private Task OnAuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
		{
			return Task.FromResult(0);
		}
		#endregion
	}
}