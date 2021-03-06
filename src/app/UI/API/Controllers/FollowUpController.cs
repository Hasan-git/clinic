﻿using System;
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
    [EnableCors("*", "*", "*")]
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
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

        // GET: api/FollowUp/getlastVisitByConsultationId
        [HttpGet]
        [ResponseType(typeof(FollowUp))]
        [Route("api/FollowUp/getlastVisitByConsultationId/{id}")]
        public async Task<IHttpActionResult> getlastVisitByConsultationId(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var consultation = await Uow.ConsultationRepository.GetById(id);

                var followUp = await Uow.FollowUpRepository.GetLastvisitByConsultationId(id);

                if (followUp == null)
                {
                    return Ok(consultation);
                }
                else if(consultation == null && followUp == null)
                {
                    return NotFound();
                }

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
            if (followUp == null)
                return BadRequest("FollowUp cannot be null");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
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
        [Route("api/FollowUp/update"), HttpPost]
        public async Task<IHttpActionResult> Update([FromBody]FollowUp followUp)
        {
            if (followUp == null)
                return BadRequest("followUp cannot be null");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
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

        [Route("api/followup/Delete"), HttpDelete]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            if (id == null)
                return BadRequest("An error occured");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var followup = await Uow.FollowUpRepository.GetById(id);
                if (followup == null)
                    return NotFound();

                Uow.FollowUpRepository.Delete(followup);
                //followup.IsDeleted = true;
                //Uow.FollowUpRepository.Update(followup);
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
