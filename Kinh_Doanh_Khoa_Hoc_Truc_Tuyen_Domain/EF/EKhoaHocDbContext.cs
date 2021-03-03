using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Configurations;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.EF
{
    public class EKhoaHocDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public EKhoaHocDbContext(DbContextOptions options) : base(options)
        {
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<EntityEntry> modified = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);
            foreach (EntityEntry item in modified)
            {
                if (item.Entity is IDateTracking changedOrAddedItem)
                {
                    if (item.State == EntityState.Added)
                    {
                        changedOrAddedItem.CreationTime = DateTime.Now;
                    }
                    else
                    {
                        changedOrAddedItem.LastModificationTime = DateTime.Now;
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AnnouncementConfiguration());
            builder.ApplyConfiguration(new AnnouncementUserConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new CommandConfiguration());
            builder.ApplyConfiguration(new CommandInFunctionConfiguration());
            builder.ApplyConfiguration(new CourseConfiguration());
            builder.ApplyConfiguration(new FunctionConfiguration());
            builder.ApplyConfiguration(new CommentConfiguration());
            builder.ApplyConfiguration(new LessonConfiguration());
            builder.ApplyConfiguration(new OrderConfiguration());
            builder.ApplyConfiguration(new OrderDetailConfiguration());
            builder.ApplyConfiguration(new PermissionConfiguration());
            builder.ApplyConfiguration(new PromotionConfiguration());
            builder.ApplyConfiguration(new ActivateCourseConfiguration());
            builder.ApplyConfiguration(new PromotionInCourseConfiguration());
            builder.ApplyConfiguration(new FeedBackConfiguration());
            builder.HasSequence("KhoaHocSequence");
            base.OnModelCreating(builder);
        }

        public DbSet<FeedBack> FeedBacks { get; set; }

        public DbSet<Announcement> Announcements { get; set; }

        public DbSet<AnnouncementUser> AnnouncementUsers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Command> Commands { get; set; }

        public DbSet<CommandInFunction> CommandInFunctions { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Function> Functions { get; set; }

        public DbSet<Lesson> Lessons { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<Promotion> Promotions { get; set; }

        public DbSet<ActivateCourse> ActivateCourses { get; set; }

        public DbSet<PromotionInCourse> PromotionInCourses { get; set; }
    }
}