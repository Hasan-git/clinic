using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Clinic.Common;
using Clinic.Core.Domain.Repositories;

namespace Api.Controllers
{
    public class BaseController : ApiController
    {
        protected IUnitOfWork Uow { get; private set; }

        public BaseController()
           : this(IoC.Resolve<IUnitOfWork>())
        {
        }
        public BaseController(IUnitOfWork uow)
        {
            Uow = uow;
        }
    }
}
