using Infrastructure.Entities;
using Infrastructure.ModelsX;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public partial class IrisContext : DbContext
{
    public IrisContext()
    {
    }

    public IrisContext(DbContextOptions<IrisContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Analytic> Analytics { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientService> ClientServices { get; set; }

    public virtual DbSet<ClientServiceCategory> ClientServiceCategories { get; set; }

    public virtual DbSet<ContactInformation> ContactInformations { get; set; }

    public virtual DbSet<ContactRequest> ContactRequests { get; set; }

    public virtual DbSet<DetailedService> DetailedServices { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobApplication> JobApplications { get; set; }

    public virtual DbSet<JobCategory> JobCategories { get; set; }

    public virtual DbSet<JobStatus> JobStatuses { get; set; }

    public virtual DbSet<JobTag> JobTags { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Page> Pages { get; set; }

    public virtual DbSet<Partner> Partners { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<SocialLink> SocialLinks { get; set; }

    public virtual DbSet<Step> Steps { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<TeamMember> TeamMembers { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserPermission> UserPermissions { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<VisitLog> VisitLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Analytic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Analytic__3214EC27D062C62F");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Number).HasMaxLength(50);
            entity.Property(e => e.SectionId).HasColumnName("SectionID");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Section).WithMany(p => p.Analytics)
                .HasForeignKey(d => d.SectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Analytics_Section");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Client__3214EC27A2B4B06D");

            entity.ToTable("Client");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Image).HasMaxLength(1000);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.Url).HasMaxLength(1000);
        });

        modelBuilder.Entity<ClientService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ClientSe__3214EC2709DD9DF2");

            entity.ToTable("ClientService");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ClientServiceCategoryId).HasColumnName("ClientServiceCategoryID");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ServiceImagePath).HasMaxLength(255);
            entity.Property(e => e.ServiceName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.ClientServiceCategory).WithMany(p => p.ClientServices)
                .HasForeignKey(d => d.ClientServiceCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientService_ClientServiceCategory");
        });

        modelBuilder.Entity<ClientServiceCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ClientSe__3214EC27326E0C92");

            entity.ToTable("ClientServiceCategory");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CategoryImagePath).HasMaxLength(255);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ServiceCategoryName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<ContactInformation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ContactI__3214EC2716C93ED3");

            entity.ToTable("ContactInformation");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<ContactRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ContactR__3214EC27329E4CF9");

            entity.ToTable("ContactRequest");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(1000);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<DetailedService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Detailed__3214EC27180E5406");

            entity.ToTable("DetailedService");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IconRelativePath).HasMaxLength(255);
            entity.Property(e => e.SectionId).HasColumnName("SectionID");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Section).WithMany(p => p.DetailedServices)
                .HasForeignKey(d => d.SectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetailedService_Section");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Job__3214EC27D1616EAA");

            entity.ToTable("Job");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.Country).HasMaxLength(255);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Currency).HasMaxLength(255);
            entity.Property(e => e.ExpiresOn).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.JobCategoryId).HasColumnName("JobCategoryID");
            entity.Property(e => e.JobStatusId).HasColumnName("JobStatusID");
            entity.Property(e => e.MaxSalary).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MinSalary).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.JobCategory).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.JobCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Job__JobCategory__30C33EC3");

            entity.HasOne(d => d.JobStatus).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.JobStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Job__JobStatusID__31B762FC");
        });

        modelBuilder.Entity<JobApplication>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobAppli__3214EC277D355CCD");

            entity.ToTable("JobApplication");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Contact).HasMaxLength(200);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FileRelativePath).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.JobId).HasColumnName("JobID");
            entity.Property(e => e.LastName).HasMaxLength(200);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Job).WithMany(p => p.JobApplications)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JobApplic__JobID__40058253");
        });

        modelBuilder.Entity<JobCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobCateg__3214EC270C22696F");

            entity.ToTable("JobCategory");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ImageRelativePath).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.JobCategoryName).HasMaxLength(255);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<JobStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobStatu__3214EC27B5A87B5D");

            entity.ToTable("JobStatus");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.JobStatusName).HasMaxLength(255);
        });

        modelBuilder.Entity<JobTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobTag__3214EC279D09B9F9");

            entity.ToTable("JobTag");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.JobId).HasColumnName("JobID");
            entity.Property(e => e.TagId).HasColumnName("TagID");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Job).WithMany(p => p.JobTags)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JobTag__JobID__367C1819");

            entity.HasOne(d => d.Tag).WithMany(p => p.JobTags)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JobTag__TagID__37703C52");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC27F82537DD");

            entity.ToTable("Notification");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Message).HasMaxLength(255);
            entity.Property(e => e.ReceiverId).HasColumnName("ReceiverID");
            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });


        modelBuilder.Entity<Page>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Page__3214EC278FFED070");

            entity.ToTable("Page");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.PageName).HasMaxLength(255);
            entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<Partner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Partner__3214EC27C96FFB26");

            entity.ToTable("Partner");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .IsFixedLength();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC271B2B50C5");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Expires).HasColumnType("datetime");
            entity.Property(e => e.Revoked).HasColumnType("datetime");
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Used).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefreshTo__UserI__43D61337");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Section__3214EC27A65CFE45");

            entity.ToTable("Section");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BackgroundImageRelativePath).HasMaxLength(255);
            entity.Property(e => e.ButtonText).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.PageId).HasColumnName("PageID");
            entity.Property(e => e.SectionImageRelativePath).HasMaxLength(255);
            entity.Property(e => e.SectionName).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Page).WithMany(p => p.Sections)
                .HasForeignKey(d => d.PageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Section_Page");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Service__3214EC27387E722E");

            entity.ToTable("Service");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LogoRelativePath).HasMaxLength(255);
            entity.Property(e => e.SectionId).HasColumnName("SectionID");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Section).WithMany(p => p.Services)
                .HasForeignKey(d => d.SectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Service_Section");
        });

        modelBuilder.Entity<SocialLink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SocialLi__3214EC2792626493");

            entity.ToTable("SocialLink");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ContactInformationId).HasColumnName("ContactInformationID");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PlatformName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.Url).HasMaxLength(500);

            entity.HasOne(d => d.ContactInformation).WithMany(p => p.SocialLinks)
                .HasForeignKey(d => d.ContactInformationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SocialLink_ContactInformation");
        });

        modelBuilder.Entity<Step>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Step__3214EC07D37D1ACD");

            entity.ToTable("Step");

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Section).WithMany(p => p.Steps)
                .HasForeignKey(d => d.SectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Step_Section");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tag__3214EC275BDADDEF");

            entity.ToTable("Tag");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.TagName).HasMaxLength(255);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<TeamMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TeamMemb__3214EC275B942D4F");

            entity.ToTable("TeamMember");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Testimon__3214EC270F2C5309");

            entity.ToTable("Testimonial");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ClientName).HasMaxLength(255);
            entity.Property(e => e.ClientOccupation).HasMaxLength(255);
            entity.Property(e => e.Comment).IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ImageRelativePath).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC27B565C8C8");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__A9D105346A4B767E").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.ProfileImageRelativePath).HasMaxLength(255);
            entity.Property(e => e.SaltHash)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TokenExpiry).HasColumnType("datetime");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");

            entity.HasOne(d => d.UserRole).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__UserRoleID__571DF1D5");
        });

        modelBuilder.Entity<UserPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserPerm__3214EC2765E4C060");

            entity.ToTable("UserPermission");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CanCreate).HasDefaultValue(true);
            entity.Property(e => e.CanDelete).HasDefaultValue(true);
            entity.Property(e => e.CanUpdate).HasDefaultValue(true);
            entity.Property(e => e.CanView).HasDefaultValue(true);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModuleName).HasMaxLength(100);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.UserPermissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPermission_User");
        });


        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserRole__3214EC2718DDA693");

            entity.ToTable("UserRole");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UserRoleName).HasMaxLength(255);
        });

        modelBuilder.Entity<VisitLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VisitLog__3214EC07E5604B07");

            entity.Property(e => e.ClientId).HasMaxLength(50);
            entity.Property(e => e.IpAddress).HasMaxLength(50);
            entity.Property(e => e.PageUrl).HasMaxLength(500);
            entity.Property(e => e.Referrer).HasMaxLength(500);
            entity.Property(e => e.SessionId).HasMaxLength(50);
            entity.Property(e => e.UserAgent).HasMaxLength(500);
            entity.Property(e => e.VisitDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.VisitorFingerprint).HasMaxLength(128);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public Task<int> SaveChangesAsync(int userId, CancellationToken cancellationToken = default)
    {
        SetDefaultValues(userId);
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetDefaultValues(int userId)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                foreach (var property in entry.Properties)
                {
                    switch (property.Metadata.Name)
                    {
                        case "IsActive":
                        case "IsEnabled":
                            property.CurrentValue = true;
                            break;
                        case "CreatedOn":
                            property.CurrentValue = DateTime.UtcNow;
                            break;
                        case "CreatedBy":
                            property.CurrentValue = userId;
                            break;
                    }
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                foreach (var property in entry.Properties)
                {
                    switch (property.Metadata.Name)
                    {
                        case "UpdatedBy":
                            if (property.IsModified)
                            {
                                property.CurrentValue = userId;
                            }
                            break;
                        case "UpdatedOn":
                            if (property.IsModified)
                            {
                                property.CurrentValue = DateTime.UtcNow;
                            }
                            break;
                    }
                }
            }
        }
    }
}
