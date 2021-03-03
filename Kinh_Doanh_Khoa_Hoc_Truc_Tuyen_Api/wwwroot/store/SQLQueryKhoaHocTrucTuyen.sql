 SELECT * FROM Courses
 SELECT * FROM Categories
 SELECT * From Lessons
DBCC CHECKIDENT ('Courses', RESEED, 10) 
 DBCC CHECKIDENT ('Categories', RESEED, 0) 
  DBCC CHECKIDENT ('Lessons', RESEED, 0) 
 delete from Lessons
 delete from Courses
 delete from Categories
 //=================================================nhập courses================================================================
 set dateformat dmy
 delete from Courses
 //Khóa học lập trình
 insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId) 
 values(N'Học Lập Trình C/C++ Từ A - Z',N'Trang bị cho bạn kỹ năng lập trình ngôn ngữ C/C++ từ cơ bản đến nâng cao, được minh họa thông qua các bài tập thực hành thực tế nhất về C/C++','images/hoc-lap-trinh-c-c-a-toi-z-duong-tich-dat_m_1555574622.jpg','Ngon Ngu Lap Trinh','499000','2020-02-12 00:00:00.0000000','2020-02-12 00:00:00.0000000','1','59')
 insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId) 
 values(N'Lập Trình Android Từ Cơ Bản Đến Thành Thạo',N'Khóa học lập trình Android toàn tập tạo dựng một kiến thức vững chắc để học viên có thể tự vận hành các ứng dụng trên Appstore một cách nhanh chóng','images/hongvt@unica.vn/lap-trinh-android-tu-co-ban-den-thanh-thao_m.png','Lap Trinh Android','499000','2020-02-12 00:00:00.0000000','2020-02-12 00:00:00.0000000','1','62')
 insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId) 
 values(N'Toàn Tập Ngôn Ngữ Lập Trình C#',N'Học lập trình C# bài bản chi tiết nhất tại nhà. Khóa học à cẩm nang đầy đủ về C# cung cấp trọn bộ kiến thức cơ bản - có thể tạo ra một ứng dụng C# hoàn chỉnh','images/hoc-toan-tap-ngon-ngu-lap-trinh-c_m_1555637351.jpg','Lap Trinh C#','499000','2020-02-12 00:00:00.0000000','2020-02-12 00:00:00.0000000','1','59')
 insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
  values(N'Học Lập Trình JavaScript',N'Học lập trình Javascript đã tổng hợp lại kiến thức trong quá trình làm việc','images/hoc-lap-trinh-javacript_m_1561523648.jpg','Lap Trinh JavaScript','599000','2020-02-12 00:00:00.0000000','2020-02-12 00:00:00.0000000','1','60')
 insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
  values(N'Lập Trình Kotlin Toàn Tập',N'Khóa học sẽ giúp bạn có được kiến thức toàn diện về ngôn ngữ lập trình Kotlin, phát triển được trên phần mềm java,Native,Web cực đơn giản sau khi học xong khóa này','images/lap-trinh-kotlin-toan-tap_m_1555658121.jpg','Lap Trinh Kotlin','699000','2020-02-12 00:00:00.0000000','2020-02-12 00:00:00.0000000','1','62')
 insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId) 
 values(N'Docker Cở Bản',N'Khóa học sẽ hướng dẫn bạn các thao tác cơ bản làm việc với Docker và áp dụng Docker vào các yêu cầu môi trường cụ thể.','images/docker-co-ban_m_1561455294.jpg','Docker','349000','2020-02-12 00:00:00.0000000','2020-02-12 00:00:00.0000000','1','59')
 insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId) 
 values(N'React Native Cơ Bản',N'Khóa học sẽ hướng dẫn bạn các thao tác cơ bản làm việc với React Native và áp dụng React Native dựng app.','images/react-native.png','ReactNaive','349000','2020-02-12 00:00:00.0000000','2020-02-12 00:00:00.0000000','1','59')

 //Khóa học Ngoại Ngữ
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'TOEIC thần tốc dành cho người mất gốc',N'Khóa học Toeic online chính là cuốn cẩm nang toàn diện giúp bạn bắt đầu ôn thi Toeic từ phần cơ bản nhất: từ vựng, phương pháp phân tích câu, từ và hình ảnh giúp bạn dễ dàng ghi nhớ từ vựng và rèn luyện phản xạ tự nhiên, dễ dàng và tự tin hơn trong giao tiếp','images/anh-van-giao-tiep-cho-nguoi-hoan-toan-mat-goc_m_1555555777.jpg','TOEIC','479000','02/12/2020','02/12/2020','1','15')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Anh văn giao tiếp cho người hoàn toàn mất gốc',N'Khóa học tiếng anh cho người mất gốc khơi dậy niềm đam mê tiếng Anh, tự tin giao tiếp tiếng Anh như người bản xứ, mở ra cơ hội học tập, làm việc tại các công ty đa quốc gia và tự tin hơn trong giao tiếp với người bản địa dù ở bất kỳ hoàn cảnh nào','images/hoc-tieng-nhat-that-de_m_1555562005.jpg','Anh Van Giao Tiep','549000','02/12/2020','02/12/2020','1','15')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Học tiếng Nhật thật dễ',N'Học tiếng Nhật vỡ lòng một cách sinh động, hứng thú, đầy cảm quan với một giáo trình được biên soạn chi tiết, dễ hiểu và dễ ứng dụng.','images/hoc-chinh-phuc-ielts-speaking_m_1555657441.jpg','Hoc Tieng Nhat','199000','02/12/2020','02/12/2020','1','17')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Chiến thuật chinh phục IELTS Speaking',N'Hướng dẫn bạn hiểu toàn diện về IELTS Speaking, các chiến thuật để đạt điểm cao trong kì thi trong 35 bài từ giảng viên Lan Anh','images/hoc-gioi-tieng-anh-toan-dien_m_1555572380.jpg','Chinh Phuc IELTS','199000','02/12/2020','02/12/2020','1','15')
 

  //Khóa học Marketing
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Facebook Marketing từ A - Z',N'Trọn bộ khoá học Facebook Marketing Online từ A-Z của giảng viên Hồ Ngọc Cương sẽ hướng dẫn chuyên sâu bạn về các thủ thuật liên quan đến quảng cáo Facebook, giúp bạn tự lên các chiến lược về Facebook Marketing hoàn hảo cũng như tự chạy quảng cáo chuyên nghiệp.','Facebook Marketing A-Z','images/facebook-marketing-a-z_m_1555557477.jpg','199000','02/12/2020','02/12/2020','1','19')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'VUA EMAIL MARKETING',N'Khóa học Email Marketing của giảng viên Hán Quang Dự sẽ giúp bạn đưa ra những chiến lược kinh doanh cho công ty bằng giải pháp Email Marketing, cách sử dụng công cụ Getresponse, nâng cao chất lượng tìm kiếm, chăm sóc khách hàng.','Vua Email Marketing','images/Vua-email-marketing-han-quang-du_m_1555569804.jpg','199000','02/12/2020','02/12/2020','1','21')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Hướng dẫn làm web Landing Page bán hàng đỉnh cao dành cho người không chuyên',N'Khóa học "Hướng dẫn làm web Landing Page bán hàng đỉnh cao dành cho người không chuyên" sẽ hướng dẫn bạn làm landing page ngay cả khi bạn không biết gì về kỹ thuật IT.','Lam Web Sieu Toc','images/hoc-lam-web-sieu-toc-cho-nguoi-khong-chuyen_m_1555658815.jpg','199000','02/12/2020','02/12/2020','1','24')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Học SEO từ A-Z',N'Khóa Học Seo từ chuyên gia - Trải nghiệm cách bán hàng đỉnh cao trên công cụ tìm kiếm Google với khoá học Seo từ A-Z lên Top bất kỳ từ khoá dài và từ khoá ngắn nào.','SEO','images/khoa-hoc-seo-online_m_1555575597.jpg','199000','02/12/2020','02/12/2020','1','23')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Facebook Marketing Du Kích Tiếp cận hàng ngàn khách hàng với chi phí bằng 0',N'Cách thức tiếp cận hàng ngàn khách hàng tiềm năng trên Facebook với chi phí 0 đồng thông qua các công cụ Marketing Online mới nhất','Facebook Marketing','images/Facebook-marketing-chi-phi-0-dong_m_1555573466.jpg','199000','02/12/2020','02/12/2020','1','19')

   //Khóa học thiết kế
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Thiết kế nội thất bằng 3D-Max',N'Bạn muốn tự tay thiết kế nội thất cho gia đình? Bạn muốn theo đuổi ngành thiết kế nội thất. Tham gia khóa học thiết kế nội thất online bạn sẽ nắm được kỹ thuật về thiết kế bằng 3D-Max chuyên nghiệp','Thiet ke noi that 3D-Max','images/2020/11/thiet-ke-noi-that-bang-3ds-max_m_1606357631.jpg','599000','02/12/2020','02/12/2020','1','34')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Tạo hình thiết kế chuyên nghiệp với 3DSMAX',N'Bạn sẽ sử dụng thành thạo các nhóm lệnh và các công cụ của 3DSMAX, biết cách tạo hình sản phẩm, tạo ra được các sản phẩm 3D hoàn mỹ.','Tao hinh voi 3DSMAX','images/thaoptt09@gmail.com/3DSMAX_m.png','199000','02/12/2020','02/12/2020','1','33')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Tuyệt chiêu luyện AUTOCAD',N'Vẽ kỹ thuật, thiết kế, trình bày, xử lý bản vẽ kỹ thuật trên máy tính đơn giản, nhanh chóng với công cụ AUTOCAD','Luyen AutoCad','images/tuyet-chieu-luyen-autocad_m_1555575221.jpg','599000','02/12/2020','02/12/2020','1','31')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Cẩm nang A-Z Illustrator cho Designer',N'Giúp bạn nhanh chóng làm chủ phần mềm Adobe Illustrator, cung cấp nền tảng kiến thức cơ bản để tạo ra các sản phẩm thiết kế nâng cao và chủ động trong thiết kế','Cam nang Illustrator','images/cam-nang-illustrator-cho-designer_m_1561368077.jpg','699000','02/12/2020','02/12/2020','1','33')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Tuyệt chiêu luyện REVIT ARCH',N'Khóa học Revit Arch - Giúp học viên hiểu quy trình các bước dựng hình một công trình kiến trúc thực tế bằng Revit Arch đến cách triển khai chi tiết các bản vẽ sau 54 bài giảng','Luyen REVIT ARCH','images/tuyet-chieu-luyen-revit-arch_m_1555575662.jpg','699000','02/12/2020','02/12/2020','1','31')

  
  //Khóa Học Tin Học Văn Phòng
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
  values(N'Thiết kế Powerpoint chuyên nghiệp',N'Khóa học thiết kế PowerPoint chuyên nghiệp giúp học viên trình bày bài thuyết trình ấn tượng với những slide cuốn hút, lôi kéo khán giả','Thiet Ke PowerPoint','images/thiet-ke-powerpoint-chuyen-nghiep_m_1555572817.jpg','140000','02/12/2020','02/12/2020','1','30')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Microsoft Excel 2013 nâng cao',N'Microsoft Excel ứng dụng rất nhiều trong đời sống, công việc. Khóa học này sẽ giúp bạn học chuyên sâu Microsoft Excel 2013 nâng cao giúp công việc quản lí của bạn dễ dàng hơn','Microsoft Execel 2013 Nang Cao','images/thaoptt09@gmail.com/Microsoft_excell_213_nang_cao_m.png','175000','02/12/2020','02/12/2020','1','27')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Thành thạo với Google Spreadsheets',N'Sau khóa học bạn sẽ tự tin sử dụng thành thạo 2 phần mềm Google Sheet và Excel. Ứng dụng vào công việc hay học tập, vừa học vừa làm','Thanh Thao Google Spreadsheets','images/thanh-thao-google-spredsheets_m_1561369930.jpg','175000','02/12/2020','02/12/2020','1','29')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Làm chủ Word 2016 từ cơ bản đến nâng cao',N'Làm chủ Word 2016 từ cơ bản đến nâng cao giúp bạn tự tin làm chủ phần mềm Word 2016, nâng cao kỹ năng tin học văn phòng, tự tin thi chứng chỉ','Lam Chu Word tu co ban den nang cao','images/lam-chu-word-tu-co-ban-den-nang-cao_m_1555658502.jpg','199000','02/12/2020','02/12/2020','1','28')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Thành thạo cách xử lý về hóa đơn chứng từ kế toán - Học là biết LÀM',N'Khóa học cung cấp cho bạn toàn bộ kiến thức về Hóa đơn, từ cách viết hóa đơn, đến xử lý sai sót khi lập hóa đơn, phân loại hóa đơn.','Thanh thao xu ly don tu ke toan','images/xu-ly-hoa-don-chung-tu-ke-toan_m_1555576403.jpg','280000','02/12/2020','02/12/2020','1','29')
  
  // Khóa Học Kinh Doanh
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Bí quyết kiếm tiền trên Youtube',N'Khám phá ngay các tuyệt chiêu kiếm tiền tuyệt vời trên Youtube: cách để có những video triệu view hấp dẫn, cách kéo lượt traffic, cách SEO kênh... để biến Youtube thành kênh kiếm tiền thụ động bền vững siêu hot của bạn!','Kiem Tien Youtube','images/hoc-bi-quyet-kiem-tien-voi-youtube_m_1555571473.jpg','549000','02/12/2020','02/12/2020','1','41')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Kinh doanh Online hiệu quả với sản phẩm có sẵn',N'Nắm trong tay những bí quyết lập kế hoạch và triển khai chạy quảng cáo hiệu quả trên 5 kênh Marketing Online thịnh hành nhất để bạn kinh doanh online thành công với bất cứ sản phẩm nào','Kinh Doanh Online','images/kinh-doanh-online-san-pham-co-san_m_1555656673.jpg','599000','02/12/2020','02/12/2020','1','39')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Khởi nghiệp từ con số 1',N'Khóa học khởi nghiệp này sẽ giúp bạn được đào tạo bài bản, chuyên nghiệp các phương thức chuẩn bị tài chính, nhân lực, lập kế hoạch, biện pháp phòng tránh, giảm thiểu rủi ro khi khởi nghiệp, cải thiện tình trạng kinh doanh hiệu quả','Khoi Nghiep','images/khoi-nghiep-tu-con-so-1_m_1561365572.jpg','599000','02/12/2020','02/12/2020','1','40')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Khởi nghiệp kinh doanh online với số vốn 0 đồng',N'Bạn muốn học Kinh Doanh Online nhưng Không có Vốn Trong tay? Bạn không biết bắt đầu từ đâu. Khoá học giúp bạn xây dựng hệ thống Khởi nghiệp kinh doanh tự động từ 2 bàn tay trắng.','Khoi Nghiep Kinh Doanh','images/bi-quyet-kinh-doanh-my-pham-online_m_1555564995.jpg','399000','02/12/2020','02/12/2020','1','40')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Kinh doanh mỹ phẩm online',N'Khóa học kinh doanh mỹ phẩm online giúp học viên biết các kênh bán hàng hiệu quả, ứng dụng tìm kiếm, chăm sóc khách hàng, viết nội dung cuốn hút, tăng doanh thu bán hàng mỹ phẩm.','Kinh Doanh My Pham','images/khoi-nghiep-kinh-doanh-voi-von-0-dong_m_1555561955.jpg','599000','02/12/2020','02/12/2020','1','39')


  // Khóa học Phát Triển Cá Nhân 
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Bí quyết viết CV - Dự phỏng vấn',N'Bạn sẽ nắm toàn bộ cách viết CV ấn tượng - Dự phỏng vấn tự tin - Chinh phục nhà tuyển dụng thành công','CACH VIET CV','images/bi-quyet-viet-cv-du-phong-van_m_1555575879.jpg','399000','02/12/2020','02/12/2020','1','49')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Làm chủ kỹ năng ghi nhớ',N'Cải thiện khả năng ghi nhớ một cách dễ dàng thông qua các trò chơi về làm chủ con số, nhớ từ vựng tiếng Anh, nhớ sự kiện lịch sử, nhớ nội dung bài giảng','CAI THIEN GHI NHO','images/khoa-hoc-làm-chu-ky-nang-ghi-nho_m_1555656874.jpg','449000','02/12/2020','02/12/2020','1','49')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Nghệ thuật quyến rũ khán giả trong thuyết trình',N'Xoá tan nỗi sợ hãi, tăng sự tự tin và quyến rũ của bạn khi thuyết trình. Đồng thời nắm vững kỹ thuật sử dụng ngôn ngữ cơ thể và di chuyển trên sân khấu và Tự tin khi đứng trước đám đông','XOA TAN NOI SO HAI','images/nghe-thuat-quyen-ru-khan-gia_m_1558075961.jpg','599000','02/12/2020','02/12/2020','1','49')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Giải mã tính cách qua sinh trắc vân tay',N'Sinh trắc học dấu vân tay - nơi thấu hiểu mỗi con người giúp bạn thấu hiểu và đọc vị được tính cách của mình cũng như bất kỳ ai.','GIAI MA QUA SINH TRAC HOC','images/giai-ma-tinh-cach-qua-sinh-trac-van-tay_m_1555642638.jpg','499000','02/12/2020','02/12/2020','1','49')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Giao tiếp qua điện thoại',N'Giao tiếp qua điện thoại - Bí quyết để thành công - Giao tiếp gián tiếp đang có xu hướng thay thế dần dần các phương thức giao tiếp trực tiếp truyền thống.','GIAO TIEP QUA DIEN THOAI','images/xuan2016/thaykha_m.jpg','499000','02/12/2020','02/12/2020','1','50')

  //Khóa Học Sales, Bán Hàng
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Bí quyết chốt đơn thành công 90% - Telesale, Bán hàng online',N'Khóa học cung cấp cho bạn 4 bước xây dựng kịch bản bán hàng và 7 chiến lược chốt sale khiến khách hàng không thể cưỡng lại được. Tăng tỉ lệ chốt đơn lên 90%.','Xay Dung Ban Hang','images/bi-quyet-chot-sale-thanh-cong_m_1555572889.jpg','200000','02/12/2020','02/12/2020','1','56')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Chiến binh bán hàng',N'Khóa học giúp bạn đập tan mọi rào cản khó khăn từ khách hàng, nâng cao khả năng giao tiếp, đọc vị tâm lý khách hàng trong 60s.','Giai quyet rao can cua khach hang','images/thaoptt09@gmail.com/chienbinhbanhang-11_m.jpg','175000','02/12/2020','02/12/2020','1','57')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Nghệ thuật bán hàng qua điện thoại',N'Bí quyết để trở thành Sát thủ bán hàng qua điện thoại, có trong tay chiến lược xây dựng danh sách khách hàng, cùng hàng loạt tuyệt chiêu để có được những kịch bản tele ưng ý','Nghe thuat ban hang','images/kich-ban-telesale_m_1555569937.jpg','240000','02/12/2020','02/12/2020','1','56')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Tuyệt chiêu để chốt Sales',N'Làm sao để tiếp cận khách hàng, bí quyết chốt đơn ngay trong vài phút gặp mặt? Khoá học giúp nắm được những kỹ thuật căn bản để Tiếp cận, Chào hàng, Vượt qua phản đối, chốt Sales.','Tuyet chieu chot sales','images/tuyet-chieu-de-chot-sale_m_1557996232.jpg','175000','02/12/2020','02/12/2020','1','55')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'36 Tuyệt chiêu bán hàng siêu đẳng',N'Với 36 tuyệt chiêu bán hàng siêu đẳng bạn sẽ nắm trong tay tất tần tận các kỹ năng, phương pháp, kịch bản để thuyết phục khách hàng tin tưởng và sử dụng sản phẩm của mình lâu dài và hiệu quả nhất','Tuyet chieu ban hang','images/tuyet-chieu-ban-hang-nguyen-vinh-cuong_m_1555570080.jpg','499000','02/12/2020','02/12/2020','1','56')
 
  //Khóa học Sức Khỏe - Giới Tính
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Bí quyết trẻ đẹp từ trong ra ngoài',N'Là con gái thì phải biết "Làm trẻ đẹp da từ trong da ngoài". Biết cách xây dựng nền tảng cho bản thân để trở nên đẹp, thấy giá trị của bản thân và yêu bản thân hơn.','Bi Quyet Lam Tre','images/dep-tu-trong-ra-ngoai_m_1557995881.jpg','499000','02/12/2020','02/12/2020','1','10')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Massage dưỡng sinh Đầu - Vai - Gáy',N'Massage là một phương pháp giúp bảo vệ sức khỏe và thư giãn hệ thần kinh. Massage thường xuyên nhằm tăng cường thể chất, củng cố hệ miễn dịch để chăm sóc bản thân và gia đình mình một cách hiệu quả.','Massage Duong Sinh','images/massage-duong-sinh-dau-vai-gay_m_1557995407.jpg','599000','02/12/2020','02/12/2020','1','70')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Tập Yoga cơ bản ngay tại nhà với Nguyễn Hiếu',N'Học Yoga cơ bản ngay tại nhà giúp cải thiện sức khoẻ tinh thần, thể chất của bạn. Ngoài ra, việc học Yoga Online tại nhà cũng giúp bạn tiết kiệm được nhiều thời gian và chi phí so với việc học ở các trung tâm.','Tap Yoga Co Ban','images/tap-yoga-co-ban-tai-nha-cung-nguyen-hieu-yoga_m_1555558750.jpg','549000','02/12/2020','02/12/2020','1','69')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'36 Thế Yoga tăng cường sinh lý',N'Bài tập kích hoạt sinh lực, tăng sự linh hoạt, quyến rũ, tạo niềm vui cuộc sống, hạnh phúc gia đình.','36 The Yoga','images/hoc-yoga-tang-cuong-sinh-ly_m_1556177994.jpg','399000','02/12/2020','02/12/2020','1','69')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Yoga cho Thân Khỏe, Tâm An',N'Khóa học giúp bạn cảm nhận chuyển biến tích cực về thể chất lẫn tinh thần. Nhờ đó bạn có thể làm việc hiệu quả và tận hưởng cuộc sống với năng lượng dồi dào.','Yoga cho than khoe','images/yoga-than-khoe-tam-an_m_1558076272.jpg','399000','02/12/2020','02/12/2020','1','69')
  
  //Khóa học Phong Cách Sống
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Tự học piano trong 10 ngày',N'Khóa học dành cho người học từ 16 tuổi trở lên. Sau khóa học, bạn sẽ đọc hiểu được các bản nhạc piano cơ bản, chơi được piano bằng 2 tay, đồng thời phát triển kỹ năng thị tấu vừa đàn vừa nhìn bản nhạc.','Tu Hoc Piano','images/tu-hoc-piano-trong-10-ngay_m_1555574091.jpg','479000','02/12/2020','02/12/2020','1','77')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Kỹ năng học guitar hiệu quả cho người mới bắt đầu',N'Bạn sẽ nắm chắc kiến thức âm nhạc, cách cảm âm và kỹ năng chơi guitar cơ bản. Hoàn thành khóa học guitar bạn có thể đánh được những bài hát cơ bản và đệm hát một cách chuyên nghiệp. Bạn có thể hoàn toàn tự tin biểu diễn trước đám đông thể hiện cá tính âm nhạc riêng của mình.','Hoc guitar','images/guitar-cho-nguoi-moi-bat-dau_m_1555574034.jpg','449000','02/12/2020','02/12/2020','1','77')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Trở thành ảo thuật gia chỉ trong 21 ngày',N'Bạn sẽ thực hành tốt ngay sau khi hoàn thành khóa học ảo thuật đến từ chuyên gia cùng vời những màn ảo thuật đơn giản, vui vẻ đầy bất ngờ, kịch tính và cuốn hút khán giả. Bạn sẽ trở nên tự tin hơn với "tài lẻ" của mình và nhiều người tán phục','Hoc Lam Ao Thuat','images/tro-thanh-phu-thuy-trong-21-ngay_m_1555577506.jpg','499000','02/12/2020','02/12/2020','1','76')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Học Độc tấu Guitar trong 36 ngày',N'Học Độc tấu Guitar trong 36 ngày giúp bạn nắm được những kỹ năng chơi đàn cơ bản, từ đó tiếp tục học độc tấu cổ điển hoặc đệm hát, ginger style.','Doc Tau Guitar','images/hoc-doc-tau-guitar-trong-36-ngay_m_1557995488.jpg','199000','02/12/2020','02/12/2020','1','77')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Tự học thổi sáo trúc trong 21 ngày',N'Khóa học sáo trúc "Thổi Sáo Trúc với lộ trình 21 ngày" sẽ giúp bạn nắm chắc những kỹ năng cơ bản cần thiết để chơi sáo, giúp tự tin trổ tài thổi sáo của mình trong những buổi văn nghệ.','Thoi Soa Truc','images/tu-hoc-thoi-sao-trong-21-ngay_m_1555575179.jpg','499000','02/12/2020','02/12/2020','1','77')
   
   //Khóa học Hôn Nhân và Gia Đình
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Bí quyết giữ lửa hôn nhân và hạnh phúc gia đình',N'Bí quyết giữ hạnh phúc gia đình giúp cân bằng cuộc sống hôn nhân và gia đình - Là chìa khóa đảm bảo cho cuộc hôn nhân của mình luôn nồng ấm và hạnh phúc hơn','Bi Quyet Giu Lua Hon Nhan','images/bi-quyet-giu-lua-hon-nhan_m_1558075360.jpg','499000','02/12/2020','02/12/2020','1','85')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Nghệ thuật quyến rũ bạn đời',N'Bí quyết giữ hạnh phúc gia đình giúp cân bằng cuộc sống hôn nhân và gia đình - Là chìa khóa đảm bảo cho cuộc hôn nhân của mình luôn nồng ấm và hạnh phúc hơn','Quyen Ru Ban Doi','images/khoa-hoc-nghe-thuat-quyen-ru-ban-doi_m_1555571511.jpg','399000','02/12/2020','02/12/2020','1','84')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Bí quyết Tán gái',N'Khóa học Bí quyết Tán gái sẽ mang đến cho phái nam những phương pháp làm quen, bí kíp tán gái và chinh phục bạn gái bá đạo và hiệu quả nhất để nhanh chóng chinh phục được trái tim của nàng','Bi Quyet Tan Gai','images/hoc-bi-kip-tan-gai_m_1555642006.jpg','399000','02/12/2020','02/12/2020','1','84')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Trở thành nghệ sĩ tán gái bậc thầy',N'Khóa học Bí quyết Tán gái sẽ mang đến cho phái nam những phương pháp làm quen, bí kíp tán gái và chinh phục bạn gái bá đạo và hiệu quả nhất để nhanh chóng chinh phục được trái tim của nàng','Bac Thay Tan Gai','images/nghe-thuat-tan-gai-bac-thay_m_1555573992.jpg','599000','02/12/2020','02/12/2020','1','84')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'7 ngày trước cưới - hành trang cho cuộc sống hôn nhân',N'Khoá học 7 ngày trước cưới - hành trang cho cuộc sống hôn nhân giúp bạn có tâm lý, tư tưởng vững vàng khi bước vào cuộc sống hôn nhân.','Hanh Trang Cuoc Song Hon Nhan','images/7-ngay-truoc-cuoi-hanh-trang-cho-cuoc-song-hon-nhan_m_1561427058.jpg','599000','02/12/2020','02/12/2020','1','85')

 //Khóa Học Nuôi Dạy Con
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'19 Tuyệt chiêu nuôi dạy con thành tài',N'Lắng nghe con, phát huy điểm mạnh của con, gần gũi với con cái, thấu hiểu con cái để nuôi dạy con tốt nhất.','Tuyet Chieu Nuoi Day Con','images/19tuyet-chieu-day-con-thanh-tai_m_1555577063.jpg','199000','02/12/2020','02/12/2020','1','82')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Bí quyết cho trẻ ăn dặm lớn nhanh, khỏe mạnh',N'Dinh dưỡng cho trẻ nhỏ, cách cho ăn dặm, chăm sóc con từ 6 - 12 tháng tuổi thông minh, khoẻ mạnh, lớn nhanh.','Bi Quyet Cho Tre An Lon Nhanh','images/thaoptt09@gmail.com/ANdam_m.jpg','399000','02/12/2020','02/12/2020','1','82')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Dạy con phát triển toàn diện theo phương pháp Do Thái',N'Yêu con, thương con nhưng phải dạy con đúng phương pháp. Vì thế hãy dạy con phát triển toàn diện theo phương pháp Do Thái','Dat Con Phat Trien Toan Dien','images/day-con-phat-trien-toan-dien-theo-phuong-phap-do-thai_m_1555659245.jpg','199000','02/12/2020','02/12/2020','1','82')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Nuôi dạy con kiệt xuất theo phương pháp người Do Thái',N'Khóa học dành cho các bậc cha mẹ đang muốn xây dựng con thành một đứa trẻ thành công, dạy con tư duy, kiến thức nền tảng để mạnh mẽ bước vào đời giúp trẻ phát triển toàn diện trogn tương lai','Nuoi Day Con','images/hoc-nuoi-day-con-kiet-suot-theo-phuong-phap-nguoi-do-thai_m_1555657021.jpg','199000','02/12/2020','02/12/2020','1','82')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Bento - Con ăn khỏe, cả nhà vui vẻ',N'Bạn sẽ biết cách làm những hộp cơm Bento dinh dưỡng, bắt mắt, hấp dẫn. Chắc chắn Bento là giải pháp tuyệt vời cho các bà mẹ có con kén ăn.','Bento danh cho con an','images/nguyenthiDieuLinh/hinhchily800_(1)_m.jpg','199000','02/12/2020','02/12/2020','1','81')
 
  //Khóa Học Nhiếp Ảnh, Dựng Phim
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Làm phim hoạt hình 2D với MAYA',N'Hướng dẫn làm phim hoạt hình 2D tạo nên 1 tác phẩm tuyệt vời một cách chuyên nghiệp với Maya','Lam Phim Hoat Hinh 2D','images/tao-hoat-hinh-2d-voi-maya_m_1556178292.jpg','749000','02/12/2020','02/12/2020','1','88')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Làm phim hoạt hình 3D với MAYA',N'3D Maya giúp nâng cao năng suất làm việc, xử lý mô hình làm mịn toàn diện và render linh hoạt để bạn hoàn thành trọn vẹn ước mơ tạo ra bộ phim hoạt hình của chính mình','Lam Phim Hoat Hinh 3D','images/khoa-hoc-lam-phim-hoat-hinh-3d-voi-maya_m_1555642625.jpg','749000','02/12/2020','02/12/2020','1','88')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Học nhiếp ảnh từ cơ bản đến nâng cao',N'Tất cả những bí mật về nhiếp ảnh và xử lý ảnh sẽ được bật mí ngay trong khóa học để bạn có được những bức ảnh đẹp nhất, chất nhất và ấn tượng nhất!','Hoc Nhiep Anh','images/hoc-nhiep-anh-tu-co-ban-den-nang-cao_m_1555575047.jpg','599000','02/12/2020','02/12/2020','1','89')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Nhiếp ảnh cơ bản',N'Giúp bạn tự tin thực hiện các thao tác về máy ảnh và kỹ năng chụp ảnh chuyên nghiệp, có được những bức ảnh đẹp tuyệt vời cùng những khoảnh khắc có một không hai trong nhiếp ảnh! ','Nhiep Anh Co Ban','images/nhiep-anh-co-ban_m_1558082726.jpg','599000','02/12/2020','02/12/2020','1','89')
  insert into Courses(Name,Description,Content,Image,Price,CreationTime,LastModificationTime,Status,CategoryId)
   values(N'Sản xuất phim quảng cáo và hoạt hình 2D với Photoshop và After effects',N'Học cách thiết kế nhân vật, vẽ storyboard bằng phần mềm: Photoshop, Illustrator tạo xương, chuyển động bằng phần mềm: After effects','San xuat quang cao va hoat hinh 2D','images/san-xuat-phim-quang-cao-va-hoat-hinh-2d_m_1561429115.jpg','699000','02/12/2020','02/12/2020','1','88')

 select * from Courses
  
