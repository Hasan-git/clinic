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

namespace Api.Controllers
{
    [EnableCors("*", "*","*")]
    
    public class PatientsController : BaseController
    {
        
        // GET: api/Products
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

        // GET: api/Products/5
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

        // POST: api/Products
        [ResponseType(typeof(Patient))]
        //public async Task<IHttpActionResult> Post([FromBody]Patient model)
        public async Task<IHttpActionResult> Post([FromBody]Patient patient)
        {
            try
            {
                if (patient == null)
                    return BadRequest("Patient cannot be null");
                
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                Uow.PatientRepository.Add(patient);
                await Uow.Commit();
                return Ok();
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
       

        // PUT: api/Patients/5
        [Route("api/uploadTest"), HttpPost]
        public async Task<HttpResponseMessage> Postc()
        {


            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
          

            if (httpRequest.Files.Count > 0)
            {
                var files = new List<string>();

                var input = HttpContext.Current.Request.Form["consultationId"];
                //var followUp = HttpContext.Current.Request.Form["followUpId"];

                var conId = new Guid(input);

                Consultation consultation = await Uow.ConsultationRepository.GetById(conId);
                var images = new List<Images>();

                // interate the files and save on the server
                foreach (string file in httpRequest.Files)
                {
                    var idx = Guid.NewGuid();
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/images/"+ idx +"@"+ postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    images.Add(new Images()
                    {
                        Id= idx,
                        ImageName = postedFile.FileName
                    });

                    files.Add(filePath);
                }
                consultation.Images = images;

                Uow.ConsultationRepository.Update(consultation);
                await Uow.Commit();
                result = Request.CreateResponse(HttpStatusCode.Created, consultation);

            }
            else
            {
                // return BadRequest (no file(s) available)
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return result;
        }

        //// DELETE: api/Products/5
        //public void Delete(int id)
        //{
        //}
    }
}
