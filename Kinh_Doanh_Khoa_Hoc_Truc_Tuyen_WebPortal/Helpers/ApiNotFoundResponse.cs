namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Helpers
{
    public class ApiNotFoundResponse : ApiResponse
    {
        public ApiNotFoundResponse(string message) : base(404, message)
        {
        }
    }
}