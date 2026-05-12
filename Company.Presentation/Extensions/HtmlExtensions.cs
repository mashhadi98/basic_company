using Microsoft.AspNetCore.Mvc.Rendering;

public static class HtmlExtensions
{
    public static string IsActive(this IHtmlHelper html, string controller, string action = null)
    {
        var currentController = html.ViewContext.RouteData.Values["controller"]?.ToString();
        var currentAction = html.ViewContext.RouteData.Values["action"]?.ToString();

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

    public static string IsMenuOpen(this IHtmlHelper html, string controller)
    {
        var currentController = html.ViewContext.RouteData.Values["controller"]?.ToString();
        return currentController == controller ? "menu-open" : "";
    }
}
