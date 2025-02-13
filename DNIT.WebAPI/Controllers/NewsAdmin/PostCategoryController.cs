using Microsoft.AspNetCore.Mvc;

namespace DNIT.WebAPI.Controllers.NewsAdmin
{
  public class PostCategoryController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}
