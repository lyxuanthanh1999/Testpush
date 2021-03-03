using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Extensions;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Filter;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Helpers;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.HubConfig;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Services;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.EF;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Enums;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly EKhoaHocDbContext _khoaHocDbContext;

        private ILogger<OrdersController> _logger;

        private readonly IStorageService _storageService;

        private readonly UserManager<AppUser> _userManager;

        private readonly IHubContext<ChatHub> _hubContext;

        public OrdersController(IWebHostEnvironment webHostEnvironment, EKhoaHocDbContext khoaHocDbContext, ILogger<OrdersController> logger, IStorageService storageService, UserManager<AppUser> userManager, IWebHostEnvironment hostingEnvironment, IHubContext<ChatHub> hubContext)
        {
            _khoaHocDbContext = khoaHocDbContext;
            _logger = logger;
            _storageService = storageService;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _hubContext = hubContext;
        }

        [HttpGet("filter")]
        public IActionResult GetOrdersPaging(string filter, int pageIndex, int pageSize)
        {
            var query = _khoaHocDbContext.Orders.AsNoTracking().AsEnumerable();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x =>
                    x.Email.ToLower().Equals(filter.ToLower()) || x.Name.ToLower().Contains(filter.ToLower()) ||
                    x.Name.convertToUnSign().ToLower().Contains(filter.convertToUnSign().ToLower()));
            }

            var data = query.ToList();
            var totalRecords = data.Count();
            var items = data.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(x => new OrderViewModel
            {
                Id = x.Id,
                UserId = x.UserId,
                CreationTime = x.CreationTime,
                LastModificationTime = x.LastModificationTime,
                Status = x.Status,
                Message = x.Message,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber,
                Name = x.Name,
                Email = x.Email,
                PaymentMethod = x.PaymentMethod,
                Total = x.Total
            }).ToList();

            var pagination = new Pagination<OrderViewModel>
            {
                Items = items,
                TotalRecords = totalRecords,
                PageSize = pageSize,
                PageIndex = pageIndex
            };
            return Ok(pagination);
        }

        [HttpGet("account-{userId}-orders")]
        public async Task<IActionResult> GetOrdersForClientPaging(string userId, string sortBy, int pageIndex, int pageSize)
        {
            var lstOrderViewModels = new List<OrderViewModel>();
            var query = _khoaHocDbContext.Orders.Include(x => x.OrderDetails).AsNoTracking().Where(x => x.UserId == Guid.Parse(userId));
            query = sortBy switch
            {
                "15daysAgo" => query.Where(x => x.CreationTime.Date <= (DateTime.Now.Date.AddDays(-15))),
                "30daysAgo" => query.Where(x => x.CreationTime.Date <= (DateTime.Now.Date.AddDays(-30))),
                "6monthsAgo" => query.Where(x => x.CreationTime.Date <= (DateTime.Now.Date.AddMonths(-6))),
                "1yearAgo" => query.Where(x => x.CreationTime.Date <= (DateTime.Now.Date.AddYears(-1))),
                _ => query
            };
            var totalRecords = query.Count();
            query = query.OrderByDescending(x => x.CreationTime).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            foreach (var item in query)
            {
                var user = await _userManager.FindByIdAsync(item.UserId.ToString());
                var lstOrderDetailViewModel = new List<OrderDetailViewModel>();
                foreach (var orderDetail in item.OrderDetails)
                {
                    var orderDetailViewModel = new OrderDetailViewModel();
                    var course = _khoaHocDbContext.ActivateCourses.Include(y => y.Course).FirstOrDefault(y => y.Id == orderDetail.ActiveCourseId)?.Course;
                    orderDetailViewModel.ActiveCourseId = orderDetail.ActiveCourseId;
                    orderDetailViewModel.Price = orderDetail.Price;
                    orderDetailViewModel.PromotionPrice = orderDetail.PromotionPrice;
                    orderDetailViewModel.OrderId = orderDetail.OrderId;
                    orderDetailViewModel.CourseName = course?.Name;
                    orderDetailViewModel.CourseImage = _storageService.GetFileUrl(course?.Image);
                    lstOrderDetailViewModel.Add(orderDetailViewModel);
                }
                var orderViewModel = new OrderViewModel();
                orderViewModel.Id = item.Id;
                orderViewModel.UserId = item.UserId;
                orderViewModel.CreationTime = item.CreationTime;
                orderViewModel.LastModificationTime = item.LastModificationTime;
                orderViewModel.Status = item.Status;
                orderViewModel.Message = item.Message;
                orderViewModel.Address = item.Address;
                orderViewModel.PhoneNumber = item.PhoneNumber ?? user.PhoneNumber;
                orderViewModel.Name = item.Name ?? item.AppUser.Name;
                orderViewModel.Email = item.Email ?? item.AppUser.Email;
                orderViewModel.PaymentMethod = item.PaymentMethod;
                orderViewModel.Total = item.Total;
                orderViewModel.ImageUser = _storageService.GetFileUrl(user.Avatar);
                orderViewModel.OrderDetails = lstOrderDetailViewModel;
                lstOrderViewModels.Add(orderViewModel);
            }

            var items = lstOrderViewModels;
            var pagination = new Pagination<OrderViewModel>
            {
                Items = items,
                TotalRecords = totalRecords,
                PageSize = pageSize,
                PageIndex = pageIndex
            };
            return Ok(pagination);
        }

        [HttpPut("status-type-{statusType}/multi-items")]
        [ValidationFilter]
        public async Task<IActionResult> PutStatusOrder(int statusType, List<int> request)
        {
            var lstAnnouncementToUser = new List<AnnouncementToUserViewModel>();
            foreach (var orderId in request)
            {
                var order = await _khoaHocDbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
                if (order == null)
                {
                    return NotFound(new ApiNotFoundResponse($"Không thể tìm thấy đơn hàng với id: {orderId}"));
                }
                if (statusType == 1)
                {
                    order.Status = OrderStatus.InProgress;
                }
                else if (statusType == 2)
                {
                    order.Status = OrderStatus.Returned;
                }
                else if (statusType == 3)
                {
                    order.Status = OrderStatus.Cancelled;
                }
                else
                {
                    order.Status = OrderStatus.Completed;
                }
                _khoaHocDbContext.Orders.Update(order);
                if (!string.IsNullOrEmpty(order.UserId.ToString()))
                {
                    var userCurrent = await _userManager.FindByNameAsync(User.Identity.Name);
                    var announceId = Guid.NewGuid();
                    var announcement = new Announcement();
                    announcement.UserId = userCurrent.Id;
                    announcement.Status = 1;
                    if (statusType == 1)
                    {
                        announcement.Content = $"Đơn hàng của bạn đã được tiếp nhận và đang trong quá trình xử lý";
                    }
                    else if (statusType == 2)
                    {
                        announcement.Content = $"Đơn hàng của bạn đã được trả lại";
                    }
                    else if (statusType == 3)
                    {
                        announcement.Content = $"Đơn hàng của bạn đã bị hủy";
                    }
                    else
                    {
                        announcement.Content = $"Đơn hàng của bạn đã được giao";
                    }
                    announcement.Title = "Thông báo đơn hàng";
                    announcement.Id = announceId;
                    announcement.EntityId = order.Id.ToString();
                    announcement.EntityType = "orders";
                    announcement.Image = "images/defaultAvatar.png";
                    await _khoaHocDbContext.Announcements.AddAsync(announcement);
                    var announceUser = new AnnouncementUser()
                    {
                        UserId = order.UserId!.Value,
                        AnnouncementId = announceId,
                        HasRead = false
                    };
                    await _khoaHocDbContext.AnnouncementUsers.AddAsync(announceUser);
                    var result = await _khoaHocDbContext.SaveChangesAsync();
                    var announcementViewmodel = new AnnouncementViewModel();
                    announcementViewmodel.UserId = announcement.UserId;
                    announcementViewmodel.Status = announcement.Status;
                    announcementViewmodel.Content = announcement.Content;
                    announcementViewmodel.Title = announcement.Title;
                    announcementViewmodel.Id = announcement.Id;
                    announcementViewmodel.EntityId = announcement.EntityId;
                    announcementViewmodel.EntityType = announcement.EntityType;
                    announcementViewmodel.Image = announcement.Image;
                    announcementViewmodel.CreationTime = announcement.CreationTime;
                    announcementViewmodel.LastModificationTime = announcement.LastModificationTime;
                    lstAnnouncementToUser.Add(new AnnouncementToUserViewModel()
                    {
                        AnnouncementViewModel = announcementViewmodel,
                        UserId = order.UserId!.Value.ToString()
                    });
                    if (result < 0)
                    {
                        return BadRequest(new ApiBadRequestResponse("Cập nhật trạng thái thất bại"));
                    }
                }
                else
                {
                    var result = await _khoaHocDbContext.SaveChangesAsync();
                    if (result < 0)
                    {
                        return BadRequest(new ApiBadRequestResponse("Cập nhật trạng thái thất bại"));
                    }
                }
            }
            return Ok(lstAnnouncementToUser);
        }

        [HttpPut("status-type/client")]
        [ValidationFilter]
        public async Task<IActionResult> PutStatusForClientOrder(OrderStatusRequest request)
        {
            var order = await _khoaHocDbContext.Orders.FirstOrDefaultAsync(x => x.Id == request.OrderId);
            if (order == null)
            {
                return NotFound(new ApiNotFoundResponse($"Không thể tìm thấy đơn hàng với id: {request.OrderId}"));
            }
            if (request.StatusType == 4)
            {
                order.Status = OrderStatus.Completed;
                var orderDetails = _khoaHocDbContext.OrderDetails.Where(x => x.OrderId == order.Id);
                foreach (var orderDetail in orderDetails)
                {
                    var activeCourse = _khoaHocDbContext.ActivateCourses.FirstOrDefault(x => x.Id.Equals(orderDetail.ActiveCourseId));
                    if (activeCourse != null)
                    {
                        activeCourse.Status = true;
                        _khoaHocDbContext.Update(activeCourse);
                    }
                }
            }
            else
            {
                order.Status = OrderStatus.Cancelled;
            }
            _khoaHocDbContext.Orders.Update(order);

            var userCurrent = await _userManager.FindByNameAsync(User.Identity.Name);
            var announceId = Guid.NewGuid();
            var announcement = new Announcement();
            announcement.UserId = userCurrent.Id;
            announcement.Status = 1;
            if (request.StatusType == 4)
            {
                announcement.Content = $"Đơn hàng của bạn đã được giao";
            }
            else
            {
                announcement.Content = $"Đơn hàng của bạn đã bị hủy";
            }
            announcement.Title = "Thông báo đơn hàng";
            announcement.Id = announceId;
            announcement.EntityId = order.Id.ToString();
            announcement.EntityType = "orders";
            announcement.Image = "images/defaultAvatar.png";
            await _khoaHocDbContext.Announcements.AddAsync(announcement);
            var announceUser = new AnnouncementUser()
            {
                UserId = order.UserId!.Value,
                AnnouncementId = announceId,
                HasRead = false
            };
            await _khoaHocDbContext.AnnouncementUsers.AddAsync(announceUser);
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest(new ApiBadRequestResponse("Cập nhật trạng thái thất bại"));
        }

        [HttpPost("export-excel")]
        public IActionResult PostExportOrder(OrderViewModel order)
        {
            try
            {
                var webRootFolder = _hostingEnvironment.WebRootPath;
                var resultFile = $"Bill_{order.Name.Replace(" ", "_").convertToUnSign()}_{DateTime.Now:dd-MM-yyyy}_{order.UserId ?? Guid.NewGuid()}.xlsx";
                var resultFilePdf = $"Bill_{order.Name.Replace(" ", "_").convertToUnSign()}_{DateTime.Now:dd-MM-yyyy}_{order.UserId ?? Guid.NewGuid()}.pdf";
                var date = DateTime.UtcNow.Date;
                var templateDocument = Path.Combine(webRootFolder, "attachments\\form", "Hoa_Don_Ban_Hang_Le.xlsx");
                var templateResultDocument = Path.Combine(webRootFolder, "attachments\\export-files", resultFile);
                var templatePdfResultDocument = Path.Combine(webRootFolder, "attachments\\export-files", resultFilePdf);
                FileInfo file = new FileInfo(templateResultDocument);
                if (file.Exists)
                {
                    file.Delete();
                    file = new FileInfo(templateResultDocument);
                }
                FileInfo filePdf = new FileInfo(templatePdfResultDocument);
                if (filePdf.Exists)
                {
                    filePdf.Delete();
                }
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                FileStream templateDocumentStream = new FileStream(templateDocument, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        throw new Exception("Không kết nối được với file");
                    }
                    worksheet.Cells[4, 3].Value = User.GetFullName() ?? "Hệ Thống";
                    worksheet.Cells[5, 3].Value = order.Name;
                    worksheet.Cells[6, 3].Value = order.Email;
                    worksheet.Cells[7, 3].Value = order.PhoneNumber;
                    worksheet.Cells[8, 3].Value = order.Address;
                    var stt = 1;
                    var index = 11;
                    foreach (var detail in order.OrderDetails)
                    {
                        if (index == 21)
                        {
                            worksheet.InsertRow(index, order.OrderDetails.Count - stt);
                        }

                        // Fill Data
                        worksheet.Cells[index, 1].Value = stt;
                        worksheet.Cells[index, 2].Value = detail.CourseName;
                        worksheet.Cells[index, 3].Value = detail.ActiveCourseId;
                        worksheet.Cells[index, 4].Value = $"{detail.PromotionPrice ?? detail.Price:0,0 VNĐ}";

                        // Bottom
                        worksheet.Cells[index, 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[index, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[index, 3].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[index, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                        // Top
                        worksheet.Cells[index, 1].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[index, 2].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[index, 3].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[index, 4].Style.Border.Top.Style = ExcelBorderStyle.Medium;

                        //Right
                        worksheet.Cells[index, 1].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[index, 2].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[index, 3].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[index, 4].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                        // Left
                        worksheet.Cells[index, 1].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[index, 2].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[index, 3].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[index, 4].Style.Border.Left.Style = ExcelBorderStyle.Medium;

                        // Horizontal Alignment
                        worksheet.Cells[index, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[index, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        worksheet.Cells[index, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[index, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        // Vertical Alignment
                        worksheet.Cells[index, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        worksheet.Cells[index, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        worksheet.Cells[index, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        worksheet.Cells[index, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        // Font Bold
                        worksheet.Cells[index, 1].Style.Font.Bold = true;
                        worksheet.Cells[index, 2].Style.Font.Bold = true;
                        worksheet.Cells[index, 3].Style.Font.Bold = true;
                        worksheet.Cells[index, 4].Style.Font.Bold = true;

                        // Update Index
                        index++;
                        stt++;
                    }

                    if (index > 21)
                    {
                        worksheet.Cells[index + 1, 4].Value = $"{order.Total:0,0 VNĐ}";
                        var thanhTien = order.Total.ToString();
                        worksheet.Cells[index + 2, 3].Value = double.Parse(thanhTien ?? "0").ChuyenSoSangChuoi();
                        worksheet.Cells[index + 2, 3].Style.Font.Bold = true;
                        worksheet.Cells[index + 4, 3].Value =
                            $"Ngày {date.Day} tháng {date.Month} năm {date.Year}";
                    }
                    else
                    {
                        worksheet.Cells[23, 4].Value = $"{order.Total:0,0 VNĐ}";
                        var thanhTien = order.Total.ToString();
                        worksheet.Cells[24, 3].Value = double.Parse(thanhTien ?? "0").ChuyenSoSangChuoi();
                        worksheet.Cells[24, 3].Style.Font.Bold = true;
                        worksheet.Cells[26, 3].Value =
                            $"Ngày {date.Day} tháng {date.Month} năm {date.Year}";
                    }

                    package.SaveAs(file);
                    Workbook workbook = new Workbook();
                    //Load excel file
                    workbook.LoadFromFile(templateResultDocument);
                    //Save excel file to pdf file.
                    workbook.SaveToFile(templatePdfResultDocument, FileFormat.PDF);
                }
                var resultFileRes = new ResultFileResponse()
                {
                    FileExcel = resultFile,
                    FilePdf = resultFilePdf
                };
                return Ok(resultFileRes);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetDetail(int id)
        {
            var order = _khoaHocDbContext.Orders.Include(x => x.OrderDetails).FirstOrDefault(x => x.Id == id);
            if (order == null)
            {
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy đơn hàng với id: {id}"));
            }
            var orderDetailViewModel = order.OrderDetails?.Select(x => new OrderDetailViewModel()
            {
                ActiveCourseId = x.ActiveCourseId,
                Price = x.Price,
                PromotionPrice = x.PromotionPrice,
                OrderId = x.OrderId,
                CourseName = _khoaHocDbContext.ActivateCourses.Include(y => y.Course).FirstOrDefault(y => y.Id == x.ActiveCourseId)?.Course.Name
            }).ToList();
            var orderViewModel = new OrderViewModel
            {
                Id = order.Id,
                Address = order.Address,
                PhoneNumber = order.PhoneNumber,
                UserId = order.UserId,
                CreationTime = order.CreationTime,
                LastModificationTime = order.LastModificationTime,
                Status = order.Status,
                Message = order.Message,
                Email = order.Email,
                Name = order.Name,
                PaymentMethod = order.PaymentMethod,
                Total = order.Total,
                OrderDetails = orderDetailViewModel
            };
            return Ok(orderViewModel);
        }

        [HttpGet("{id}/user-{userId}")]
        public IActionResult GetDetailByUserId(int id, string userId)
        {
            var order = _khoaHocDbContext.Orders.Include(x => x.OrderDetails).FirstOrDefault(x => x.Id == id && x.UserId == Guid.Parse(userId));
            if (order == null)
            {
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy đơn hàng với id: {id}"));
            }
            var lstOrderDetailViewModel = new List<OrderDetailViewModel>();
            foreach (var orderDetail in order.OrderDetails)
            {
                var orderDetailViewModel = new OrderDetailViewModel();
                var course = _khoaHocDbContext.ActivateCourses.Include(y => y.Course).FirstOrDefault(y => y.Id == orderDetail.ActiveCourseId)?.Course;
                orderDetailViewModel.ActiveCourseId = orderDetail.ActiveCourseId;
                orderDetailViewModel.Price = orderDetail.Price;
                orderDetailViewModel.PromotionPrice = orderDetail.PromotionPrice;
                orderDetailViewModel.OrderId = orderDetail.OrderId;
                orderDetailViewModel.CourseName = course?.Name;
                orderDetailViewModel.CourseImage = _storageService.GetFileUrl(course?.Image);
                lstOrderDetailViewModel.Add(orderDetailViewModel);
            }
            var orderViewModel = new OrderViewModel
            {
                Id = order.Id,
                Address = order.Address,
                PhoneNumber = order.PhoneNumber,
                UserId = order.UserId,
                CreationTime = order.CreationTime,
                LastModificationTime = order.LastModificationTime,
                Status = order.Status,
                Message = order.Message,
                Email = order.Email,
                Name = order.Name,
                PaymentMethod = order.PaymentMethod,
                Total = order.Total,
                OrderDetails = lstOrderDetailViewModel
            };
            return Ok(orderViewModel);
        }

        [HttpGet("check-{id}/user-{userId}")]
        public IActionResult GetCheckDetailByUserId(int id, string userId)
        {
            var order = _khoaHocDbContext.Orders.FirstOrDefault(x => x.Id == id && x.UserId == Guid.Parse(userId));
            if (order == null)
            {
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy đơn hàng với id: {id}"));
            }
            return Ok(order);
        }

        [HttpGet("user-{id}")]
        public async Task<IActionResult> GetOrders(string id)
        {
            var data = _khoaHocDbContext.Orders.Where(x => x.UserId == Guid.Parse(id));
            if (!data.Any())
            {
                return Ok(new List<OrderDetailViewModel>());
            }

            var orderViewModel = await data.Select(x => new OrderViewModel
            {
                Status = x.Status,
                Message = x.Message,
                LastModificationTime = x.LastModificationTime,
                CreationTime = x.CreationTime,
                UserId = x.UserId,
                Total = x.Total,
                PaymentMethod = x.PaymentMethod,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber,
                Name = x.Name,
                Email = x.Email,
                Id = x.Id
            }).ToListAsync();
            return Ok(orderViewModel);
        }

        [HttpPut("{orderId}/order-details/{activeCourseId}")]
        [ValidationFilter]
        public async Task<IActionResult> PutOrderDetail(OrderDetailCreateRequest request)
        {
            var orderDetail = _khoaHocDbContext.OrderDetails.FirstOrDefault(x => x.ActiveCourseId == request.ActiveCourseId && x.OrderId == request.OrderId);
            if (orderDetail == null)
            {
                return BadRequest(new ApiBadRequestResponse($"Không thể tìm thấy mã kích họa {request.ActiveCourseId} và mã đơn hàng {request.OrderId}"));
            }
            orderDetail.ActiveCourseId = request.ActiveCourseId;
            orderDetail.OrderId = request.OrderId;
            orderDetail.Price = request.Price;
            orderDetail.PromotionPrice = request.PromotionPrice;
            _khoaHocDbContext.OrderDetails.Update(orderDetail);
            await _khoaHocDbContext.SaveChangesAsync();
            var order = _khoaHocDbContext.Orders.Include(x => x.OrderDetails).FirstOrDefault(x => x.Id == request.OrderId);
            if (order == null)
            {
                return BadRequest(new ApiBadRequestResponse("Cập nhật thất bại"));
            }
            order!.Total = 0;
            var lstOrderDetail = order!.OrderDetails;
            lstOrderDetail.ForEach(x => order.Total += x.PromotionPrice ?? x.Price);
            _khoaHocDbContext.Orders.Update(order);
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest(new ApiBadRequestResponse("Tạo thất bại"));
        }

        [HttpPost("create")]
        public async Task<IActionResult> PostOrder(OrderCreateRequest request)
        {
            var order = new Order();
            order.Status = request.Status;
            order.Message = request.Message;
            order.Name = request.Name;
            order.Address = request.Address;
            order.Email = request.Email;
            order.PaymentMethod = request.PaymentMethod;
            order.PhoneNumber = request.PhoneNumber;
            order.UserId = request.UserId;
            order.Total = request.OrderDetails.Sum(x => x.PromotionPrice ?? x.Price);
            await _khoaHocDbContext.Orders.AddAsync(order);
            await _khoaHocDbContext.SaveChangesAsync();
            var lstOrderDetails = new List<OrderDetail>();
            foreach (var orderDetailCreateRequest in request.OrderDetails)
            {
                var detail = new OrderDetail();
                detail.Price = orderDetailCreateRequest.Price;
                detail.PromotionPrice = orderDetailCreateRequest.PromotionPrice;
                detail.ActiveCourseId = orderDetailCreateRequest.ActiveCourseId;
                detail.OrderId = order.Id;
                orderDetailCreateRequest.OrderId = order.Id;
                lstOrderDetails.Add(detail);
            }

            await _khoaHocDbContext.OrderDetails.AddRangeAsync(lstOrderDetails);
            var result = await _khoaHocDbContext.SaveChangesAsync();
            var orderViewModel = new OrderViewModel();
            orderViewModel.Id = order.Id;
            orderViewModel.CreationTime = orderViewModel.CreationTime;
            orderViewModel.Status = order.Status;
            orderViewModel.Message = order.Message;
            orderViewModel.Name = order.Name;
            orderViewModel.Address = order.Address;
            orderViewModel.Email = order.Email;
            orderViewModel.PaymentMethod = order.PaymentMethod;
            orderViewModel.PhoneNumber = order.PhoneNumber;
            orderViewModel.UserId = order.UserId;
            orderViewModel.Total = order.OrderDetails.Sum(x => x.PromotionPrice ?? x.Price);
            foreach (var detailViewModel in request.OrderDetails)
            {
                orderViewModel.OrderDetails.Add(new OrderDetailViewModel
                {
                    ActiveCourseId = detailViewModel.ActiveCourseId,
                    Price = detailViewModel.Price,
                    PromotionPrice = detailViewModel.PromotionPrice,
                    OrderId = detailViewModel.OrderId,
                    CourseName = detailViewModel.CourseName
                });
            }
            if (result > 0)
            {
                return Ok(orderViewModel);
            }

            return BadRequest(new ApiBadRequestResponse("Tạo thất bại"));
        }

        [HttpPost("create-delivery")]
        public async Task<IActionResult> PostOrderForDelivery(OrderCreateRequest request)
        {
            var order = new Order();
            order.Status = request.Status;
            order.Message = request.Message;
            order.Name = request.Name;
            order.Address = request.Address;
            order.Email = request.Email;
            order.PaymentMethod = request.PaymentMethod;
            order.PhoneNumber = request.PhoneNumber;
            order.UserId = request.UserId;
            order.Total = request.OrderDetails.Sum(x => x.PromotionPrice ?? x.Price);
            await _khoaHocDbContext.Orders.AddAsync(order);
            await _khoaHocDbContext.SaveChangesAsync();
            var lstOrderDetails = new List<OrderDetail>();
            foreach (var orderDetailCreateRequest in request.OrderDetails)
            {
                var detail = new OrderDetail();
                detail.Price = orderDetailCreateRequest.Price;
                detail.PromotionPrice = orderDetailCreateRequest.PromotionPrice;
                detail.ActiveCourseId = orderDetailCreateRequest.ActiveCourseId;
                detail.OrderId = order.Id;
                orderDetailCreateRequest.OrderId = order.Id;
                lstOrderDetails.Add(detail);
            }
            await _khoaHocDbContext.OrderDetails.AddRangeAsync(lstOrderDetails);

            var announceOrderClient = new AnnouncementToUserForOrderClientViewModel();
            var announceId = Guid.NewGuid();
            var announcement = new Announcement();
            if (!string.IsNullOrEmpty(order.UserId.ToString()))
            {
                announcement.UserId = Guid.Parse(User.GetUserId());
            }
            announcement.Status = 1;
            announcement.Content = $"Bạn có 1 đơn hàng mới từ {request.Name} với tin nhắn: {request.Message.formatData(30)}";
            announcement.Title = "Thông báo đơn hàng mới";
            announcement.Id = announceId;
            announcement.EntityId = order.Id.ToString();
            announcement.EntityType = "orders";
            announcement.Image = "images/defaultAvatar.png";
            await _khoaHocDbContext.Announcements.AddAsync(announcement);
            var users = await _userManager.GetUsersInRoleAsync("Admin");
            foreach (var user in users)
            {
                var announceUser = new AnnouncementUser()
                {
                    UserId = user.Id,
                    AnnouncementId = announceId,
                    HasRead = false
                };
                await _khoaHocDbContext.AnnouncementUsers.AddAsync(announceUser);
            }
            var result = await _khoaHocDbContext.SaveChangesAsync();
            var announcementViewmodel = new AnnouncementViewModel();
            announcementViewmodel.UserId = announcement.UserId;
            announcementViewmodel.Status = announcement.Status;
            announcementViewmodel.Content = announcement.Content;
            announcementViewmodel.Title = announcement.Title;
            announcementViewmodel.Id = announcement.Id;
            announcementViewmodel.EntityId = announcement.EntityId;
            announcementViewmodel.EntityType = announcement.EntityType;
            announcementViewmodel.Image = announcement.Image;
            announcementViewmodel.CreationTime = announcement.CreationTime;
            announcementViewmodel.LastModificationTime = announcement.LastModificationTime;
            announceOrderClient.AnnouncementViewModel = announcementViewmodel;
            var orderViewModel = new OrderViewModel();
            orderViewModel.Id = order.Id;
            orderViewModel.CreationTime = orderViewModel.CreationTime;
            orderViewModel.Status = order.Status;
            orderViewModel.Message = order.Message;
            orderViewModel.Name = order.Name;
            orderViewModel.Address = order.Address;
            orderViewModel.Email = order.Email;
            orderViewModel.PaymentMethod = order.PaymentMethod;
            orderViewModel.PhoneNumber = order.PhoneNumber;
            orderViewModel.UserId = order.UserId;
            orderViewModel.Total = order.OrderDetails.Sum(x => x.PromotionPrice ?? x.Price);

            foreach (var detailViewModel in request.OrderDetails)
            {
                orderViewModel.OrderDetails.Add(new OrderDetailViewModel
                {
                    ActiveCourseId = detailViewModel.ActiveCourseId,
                    Price = detailViewModel.Price,
                    PromotionPrice = detailViewModel.PromotionPrice,
                    OrderId = detailViewModel.OrderId,
                    CourseName = detailViewModel.CourseName
                });
            }
            announceOrderClient.OrderViewModel = orderViewModel;
            await _hubContext.Clients.All.SendAsync("ReceiveMessageFromServer", users.Select(x => x.Id).ToList(), announcementViewmodel);
            if (result > 0)
            {
                return Ok(announceOrderClient);
            }

            return BadRequest(new ApiBadRequestResponse("Tạo thất bại"));
        }
    }
}