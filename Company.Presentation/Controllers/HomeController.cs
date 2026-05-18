using Company.Application.Abstractions;
using Company.Application.Features.Blog.Queries;
using Company.Application.Features.CompanyFeatures.Queries;
using Company.Application.Features.Customers.Queries;
using Company.Application.Features.Products.Queries;
using Company.Application.Features.SiteSettings.Queries;
using Company.Application.Features.StaticPages.Queries;
using Company.Domain.Entities;
using Company.Infrastructure.Persistence;
using Company.Presentation.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache;
        private readonly IAppInfoService _appInfo;
        private readonly AppDbContext _db;
        private const string HomePageCacheKey = "HomePageViewModel";

        public HomeController(IMediator mediator, IMemoryCache cache, IAppInfoService appInfo, AppDbContext db)
        {
            _mediator = mediator;
            _cache = cache;
            _appInfo = appInfo;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _cache.GetOrCreateAsync(HomePageCacheKey, async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(30);
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2);
                return await BuildHomeViewModelAsync();
            });

            model.ApplicationName = _appInfo.ApplicationName;
            var successMessage = TempData?[
                "HomeOrderSuccess"] as string;
            model.OrderSuccessMessage = successMessage ?? string.Empty;
            model.OrderRequest = new OrderRequestViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitOrder(OrderRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = await BuildHomeViewModelAsync();
                viewModel.ApplicationName = _appInfo.ApplicationName;
                viewModel.OrderRequest = model;
                return View("Index", viewModel);
            }

            var order = new OrderRequest
            {
                PhoneNumber = model.PhoneNumber,
                FullName = model.FullName,
                Description = model.Description,
                Status = OrderRequestStatus.Registered,
                CreatedAt = DateTime.UtcNow
            };

            _db.OrderRequests.Add(order);
            await _db.SaveChangesAsync();

            TempData["HomeOrderSuccess"] = "سپاس از ثبت سفارش شما. سفارش شما با موفقیت در سیستم ما ثبت گردید. پشتیبان‌های ما در اولین فرصت با شما تماس خواهند گرفت.\nدر صورت نیاز فوری با شماره ۰۲۱-۴۴۹۸۷۶۵۴ تماس بگیرید.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<HomeViewModel> BuildHomeViewModelAsync()
        {
            var settings = await _mediator.Send(new GetSiteSettingsQuery { IsPublished = true });
            var features = await _mediator.Send(new GetCompanyFeaturesQuery { IsPublished = true, Take = 10 });
            var customers = await _mediator.Send(new GetCustomersQuery { IsPublished = true, Take = 20 });
            var products = await _mediator.Send(new GetProductsQuery { IsPublished = true, Take = 12 });
            var blogPosts = await _mediator.Send(new GetBlogPostsQuery { IsPublished = true, Take = 3 });
            var aboutPage = await _mediator.Send(new GetStaticPageByKeyQuery { Key = "aboutus" })
                            ?? await _mediator.Send(new GetStaticPageByKeyQuery { Key = "about-us" });

            var model = new HomeViewModel
            {
                Header = new HomeViewModel.HeaderSection
                {
                    LogoUrl = GetSettingValue(settings, "HeaderLogo", "/images/no-image.png"),
                    Texts = new List<string>
                    {
                        GetSettingValue(settings, "HeaderText1"),
                        GetSettingValue(settings, "HeaderText2"),
                        GetSettingValue(settings, "HeaderText3")
                    }
                },
                Services = features.OrderBy(f => f.SortOrder).ToList(),
                ServicesDescription = GetSettingValue(settings, "ServicesSectionDescription"),
                AboutUs = new HomeViewModel.AboutUsSection
                {
                    Page = aboutPage,
                    ImageUrl = aboutPage?.Image ?? GetSettingValue(settings, "AboutUsImage", "/images/no-image.png"),
                    Statistics = BuildStatistics(settings)
                },
                FeaturedProducts = products
                    .Where(p => p.IsFeatured)
                    .OrderBy(p => p.SortOrder)
                    .Take(8)
                    .ToList(),
                ProductsDescription = GetSettingValue(settings, "ProductsSectionDescription"),
                Customers = customers.OrderBy(c => c.SortOrder).ToList(),
                CustomersDescription = GetSettingValue(settings, "CustomersSectionDescription"),
                LatestBlogPosts = blogPosts.OrderByDescending(p => p.PublishedDate).ToList(),
                BlogDescription = GetSettingValue(settings, "BlogSectionDescription"),
                OrderRequestDescription = GetSettingValue(settings, "OrderRequestDescription"),
                Footer = new HomeViewModel.FooterSection
                {
                    CompanyName = GetSettingValue(settings, "FooterCompanyName"),
                    Description = GetSettingValue(settings, "FooterDescription"),
                    SocialLinks = ParseCommaSeparatedLinks(GetSettingValue(settings, "FooterSocialLinks")),
                    FactoryAddress = GetSettingValue(settings, "FooterFactoryAddress"),
                    Phone = GetSettingValue(settings, "FooterPhone")
                }
            };

            return model;
        }

        private static string GetSettingValue(IEnumerable<Company.Application.Features.SiteSettings.DTOs.SiteSettingDto> settings, string key, string defaultValue = "")
        {
            return settings.FirstOrDefault(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase))?.Value?.Trim() ?? defaultValue;
        }

        private static List<HomeViewModel.StatisticItem> BuildStatistics(IEnumerable<Company.Application.Features.SiteSettings.DTOs.SiteSettingDto> settings)
        {
            return Enumerable.Range(1, 4)
                .Select(i => new HomeViewModel.StatisticItem
                {
                    Title = GetSettingValue(settings, $"AboutStat{i}Title"),
                    Value = GetSettingValue(settings, $"AboutStat{i}Value")
                })
                .Where(x => !string.IsNullOrEmpty(x.Title) || !string.IsNullOrEmpty(x.Value))
                .ToList();
        }

        private static List<string> ParseCommaSeparatedLinks(string rawLinks)
        {
            return rawLinks?.Split(new[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList() ?? new List<string>();
        }
    }
}
