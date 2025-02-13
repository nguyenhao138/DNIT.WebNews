using Microsoft.AspNetCore.Mvc;

namespace DNIT.WebNews.Controllers
{
  public class PostListController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}
