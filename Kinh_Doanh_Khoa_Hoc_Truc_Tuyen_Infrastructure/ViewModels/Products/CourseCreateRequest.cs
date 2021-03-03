using Microsoft.AspNetCore.Http;
using System;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products
{
    public class CourseCreateRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IFormFile Image { get; set; }

        public string Content { get; set; }

        public long Price { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public int Status { get; set; }

        public int CategoryId { get; set; }
    }
}