using System;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Helpers
{
    public static class ConvertNumberToString
    {
        public static readonly string[] MNumText = { "Không", "Một", "Hai", "Ba", "Bốn", "Năm", "Sáu", "Bảy", "Tám", "Chín" };

        private static string DocHangChuc(double so, bool daydu)
        {
            string chuoi = "";
            //Hàm để lấy số hàng chục ví dụ 21/10 = 2
            var chuc = Convert.ToInt64(Math.Floor(so / 10));
            //Lấy số hàng đơn vị bằng phép chia 21 % 10 = 1
            var donvi = (long)so % 10;
            //Nếu số hàng chục tồn tại tức >=20
            if (chuc > 1)
            {
                chuoi = " " + MNumText[chuc] + " Mươi";
                if (donvi == 1)
                {
                    chuoi += " Mốt";
                }
            }
            else if (chuc == 1)
            {//Số hàng chục từ 10-19
                chuoi = " Mười";
                if (donvi == 1)
                {
                    chuoi += " Một";
                }
            }
            else if (daydu && donvi > 0)
            {//Nếu hàng đơn vị khác 0 và có các số hàng trăm ví dụ 101 => thì biến daydu = true => và sẽ đọc một trăm lẻ một
                chuoi = " Lẻ";
            }
            if (donvi == 5 && chuc >= 1)
            {//Nếu đơn vị là số 5 và có hàng chục thì chuỗi sẽ là " lăm" chứ không phải là " năm"
                chuoi += " Lăm";
            }
            else if (donvi > 1 || (donvi == 1 && chuc == 0))
            {
                chuoi += " " + MNumText[donvi];
            }
            return chuoi;
        }

        private static string DocHangTram(double so, bool daydu)
        {
            string chuoi;
            //Lấy số hàng trăm ví du 434 / 100 = 4 (hàm Floor sẽ làm tròn số nguyên bé nhất)
            Int64 tram = Convert.ToInt64(Math.Floor(so / 100));
            //Lấy phần còn lại của hàng trăm 434 % 100 = 34 (dư 34)
            so = so % 100;
            if (daydu || tram > 0)
            {
                chuoi = " " + MNumText[tram] + " Trăm";
                chuoi += DocHangChuc(so, true);
            }
            else
            {
                chuoi = DocHangChuc(so, false);
            }
            return chuoi;
        }

        private static string DocHangTrieu(double so, bool daydu)
        {
            string chuoi = "";
            //Lấy số hàng triệu
            var trieu = Convert.ToInt64(Math.Floor(so / 1000000));
            //Lấy phần dư sau số hàng triệu ví dụ 2,123,000 => so = 123,000
            so = so % 1000000;
            if (trieu > 0)
            {
                chuoi = DocHangTram(trieu, daydu) + " Triệu";
                daydu = true;
            }
            //Lấy số hàng nghìn
            var nghin = Convert.ToInt64(Math.Floor(so / 1000));
            //Lấy phần dư sau số hàng nghin
            so = so % 1000;
            if (nghin > 0)
            {
                chuoi += DocHangTram(nghin, daydu) + " Nghìn";
                daydu = true;
            }
            if (so > 0)
            {
                chuoi += DocHangTram(so, daydu);
            }
            return chuoi;
        }

        public static string ChuyenSoSangChuoi(this double so)
        {
            if (so == 0)
                return MNumText[0];
            string chuoi = "", hauto = "";
            long ty;
            do
            {
                //Lấy số hàng tỷ
                ty = Convert.ToInt64(Math.Floor(so / 1000000000));
                //Lấy phần dư sau số hàng tỷ
                so = so % 1000000000;
                if (ty > 0)
                {
                    hauto = " Tỷ";
                    chuoi = DocHangTrieu(ty, false);
                }
                else
                {
                    chuoi = chuoi + hauto + DocHangTrieu(so, false);
                }
            } while (ty > 0);
            return chuoi + " Đồng";
        }
    }
}