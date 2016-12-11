using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Clinic.Common;
using Ninject;

namespace Clinic.UI.Common.Helpers
{
    public static class BootstrapHelper
    {
        public static StandardKernel LoadNinjectKernel(IEnumerable<Assembly> assemblies)
        {
            var standardKernel = new StandardKernel();
            try
            {

                foreach (var assembly in assemblies)
                {
                    assembly
                        .GetTypes()
                        .Where(t =>
                               t.GetInterfaces()
                                   .Any(i =>
                                        i.Name == typeof(INinjectModuleBootstrapper).Name))
                        .ToList()
                        .ForEach(t =>
                        {
                            var ninjectModuleBootstrapper =
                                (INinjectModuleBootstrapper)Activator.CreateInstance(t);

                            standardKernel.Load(ninjectModuleBootstrapper.GetModules());
                        });
                }
            }
            catch (ReflectionTypeLoadException exc)
            {
                //StringBuilder sb = new StringBuilder();
                //foreach (Exception exSub in ex.LoaderExceptions)
                //{
                //    sb.AppendLine(exSub.Message);
                //    FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                //    if (exFileNotFound != null)
                //    {
                //        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                //        {
                //            sb.AppendLine("Fusion Log:");
                //            sb.AppendLine(exFileNotFound.FusionLog);
                //        }
                //    }
                //    sb.AppendLine();
                //}
                //string errorMessage = sb.ToString();
                throw new AggregateException(exc.LoaderExceptions).Flatten();
            }

            return standardKernel;
        }
    }
}
