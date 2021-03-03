using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities
{
    [Table("FeedBacks")]
    public class FeedBack : IDateTracking
    {
        public int Id { get; set; }

        public string Name { set; get; }

        public string Email { set; get; }

        public string PhoneNumber { get; set; }

        public string Message { set; get; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }
}