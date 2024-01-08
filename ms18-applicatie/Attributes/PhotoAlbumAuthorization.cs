using Maasgroep.SharedKernel.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ms18_applicatie.Attributes;

public class PhotoAlbumAuthorization : Attribute, IAuthorizationFilter
{
    private readonly string _permission;

    public PhotoAlbumAuthorization(string permission = "")
    {
        _permission = permission;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.Items["CurrentUser"] as MemberModel;
        if (user == null || (!user.Permissions.Contains("admin") && !user.Permissions.Contains(_permission)))
        {
            context.Result = new ForbidResult();
        }
    }
}
