using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Api.Models;
using Clinic.Common.Core.Extensions;
using Clinic.Common.Core.Services;
using Clinic.Core.Domain.Models;
using Clinic.Core.Domain.Repositories;
using Clinic.Infrastructure.Data;
using Clinic.Infrastructure.Data.Repositories;
using Newtonsoft.Json;
using System.Web;
using System.Net.Http;
using System.Net;
using Clinic.Common.Core;

namespace Api.Controllers
{
    [EnableCors("*", "*","*")]
    public class PatientsController : BaseController
    {
        
        // GET: api/Patients
        [HttpGet]
        [ResponseType(typeof(Patient))]
        public async Task<IHttpActionResult> Get()
        {
            
            try
            {
                var patients = await Uow.PatientRepository.GetAll();
                return Ok(patients);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
           
        }

        // GET: api/Patients/5
        [ResponseType(typeof(Patient))]
        public async  Task<IHttpActionResult> Get(Guid id)
        {
            try
            {
                var patient = await Uow.PatientRepository.GetById(id);
                if (patient == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(patient);
                }
                
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/Patients
        [ResponseType(typeof(Patient))]
        //public async Task<IHttpActionResult> Post([FromBody]Patient model)
        public async Task<IHttpActionResult> Post([FromBody]Patient patient)
        {
            if (patient == null)
                return BadRequest("Patient cannot be null");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {

                //patient.MedicalStatus.Id = SequentialGuid.Generate();
                patient.MedicalStatus = new MedicalStatus();
                patient.MedicalStatus.Id = Guid.NewGuid();
                patient.Id = Guid.NewGuid();

                Uow.PatientRepository.Add(patient);
                await Uow.Commit();
                return Ok(new { patientId = patient.Id });
            }
            catch (Exception ex)
            {
                Logger.Error(this, "", ex);
                return InternalServerError(ex);
            }
        }

        // PUT: api/Patients/5
        [ResponseType(typeof(Patient))]
        public async Task<IHttpActionResult> Put( [FromBody]Patient patient)
        {
            try
            {
                
                if (patient == null)
                    return BadRequest("Patient cannot be null");
               
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
               
                Uow.PatientRepository.Update(patient);
                await Uow.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // PUT: api/Patients/5
        [Route("api/Patients/{id}/Doctor"),HttpGet]
        [ResponseType(typeof(Patient))]
        public async Task<IHttpActionResult> GetByDoctor(Guid id)
        {
            try
            {

                if (id == null)
                    return BadRequest("Patient cannot be null");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

               var patients = await Uow.PatientRepository.GetByDoctorId(id);
                if (patients == null)
                        return NotFound();
                
                return Ok(patients);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //// DELETE: api/Patients/5
        [Route("api/Patients/delete"),HttpDelete]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            if (id == null)
                return BadRequest("Patient id cannot be null");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var patient = await Uow.PatientRepository.GetById(id);
                if (patient == null)
                    return NotFound();
                patient.IsDeleted = true;
                Uow.PatientRepository.Update(patient);
                await Uow.Commit();
                return Ok("Deleted");

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            
        }
    }
}
