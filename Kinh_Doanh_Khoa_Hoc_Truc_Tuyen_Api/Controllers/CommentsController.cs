using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Filter;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Helpers;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.EF;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly EKhoaHocDbContext _khoaHocDbContext;

        private ILogger<CommentsController> _logger;

        private readonly UserManager<AppUser> _userManager;

        public CommentsController(EKhoaHocDbContext khoaHocDbContext, ILogger<CommentsController> logger, UserManager<AppUser> userManager)
        {
            _khoaHocDbContext = khoaHocDbContext;
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _userManager = userManager;
        }

        [HttpGet("{entityType}/{entityId}/filter")]
        public async Task<IActionResult> GetCommentsPaging(int entityId, string entityType, string filter, int pageIndex, int pageSize)
        {
            var query = _khoaHocDbContext.Comments.Include(x => x.AppUser).AsNoTracking().Where(x => x.EntityId == entityId && x.EntityType == entityType).AsEnumerable();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x =>
                    x.AppUser.Name.ToLower().Contains(filter.ToLower()) ||
                    x.AppUser.Name.convertToUnSign().ToLower().Contains(filter.convertToUnSign().ToLower()) ||
                    x.AppUser.Email.ToLower().Equals(filter.ToLower()) || x.Content.ToLower().Contains(filter.ToLower()) ||
                    x.Content.convertToUnSign().ToLower().Contains(filter.convertToUnSign().ToLower()));
            }

            var lstCommentViewModel = new List<CommentViewModel>();
            foreach (var comment in query)
            {
                var commentReply = await _khoaHocDbContext.Comments.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.Id == comment.ReplyId);
                var commentViewModel = new CommentViewModel();
                commentViewModel.Id = comment.Id;
                commentViewModel.Content = comment.Content;
                commentViewModel.CreationTime = comment.CreationTime;
                commentViewModel.LastModificationTime = comment.LastModificationTime;
                commentViewModel.EntityId = comment.EntityId;
                commentViewModel.EntityType = comment.EntityType;
                commentViewModel.UserId = comment.UserId;
                commentViewModel.OwnerUser = comment.AppUser.Name + " (" + comment.AppUser.Email + ")";
                commentViewModel.ReplyId = comment.ReplyId;
                if (commentReply != null)
                {
                    commentViewModel.ReplyUser = commentReply.AppUser.Name + " (" + commentReply.AppUser.Email + ")";
                }
                lstCommentViewModel.Add(commentViewModel);
            }
            var totalRecords = lstCommentViewModel.Count();
            var items = lstCommentViewModel.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var pagination = new Pagination<CommentViewModel>
            {
                Items = items,
                TotalRecords = totalRecords,
                PageSize = pageSize,
                PageIndex = pageIndex
            };
            return Ok(pagination);
        }

        [HttpGet("{entityType}/client")]
        public async Task<IActionResult> GetCommentsForClient(int? entityId, string entityType)
        {
            IQueryable<Comment> query;
            if (entityType == "lessons" && entityId == null)
            {
                var lesson = _khoaHocDbContext.Lessons.FirstOrDefault(x => x.SortOrder == 1);
                query = _khoaHocDbContext.Comments.Include(x => x.AppUser).AsNoTracking().Where(x => x.EntityId == lesson.Id && x.EntityType == entityType).AsQueryable();
            }
            else
            {
                query = _khoaHocDbContext.Comments.Include(x => x.AppUser).AsNoTracking().Where(x => x.EntityId == entityId && x.EntityType == entityType).AsQueryable();
            }
            var items = await query.Select(x => new CommentViewModel()
            {
                Id = x.Id,
                Content = x.Content,
                CreationTime = x.CreationTime,
                LastModificationTime = x.LastModificationTime,
                EntityId = x.EntityId,
                EntityType = x.EntityType,
                UserId = x.UserId,
                OwnerUser = x.AppUser.Name + " (" + x.AppUser.Email + ")"
            }).ToListAsync();
            return Ok(items);
        }

        [HttpGet("{entityType}/{entityId}/hierarchical")]
        public async Task<IActionResult> GetCommentForHierarchical(int entityId, string entityType, int pageIndex, int pageSize)
        {
            var query = _khoaHocDbContext.Comments.Include(x => x.AppUser).AsNoTracking().Where(x => x.EntityId == entityId && x.EntityType == entityType && x.ReplyId == null).AsQueryable();
            var totalRecords = await query.CountAsync();
            var rootComments = await query.OrderByDescending(x => x.CreationTime)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).Select(x => new CommentViewModel()
                {
                    Id = x.Id,
                    Content = x.Content,
                    CreationTime = x.CreationTime,
                    LastModificationTime = x.LastModificationTime,
                    EntityId = x.EntityId,
                    EntityType = x.EntityType,
                    UserId = x.UserId,
                    OwnerUser = x.AppUser.Name + " (" + x.AppUser.Email + ")",
                    ReplyId = x.EntityId
                }).ToListAsync();
            foreach (var commentViewModel in rootComments)//only loop through root categories
            {
                // you can skip the check if you want an empty list instead of null
                // when there is no children
                var repliedQuery = _khoaHocDbContext.Comments.Include(x => x.AppUser).AsNoTracking()
                    .Where(x => x.EntityId == entityId && x.EntityType == entityType && x.ReplyId == commentViewModel.Id).AsQueryable();
                var totalRepliedCommentsRecords = await repliedQuery.CountAsync();

                var repliedComments = await repliedQuery.OrderByDescending(x => x.CreationTime)
                    .Take(pageSize).Select(x => new CommentViewModel()
                    {
                        Id = x.Id,
                        Content = x.Content,
                        CreationTime = x.CreationTime,
                        LastModificationTime = x.LastModificationTime,
                        EntityId = x.EntityId,
                        EntityType = x.EntityType,
                        UserId = x.UserId,
                        OwnerUser = x.AppUser.Name + " (" + x.AppUser.Email + ")",
                        ReplyId = x.EntityId
                    }).ToListAsync();
                commentViewModel.Children = new Pagination<CommentViewModel>
                {
                    TotalRecords = totalRepliedCommentsRecords,
                    PageIndex = 1,
                    PageSize = pageSize,
                    Items = repliedComments
                };
            }

            var result = new Pagination<CommentViewModel>
            {
                TotalRecords = totalRecords,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Items = rootComments
            };
            return Ok(result);
        }

        [HttpGet("{entityType}/{entityId}/root-{rootCommentId}/replied")]
        public async Task<IActionResult> GetRepliedCommentsPaging(int entityId, string entityType, int rootCommentId, int pageIndex, int pageSize)
        {
            var query = _khoaHocDbContext.Comments.Include(x => x.AppUser).AsNoTracking().Where(x => x.EntityId == entityId && x.EntityType == entityType && x.ReplyId == rootCommentId).AsQueryable();
            var totalRecords = await query.CountAsync();
            var rootComments = await query.OrderByDescending(x => x.CreationTime)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).Select(x => new CommentViewModel()
                {
                    Id = x.Id,
                    Content = x.Content,
                    CreationTime = x.CreationTime,
                    LastModificationTime = x.LastModificationTime,
                    EntityId = x.EntityId,
                    EntityType = x.EntityType,
                    UserId = x.UserId,
                    OwnerUser = x.AppUser.Name + " (" + x.AppUser.Email + ")",
                    ReplyId = x.EntityId
                }).ToListAsync();

            var result = new Pagination<CommentViewModel>
            {
                TotalRecords = totalRecords,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Items = rootComments
            };
            return Ok(result);
        }

        [HttpGet("{commentId}")]
        public async Task<IActionResult> GetCommentDetail(int commentId)
        {
            var comment = await _khoaHocDbContext.Comments.FirstOrDefaultAsync(x => x.Id == commentId);
            if (comment == null)
            {
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy bình luận với id: {commentId}"));
            }
            var user = await _userManager.FindByIdAsync(comment.UserId.ToString());
            var userReply = await _khoaHocDbContext.Comments.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.Id == comment.ReplyId);
            var commentViewModel = new CommentViewModel
            {
                Id = comment.Id,
                Content = comment.Content,
                CreationTime = comment.CreationTime,
                LastModificationTime = comment.LastModificationTime,
                EntityId = comment.EntityId,
                EntityType = comment.EntityType,
                UserId = comment.UserId,
                OwnerUser = user?.Name + " (" + user?.Email + ")",
                ReplyId = comment.ReplyId,
                ReplyUser = userReply != null ? userReply.AppUser.Name + " (" + userReply.AppUser.Email + ")" : ""
            };
            return Ok(commentViewModel);
        }

        [HttpPost("{entityType}/{entityId}")]
        [ValidationFilter]
        public async Task<IActionResult> PostComment([FromBody] CommentCreateRequest request)
        {
            var comment = new Comment();
            comment.Content = request.Content;
            comment.EntityType = request.EntityType;
            comment.EntityId = request.EntityId;
            comment.ReplyId = request.ReplyId;
            comment.UserId = Guid.Parse(request.UserId);
            if (request.EntityType.Equals("courses"))
            {
                var course = await _khoaHocDbContext.Courses.FirstOrDefaultAsync(x => x.Id == request.EntityId);
                if (course == null)
                {
                    return BadRequest(new ApiBadRequestResponse($"Không thể tìm thấy khóa học với id: {request.EntityId}"));
                }
                await _khoaHocDbContext.Comments.AddAsync(comment);
            }
            else
            {
                var lesson = await _khoaHocDbContext.Lessons.FirstOrDefaultAsync(x => x.Id == request.EntityId);
                if (lesson == null)
                {
                    return BadRequest(new ApiBadRequestResponse($"Không thể tìm thấy bài học với id: {request.EntityId}"));
                }

                await _khoaHocDbContext.Comments.AddAsync(comment);
            }
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return CreatedAtAction(nameof(GetCommentDetail), new { commentId = comment.Id }, new CommentViewModel()
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    CreationTime = comment.CreationTime,
                    LastModificationTime = comment.LastModificationTime,
                    EntityId = comment.EntityId,
                    EntityType = comment.EntityType,
                    UserId = comment.UserId,
                    ReplyId = comment.ReplyId
                });
            }

            return BadRequest(new ApiBadRequestResponse("Tạo bình luận thất bại"));
        }

        [HttpPut("{commentId}/{entityType}/{entityId}")]
        [ValidationFilter]
        public async Task<IActionResult> PutComment(int commentId, [FromBody] CommentCreateRequest request)
        {
            var comment = await _khoaHocDbContext.Comments.FirstOrDefaultAsync(x => x.Id == commentId);
            if (comment == null)
            {
                return BadRequest(new ApiBadRequestResponse($"Không thể tìm thấy bình luận với id: {commentId}"));
            }

            var user = await _userManager.FindByIdAsync(comment.UserId.ToString());
            if (user.Id != Guid.Parse(request.UserId))
                return Forbid();
            comment.Content = request.Content;
            _khoaHocDbContext.Comments.Update(comment);
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest(new ApiBadRequestResponse("Cập nhật thất bại"));
        }

        [HttpPost("delete-multi-items")]
        [ValidationFilter]
        public async Task<IActionResult> DeleteComment(List<int> request)
        {
            foreach (var commentId in request)
            {
                var comment = await _khoaHocDbContext.Comments.FirstOrDefaultAsync(x => x.Id == commentId);
                if (comment == null)
                {
                    return NotFound(new ApiNotFoundResponse($"Không thể tìm thấy comment với id: {commentId}"));
                }
                _khoaHocDbContext.Comments.Remove(comment);
            }
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest(new ApiBadRequestResponse($"Tạo thất bại"));
        }

        [HttpPost("delete-single-comment")]
        [ValidationFilter]
        public async Task<IActionResult> DeleteSingleComment(int id)
        {
            var comment = await _khoaHocDbContext.Comments.Include(x => x.AppUser).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null)
            {
                return NotFound(new ApiNotFoundResponse($"Không thể tìm thấy bình luận với id: {id}"));
            }
            _khoaHocDbContext.Comments.Remove(comment);
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                var commentViewModel = new CommentViewModel()
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    CreationTime = comment.CreationTime,
                    LastModificationTime = comment.LastModificationTime,
                    EntityId = comment.EntityId,
                    EntityType = comment.EntityType,
                    UserId = comment.UserId,
                    OwnerUser = comment.AppUser.Name + " (" + comment.AppUser.Email + ")",
                    ReplyId = comment.ReplyId
                };
                return Ok(commentViewModel);
            }
            return BadRequest(new ApiBadRequestResponse($"Xóa bình luận thất bại"));
        }
    }
}