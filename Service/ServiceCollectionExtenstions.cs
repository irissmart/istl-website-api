using Framework;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service.Interface;

namespace Service
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextFactory<IrisContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IPageService, PageService>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<ITestimonialService, TestimonialService>();
            services.AddScoped<IClientServiceCategoryService, ClientServiceCategoryService>();
            services.AddScoped<IClientServiceService, ClientServiceService>();
            services.AddScoped<IPartnerService, PartnerService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IContactRequestService, ContactRequestService>();
            services.AddScoped<IJobCategoryService, JobCategoryService>();
            services.AddScoped<ITeamMemberService, TeamMemberService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJobApplicationService, JobApplicationService>();
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<IJobTagService, JobTagService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IVisitorStatsService, VisitorStatService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserPermission, UserPermission>();

            return services;
        }
    }
}