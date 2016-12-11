using System;
using Ninject;

namespace Clinic.Common
{
    public static class IoC
    {
        private static StandardKernel _kernel;
        public static T Resolve<T>()
        {
            return _kernel.Get<T>();
        }

        public static object Resolve(Type modelType)
        {
            return _kernel.Get(modelType);
        }

        public static void Set(StandardKernel kernel)
        {
            _kernel = kernel;
        }

        public static bool CanResolve<T>()
        {
            var failed = false;
            try
            {
                //var type = _kernel.Get<T>();
                var type = _kernel.TryGet<T>();
                failed = object.ReferenceEquals(type, null);
            }
            catch (Exception exc)
            {
                failed = true;
            }
            return !failed;

        }
    }
}
