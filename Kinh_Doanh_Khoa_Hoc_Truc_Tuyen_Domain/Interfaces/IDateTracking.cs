using System;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Interfaces
{
    public interface IDateTracking
    {
        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }
}