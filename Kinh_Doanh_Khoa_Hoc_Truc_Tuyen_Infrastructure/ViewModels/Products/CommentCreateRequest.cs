namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products
{
    public class CommentCreateRequest
    {
        public string Content { get; set; }

        public int EntityId { get; set; }

        public string EntityType { get; set; }

        public int Id { get; set; }

        public string UserId { get; set; }

        public int? ReplyId { get; set; }
    }
}