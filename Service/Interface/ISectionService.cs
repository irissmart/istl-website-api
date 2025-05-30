using Framework.Model;
using Service.DTOs;
using Service.DTOs.Request;

namespace Service.Interface
{
    public interface ISectionService
    {
        Task<dynamic> GetByPageIdAsync(int pageId);
        Task<BaseResponse<Task>> UpdateHomeSectionsAsync(int userId, HomeSectionsUpdateDTO request);
        Task<BaseResponse<Task>> UpdateServiceSectionsImagesAsync(int userId, ServiceSectionsImagesUpdateDTO request);
        Task<BaseResponse<Task>> UpdateHomeSectionsImagesAsync(int userId, HomeSectionsImagesUpdateDTO request);
        Task<BaseResponse<Task>> UpdateAboutSectionsImagesAsync(int userId, AboutSectionsImagesUpdateDTO request);
        Task<BaseResponse<Task>> UpdateContactSectionImageAsync(int userId, ContactSectionImageUpdateDTO request);
        Task<BaseResponse<Task>> UpdateManagementSectionImageAsync(int userId, ManagementSectionImageUpdateDTO request);
        Task<BaseResponse<Task>> UpdateVacancySectionImageAsync(int userId, VacancySectionImageUpdateDTO request);
        Task<BaseResponse<Task>> UpdatePartnerSectionImageAsync(int userId, PartnerSectionImageUpdateDTO request);
        Task<BaseResponse<Task>> UpdateSitemapSectionImageAsync(int userId, SitemapSectionImageUpdateDTO request);
        Task<BaseResponse<Task>> UpdateAsync(int userId, SectionsUpdateDTO request);
    }
}
