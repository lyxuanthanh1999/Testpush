using System;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int EntityId { get; set; }

        public string EntityType { get; set; }

        public Guid UserId { get; set; }

        public string OwnerUser { get; set; }

        public int? ReplyId { get; set; }

        public string ReplyUser { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public Pagination<CommentViewModel> Children { get; set; } = new Pagination<CommentViewModel>();
    }
}