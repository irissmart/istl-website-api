using Framework.Interface;
using Framework.Model;
using Framework.Service;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.DTOs;
using Service.DTOs.Request;
using Service.DTOs.Response;
using Service.Interface;
using Page = Service.Enums.Page;

namespace Service
{
    public class SectionService : BaseDatabaseService<IrisContext>, ISectionService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        private readonly IContactService _contactService;
        private readonly string? _uploadPath;

        public SectionService(IrisContext context
            , IConfiguration configuration
            , IFileService fileService
            , IContactService contactService) : base(context)
        {
            _configuration = configuration;
            _fileService = fileService;
            _contactService = contactService;
            _uploadPath = _configuration["UploadPath"];
        }

        public async Task<dynamic> GetByPageIdAsync(int pageId)
        {
            switch (pageId)
            {

                case (int)Page.Home:
                    return await GetAllHomeSectionsAsync();

                case (int)Page.Contact:
                    return await _contactService.GetAsync();

                default:
                    return await GetAllSectionsAsync(pageId);
            }
        }

        private async Task<BaseResponse<HomeSectionsDTO>> GetAllHomeSectionsAsync()
        {
            return await HandleActionAsync(async () =>
            {
                var sections = await _context.Sections
                    .AsNoTracking()
                    .Where(x => x.PageId == (int)Page.Home && x.IsActive)
                    .Include(x => x.Analytics)
                    .Include(x => x.Services)
                    .Include(x => x.DetailedServices)
                    .Include(x => x.Steps)
                    .AsSplitQuery()
                    .Select(section => new HomeSectionDTO
                    {
                        Id = section.Id,
                        SectionName = section.SectionName,
                        Title = section.Title,
                        Description = section.Description,
                        ButtonText = section.ButtonText,
                        BackgroundImagePath = section.BackgroundImageRelativePath != null ? _configuration["ImageUrl"] + section.BackgroundImageRelativePath : null,
                        SectionImagePath = section.SectionImageRelativePath != null ? _configuration["ImageUrl"] + section.SectionImageRelativePath : null,
                        Analytics = section.Analytics
                            .Where(a => a.IsActive)
                            .Select(a => new AnalyticDTO
                            {
                                Id = a.Id,
                                Number = a.Number,
                                Title = a.Title
                            })
                            .ToList(),
                        Services = section.Services
                            .Where(s => s.IsActive)
                            .Select(s => new ServiceDTO
                            {
                                Id = s.Id,
                                Title = s.Title,
                                LogoPath = s.LogoRelativePath != null ? _configuration["ImageUrl"] + s.LogoRelativePath : null
                            })
                            .ToList(),
                        DetailedServices = section.DetailedServices
                            .Where(ds => ds.IsActive)
                            .Select(ds => new DetailedServiceDTO
                            {
                                Id = ds.Id,
                                Title = ds.Title,
                                IconPath = ds.IconRelativePath != null ? _configuration["ImageUrl"] + ds.IconRelativePath : null,
                                Description = ds.Description
                            })
                            .ToList(),
                        Steps = section.Steps
                            .Where(st => st.IsActive)
                            .Select(st => new StepDTO
                            {
                                Id = st.Id,
                                Title = st.Title,
                                Description = st.Description
                            })
                            .ToList()
                    })
                    .ToListAsync();

                if (sections == null || !sections.Any())
                {
                    InitMessageResponse("NotFound");
                    return null!;
                }

                return new HomeSectionsDTO
                {
                    PageId = (int)Page.Home,
                    Sections = sections
                };
            });
        }

