using System.Collections.Generic;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels
{
    public class Pagination<T> : PaginationBase
    {
        public List<T> Items { get; set; }
    }
}