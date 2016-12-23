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

namespace Api.Controllers
{
    [EnableCors("http://localhost:16322", "*", "*")]
    public class FollowUpController : BaseController
    {
        // GET: api/FollowUp
        [ResponseType(typeof(FollowUp))]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var followUps = await Uow.FollowUpRepository.GetAll();
                return Ok(followUps);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        // GET: api/FollowUp/5
        [HttpGet]
        [ResponseType(typeof(FollowUp))]
        [Route("api/FollowUp/{id}")]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var followUp = await Uow.FollowUpRepository.GetById(id);
                if (followUp == null)
                    return NotFound();
                
                    return Ok(followUp);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // POST: api/FollowUp
        [ResponseType(typeof(FollowUp))]
        public async  Task<IHttpActionResult> Post([FromBody]FollowUp followUp)
        {
            try
            {
                if (followUp == null)
                    return BadRequest("FollowUp cannot be null");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                followUp.Id = Guid.NewGuid();

                Uow.FollowUpRepository.Add(followUp);
                await Uow.Commit();
              
                return Ok(new { followUpId = followUp.Id });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }



        // PUT: api/FollowUp/5
        public async Task<IHttpActionResult> Put([FromBody]FollowUp followUp)
        {
            try
            {
                
                if (followUp == null)
                    return BadRequest("followUp cannot be null");
                
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
               
                Uow.FollowUpRepository.Update(followUp);
                await Uow.Commit();
                
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/followup/uploadTest"), HttpPost]
        // PUT: api/Patients/5
        public async Task<IHttpActionResult> UploadFollowUpImages()
        {

            try
            {
                var httpRequest = HttpContext.Current.Request;

                if (httpRequest.Files.Count > 0)
                {
                    var files = new List<string>();

                    var guid = HttpContext.Current.Request.Form["followupId"];

                    var followupId = new Guid(guid);

                    FollowUp followup = await Uow.FollowUpRepository.GetById(followupId);
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

                        followup.Images.Add(new Images()
                        {
                            Id = imageId,
                            ImageName = imageName
                        });
                    }
                    //followup.Images = images;

                    Uow.FollowUpRepository.Update(followup);
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

        [Route("api/followup/DeleteImage"), HttpPost]
        // PUT: api/Patients/5
        public async Task<IHttpActionResult> DeleteImage(Guid id)
        {

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

        //[Route("api/Consultations/Doctor/{doctorId}")]
        //[ResponseType(typeof(Consultation))]
        //[HttpGet]
        //public async  Task<IHttpActionResult> GetByDoctor(Guid doctorId)
        //{
        //    try
        //    {

        //        if (doctorId == null)
        //            return BadRequest("Doctor cannot be null");

        //        if (!ModelState.IsValid)
        //            return BadRequest(ModelState);

        //        var appointments = await Uow.ConsultationRepository.GetAllByDoctorId(doctorId);
        //        if (appointments == null)
        //            return NotFound();

        //        return Ok(appointments);
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}

        //// DELETE: api/Appointments/5
        //public void Delete(int id)
        //{
        //}
    }
}
