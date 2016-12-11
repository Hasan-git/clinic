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
    [EnableCors("http://localhost:16322", "*", "*")]
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
                var patients = await Uow.AppointmentRepository.GetAll();
                return Ok(patients);
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
                
                Uow.AppointmentRepository.Add(appointment);
                await Uow.Commit();
              
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //// POST: api/Appointments
        //[ResponseType(typeof(Appointment))]
        //public IHttpActionResult Post([FromBody]Appointment appointment)
        //{
        //    try
        //    {
        //        if (appointment == null)
        //        {
        //            return BadRequest("Appointment cannot be null");
        //        }

        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        var appointmentRepository = new Models.AppointmentRepository();
        //        var newProduct = appointmentRepository.Save(appointment);
        //        if (newProduct == null)
        //        {
        //            return Conflict();
        //        }
        //        return Created<Appointment>(Request.RequestUri + newProduct.Id.ToString(),
        //            newProduct);
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}

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


        //// PUT: api/Appointments/5
        //public IHttpActionResult Put(int id, [FromBody]Appointment appointment)
        //{
        //    try
        //    {
        //        Thread.Sleep(3000);
        //        if (appointment == null)
        //        {
        //            return BadRequest("Patient cannot be null");
        //        }
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        var appointmentRepository = new AppointmentRepository();
        //        var updateAppointment = appointmentRepository.Save(id, appointment);
        //        if (updateAppointment == null)
        //        {
        //            return NotFound();

        //        }
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}

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

        //// DELETE: api/Appointments/5
        //public void Delete(int id)
        //{
        //}
    }
}
