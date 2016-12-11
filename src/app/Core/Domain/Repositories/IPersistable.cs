using System;

namespace Clinic.Core.Domain.Repositories
{
 
        public interface IPersistable
        {
            Guid Id { get; set; }
        
        } 
    
}