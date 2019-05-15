﻿using Dnc.AspNetCore.Filters;
using Dnc.Data;
using Dnc.Seedwork;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Dnc.AspNetCore
{
    /// <summary>
    /// Extension methods for <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configure AspnetCore using Dnc.Core.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAspNetCore(this IApplicationBuilder app, AspNetCoreType aspNetCoreType = AspNetCoreType.Api)
        {
            app.UseAuthentication();
            if (aspNetCoreType == AspNetCoreType.Api)
            {
                app.UseSwaggerAPIDoc();
                app.UseMvc();
            }
            else
            {
                app.UseMvc(routes =>
                {
                    routes.MapRoute(name: "static_areas", template: "{area:exists}/{controller=Home}-{action=Index}.html");

                    routes.MapRoute(name: "static", template: "{controller=Home}-{action=Index}.html");

                    routes.MapRoute(name: "areas", template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                    routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");

                    routes.MapRoute(name: "custom", template: "{area:exists}_{controller=Home}_{action=Index}.{id?}");//Custom route template.
                });

            }
            return app;
        }
    }
}
