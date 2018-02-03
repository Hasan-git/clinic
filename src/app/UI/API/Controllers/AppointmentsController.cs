using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Clinic.Core.Domain.Models;
using Api.Hubs;
using System.Web;

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
                var appointments = await Uow.AppointmentRepository.GetAppointments();
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

        [Route("api/Appointments/patient/{id}"), HttpGet]
        public async Task<IHttpActionResult> TodayPatientEvent(Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appointment = await Uow.AppointmentRepository.GetBypatientId(id);

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

                if (appointment.PatientId.HasValue)
                {
                    var latestConsultation = await Uow.ConsultationRepository.GetLastConditionByPatientId(appointment.PatientId ?? Guid.Empty);
                    var latestFollowUp = await Uow.FollowUpRepository.GetLastFollowUpByPatientId(appointment.PatientId ?? Guid.Empty);


                    if ( latestFollowUp !=null && latestConsultation != null ) {

                        if ((latestConsultation.EntryDate > latestFollowUp.EntryDate) )
                        {
                            appointment.LastVisit = latestConsultation.EntryDate.ToString();
                            appointment.LastVisitId = latestConsultation.Id;
                            appointment.LastVisitType = "consultation";

                        }
                        else if (latestConsultation.EntryDate < latestFollowUp.EntryDate )
                        {
                            appointment.LastVisit = latestFollowUp.EntryDate.ToString();
                            appointment.LastVisitId = latestFollowUp.Id;
                            appointment.LastVisitType = "followup";
                        }

                    } else if ( latestFollowUp == null && latestConsultation != null ) {
                            appointment.LastVisit = latestConsultation.EntryDate.ToString();
                            appointment.LastVisitId = latestConsultation.Id;
                            appointment.LastVisitType = "consultation";
                    }
                    else if (latestFollowUp != null && latestConsultation == null  ) {
                            appointment.LastVisit = latestFollowUp.EntryDate.ToString();
                            appointment.LastVisitId = latestFollowUp.Id;
                            appointment.LastVisitType = "followup";
                    }

                }

                Uow.AppointmentRepository.Add(appointment);
                await Uow.Commit();

                //MainHub.newAppointment(appointment);

                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/Appointments/payment/{id}/{payment}"), HttpPost]
        public async Task<IHttpActionResult> Payment(Guid id, string payment)
        {
            try
            {

                if (id == null)
                    return BadRequest("id cannot be null");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appointment = await Uow.AppointmentRepository.GetById(id);

                if (appointment == null)
                    return NotFound();

                appointment.Payment = payment;
                Uow.AppointmentRepository.Update(appointment);

                await Uow.Commit();

                MainHub.paymentReleased(appointment);

                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // PUT: api/Appointments/5
        [ HttpPost]
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

                MainHub.updatedAppointment(appointment);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/Appointments/updateAppointment"), HttpPost]
        public async Task<IHttpActionResult> updateAppointment( [FromBody]Appointment appointment)
        {
            try
            {

                if (appointment == null)
                    return BadRequest("appointment cannot be null");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Uow.AppointmentRepository.Update(appointment);
                await Uow.Commit();

                MainHub.updatedAppointment(appointment);
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

                MainHub.eventStatus(id, appointment.EventStatus,appointment.PatientName);
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

                await Uow.Commit();

                MainHub.removed(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
           
        }
    }
}
