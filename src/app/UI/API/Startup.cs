using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.Owin;
using Owin;
using Api.Models.Mappers;
using System.Web.Services.Description;
using StructureMap;
using Api.DependencyResolution;
using System.Diagnostics;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Newtonsoft.Json;
using Api.Helpers;

[assembly: OwinStartup(typeof(Api.Startup))]
namespace Api
{
    public partial class Startup 
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            IContainer container = IoC.Initialize();
            container.AssertConfigurationIsValid();
            Debug.WriteLine(container.WhatDoIHave());
            HttpConfiguration httpConfiguration = new HttpConfiguration();
            httpConfiguration.DependencyResolver = new StructureMapWebApiDependencyResolver(container);
            WebApiConfig.Register(httpConfiguration);
            app.UseWebApi(httpConfiguration);

            ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);

            //app.MapSignalR("/signalr", new HubConfiguration()
            //{
            //    EnableJSONP = true,
            //    EnableJavaScriptProxies = true
            //});
            // Branch the pipeline here for requests that start with "/signalr"


            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new SignalRContractResolver();
            var serializer = JsonSerializer.Create(settings);
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);

            app.Map("/signalr", map =>
            {
                // Setup the CORS middleware to run before SignalR.
                // By default this will allow all origins. You can 
                // configure the set of origins and/or http verbs by
                // providing a cors options with a different policy.
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    EnableDetailedErrors = true,
                    EnableJSONP = true,
                    EnableJavaScriptProxies = true
                };
                // Run the SignalR pipeline. We're not using MapSignalR
                // since this branch already runs under the "/signalr"
                // path.
                map.RunSignalR(hubConfiguration);
            });
        }
    }
}
