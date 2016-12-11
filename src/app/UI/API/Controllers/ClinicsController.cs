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
    [EnableCors("http://localhost:16322", "*", "*")]
    public class ClinicsController : BaseController
    {

        // GET: api/Clinics
        [HttpGet]
        [ResponseType(typeof(Clinic.Core.Domain.Models.Clinic))]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                
                var clinics = await Uow.ClinicRepository.GetAll();
               
                return Ok(clinics);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/Clinics/5
        [ResponseType(typeof(Clinic.Core.Domain.Models.Clinic))]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            try
            {
                var clinic = await Uow.ClinicRepository.GetById(id);
                if (clinic == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(clinic);
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/Clinics/Doctor/5
        [ResponseType(typeof(Clinic.Core.Domain.Models.Clinic))]
        [Route("api/Clinics/Doctor/{id}"), HttpGet]
        public async Task<IHttpActionResult> GetDoctorClinis(Guid id)
        {
            try
            {
                var clinic = await Uow.ClinicRepository.GetDoctorClinics(id);
                if (clinic == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(clinic);
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/Clinics
        [ResponseType(typeof(Clinic.Core.Domain.Models.Clinic))]
        public async Task<IHttpActionResult> Post([FromBody]Clinic.Core.Domain.Models.Clinic clinic)
        {
            try
            {
                if (clinic == null)
                    return BadRequest("Clinic cannot be null");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Uow.ClinicRepository.Add(clinic);
                await Uow.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(this, "", ex);
                return InternalServerError(ex);
            }
        }

        // PUT: api/Clinics/5
        [ResponseType(typeof(Clinic.Core.Domain.Models.Clinic))]
        public async Task<IHttpActionResult> Put(Guid id, [FromBody]Clinic.Core.Domain.Models.Clinic clinic)
        {
            try
            {
                if (clinic == null)
                    return BadRequest("Clinic cannot be null");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Uow.ClinicRepository.Update(clinic);
                await Uow.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //// DELETE: api/Clinics/5
        //public void Delete(int id)
        //{
        //}


    }
}
