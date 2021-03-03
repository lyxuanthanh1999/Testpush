using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Enums;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities
{
    [Table("Orders")]
    public class Order : IDateTracking
    {
        public Order()
        {
        }

        public Order(int id, PaymentMethod paymentMethod, DateTime creationTime, Guid? userId)
        {
            Id = id;
            PaymentMethod = paymentMethod;
            CreationTime = creationTime;
            UserId = userId;
        }

        public int Id { get; set; }

        public PaymentMethod PaymentMethod { set; get; }

        public Guid? UserId { set; get; }

        public long? Total { get; set; }

        public string Address { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Message { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public AppUser AppUser { set; get; }

        public List<OrderDetail> OrderDetails { set; get; }
    }
}