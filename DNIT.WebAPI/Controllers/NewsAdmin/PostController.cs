using Microsoft.AspNetCore.Mvc;

namespace DNIT.WebAPI.Controllers.NewsAdmin
{
  public class PostController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}
