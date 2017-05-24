using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Api.Models;
using Clinic.Common.Core.Services;
using Clinic.Core.Domain.Models;
using Clinic.Infrastructure.Data;

namespace Api.Controllers
{
    [EnableCors("*", "*", "*")]
    public class AssistantsController : BaseController
    {
        
        // GET: api/Assistants
        [HttpGet]
        [ResponseType(typeof(Assistant))]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var assistants = await Uow.AssistantRepository.GetAll();
               
                return Ok(assistants);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/Assistants/5
        [ResponseType(typeof(Assistant))]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            try
            {
                var assistant = await Uow.AssistantRepository.GetById(id);
                if (assistant == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(assistant);
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/Assistants
        [ResponseType(typeof(Assistant))]
        public async Task<IHttpActionResult> Post([FromBody]Assistant assistant)
        {
            try
            {
                if (assistant == null)
                    return BadRequest("Assistant cannot be null");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Uow.AssistantRepository.Add(assistant);
                await Uow.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(this, "", ex);
                return InternalServerError(ex);
            }
        }

        // PUT: api/Assistants/5
        [ResponseType(typeof(Assistant))]
        public async Task<IHttpActionResult> Put(Guid id, [FromBody]Assistant assistant)
        {
            try
            {
                if (assistant == null)
                    return BadRequest("Assistant cannot be null");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Uow.AssistantRepository.Update(assistant);
                await Uow.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //// DELETE: api/Assistants/5
        //public void Delete(int id)
        //{
        //}


    }
}
