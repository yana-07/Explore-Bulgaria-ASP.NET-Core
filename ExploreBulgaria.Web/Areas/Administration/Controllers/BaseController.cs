using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ExploreBulgaria.Services.Constants.GlobalConstants;


namespace ExploreBulgaria.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = AdministratorRoleName)]
    [Area(AdministrationAreaName)]
    public class BaseController : Controller
    {
    }
}
