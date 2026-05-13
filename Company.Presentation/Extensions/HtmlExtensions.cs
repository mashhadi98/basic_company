using Microsoft.AspNetCore.Mvc.Rendering;

public static class HtmlExtensions
{
    public static string IsActive(this IHtmlHelper html, string controller, string? action = null)
    {
        string? currentController = html.ViewContext.RouteData.Values["controller"]?.ToString();
        string? currentAction = html.ViewContext.RouteData.Values["action"]?.ToString();

        if (string.IsNullOrEmpty(action))
            return currentController == controller ? "active" : "";

        // برای Edit و Details هم منوی لیست فعال شود
        if (action.Equals("Edit", StringComparison.OrdinalIgnoreCase) ||
            action.Equals("Details", StringComparison.OrdinalIgnoreCase))
        {
            return currentController == controller ? "active" : "";
        }

        return currentController == controller && currentAction == action ? "active" : "";
    }

    public static string IsMenuOpen(this IHtmlHelper html, params string[] controllers)
    {
        var currentController = html.ViewContext.RouteData.Values["controller"]?.ToString();
        return controllers.Any(controller => string.Equals(currentController, controller, StringComparison.OrdinalIgnoreCase)) ? "menu-open" : "";
    }
}
