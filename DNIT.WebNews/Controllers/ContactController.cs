using Microsoft.AspNetCore.Mvc;

namespace DNIT.WebNews.Controllers
{
  public class ContactController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
    public IActionResult About()
    {
      return View();
    }
   
  }
}
