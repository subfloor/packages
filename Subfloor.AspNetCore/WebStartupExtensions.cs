using System;
using System.Net.Http;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Subfloor.Dtos;

namespace Subfloor.AspNetCore
{
    public static class WebStartupExtensions
    {
        //the options pattern would make this much cleaner.
        public static void AddSubfloorAuthentication(this IServiceCollection services, string authorizeRequestUri = null, string[] additionalScopes = null)
        {
            
            var Configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            var subfloorConfig = Configuration.GetSection("Subfloor");

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "cookie";
                options.DefaultChallengeScheme = "oidc";
                options.DefaultSignOutScheme = "oidc";
            })
                .AddCookie("cookie", options =>                     //gotta deal with this fucking shit
                {
                    if (subfloorConfig.GetValue<bool>("IsBlazor"))
                    {
                        options.Cookie.Name = "__Host-blazor";
                        options.Cookie.SameSite = SameSiteMode.Strict;
                    }
                    options.AccessDeniedPath = "/Unauthorized";
                })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = subfloorConfig.GetValue<string>("Identity:Authority");
                    options.ClientId = subfloorConfig.GetValue<string>("Identity:Client_Id"); ;
                    options.ClientSecret = subfloorConfig.GetValue<string>("Identity:ClientSecret"); ;

                    options.ResponseType = "code";
                    options.ResponseMode = "query";

                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("offline_access");

                    if(additionalScopes != null)
                    {
                        foreach(var additionalScope in additionalScopes)
                        {
                            options.Scope.Add(additionalScope);
                        }
                    }

                    options.MapInboundClaims = false;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;

                    options.Events = new Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectEvents
                    {
                        OnTicketReceived = async n =>
                        {
                            try
                            {
                                //for when we implement authorization on the authorization call!
                                //var accessToken = httpContextAccessor.HttpContext.GetTokenAsync("access_token").Result;
                                //AdminClient.SetBearerToken(accessToken);

                                // attempt to authorize the authenticated user to this client
                                var userId = Guid.Parse(n.Principal.Claims.First(x => x.Type == "sub").Value);
                                var clientId = subfloorConfig.GetValue<Guid>("ClientId");
                                var authorizationApiBaseUri = subfloorConfig.GetValue<string>("ApiBaseUri");

                                var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
                                var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
                                var httpClient = httpClientFactory.CreateClient();

                                //if no value is passed in the method call we'll default to a subfloor-based authorization.
                                //otherwise - allow the calling client to determine its own client:user authorization endpoint
                                if(String.IsNullOrEmpty(authorizeRequestUri))
                                {
                                    authorizeRequestUri = $"{authorizationApiBaseUri}/ClientAuthorization/{clientId}/identities/{userId}/authorized";
                                }

                                var httpRequestMessage = new HttpRequestMessage { RequestUri = new Uri(authorizeRequestUri), Method = HttpMethod.Get };
                                var httpResponse = await httpClient.SendAsync(httpRequestMessage);
                                var httpResponseMessage = await httpResponse.Content.ReadAsStringAsync();
                                var authorizeResult = JsonSerializer.Deserialize<ServiceResult>(httpResponseMessage);

                                if (!authorizeResult.Success)
                                {
                                    n.Response.Redirect("/Unauthorized");
                                    n.HandleResponse();
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                n.Response.Redirect("/Oops");
                                n.HandleResponse();
                                return;
                            }
                        },
                        OnRedirectToIdentityProvider = n =>
                        {
                            try
                            {
                                n.ProtocolMessage.AcrValues = $"idp:{n.Request.Query["idp"]}";
                            }
                            catch { }

                            return System.Threading.Tasks.Task.FromResult(0);
                        }
                    };
                });
        }
    }
}