//=================================================Endnhập courses================================================================

//=================================================Nhập Categories================================================================
 

	insert into Categories(Name,SortOrder) values(N'Ngoại Ngữ','1')
	insert into Categories(Name,SortOrder) values(N'Marketing','2')
	insert into Categories(Name,SortOrder) values(N'Tin Học Văn Phòng','3')
	insert into Categories(Name,SortOrder) values(N'Thiết Kế','4')
	insert into Categories(Name,SortOrder) values(N'Kinh Doanh - Khởi Nghiệp','5')
	insert into Categories(Name,SortOrder) values(N'Phát Triển Cá Nhân ','6')
	insert into Categories(Name,SortOrder) values(N'Sales, Bán Hàng','7')
	insert into Categories(Name,SortOrder) values(N'Công Nghệ Thông Tin','8')
	insert into Categories(Name,SortOrder) values(N'Sức Khỏe - Giới Tính','9')
	insert into Categories(Name,SortOrder) values(N'Phong Cách Sống','10')
	insert into Categories(Name,SortOrder) values(N'Nuôi Dạy Con','11')
	insert into Categories(Name,SortOrder) values(N'Hôn Nhân và Gia Đình','12')
	insert into Categories(Name,SortOrder) values(N'Nhiếp Ảnh, Dựng Phim','13')
	//Ngoại Ngữ

	insert into Categories(Name,SortOrder,ParentId) values(N'Tiếng Hàn','1','1')
	insert into Categories(Name,SortOrder,ParentId) values(N'Tiếng Anh','2','1')
	insert into Categories(Name,SortOrder,ParentId) values(N'Tiếng Trung ','3','1')
	insert into Categories(Name,SortOrder,ParentId) values(N'Tiếng Nhật','4','1')
	insert into Categories(Name,SortOrder,ParentId) values(N'Tiếng Đức','5','1')
	//Marketing
	
	insert into Categories(Name,SortOrder,ParentId) values(N'Facebook Marketing','1','2')
	insert into Categories(Name,SortOrder,ParentId) values(N'Zalo Marketing','2','2')
	insert into Categories(Name,SortOrder,ParentId) values(N'Email Marketing','3','2')
	insert into Categories(Name,SortOrder,ParentId) values(N'Google Marketing','4','2')
	insert into Categories(Name,SortOrder,ParentId) values(N'Seo','5','2')
	insert into Categories(Name,SortOrder,ParentId) values(N'Branding','6','2')
	insert into Categories(Name,SortOrder,ParentId) values(N'Content Marketing','7','2')
	insert into Categories(Name,SortOrder,ParentId) values(N'Video Marketing','8','2')

	//Tin Học Văn Phòng
	insert into Categories(Name,SortOrder,ParentId) values(N'Excel','1','3')
	insert into Categories(Name,SortOrder,ParentId) values(N'Word','2','3')
	insert into Categories(Name,SortOrder,ParentId) values(N'Kế Toán','3','3')
	insert into Categories(Name,SortOrder,ParentId) values(N'Power Point','4','3')

	//Thiết Kế
	insert into Categories(Name,SortOrder,ParentId) values(N'Phần Mềm Thiết Kế','1','4')
	insert into Categories(Name,SortOrder,ParentId) values(N'Thiết Kế Web','2','4')
	insert into Categories(Name,SortOrder,ParentId) values(N'Thiết Kế Đồ Họa','3','4')
	insert into Categories(Name,SortOrder,ParentId) values(N'Thiết Kế Nội Thất','4','4')
	insert into Categories(Name,SortOrder,ParentId) values(N'Kiến Trúc Nội Thất','5','4')

	//Kinh Doanh Khởi Nghiệp
	insert into Categories(Name,SortOrder,ParentId) values(N'Chiến lược kinh doanh','1','5')
	insert into Categories(Name,SortOrder,ParentId) values(N'Bất động sản','2','5')
	insert into Categories(Name,SortOrder,ParentId) values(N'Crypto','3','5')
	insert into Categories(Name,SortOrder,ParentId) values(N'Kinh doanh Online','4','5')
	insert into Categories(Name,SortOrder,ParentId) values(N'Startup','5','5')
	insert into Categories(Name,SortOrder,ParentId) values(N'Kiếm tiền Online','6','5')
	insert into Categories(Name,SortOrder,ParentId) values(N'Quản trị doanh nghiệp','7','5')
	insert into Categories(Name,SortOrder,ParentId) values(N'Chứng khoán','8','5')
	insert into Categories(Name,SortOrder,ParentId) values(N'Dropshipping','9','5')

	//Phát Triển Cá Nhân
	
	
	insert into Categories(Name,SortOrder,ParentId) values(N'Thương hiệu cá nhân','1','6')
	insert into Categories(Name,SortOrder,ParentId) values(N'Tài chính cá nhân','2','6')
	insert into Categories(Name,SortOrder,ParentId) values(N'Kỹ năng lãnh đạo','3','6')
	insert into Categories(Name,SortOrder,ParentId) values(N'MC','4','6')
	insert into Categories(Name,SortOrder,ParentId) values(N'Phát triển bản thân','5','6')
	insert into Categories(Name,SortOrder,ParentId) values(N'Giao Tiếp','5','6')
	insert into Categories(Name,SortOrder,ParentId) values(N'Thuyết Trình','6','6')
	insert into Categories(Name,SortOrder,ParentId) values(N'Kỹ Năng Giao Tiếp','7','6')

	//Sales, Bán Hàng
	
	insert into Categories(Name,SortOrder,ParentId) values(N'Bán Hàng Online','1','7')
	insert into Categories(Name,SortOrder,ParentId) values(N'Livestream','2','7')
	insert into Categories(Name,SortOrder,ParentId) values(N'Telesales','3','7')
	insert into Categories(Name,SortOrder,ParentId) values(N'Bán hàng','4','7')
	insert into Categories(Name,SortOrder,ParentId) values(N'Chăm sóc khách hàng','5','7')

	//Công Nghệ Thông Tin
	insert into Categories(Name,SortOrder,ParentId) values(N'Cơ sở dữ liệu','1','8')
	insert into Categories(Name,SortOrder,ParentId) values(N'Lâp Trình','2','8')
	insert into Categories(Name,SortOrder,ParentId) values(N'Ngôn Ngữ Lập Trình','3','8')
	insert into Categories(Name,SortOrder,ParentId) values(N'Lập Trình Web','4','8')
	insert into Categories(Name,SortOrder,ParentId) values(N'Lập Trình Android','5','8')
	insert into Categories(Name,SortOrder,ParentId) values(N'Lập Trình IOS','6','8')
	select * from Categories
	//Sức khỏe giới tính
	insert into Categories(Name,SortOrder,ParentId) values(N'Giảm Cân','1','9')
	insert into Categories(Name,SortOrder,ParentId) values(N'Thiền','2','9')
	insert into Categories(Name,SortOrder,ParentId) values(N'Phòng Thế','3','9')
	insert into Categories(Name,SortOrder,ParentId) values(N'Fitness - Gym','4','9')
	insert into Categories(Name,SortOrder,ParentId) values(N'Tình Yêu','5','9')
	insert into Categories(Name,SortOrder,ParentId) values(N'Yoga','6','9')
	insert into Categories(Name,SortOrder,ParentId) values(N'Massage','7','9')
	insert into Categories(Name,SortOrder,ParentId) values(N'Dinh Dưỡng','8','9')
	insert into Categories(Name,SortOrder,ParentId) values(N'Mang Thai','9','9') 

	//phong cách sống
	insert into Categories(Name,SortOrder,ParentId) values(N'Làm đẹp','1','10')
	insert into Categories(Name,SortOrder,ParentId) values(N'Handmade','2','10')
	insert into Categories(Name,SortOrder,ParentId) values(N'Tử Vi','3','10')
	insert into Categories(Name,SortOrder,ParentId) values(N'Ảo Thuật ','4','10')
	insert into Categories(Name,SortOrder,ParentId) values(N'Âm Nhạc','5','10')
	insert into Categories(Name,SortOrder,ParentId) values(N'Ẩm thực - Nấu Ăn','6','10')
	insert into Categories(Name,SortOrder,ParentId) values(N'Dance - Zumba','7','10')
	insert into Categories(Name,SortOrder,ParentId) values(N'Phong Thủy','8','10') 

	//Nuôi Dạy Con
	insert into Categories(Name,SortOrder,ParentId) values(N'Dinh dưỡng cho con','1','11')
	insert into Categories(Name,SortOrder,ParentId) values(N'Phương pháp dạy con','2','11')
	insert into Categories(Name,SortOrder,ParentId) values(N'Giáo dục giới tính','3','11')

	//Hôn Nhân Gia Đình
	insert into Categories(Name,SortOrder,ParentId) values(N'Hôn Nhân','1','12')
	insert into Categories(Name,SortOrder,ParentId) values(N'Đời sống vợ chồng','2','12')

	//Nhiếp Ảnh, Dựng Phim
	insert into Categories(Name,SortOrder,ParentId) values(N'3d animation','1','13')
	insert into Categories(Name,SortOrder,ParentId) values(N'Biên tập video','2','13')
	insert into Categories(Name,SortOrder,ParentId) values(N'Dựng phim','3','13')
	insert into Categories(Name,SortOrder,ParentId) values(N'Chụp ảnh','4','13')
	insert into Categories(Name,SortOrder,ParentId) values(N'UI-UX','5','13')
	insert into Categories(Name,SortOrder,ParentId) values(N'Kỹ xảo','6','13')

