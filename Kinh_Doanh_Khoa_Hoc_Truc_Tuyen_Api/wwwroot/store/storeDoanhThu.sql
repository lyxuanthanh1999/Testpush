create PROC GetRevenueDaily
	@fromDate VARCHAR(10),
	@toDate VARCHAR(10)
AS
BEGIN
				select CAST(o.CreationTime AS DATE) as Date,
				sum(IIF(od.PromotionPrice is null, od.Price,od.PromotionPrice)) as Revenue,
				sum(IIF(od.PromotionPrice is null, od.Price,od.PromotionPrice) * 30 / 100) as Profit
				from Orders o
				inner join OrderDetails od
				on o.Id = od.OrderId
				where o.CreationTime <= cast(@toDate as date) 
				AND o.CreationTime >= cast(@fromDate as date)
				group by o.CreationTime
END
go
EXEC dbo.GetRevenueDaily @fromDate = N'2020-12-1', @toDate = N'2020-12-14'


create proc GetCountSalesDaily
	@fromDate VARCHAR(10),
	@toDate VARCHAR(10)
AS
BEGIN
				select CAST(o.CreationTime AS DATE) as Date,
				c.Name,
				u.UserName,
				u.Email,
				u.Name,
				count(c.Name) as CountProducts,
				sum(IIF(od.PromotionPrice is null, od.Price,od.PromotionPrice)) as Revenue,
				sum(IIF(od.PromotionPrice is null, od.Price,od.PromotionPrice) * 30 / 100) as Profit
				from Orders o
				inner join OrderDetails od
				on o.Id = od.OrderId
				inner join ActivateCourses ac
				on ac.Id = od.ActiveCourseId
				inner join Courses c
				on c.Id = ac.CourseId
				inner join AspNetUsers u
				on c.CreatedUserName = u.UserName
				where o.CreationTime <= cast(@toDate as date) 
				AND o.CreationTime >= cast(@fromDate as date)
				group by o.CreationTime, c.Name, u.UserName, u.Email, u.Name
				order by o.CreationTime
END
go
EXEC dbo.GetCountSalesDaily @fromDate = N'2020-12-1', @toDate = N'2020-12-14'