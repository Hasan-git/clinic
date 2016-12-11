using System.Threading.Tasks;

namespace Clinic.Core.Domain.Repositories
{
    /// <summary>
    /// Interface for the AlMaknaz "Unit of Work"
    /// </summary>
    public interface IUnitOfWork
    {
        // Save pending changes to the data store.
        //void Commit();
        Task Commit();
        void RollBack();

        // Repositories
        IAppointmentRepository AppointmentRepository { get; }
        IAssistantRepository AssistantRepository { get; }
        IClinicRepository ClinicRepository { get; }
        IConsultationRepository ConsultationRepository { get; }
        IDoctorRepository DoctorRepository { get; }
        IFollowUpRepository FollowUpRepository { get; }
        IPatientRepository PatientRepository { get; }
        IUserRepository UserRepository { get; }







        //IRepository<MiscCategory> MiscCategory { get; }        

    }    
}