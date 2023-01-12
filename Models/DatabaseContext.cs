using Microsoft.EntityFrameworkCore;

namespace JWTAPI.Models
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee>? Employees { get; set; }
        public virtual DbSet<UserInfo>? UserInfos { get; set; }
        public virtual DbSet<Logs>? Loging { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Logs>(entity =>
            {

                entity.ToTable("Logs");
                entity.Property(e => e.LogId).HasColumnName("LogId");
                entity.Property(e => e.userID).HasColumnName("userID");
                entity.Property(e => e.Timestamp).IsUnicode(false);
                entity.Property(e => e.Descryption).HasMaxLength(60).IsUnicode(false);
            });


            modelBuilder.Entity<UserInfo>(entity =>
            {
                // entity.HasNoKey();
                entity.ToTable("UserInfo");
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.DisplayName).HasMaxLength(60).IsUnicode(false);
                entity.Property(e => e.UserName).HasMaxLength(30).IsUnicode(false);
                entity.Property(e => e.Email).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Password).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.CreatedDate).IsUnicode(false);
                entity.Property(e => e.LastLogin).IsUnicode(false);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");
                entity.Property(e => e.EmployeeID).HasColumnName("EmployeeID");
                entity.Property(e => e.EmployeeName).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.LoginID).HasMaxLength(256).IsUnicode(false);
                entity.Property(e => e.Password).HasMaxLength(256).IsUnicode(false);
                entity.Property(e => e.ExpireTime).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
