using Company.Application.Features.Blog.DTOs;
using Company.Application.Features.CompanyFeatures.DTOs;
using Company.Application.Features.Customers.DTOs;
using Company.Application.Features.Products.DTOs;
using Company.Application.Features.StaticPages.DTOs;

namespace Company.Presentation.Models;

public class HomeViewModel
{
    public string ApplicationName { get; set; } = string.Empty;
    public HeaderSection Header { get; set; } = new();
    public List<CompanyFeatureDto> Services { get; set; } = new();
    public string ServicesDescription { get; set; } = string.Empty;
    public AboutUsSection AboutUs { get; set; } = new();
    public List<ProductDto> FeaturedProducts { get; set; } = new();
    public string ProductsDescription { get; set; } = string.Empty;
    public List<CustomerDto> Customers { get; set; } = new();
    public string CustomersDescription { get; set; } = string.Empty;
    public List<BlogPostDto> LatestBlogPosts { get; set; } = new();
    public string BlogDescription { get; set; } = string.Empty;
    public string OrderRequestDescription { get; set; } = string.Empty;
    public OrderRequestViewModel OrderRequest { get; set; } = new();
    public string OrderSuccessMessage { get; set; } = string.Empty;
    public FooterSection Footer { get; set; } = new();

    public class HeaderSection
    {
        public string LogoUrl { get; set; } = "/images/no-image.png";
        public List<string> Texts { get; set; } = new();
    }

    public class AboutUsSection
    {
        public StaticPageDto? Page { get; set; }
        public string ImageUrl { get; set; } = "/images/no-image.png";
        public List<StatisticItem> Statistics { get; set; } = new();
    }

    public class StatisticItem
    {
        public string Title { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

    public class FooterSection
    {
        public string CompanyName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> SocialLinks { get; set; } = new();
        public string FactoryAddress { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
