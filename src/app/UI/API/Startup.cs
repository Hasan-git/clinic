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
        }
    }
}
