using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Controllers
{
    public class StudentController : Controller
    {
        private List<Student> listStudents = new List<Student>();
        public StudentController()
        {
            listStudents = new List<Student>()
            {
                new Student() { Id = 101, Name = "Hải Nam", Branch = Branch.IT,
                    Gender = Gender.Male, IsRegular = true,
                    Address = "A1-2018", Email = "nam@g.com" },
                new Student() { Id = 102, Name = "Minh Tú",
                    Gender = Gender.Female, IsRegular = true, Branch = Branch.BE,
                    Address = "A1-2019", Email = "tu@g.com" },
                new Student() { Id = 103, Name = "Hoàng Phong", Branch = Branch.CE,
                    Gender = Gender.Male, IsRegular = false,
                    Address = "A1-2020", Email = "phong@g.com" },
                new Student() { Id = 104, Name = "Xuân Mai", Branch = Branch.EE,
                    Gender = Gender.Female, IsRegular = false,
                    Address = "A1-2021", Email = "mai@g.com" }
            };
        }

        [Route("Admin/Student/List")]
        public IActionResult Index()
        {
            return View(listStudents);
        }

        [Route("Admin/Student/Add", Name = "AddGet")]
        [HttpGet]
        public IActionResult Create()
        {
            //lấy danh sách các giá trị Gender để hiển thị radio button trên form
            ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList(); 
            
            //lấy dạnh sách các giá trị Branch để hiển thị select-option trên form
            //Để hiển thị select-option trên View cần dùng List<SelectListItem>
            ViewBag.AllBranches = new List<SelectListItem>()
            {
                new SelectListItem { Text = "IT", Value = "1" },
                new SelectListItem { Text = "BE", Value = "2" },
                new SelectListItem { Text = "CE", Value = "3" },
                new SelectListItem { Text = "EE", Value = "4" }
            };
            return View();
        }
        [Route("Admin/Student/Add", Name = "AddPost")]
        [HttpPost]
        public async Task<IActionResult> Create(Student s, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("imageFile", "Only JPG, JPEG, PNG, and GIF files are allowed.");
                }
                
                // Validate file size (5MB limit)
                if (imageFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("imageFile", "File size cannot exceed 5MB.");
                }
                
                if (ModelState.IsValid)
                {
                    // Generate unique filename
                    var fileName = Guid.NewGuid().ToString() + fileExtension;
                    var filePath = Path.Combine("wwwroot/uploads", fileName);
                    
                    // Save file
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    
                    // Set image path in student object
                    s.ImagePath = "/uploads/" + fileName;
                }
            }
            
            if (ModelState.IsValid)
            {
                s.Id = listStudents.Last<Student>().Id + 1;
                listStudents.Add(s);
                return View("Index", listStudents);
            }
            else
            {
                // Return to create view with validation errors
                ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
                ViewBag.AllBranches = new List<SelectListItem>()
                {
                    new SelectListItem { Text = "IT", Value = "1" },
                    new SelectListItem { Text = "BE", Value = "2" },
                    new SelectListItem { Text = "CE", Value = "3" },
                    new SelectListItem { Text = "EE", Value = "4" }
                };
                return View(s);
            }
        }
    }
}
