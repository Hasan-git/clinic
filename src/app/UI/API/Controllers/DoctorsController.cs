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

namespace Api.Controllers
{
    [EnableCors("*", "*", "*")]
    public class DoctorsController : BaseController
    {
        // GET: api/Products
        [HttpGet]
        [ResponseType(typeof(Doctor))]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var doctors = await Uow.DoctorRepository.GetAll();
               
                return Ok(doctors);

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        // GET: api/Doctors/5
        [ResponseType(typeof(Doctor))]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            try
            {
                var doctor = await Uow.DoctorRepository.GetById(id);
                if (doctor == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(doctor);
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/Doctors
        [ResponseType(typeof(Doctor))]
        public async Task<IHttpActionResult> Post([FromBody]Doctor doctor)
        {
            try
            {                
                if (doctor == null)
                    return BadRequest("Doctor cannot be null");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Uow.DoctorRepository.Add(doctor);
                await Uow.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(this, "", ex);
                return InternalServerError(ex);
            }
        }

        // PUT: api/Doctors/5
        [ResponseType(typeof(Doctor))]
        public async Task<IHttpActionResult> Put(Guid id, [FromBody]Doctor doctor)
        {
            try
            {
                if (doctor == null)
                    return BadRequest("Doctor cannot be null");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Uow.DoctorRepository.Update(doctor);
                await Uow.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //// DELETE: api/Doctors/5
        //public void Delete(int id)
        //{
        //}


    }
}
