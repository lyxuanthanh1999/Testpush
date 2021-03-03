using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities
{
    [Table("Comments")]
    public class Comment : IDateTracking
    {
        public int Id { get; set; }

        public string Content { get; set; }

        [Range(1, double.PositiveInfinity)]
        public int EntityId { get; set; }

        public string EntityType { get; set; }

        public Guid UserId { get; set; }

        public int? ReplyId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public AppUser AppUser { get; set; }
    }
}