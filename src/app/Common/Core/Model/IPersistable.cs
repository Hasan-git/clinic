using System;

namespace Clinic.Common.Core.Model
{
    public interface IPersistable
    {
        Guid Id { get; set; }
        Guid? Timestamp { get; set; }
    }
}