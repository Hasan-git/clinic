using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Clinic.Core.Domain.Models;
using System.Web;
using System.Collections.Generic;
using Api.Models;
using Api.Models.Mappers;
using Clinic.Core.Domain.Repositories;

namespace Api.Controllers
{
    [EnableCors("http://localhost:16322", "*", "*")]
    public class ConsultationsController : BaseController
    {

        // GET: api/Consultations
        //[System.Web.Http.Authorize(Roles = "admin")]
        [ResponseType(typeof(Consultation))]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var consultations = await Uow.ConsultationRepository.GetAll();
                return Ok(consultations);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }


        //get All doctor's consultations
        [System.Web.Http.Route("api/Consultations/restricted/{id}")]
        [ResponseType(typeof(Consultation))]
        [System.Web.Http.Authorize(Roles = "admin")]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetByDoctor(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var appointments = await Uow.ConsultationRepository.GetAllByDoctorId(id);
                if (appointments == null)
                    return NotFound();

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //Get specific doctors's consultation by consultation Id
        // GET: api/Consultations/5
        [System.Web.Http.HttpGet]
        [ResponseType(typeof(Consultation))]
        [System.Web.Http.Route("api/Consultations/{consultationId}")]
        [System.Web.Http.ActionName("Consultation")]
        public async Task<IHttpActionResult> Get(Guid consultationId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var consultation = await Uow.ConsultationRepository.GetById(consultationId);
                if (consultation == null)
                    return NotFound();
                
                    return Ok(consultation);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //Get all consultations for doctor's patient by patient Id
        // GET: api/Consultations/5/5/5
        [System.Web.Http.HttpGet]
        [ResponseType(typeof(Consultation))]
        [System.Web.Http.Route("api/Consultations/Patient/{doctorId}/{patientId}")]
        [System.Web.Http.ActionName("GetConsultations")]
        public async Task<IHttpActionResult> PatientConsultations( Guid doctorId,Guid patientId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var consultation = await Uow.ConsultationRepository.GetByPatientId(doctorId, patientId);
                if (consultation == null)
                    return NotFound();

                return Ok(consultation);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/Consultations
        
        [ResponseType(typeof(Consultation))]
        public IHttpActionResult Post(ConsultationModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var consultation = _mapper.mapConsultation(model);
                var medicalStatus = _mapper.MapMedicalStatus(model);
                //var consultation = new Consultation();
                //consultation.Id = Guid.NewGuid();

                Uow.ConsultationRepository.Add(consultation);
                Uow.MedicalRepository.Update(medicalStatus);
                Uow.Commit();
                return Ok(new { consultationId = consultation.Id });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }



        // PUT: api/Appointments/5
        [System.Web.Http.ActionName("update")]
        public async Task<IHttpActionResult> Put( [FromBody]Consultation consultation)
        {
            if (consultation == null)
                return BadRequest("Consultation cannot be null");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                Uow.ConsultationRepository.Update(consultation);
                await Uow.Commit();
                
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/consultations/uploadTest"), HttpPost]
        // PUT: api/Patients/5
        public async Task<IHttpActionResult> UploadConsultationImages()
        {

            try
            {
                var httpRequest = HttpContext.Current.Request;

                if (httpRequest.Files.Count > 0)
                {
                    var files = new List<string>();

                    var guid = HttpContext.Current.Request.Form["consultationId"];

                    var conId = new Guid(guid);

                    Consultation consultation = await Uow.ConsultationRepository.GetById(conId);
                    var images = new List<Images>();

                    // interate the files and save on the server
                    foreach (string file in httpRequest.Files)
                    {
                        var imageId = Guid.NewGuid();
                        var postedFile = httpRequest.Files[file];
                        var imageName = imageId + "@" + postedFile.FileName;
                        var filePath = HttpContext.Current.Server.MapPath("~/images/" + imageName);

                        postedFile.SaveAs(filePath);
                        files.Add(filePath);

                        consultation.Images.Add(new Images()
                        {
                            Id = imageId,
                            ImageName = imageName
                        });

                    }
                    //consultation.Images = images;

                    Uow.ConsultationRepository.Update(consultation);
                    await Uow.Commit();
                    return Ok("created");
                }
                else
                {
                    //result = Request.CreateResponse(HttpStatusCode.BadRequest);
                    return BadRequest();

                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [Route("api/consultations/DeleteImage"), HttpPost]
        // PUT: api/Patients/5
        public async Task<IHttpActionResult> DeleteImage(Guid id)
        {
            if (id == null)
                return BadRequest();

            try
            {
                var image = await Uow.ImageRepository.GetImageById(id);
                Uow.ImageRepository.Delete(image);
                await Uow.Commit();
                return Ok("deleted");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [Route("api/consultations/Delete"), HttpDelete]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            if (id == null)
                return BadRequest("An error occured");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var consultaion = await Uow.ConsultationRepository.GetById(id);
                if (consultaion == null)
                    return NotFound();
                consultaion.IsDeleted = true;
                Uow.ConsultationRepository.Update(consultaion);
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
