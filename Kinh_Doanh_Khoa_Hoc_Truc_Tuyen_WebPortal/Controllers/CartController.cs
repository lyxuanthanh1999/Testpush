using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Enums;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.Common;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Extensions;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Helpers;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Models;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Services.Implements;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Controllers
{
    public class CartController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IBaseApiClient _apiClient;

        private readonly IConfiguration _configuration;

        private readonly IEmailSender _emailSender;

        public CartController(IHttpClientFactory httpClientFactory, IConfiguration configuration, IBaseApiClient apiClient, IEmailSender emailSender)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _apiClient = apiClient;
            _emailSender = emailSender;
        }

        [HttpGet]
        [Route("cart.html")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("checkout.html")]
        public IActionResult Checkout()
        {
            var session = HttpContext.Session.Get<List<CartViewModel>>(SystemConstants.CartSession) ?? new List<CartViewModel>();
            return View(session);
        }

        [HttpGet]
        [Route("checkout-confirm.html")]
        public IActionResult CheckoutConfirm(string message)
        {
            ViewData["MessageDelivery"] = message;
            return View();
        }

        [HttpGet]
        [Route("checkout-payment-confirm.html")]
        public async Task<IActionResult> CheckoutPaymentConfirm()
        {
            string vnp_HashSecret = _configuration["VnPaySettings:vnp_HashSecret"]; //Chuoi bi mat
            var vnpayData = Request.Query;
            VnPayLibrary vnpay = new VnPayLibrary();
            if (vnpayData.Count > 0)
            {
                foreach (var s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s.Key) && s.Key.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s.Key, s.Value);
                    }
                }
            }
            //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay
            string vnp_TxnRef = vnpay.GetResponseData("vnp_TxnRef");
            var checkOrderId = vnp_TxnRef.Split("-");
            int orderId = int.Parse(checkOrderId[0]);
            //vnp_TransactionNo: Ma GD tai he thong VNPAY
            long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
            string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            //vnp_SecureHash: MD5 cua du lieu tra ve
            string vnp_SecureHash = Request.Query["vnp_SecureHash"];
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
            try
            {
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        await _apiClient.PutAsync("/api/orders/status-type/client", new OrderStatusRequest { OrderId = orderId, StatusType = 4 });
                        var email = HttpContext.Session.Get<string>(SystemConstants.EmailSession);
                        if (email != null)
                        {
                            try
                            {
                                await _emailSender.SendEmailAsync(email, "Hóa Đơn Từ Website Khóa Học Trực Tuyến", "Đây là hóa đơn mua hàng của bạn");
                                HttpContext.Session.Remove(SystemConstants.EmailSession);
                                ViewData["MessagePayment"] = "Thanh toán thành công";
                                HttpContext.Session.Remove(SystemConstants.AttachmentSession);
                            }
                            catch (Exception e)
                            {
                                ViewData["MessagePayment"] = "Có lỗi xảy ra trong quá trình xử lý email";
                                HttpContext.Session.Remove(SystemConstants.AttachmentSession);
                                return View();
                            }
                        }
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        await _apiClient.PutAsync("/api/orders/status-type/client", new OrderStatusRequest { OrderId = orderId, StatusType = 3 });
                        HttpContext.Session.Remove(SystemConstants.AttachmentSession);
                        ViewData["MessagePayment"] = "Thanh toán không thành công";
                    }
                }
                else
                {
                    await _apiClient.PutAsync("/api/orders/status-type/client", new OrderStatusRequest { OrderId = orderId, StatusType = 3 });
                    HttpContext.Session.Remove(SystemConstants.AttachmentSession);
                    ViewData["MessagePayment"] = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }
            catch (Exception e)
            {
                ViewData["MessagePayment"] = e.Message;
            }
            return View();
        }

        #region Ajax Method

        [HttpPost]
        public async Task<IActionResult> CheckoutForDelivery(OrderCreateRequest model)
        {
            var session = HttpContext.Session.Get<List<CartViewModel>>(SystemConstants.CartSession);
            if (session != null)
            {
                try
                {
                    var listOrderDetails = new List<OrderDetailCreateRequest>();
                    foreach (var cartViewModel in session)
                    {
                        var active = new ActiveCourseCreateRequest();
                        active.Status = false;
                        active.CourseId = cartViewModel.CourseViewModel.Id;
                        if (User.Identity.IsAuthenticated)
                        {
                            active.UserId = User.GetUserId();
                        }
                        var key = await _apiClient.PostReturnStringAsync($"/api/courses/create-active-course", active);
                        var detail = new OrderDetailCreateRequest();
                        detail.CourseName = cartViewModel.CourseViewModel.Name;
                        detail.Price = cartViewModel.Price;
                        detail.PromotionPrice = cartViewModel.PromotionPrice;
                        detail.ActiveCourseId = Guid.Parse(key);
                        listOrderDetails.Add(detail);
                    }

                    if (User.Identity.IsAuthenticated)
                    {
                        model.UserId = Guid.Parse(User.GetUserId());
                    }

                    model.Status = OrderStatus.New;
                    model.OrderDetails = listOrderDetails;
                    var result = await _apiClient.PostAsync<OrderCreateRequest, AnnouncementToUserForOrderClientViewModel>($"/api/orders/create-delivery", model);
                    //var exportBill = await _apiClient.PostAsync<OrderViewModel, ApiResponse>($"/api/orders/export-excel", result.OrderViewModel);
                    //var path = _configuration["BaseAddress"] + "/attachments/export-files/" + exportBill.Message;
                    //HttpContext.Session.Set(SystemConstants.AttachmentSession, path);
                    //await _emailSender.SendEmailAsync(model.Email, "Hóa Đơn Từ Website Khóa Học Trực Tuyến", "Đây là hóa đơn mua hàng của bạn");
                    //HttpContext.Session.Remove(SystemConstants.AttachmentSession);
                    HttpContext.Session.Remove(SystemConstants.CartSession);
                    return Ok(result.AnnouncementViewModel);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> CheckoutForPayment()
        {
            var session = HttpContext.Session.Get<List<CartViewModel>>(SystemConstants.CartSession);
            if (session != null)
            {
                try
                {
                    var model = new OrderCreateRequest();
                    var listOrderDetails = new List<OrderDetailCreateRequest>();
                    var user = await _apiClient.GetAsync<UserViewModel>($"/api/users/{User.GetUserId()}");
                    foreach (var cartViewModel in session)
                    {
                        var active = new ActiveCourseCreateRequest();
                        active.Status = false;
                        active.CourseId = cartViewModel.CourseViewModel.Id;
                        active.UserId = User.GetUserId();
                        var key = await _apiClient.PostReturnStringAsync($"/api/courses/create-active-course", active);
                        var detail = new OrderDetailCreateRequest();
                        detail.CourseName = cartViewModel.CourseViewModel.Name;
                        detail.Price = cartViewModel.Price;
                        detail.PromotionPrice = cartViewModel.PromotionPrice;
                        detail.ActiveCourseId = Guid.Parse(key);
                        listOrderDetails.Add(detail);
                    }
                    model.Name = user.Name;
                    model.Email = user.Email;
                    model.PhoneNumber = user.PhoneNumber;
                    model.Message = "Thanh toán VNpay";
                    model.Status = OrderStatus.New;
                    model.UserId = user.Id;
                    model.PaymentMethod = PaymentMethod.PaymentGateway;
                    model.OrderDetails = listOrderDetails;
                    var result = await _apiClient.PostAsync<OrderCreateRequest, OrderViewModel>($"/api/orders/create", model);
                    var exportBill = await _apiClient.PostAsync<OrderViewModel, ResultFileResponse>($"/api/orders/export-excel", result);
                    var pathExcel = _configuration["BaseAddress"] + "/attachments/export-files/" + exportBill.FileExcel;
                    var pathPdf = _configuration["BaseAddress"] + "/attachments/export-files/" + exportBill.FilePdf;
                    var responseFile = new ResultFileResponse()
                    {
                        FileExcel = pathExcel,
                        FilePdf = pathPdf
                    };
                    HttpContext.Session.Remove(SystemConstants.CartSession);
                    HttpContext.Session.Set(SystemConstants.AttachmentSession, responseFile);
                    HttpContext.Session.Set(SystemConstants.EmailSession, user.Email);
                    //Get Config Info
                    string vnp_ReturnUrl = _configuration["VnPaySettings:vnp_ReturnUrl"]; //URL nhan ket qua tra ve
                    string vnp_Url = _configuration["VnPaySettings:vnp_Url"]; //URL thanh toan cua VNPAY
                    string vnp_TmnCode = _configuration["VnPaySettings:vnp_TmnCode"]; //Ma website
                    string vnp_HashSecret = _configuration["VnPaySettings:vnp_HashSecret"]; //Chuoi bi mat
                    var ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    //Build URL for VNPAY
                    VnPayLibrary vnpay = new VnPayLibrary();
                    vnpay.AddRequestData("vnp_Version", "2.0.0");
                    vnpay.AddRequestData("vnp_Command", "pay");
                    vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                    vnpay.AddRequestData("vnp_Locale", "vn");
                    vnpay.AddRequestData("vnp_CurrCode", "VND");
                    vnpay.AddRequestData("vnp_TxnRef", (result.Id + "-" + DateTime.Now.Ticks));
                    vnpay.AddRequestData("vnp_OrderInfo", result.Message);
                    vnpay.AddRequestData("vnp_OrderType", "insurance"); //default value: other
                    vnpay.AddRequestData("vnp_Amount", (result.Total * 100).ToString());
                    vnpay.AddRequestData("vnp_ReturnUrl", vnp_ReturnUrl);
                    vnpay.AddRequestData("vnp_IpAddr", ip);
                    vnpay.AddRequestData("vnp_CreateDate", result.CreationTime.ToString("yyyyMMddHHmmss"));
                    vnpay.AddRequestData("vnp_BankCode", "NCB");
                    var paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
                    return Ok(new
                    {
                        code = "00",
                        Message = "Confirm Success",
                        data = paymentUrl
                    });
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<IActionResult> GetUserById(string id)
        {
            var data = await _apiClient.GetAsync<UserViewModel>($"/api/users/{id}");
            return Ok(data);
        }

        /// <summary>
        /// Lấy Danh Sách Sản Phẩm Trong Giỏ Hàng
        /// </summary>
        /// <returns>Status 200 - List Product</returns>

        public IActionResult GetCart()
        {
            // Nếu session sẽ lấy dữ liệu ra trước nếu dữ liệu ko lấy ra dc (session == null) thì sẽ tạo mới
            var session = HttpContext.Session.Get<List<CartViewModel>>(SystemConstants.CartSession) ?? new List<CartViewModel>();
            return new OkObjectResult(session);
        }

        /// <summary>
        /// Xóa Sản Phẩm Trong Giỏ Hảng
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RemoveFromCart(int courseId)
        {
            var session = HttpContext.Session.Get<List<CartViewModel>>(SystemConstants.CartSession);
            if (session == null)
                return new EmptyResult();
            if (session.Any(item => item.CourseViewModel.Id == courseId))
            {
                var cartViewModel = session.FirstOrDefault(item => item.CourseViewModel.Id == courseId);
                session.Remove(cartViewModel);
                HttpContext.Session.Set(SystemConstants.CartSession, session);
            }
            return new OkObjectResult(courseId);
        }

        /// <summary>
        /// Thêm Sản Phẩm Vào Giỏ Hàng
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddToCart(int courseId)
        {
            var course = await _apiClient.GetAsync<CourseViewModel>($"/api/courses/{courseId}");
            var cartViewModel = new CartViewModel();
            var session = HttpContext.Session.Get<List<CartViewModel>>(SystemConstants.CartSession);
            if (session != null)
            {
                if (session.All(x => x.CourseViewModel.Id != courseId))
                {
                    long? promotion = null;
                    if (course.DiscountAmount > 0)
                    {
                        promotion = course.DiscountAmount;
                    }
                    else if (course.DiscountPercent > 0)
                    {
                        promotion = course.Price - (course.Price * course.DiscountPercent.Value / 100);
                    }
                    cartViewModel.CourseViewModel = course;
                    cartViewModel.Price = course.Price;
                    cartViewModel.PromotionPrice = promotion;
                    session.Add(cartViewModel);
                    //Update back to cart
                    HttpContext.Session.Set(SystemConstants.CartSession, session);
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                // Thêm Mới Sản Phẩm Vào Giỏ Hàng
                var lstCart = new List<CartViewModel>();
                long? promotion = null;
                if (course.DiscountAmount > 0)
                {
                    promotion = course.DiscountAmount;
                }
                else if (course.DiscountPercent > 0)
                {
                    promotion = course.Price - (course.Price * course.DiscountPercent.Value / 100);
                }
                cartViewModel.CourseViewModel = course;
                cartViewModel.Price = course.Price;
                cartViewModel.PromotionPrice = promotion;
                lstCart.Add(cartViewModel);
                HttpContext.Session.Set(SystemConstants.CartSession, lstCart);
            }
            return new OkObjectResult(cartViewModel);
        }

        /// <summary>
        /// Xoa Danh Sach San Pham Trong Gio Hang
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(SystemConstants.CartSession);
            return new OkResult();
        }

        #endregion Ajax Method
    }
}