        public async Task<BaseResponse<Task>> UpdateHomeSectionsAsync(int userId, HomeSectionsUpdateDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                var dbSections = await _context.Sections
                    .Where(x => x.PageId == (int)Page.Home && x.IsActive)
                    .Include(x => x.Analytics)
                    .Include(x => x.Services)
                    .Include(x => x.DetailedServices)
                    .Include(x => x.Steps)
                    .AsSplitQuery()
                    .ToListAsync();

                foreach (var sectionDto in request.Sections)
                {
                    var dbSection = dbSections.FirstOrDefault(s => s.Id == sectionDto.Id);
                    
                    if (dbSection == null)
                    {
                        InitMessageResponse("BadRequest");
                        return;
                    }

                    dbSection.SectionName = sectionDto.SectionName;
                    dbSection.Title = sectionDto.Title;
                    dbSection.Description = sectionDto.Description;
                    dbSection.ButtonText = sectionDto.ButtonText;

                    foreach (var analyticDto in sectionDto.Analytics)
                    {
                        if (analyticDto.Id > 0)
                        {
                            var existingAnalytic = dbSection.Analytics.FirstOrDefault(a => a.Id == analyticDto.Id);
                            
                            if (existingAnalytic != null)
                            {
                                existingAnalytic.Number = analyticDto.Number;
                                existingAnalytic.Title = analyticDto.Title;
                            }
                        }
                    }

                    foreach (var serviceDto in sectionDto.Services)
                    {
                        if (serviceDto.Id > 0)
                        {
                            var existingService = dbSection.Services.FirstOrDefault(s => s.Id == serviceDto.Id);
                            
                            if (existingService != null)
                            {
                                existingService.Title = serviceDto.Title;
                            }
                        }
                    }

                    foreach (var dsDto in sectionDto.DetailedServices)
                    {
                        if (dsDto.Id > 0)
                        {
                            var existingDetailedService = dbSection.DetailedServices.FirstOrDefault(ds => ds.Id == dsDto.Id);
                            if (existingDetailedService != null)
                            {
                                existingDetailedService.Title = dsDto.Title;
                                existingDetailedService.Description = dsDto.Description;
                            }
                        }
                    }

                    foreach (var stepDto in sectionDto.Steps)
                    {
                        if (stepDto.Id > 0)
                        {
                            var existingStep = dbSection.Steps.FirstOrDefault(s => s.Id == stepDto.Id);
                            if (existingStep != null)
                            {
                                existingStep.Title = stepDto.Title;
                                existingStep.Description = stepDto.Description;
                            }
                        }
                    }

                    _context.Sections.Update(dbSection);
                }

                await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> UpdateHomeSectionsImagesAsync(int userId, HomeSectionsImagesUpdateDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                bool hasAnyUpdates = false;

                if (request.BackgroundImage != null)
                {
                    var homeSection = await _context.Sections.FirstOrDefaultAsync();

                    if (homeSection == null)
                    {
                        InitMessageResponse("NotFound");
                        return;
                    }

                    if (!string.IsNullOrEmpty(homeSection.BackgroundImageRelativePath))
                    {
                        _fileService.DeleteFile(_uploadPath, homeSection.BackgroundImageRelativePath);
                    }

                    var image = await _fileService.UploadAsync(_uploadPath, request.BackgroundImage);

                    homeSection.BackgroundImageRelativePath = image;

                    _context.Sections.Update(homeSection);
                    hasAnyUpdates = true;
                }

                if (request.ServiceImages != null)
                {
                    var serviceSection = await _context.Services.ToListAsync();

                    if (serviceSection == null)
                    {
                        InitMessageResponse("NotFound");
                        return;
                    }

                    for (int i = 0; i < request.ServiceImages.Count && i < serviceSection.Count; i++)
                    {
                        var serviceImage = request.ServiceImages[i];

                        if (serviceImage != null)
                        {
                            if (!string.IsNullOrEmpty(serviceSection[i].LogoRelativePath))
                            {
                                _fileService.DeleteFile(_uploadPath, serviceSection[i].LogoRelativePath);
                            }

                            var image = await _fileService.UploadAsync(_uploadPath, serviceImage);
                            serviceSection[i].LogoRelativePath = image;
                        }
                    }

                    _context.Services.UpdateRange(serviceSection);
                    hasAnyUpdates = true;

                }

                if (request.DetailedServiceImages != null)
                {
                    var detailedServiceSection = await _context.DetailedServices.ToListAsync();

                    if (detailedServiceSection == null)
                    {
                        InitMessageResponse("NotFound");
                        return;
                    }

                    for (int i = 0; i < request.DetailedServiceImages.Count && i < detailedServiceSection.Count; i++)
                    {
                        var detailedServiceImage = request.DetailedServiceImages[i];

                        if (detailedServiceImage != null)
                        {
                            if (!string.IsNullOrEmpty(detailedServiceSection[i].IconRelativePath))
                            {
                                _fileService.DeleteFile(_uploadPath, detailedServiceSection[i].IconRelativePath);
                            }

                            var image = await _fileService.UploadAsync(_uploadPath, detailedServiceImage);
                            detailedServiceSection[i].IconRelativePath = image;
                        }
                    }

                    _context.DetailedServices.UpdateRange(detailedServiceSection);
                    hasAnyUpdates = true;
                }

                if (hasAnyUpdates)
                    await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> UpdateServiceSectionsImagesAsync(int userId, ServiceSectionsImagesUpdateDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                bool hasAnyUpdates = false;

                if (request.BannerImage != null)
                {
                    var serviceBannerSection = await _context.Sections
                        .Where(x => x.IsActive && x.Id == 19)
                        .FirstOrDefaultAsync();

                    if (serviceBannerSection == null)
                    {
                        InitMessageResponse("NotFound");
                        return;
                    }

                    if (!string.IsNullOrEmpty(serviceBannerSection.BackgroundImageRelativePath))
                    {
                        _fileService.DeleteFile(_uploadPath, serviceBannerSection.BackgroundImageRelativePath);
                    }

                    var image = await _fileService.UploadAsync(_uploadPath, request.BannerImage);

                    serviceBannerSection.BackgroundImageRelativePath = image;

                    _context.Sections.Update(serviceBannerSection);
                    hasAnyUpdates = true;
                }

                if (request.FirstSectionImage != null)
                {
                    var serviceFirstSection = await _context.Sections
                        .Where(x => x.IsActive && x.Id == 9)
                        .FirstOrDefaultAsync();

                    if (serviceFirstSection == null)
                    {
                        InitMessageResponse("NotFound");
                        return;
                    }

                    if (!string.IsNullOrEmpty(serviceFirstSection.SectionImageRelativePath))
                    {
                        _fileService.DeleteFile(_uploadPath, serviceFirstSection.SectionImageRelativePath);
                    }

                    var image = await _fileService.UploadAsync(_uploadPath, request.FirstSectionImage);

                    serviceFirstSection.SectionImageRelativePath = image;

                    _context.Sections.Update(serviceFirstSection);
                    hasAnyUpdates = true;

                }

                if (hasAnyUpdates)
                    await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> UpdateAboutSectionsImagesAsync(int userId, AboutSectionsImagesUpdateDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                bool hasAnyUpdates = false;

                if (request.BannerImage != null)
                {
                    var aboutBannerSection = await _context.Sections
                        .Where(x => x.IsActive && x.Id == 20)
                        .FirstOrDefaultAsync();

                    if (aboutBannerSection == null)
                    {
                        InitMessageResponse("NotFound");
                        return;
                    }

                    if (!string.IsNullOrEmpty(aboutBannerSection.BackgroundImageRelativePath))
                    {
                        _fileService.DeleteFile(_uploadPath, aboutBannerSection.BackgroundImageRelativePath);
                    }

                    var image = await _fileService.UploadAsync(_uploadPath, request.BannerImage);

                    aboutBannerSection.BackgroundImageRelativePath = image;

                    _context.Sections.Update(aboutBannerSection);
                    hasAnyUpdates = true;
                }

                if (request.FirstSectionImage != null)
                {
                    var aboutFirstSection = await _context.Sections
                        .Where(x => x.IsActive && x.Id == 7)
                        .FirstOrDefaultAsync();

                    if (aboutFirstSection == null)
                    {
                        InitMessageResponse("NotFound");
                        return;
                    }

                    if (!string.IsNullOrEmpty(aboutFirstSection.SectionImageRelativePath))
                    {
                        _fileService.DeleteFile(_uploadPath, aboutFirstSection.SectionImageRelativePath);
                    }

                    var image = await _fileService.UploadAsync(_uploadPath, request.FirstSectionImage);

                    aboutFirstSection.SectionImageRelativePath = image;

                    _context.Sections.Update(aboutFirstSection);
                    hasAnyUpdates = true;

                }

                if (request.SecondSectionImage != null)
                {
                    var aboutSecondSection = await _context.Sections
                        .Where(x => x.IsActive && x.Id == 8)
                        .FirstOrDefaultAsync();

                    if (aboutSecondSection == null)
                    {
                        InitMessageResponse("NotFound");
                        return;
                    }

                    if (!string.IsNullOrEmpty(aboutSecondSection.SectionImageRelativePath))
                    {
                        _fileService.DeleteFile(_uploadPath, aboutSecondSection.SectionImageRelativePath);
                    }

                    var image = await _fileService.UploadAsync(_uploadPath, request.SecondSectionImage);

                    aboutSecondSection.SectionImageRelativePath = image;

                    _context.Sections.Update(aboutSecondSection);
                    hasAnyUpdates = true;

                }

                if (hasAnyUpdates)
                    await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> UpdateContactSectionImageAsync(int userId, ContactSectionImageUpdateDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                bool hasAnyUpdates = false;

                if (request.BannerImage != null)
                {
                    var contactBannerSection = await _context.Sections
                        .Where(x => x.IsActive && x.Id == 23)
                        .FirstOrDefaultAsync();

                    if (contactBannerSection == null)
                    {
                        InitMessageResponse("NotFound");
                        return;
                    }

                    if (!string.IsNullOrEmpty(contactBannerSection.BackgroundImageRelativePath))
                    {
                        _fileService.DeleteFile(_uploadPath, contactBannerSection.BackgroundImageRelativePath);
                    }

                    var image = await _fileService.UploadAsync(_uploadPath, request.BannerImage);

                    contactBannerSection.BackgroundImageRelativePath = image;

                    _context.Sections.Update(contactBannerSection);
                    hasAnyUpdates = true;
                }

                if (hasAnyUpdates)
                    await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> UpdateManagementSectionImageAsync(int userId, ManagementSectionImageUpdateDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                bool hasAnyUpdates = false;

                if (request.BannerImage != null)
                {
                    var managementBannerSection = await _context.Sections
                        .Where(x => x.IsActive && x.Id == 24)
                        .FirstOrDefaultAsync();

                    if (managementBannerSection == null)
                    {
                        InitMessageResponse("NotFound");
                        return;
                    }

                    if (!string.IsNullOrEmpty(managementBannerSection.BackgroundImageRelativePath))
                    {
                        _fileService.DeleteFile(_uploadPath, managementBannerSection.BackgroundImageRelativePath);
                    }

                    var image = await _fileService.UploadAsync(_uploadPath, request.BannerImage);

                    managementBannerSection.BackgroundImageRelativePath = image;

                    _context.Sections.Update(managementBannerSection);
                    hasAnyUpdates = true;
                }

                if (request.FirstSectionImage != null)
                {
                    var managementFirstSection = await _context.Sections
                        .Where(x => x.IsActive && x.Id == 12)
                        .FirstOrDefaultAsync();

                    if (managementFirstSection == null)
                    {
                        InitMessageResponse("NotFound");
                        return;
                    }

                    if (!string.IsNullOrEmpty(managementFirstSection.SectionImageRelativePath))
                    {
                        _fileService.DeleteFile(_uploadPath, managementFirstSection.SectionImageRelativePath);
                    }

                    var image = await _fileService.UploadAsync(_uploadPath, request.FirstSectionImage);

                    managementFirstSection.SectionImageRelativePath = image;

                    _context.Sections.Update(managementFirstSection);
                    hasAnyUpdates = true;
                }

                if (request.SecondSectionImage != null)
                {
                    var managementSecondSection = await _context.Sections
                        .Where(x => x.IsActive && x.Id == 13)
                        .FirstOrDefaultAsync();

                    if (managementSecondSection == null)
                    {
                        InitMessageResponse("NotFound");
                        return;
                    }

                    if (!string.IsNullOrEmpty(managementSecondSection.SectionImageRelativePath))
                    {
                        _fileService.DeleteFile(_uploadPath, managementSecondSection.SectionImageRelativePath);
                    }

                    var image = await _fileService.UploadAsync(_uploadPath, request.SecondSectionImage);

                    managementSecondSection.SectionImageRelativePath = image;

                    _context.Sections.Update(managementSecondSection);
                    hasAnyUpdates = true;
                }

                if (request.ThirdSectionImage != null)
                {
                    var managementThirdSection = await _context.Sections
                        .Where(x => x.IsActive && x.Id == 15)
                        .FirstOrDefaultAsync();

                    if (managementThirdSection == null)
                    {
                        InitMessageResponse("NotFound");
                        return;
                    }

                    if (!string.IsNullOrEmpty(managementThirdSection.SectionImageRelativePath))
                    {
                        _fileService.DeleteFile(_uploadPath, managementThirdSection.SectionImageRelativePath);
                    }

                    var image = await _fileService.UploadAsync(_uploadPath, request.ThirdSectionImage);

                    managementThirdSection.SectionImageRelativePath = image;

                    _context.Sections.Update(managementThirdSection);
                    hasAnyUpdates = true;
                }

                if (hasAnyUpdates)
                    await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> UpdateVacancySectionImageAsync(int userId, VacancySectionImageUpdateDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                bool hasAnyUpdates = false;

                if (request.BannerImage != null)
                {
                    var managementBannerSection = await _context.Sections
                        .Where(x => x.IsActive && x.Id == 26)
                        .FirstOrDefaultAsync();

                    if (managementBannerSection == null)
                    {
                        InitMessageResponse("NotFound");
                        return;
                    }

                    if (!string.IsNullOrEmpty(managementBannerSection.BackgroundImageRelativePath))
                    {
                        _fileService.DeleteFile(_uploadPath, managementBannerSection.BackgroundImageRelativePath);
                    }

                    var image = await _fileService.UploadAsync(_uploadPath, request.BannerImage);

                    managementBannerSection.BackgroundImageRelativePath = image;

                    _context.Sections.Update(managementBannerSection);
                    hasAnyUpdates = true;
                }

                if (hasAnyUpdates)
                    await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> UpdatePartnerSectionImageAsync(int userId, PartnerSectionImageUpdateDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                bool hasAnyUpdates = false;

                if (request.BannerImage != null)
                {
                    var partnerBannerSection = await _context.Sections
                        .Where(x => x.IsActive && x.Id == 27)
                        .FirstOrDefaultAsync();

                    if (partnerBannerSection == null)
                    {
                        InitMessageResponse("NotFound");
                        return;
                    }

                    if (!string.IsNullOrEmpty(partnerBannerSection.BackgroundImageRelativePath))
                    {
                        _fileService.DeleteFile(_uploadPath, partnerBannerSection.BackgroundImageRelativePath);
                    }

                    var image = await _fileService.UploadAsync(_uploadPath, request.BannerImage);

                    partnerBannerSection.BackgroundImageRelativePath = image;

                    _context.Sections.Update(partnerBannerSection);
                    hasAnyUpdates = true;
                }

                if (hasAnyUpdates)
                    await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> UpdateSitemapSectionImageAsync(int userId, SitemapSectionImageUpdateDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                bool hasAnyUpdates = false;

                if (request.BannerImage != null)
                {
                    var sitemapBannerSection = await _context.Sections
                        .Where(x => x.IsActive && x.Id == 30)
                        .FirstOrDefaultAsync();

                    if (sitemapBannerSection == null)
                    {
                        InitMessageResponse("NotFound");
                        return;
                    }

                    if (!string.IsNullOrEmpty(sitemapBannerSection.BackgroundImageRelativePath))
                    {
                        _fileService.DeleteFile(_uploadPath, sitemapBannerSection.BackgroundImageRelativePath);
                    }

                    var image = await _fileService.UploadAsync(_uploadPath, request.BannerImage);

                    sitemapBannerSection.BackgroundImageRelativePath = image;

                    _context.Sections.Update(sitemapBannerSection);
                    hasAnyUpdates = true;
                }

                if (hasAnyUpdates)
                    await _context.SaveChangesAsync(userId);
            });
        }

        private async Task<BaseResponse<SectionsDTO>> GetAllSectionsAsync(int pageId)
        {
            return await HandleActionAsync(async () =>
            {
                var sections = await _context.Sections
                    .AsNoTracking()
                    .Where(x => x.PageId == pageId && x.IsActive)
                    .Select(section => new SectionDTO
                    {
                        Id = section.Id,
                        SectionName = section.SectionName,
                        Title = section.Title,
                        Description = section.Description,
                        ButtonText = section.ButtonText,
                        BackgroundImagePath = section.BackgroundImageRelativePath != null ? _configuration["ImageUrl"] + section.BackgroundImageRelativePath : null,
                        SectionImagePath = section.SectionImageRelativePath != null ? _configuration["ImageUrl"] + section.SectionImageRelativePath : null
                    })
                    .ToListAsync();

                if (sections == null || !sections.Any())
                {
                    InitMessageResponse("NotFound");
                    return null!;
                }

                return new SectionsDTO
                {
                    PageId = pageId,
                    Sections = sections
                };
            });
        }

        public async Task<BaseResponse<Task>> UpdateAsync(int userId, SectionsUpdateDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                if (request.PageId == (int)Page.Home || request.PageId == (int)Page.Contact)
                {
                    InitMessageResponse("BadRequest");
                    return;
                }

                var dbSections = await _context.Sections
                    .Where(x => x.PageId == request.PageId && x.IsActive)
                    .ToListAsync();

                foreach (var sectionDto in request.Sections)
                {
                    var dbSection = dbSections.FirstOrDefault(s => s.Id == sectionDto.Id);

                    if (dbSection == null)
                    {
                        InitMessageResponse("BadRequest");
                        return;
                    }

                    dbSection.SectionName = sectionDto.SectionName;
                    dbSection.Title = sectionDto.Title;
                    dbSection.Description = sectionDto.Description;
                    dbSection.ButtonText = sectionDto.ButtonText;

                    if (sectionDto.BackgroundImage != null)
                    {
                        if (!string.IsNullOrEmpty(dbSection.BackgroundImageRelativePath))
                        {
                            _fileService.DeleteFile(_uploadPath, dbSection.BackgroundImageRelativePath);
                        }

                        var image = await _fileService.UploadAsync(_uploadPath, sectionDto.BackgroundImage);

                        dbSection.BackgroundImageRelativePath = image;
                    }

                    if (sectionDto.SectionImage != null)
                    {
                        if (!string.IsNullOrEmpty(dbSection.SectionImageRelativePath))
                        {
                            _fileService.DeleteFile(_uploadPath, dbSection.SectionImageRelativePath);
                        }

                        var image = await _fileService.UploadAsync(_uploadPath, sectionDto.SectionImage);

                        dbSection.SectionImageRelativePath = image;
                    }

                    _context.Sections.Update(dbSection);
                }

                await _context.SaveChangesAsync(userId);
            });
        }
    }
}
