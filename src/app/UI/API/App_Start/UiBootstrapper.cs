﻿using System.Collections.Generic;
using Clinic.Common;
using Ninject.Modules;

namespace Api
{
    public class UiBootstrapper : INinjectModuleBootstrapper
    {
        public IList<INinjectModule> GetModules()
        {
            //this is where you will be considering priority of your modules.
            return new List<INinjectModule>()
            {
                new UiModule()
            };
        }
    }
}