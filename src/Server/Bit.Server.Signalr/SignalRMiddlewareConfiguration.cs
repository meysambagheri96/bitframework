﻿using Bit.Core.Models;
using Bit.Owin.Contracts;
using Bit.Signalr.Contracts;
using Microsoft.AspNet.SignalR;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bit.Signalr
{
    public class SignalRMiddlewareConfiguration : IOwinMiddlewareConfiguration
    {
        public virtual AppEnvironment AppEnvironment { get; set; } = default!;
        public virtual IEnumerable<ISignalRConfiguration> SignalRConfigurations { get; set; } = default!;
        public virtual Microsoft.AspNet.SignalR.IDependencyResolver DependencyResolver { get; set; } = default!;

        public virtual void Configure(IAppBuilder owinApp)
        {
            if (owinApp == null)
                throw new ArgumentNullException(nameof(owinApp));

            TypeInfo type = typeof(HubConfiguration).GetTypeInfo().Assembly.GetType("Microsoft.AspNet.SignalR.Infrastructure.MonoUtility")!.GetTypeInfo();
            FieldInfo isRunningMonoField = type.GetField("_isRunningMono", BindingFlags.NonPublic | BindingFlags.Static)!;
            if (isRunningMonoField != null)
            {
                try
                {
                    isRunningMonoField.SetValue(null, new Lazy<bool>(() => true));
                }
                catch (FieldAccessException)
                {

                }
            }

            HubConfiguration signalRConfig = new HubConfiguration
            {
                EnableDetailedErrors = AppEnvironment.DebugMode == true,
                EnableJavaScriptProxies = true,
                EnableJSONP = false,
                Resolver = DependencyResolver
            };

            SignalRConfigurations.ToList()
                .ForEach(cnfg =>
                {
                    cnfg.Configure(signalRConfig);
                });

            owinApp.Map("/signalr", innerOwinApp =>
            {
                innerOwinApp.RunSignalR(signalRConfig);
            });
        }
    }
}