//=================================================EndNhập Categories================================================================

//=================================================Nhập Lessons================================================================

	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) 
	values(N'Bài 0 Chia sẽ kinh nghiệm tìm hiểu code','videos/Bai0_Chia_se_kinh_nghiem_tim_hieu_code','','1','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId)
	 values(N'Bài 1_1 Cấu hình sublimText cho React Native','videos/Bai1_1_Cau_hinh_sublimText_Cho_React_Naitve_','','2','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId)
	 values(N'Bài 1_2 Cách Xuất log và gói code javascript','videos/Bai1_2_cach_xuat log_va_goi_code_javascript','','3','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId)
	 values(N'Bài 1 Hướng Dẫn Cài Đặt React Native','videos/Bai1_Huong dan_cai_dat_React_Native','','4','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId)
	 values(N'Bài 2 Biến Let Var Const và Template virals trong ES6','videos/Bai2_Bien_Let_ Var_ Const_va_Template_Virals_trong_ES6','','5','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId)
	 values(N'Bài 3 Arrow Function ES6','videos/Bai3_Arrow_Function ES6','','6','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId)
	 values(N'Bài 4 Kiểu Object','videos/Bai4_Kieu_Object','','7','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId)
	 values(N'Bài 5 phân rẽ cấu trúc Destruring','videos/Bai5_Phan_ra_cau_truc Destructuring','','8','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId)
	 values(N'Bài6_Rest_Paremater','videos/Bai6_Rest_Paremater','','9','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId)
	 values(N'Bài 7 Khai báo class trong ES6','videos/Bai7_Khai_bao_Class_trong_ES6','','10','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId)
	 values(N'Bài8 Cách xử lý trong ES6','videos/Bai8_Cach_xy_ly_mang trong_ES6','','11','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId)
	 values(N'Bài 9 Cách sử dụng mảng map trong ES6','videos/Bai9_Cach_su_dung_mang_Map_trong_ES6','','12','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId)
	 values(N'Bài 10 Cách sử dụng mảng Set trong ES6','videos/Bai10_cach_su_dung_mang_Set_trong_ES6','','13','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId)
	 values(N'Bài 11 Khảo sát vòng đời Component','videos/Bai11_Khao_sat_vong_doi_cua_Component','','14','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) 
	values(N'Bài 12 Tông quan về Component','videos/Bai12_Tong_quan_ve_Component','','15','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) 
	values(N'Bài 13 tìm hiểu thư mục và chạy ứng dụng helloworld','videos/Bai13_Tin_hieu_thu_muc_va_chay_ung_dung_helloworld','','16','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId)
	 values(N'Bài 14 Cách chạy code cho từng platform','videos/Bai14_Cach_chi_dinh_code_cho_tung_platform','','17','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) 
	values(N'Bài 15 Khái niệm props và state','videos/Bai15_Khai_niem_props_va_state','','18','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId)
	 values(N'Bài 16 Thực hành props','videos/Bai16_Thuc_hanh_props','','19','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) 
	values(N'Bài 17 Thực hành state','videos/Bai17Thuc_hanh_state','','20','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) 
	values(N'Bài 18 Style trong ReactNative','videos/Bai18_Style_trong_ReactNative','','21','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) 
	values(N'Bài 19 Thực hành trong Style','videos/Bai19_Thuc_hanh_Style','','22','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId)
	 values(N'Bài 20 Width vs Height vs Flex','videos/Bai20_Width_vs_Height_vs_Flex','','23','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId)
	 values(N'Bài 21 Thực hành Width vs Height vs Flex','videos/Bai21_Thuc_hanh_Width_vs_Height_vs_Flex','','24','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId)
	 values(N'Bài 22 Flexbox','videos/Bai22_Flexbox','','25','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId)
	 values(N'Bài 23 Thực hàn FlexBox','videos/Bai23_Thuc_hanh_FlexBox','','26','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId)
	 values(N'Bài 24 Text Component','videos/Bai24_Text_Component','','27','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId)
	 values(N'Bài 25 Thực hành Text Component','videos/Bai25_Thuc_hanh_Text_Component','','28','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId)
	 values(N'Bài 26 TextInput Component','videos/Bai26_TextInput_Component','','29','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId)
	 values(N'Bài 27 Thực hành TextInput','videos/','Bai27_Thuc_hanh_TextInput','30','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 28 button component','videos/Bai28_button_component','','31','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 29 image component','videos/Bai29_image_component','','32','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 30 Thực hành image','videos/Bai30_thuc_hanh_image','','33','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 31 View component','videos/Bai31_View_component','','34','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 32 Thực hành view','videos/Bai32_thuc_hanh_view','','35','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 33 Tìm hiểu ý nghĩa import và export','videos/Bai33_tim_hieu_y_nghia_import_va_export_trong_ReactNative','','36','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 34 Xây dựng giao diện đăng nhập pokemon','videos/Bai34_Xay_dung_giao_dien_dang_nhap_Pokemon','','37','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 35 Cách xử lý username và password','videos/Bai35_Cach_xu_ly_username_va_password','','38','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 36 Thiết kế ứng dụng Calculator','videos/Bai36_Thiet_ke_giao_dien_ung_dung_Calculator','','39','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 37 Xử lý ứng dụng của calculator','videos/Bai37_Xu_ly_logic_ung_dung_calculator','','40','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 38 Navigator','videos/Bai38_Navigator','','41','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 39 Truyền tham số trong Navigator','videos/Bai39_Truyen_tham_so_trong_Navigator','','42','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 40 ProgressBar','videos/Bai40_ProgressBar','','43','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 41 Activity Indicator','videos/Bai41_Activity_Indicator','','44','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 42 Notification','videos/Bai42_Notification','','45','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 43 SearchView vs ListView','videos/Bai43_SearchView_vs_ListView','','46','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 44 ScrollView','videos/Bai44_ScrollView','','47','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 45 Cách sử dụng ListView','videos/Bai45_Cach_su_dung_ListView','','48','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 46 Cách thêm dữ liệu Listview','videos/Bai46_Cach_them_du_lieu_Listview','','49','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 47 Hiển thị listview dạng object','videos/Bai47_Hien_thi_listview_dang_object','','50','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 48 Hiển thị listview dạng Grid','videos/Bai48_hien_Thi_ListView_dang_Grid','','51','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 49 Picker','videos/Bai49_Picker','','52','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 50 Switch Button','videos/Bai50_Switch_Button','','53','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 51 Silder','videos/Bai51_Silder','','54','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 52 KeyboardAvoidi gView','videos/Bai52_KeyboardAvoidi_gView','','55','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 53 DrawerLayout','videos/Bai53_DrawerLayout','','56','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 54 Cách sử dụng thư viện ViewPager,Tab_cho_Android_va_IOS','videos/Bai54_Cach_su_dung_thu_vien_ViewPager,Tab_cho_Android_va_IOS','','57','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 55 Modal','videos/Bai55_Modal','','58','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 56 WebView','videos/Bai56_WebView','','59','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 57 Cách cấu hình và sử dụng GoogleMap','videos/Bai57_Cach_cau_hình_va_su_dung_GoogleMap','','60','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 58 Tìm hiểu props mapview','videos/Bai58_Tim_hieu_props_mapview','','60','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 59 Cách sử dụng và thêm marker mapview','videos/Bai59_cach_su_dung_va_them_marker_mapview','','60','1','7')
	insert into Lessons(Name,VideoPath,Attachment,SortOrder,Status,CourseId) values(N'Bài 60 CustomCallout MapView','videos/Bai60_CustomCallout_MapView','','60','1','7')


//=================================================EndNhập Lessons================================================================


//=================================================	NhậpOrder&OrderDetail=======================================================
set dateformat DMY
	insert into Orders(PaymentMethod,UserId,Total,Message,CreationTime,LastModificationTime,Status) 
	values (3,'32fcfdc3-7d98-4489-b81a-88cff6e1b438',1,'Bạn đã mua thành công','13/12/2020','13/12/2020','0')
	insert into OrderDetails(OrderId,ActiveCourseId,Price,PromotionPrice) 
	values (1,'c0303c29-d59d-42f3-2f0e-08d89e74ee5f',499000,49900)

//=================================================	NhậpOrder&OrderDetail=======================================================
