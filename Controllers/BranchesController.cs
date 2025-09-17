using Microsoft.AspNetCore.Mvc;

namespace QuanLySinhVien.Controllers
{
    public class BranchesController : Controller
    {
        public IActionResult List()
        {
            return View();
        }
    }
}
