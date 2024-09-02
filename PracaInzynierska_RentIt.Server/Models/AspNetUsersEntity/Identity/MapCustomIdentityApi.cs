﻿using PracaInzynierska_RentIt.Server.Models.Application;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;


namespace RentIt_PracaInzynierska.Server.Models.IdentityLogin;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
/// <summary>
/// Provides extension methods for <see cref="IEndpointRouteBuilder"/> to add identity endpoints.
/// </summary>
public static class IdentityApiEndpointRouteBuilderExtensions
{
    // Validate the email address using DataAnnotations like the UserValidator does when RequireUniqueEmail = true.
    private static readonly EmailAddressAttribute _emailAddressAttribute = new();
    /// <summary>
    /// Add endpoints for registering, logging in, and logging out using ASP.NET Core Identity.
    /// </summary>
    /// <typeparam name="TUser">The type describing the user. This should match the generic parameter in <see cref="UserManager{TUser}"/>.</typeparam>
    /// <param name="endpoints">
    /// The <see cref="IEndpointRouteBuilder"/> to add the identity endpoints to.
    /// Call <see cref="EndpointRouteBuilderExtensions.MapGroup(IEndpointRouteBuilder, string)"/> to add a prefix to all the endpoints.
    /// </param>
    /// <returns>An <see cref="IEndpointConventionBuilder"/> to further customize the added endpoints.</returns>
    public static IEndpointConventionBuilder MapIdentityApiCustom<TUser>(this IEndpointRouteBuilder endpoints)
        where TUser : class, new()
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        var timeProvider = endpoints.ServiceProvider.GetRequiredService<TimeProvider>();
        var bearerTokenOptions = endpoints.ServiceProvider.GetRequiredService<IOptionsMonitor<BearerTokenOptions>>();
        var emailSender = endpoints.ServiceProvider.GetRequiredService<IEmailSender<TUser>>();
        var linkGenerator = endpoints.ServiceProvider.GetRequiredService<LinkGenerator>();
        // We'll figure out a unique endpoint name based on the final route pattern during endpoint generation.
        string? confirmEmailEndpointName = null;
        var routeGroup = endpoints.MapGroup("");
        // NOTE: We cannot inject UserManager<TUser> directly because the TUser generic parameter is currently unsupported by RDG.
        // https://github.com/dotnet/aspnetcore/issues/47338
        routeGroup.MapPost("/loginCustomFacebook", async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>>
        ([FromBody] LoginRequestCustom login, [FromQuery] bool? useCookies, [FromQuery] bool? useSessionCookies,
            [FromServices] IServiceProvider sp) =>
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://graph.facebook.com/me?fields=id,name,email&access_token={login.Token}");
            if (!response.IsSuccessStatusCode)
            {
                return TypedResults.Problem("Invalid Facebook access token.", statusCode: StatusCodes.Status400BadRequest);
            }
            var responseContent = await response.Content.ReadAsStringAsync();
            var facebookUser = JsonSerializer.Deserialize<FacebookUser>(responseContent);
            if (facebookUser.email != login.Email)
            {
                return TypedResults.Problem("We can't verify your data", statusCode: StatusCodes.Status400BadRequest);
            }
            using (var session = NHibernateHelper.OpenSession())
            {
                var query = session.Query<AspNetUsers>().First(x => x.Email == login.Email);
                if (query != null)
                {
                    if (query.Provider != "Facebook")
                    {
                        throw new Exception(
                            "Provider of this account doesn't match with provider that function expects.");
                    }
                }
                else
                {
                    throw new Exception(
                        "We can't find this email in the database. If you believe this is a mistake contact with support");
                }
            }
            var signInManager = sp.GetRequiredService<SignInManager<TUser>>();
            var useCookieScheme = (useCookies == true) || (useSessionCookies == true);
            var isPersistent = (useCookies == true) && (useSessionCookies != true);
            signInManager.AuthenticationScheme =
                useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;
            var result =
                await signInManager.PasswordSignInAsync(login.Email, "", isPersistent,
                    lockoutOnFailure: true);
            if (result.RequiresTwoFactor)
            {
                if (!string.IsNullOrEmpty(login.TwoFactorCode))
                {
                    result = await signInManager.TwoFactorAuthenticatorSignInAsync(login.TwoFactorCode, isPersistent,
                        rememberClient: isPersistent);
                }
                else if (!string.IsNullOrEmpty(login.TwoFactorRecoveryCode))
                {
                    result = await signInManager.TwoFactorRecoveryCodeSignInAsync(login.TwoFactorRecoveryCode);
                }
            }
            if (!result.Succeeded)
            {
                return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);
            }
            // The signInManager already produced the needed response in the form of a cookie or bearer token.
            return TypedResults.Empty;
        });
        routeGroup.MapPost("/loginCustomWebsite", async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>>
        ([FromBody] LoginRequestCustom login, [FromQuery] bool? useCookies, [FromQuery] bool? useSessionCookies,
            [FromServices] IServiceProvider sp) =>
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                var query = session.Query<AspNetUsers>().First(x => x.Email == login.Email);
                if (query != null)
                {
                    if (query.Provider != "Website")
                    {
                        throw new Exception(
                            "Provider of this account doesn't match with provider that function expects.");
                    }
                }
                else
                {
                    throw new Exception(
                        "We can't find this email in the database. If you believe this is a mistake contact with support");
                }
            }
            var signInManager = sp.GetRequiredService<SignInManager<TUser>>();

            var useCookieScheme = (useCookies == true) || (useSessionCookies == true);
            var isPersistent = (useCookies == true) && (useSessionCookies != true);
            signInManager.AuthenticationScheme =
                useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;
            var result = await signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent, lockoutOnFailure: true);

            if (result.RequiresTwoFactor)
            {
                if (!string.IsNullOrEmpty(login.TwoFactorCode))
                {
                    result = await signInManager.TwoFactorAuthenticatorSignInAsync(login.TwoFactorCode, isPersistent,
                        rememberClient: isPersistent);
                }
                else if (!string.IsNullOrEmpty(login.TwoFactorRecoveryCode))
                {
                    result = await signInManager.TwoFactorRecoveryCodeSignInAsync(login.TwoFactorRecoveryCode);
                }
            }
            if (!result.Succeeded)
            {
                return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);
            }
            // The signInManager already produced the needed response in the form of a cookie or bearer token.
            return TypedResults.Empty;
        });
        routeGroup.MapPost("/loginCustomGoogle",
            async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>>
            ([FromBody] LoginRequestCustom login, [FromQuery] bool? useCookies, [FromQuery] bool? useSessionCookies,
                [FromServices] IServiceProvider sp) =>
            {
                var httpClient = new HttpClient();
                var response =
                    await httpClient.GetAsync(
                        $"https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={login.Token}");
                if (!response.IsSuccessStatusCode)
                {
                    return TypedResults.Problem("Invalid Facebook access token.",
                        statusCode: StatusCodes.Status400BadRequest);
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var googleUser = JsonSerializer.Deserialize<GoogleUser>(responseContent);
                if (googleUser.email != login.Email)
                {
                    return TypedResults.Problem("We can't verify your data",
                        statusCode: StatusCodes.Status400BadRequest);
                }

                using (var session = NHibernateHelper.OpenSession())
                {
                    var query = session.Query<AspNetUsers>().First(x => x.Email == login.Email);
                    if (query != null)
                    {
                        if (query.Provider != "Google")
                        {
                            throw new Exception(
                                "Provider of this account doesn't match with provider that function expects.");
                        }
                    }
                    else
                    {
                        throw new Exception(
                            "We can't find this email in the database. If you believe this is a mistake contact with support");
                    }
                }

                var signInManager = sp.GetRequiredService<SignInManager<TUser>>();

                var useCookieScheme = (useCookies == true) || (useSessionCookies == true);
                var isPersistent = (useCookies == true) && (useSessionCookies != true);
                signInManager.AuthenticationScheme =
                    useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;
                var result =
                    await signInManager.PasswordSignInAsync(login.Email, "", isPersistent,
                        lockoutOnFailure: true);
                if (result.RequiresTwoFactor)
                {
                    if (!string.IsNullOrEmpty(login.TwoFactorCode))
                    {
                        result = await signInManager.TwoFactorAuthenticatorSignInAsync(login.TwoFactorCode,
                            isPersistent,
                            rememberClient: isPersistent);
                    }
                    else if (!string.IsNullOrEmpty(login.TwoFactorRecoveryCode))
                    {
                        result = await signInManager.TwoFactorRecoveryCodeSignInAsync(login.TwoFactorRecoveryCode);
                    }
                }

                if (!result.Succeeded)
                {
                    return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);
                }

                // The signInManager already produced the needed response in the form of a cookie or bearer token.
                return TypedResults.Empty;
            });
        return null;
    } 
    public class FacebookUser
    {
        [JsonPropertyName("email")]
        public string email { get; set; }
    }
    public class GoogleUser
    {
        [JsonPropertyName("email")]
        public string email { get; set; }
    }
}