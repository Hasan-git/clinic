using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Clinic.Core.Domain.Models;

namespace Api.Controllers
{
    [EnableCors("*", "*", "*")]
    public class AppointmentsController : BaseController
    {
        // GET: api/Appointments
        [ResponseType(typeof(Appointment))]
        public async Task<IHttpActionResult> Get()
        {

            try
            {
              
                // var user = new User();
                // Uow.UserRepository.Add(user);
                //Uow.Commit();
                var appointments = await Uow.AppointmentRepository.GetAll();
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        // GET: api/Appointments/5
        [HttpGet]
        [ResponseType(typeof(Appointment))]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appointment = await Uow.AppointmentRepository.GetById(id);
                if (appointment == null)
                    return NotFound();
                
                    return Ok(appointment);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // POST: api/Appointments
        [ResponseType(typeof(Appointment))]
        public async  Task<IHttpActionResult> Post([FromBody]Appointment appointment)
        {
            try
            {
                if (appointment == null)
                    return BadRequest("Appointment cannot be null");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var latestConsultation = await Uow.ConsultationRepository.GetLastConditionByPatientId(Guid.Parse("a5b8737b-fdc3-438e-b5b7-e054c46fbbbc"));
                var latestFollowUp = await Uow.FollowUpRepository.GetLastFollowUpByPatientId(Guid.Parse("a5b8737b-fdc3-438e-b5b7-e054c46fbbbc"));
                if (latestConsultation.EntryDate > latestFollowUp.EntryDate)
                {
                    appointment.LastVisit = latestConsultation.EntryDate.ToString();
                    appointment.LastVisitId = latestConsultation.Id;
                    appointment.LastVisitType = "consultation";

                }
                else
                {
                    appointment.LastVisit = latestFollowUp.EntryDate.ToString();
                    appointment.LastVisitId = latestFollowUp.Id;
                    appointment.LastVisitType = "followup";
                }

                Uow.AppointmentRepository.Add(appointment);
                await Uow.Commit();
              
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // PUT: api/Appointments/5
        public async Task<IHttpActionResult> Put(Guid id, [FromBody]Appointment appointment)
        {
            try
            {
                
                if (appointment == null)
                    return BadRequest("appointment cannot be null");
                
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
               
                Uow.AppointmentRepository.Update(appointment);
                await Uow.Commit();
                
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/Appointments/UpdateStatus"), HttpPost]
        public async Task<IHttpActionResult> Put(Guid id, string status)
        {
            try
            {

                if (status == null)
                    return BadRequest("appointment status cannot be null");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appointment = await Uow.AppointmentRepository.GetById(id);

                appointment.EventStatus = status;

                Uow.AppointmentRepository.Update(appointment);
                await Uow.Commit();

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }



        [Route("api/Appointments/{id}/Doctor"), HttpGet]
        [ResponseType(typeof(Appointment))]
        public async  Task<IHttpActionResult> GetByDoctor(Guid id)
        {
            try
            {

                if (id == null)
                    return BadRequest("Doctor cannot be null");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appointments = await Uow.AppointmentRepository.GetByDoctorId(id);
                if (appointments == null)
                    return NotFound();

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/Appointments/Delete"), HttpPost]
        // DELETE: api/Appointments/5
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            try
            {
                if (id == null)
                    return BadRequest("Doctor cannot be null");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appointment = await Uow.AppointmentRepository.GetById(id);

                Uow.AppointmentRepository.Delete(appointment);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
           
        }
    }
}
