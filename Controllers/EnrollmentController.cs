using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models.Entities;

namespace StudentPortal.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public EnrollmentController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ValidateStudentId(int Id)
        {
            var student = dbContext.Students
               .Where(s => s.Id == Id)
               .Select(s => new
               {
                   Name = string.IsNullOrWhiteSpace(s.MiddleName) || s.MiddleName == "-"
                            ? $"{s.FirstName} {s.LastName}"
                            : $"{s.FirstName} {s.MiddleName} {s.LastName}",
                   s.Course,
                   s.Year
               })
               .FirstOrDefault();

            if (student == null)
            {
                return Json(new { success = false, message = "ID number not found" });
            }

            return Json(new { success = true, message = "ID number found" , dataObject = student });
        }
    }
}
