using Microsoft.AspNetCore.Mvc;

namespace DNIT.WebAPI.Controllers.Account
{
  public class PostListController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}
