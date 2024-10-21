using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using StudentPortal.Models.Entities;
using System.Threading.Tasks;

namespace StudentPortal.Controllers
{
    public class SubjectSchedulesController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public SubjectSchedulesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewData["ControllerName"] = "SubjectSchedules";
            return View("AddSubjects");
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddSubjectWithScheduleViewModel viewModel)
        {
            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var subjectSchedule = new SubjectSchedule
                    {
                        EDPCode = viewModel.EDPCode,
                        SubjectCode = viewModel.SubjectCode,
                        StartTime = viewModel.StartTime,
                        EndTime = viewModel.EndTime,
                        Days = viewModel.Days,
                        Room = viewModel.Room,
                        MaxSize = viewModel.MaxSize,
                        ClassSize = viewModel.ClassSize,
                        Status = "AC",
                        XM = viewModel.XM,
                        Section = viewModel.Section,
                        Year = viewModel.Year,
                    };

                    await dbContext.SubjectSchedules.AddAsync(subjectSchedule);
                    await dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            return View("AddSubjects");
        }

        // Fetch subject codes for autocomplete
        [HttpGet]
        public JsonResult GetSubjectCodes(string term)
        {
            var subjectCodes = dbContext.Subjects
                .Where(s => s.Code.StartsWith(term))
                .Select(s => s.Code)
                .Take(4) // Limit the result to 4
                .ToList();

            return Json(subjectCodes);
        }

        // Validate if the subject code exists
        [HttpGet]
        public JsonResult ValidateSubjectCode(string code)
        {
            bool exists = dbContext.Subjects.Any(s => s.Code == code);
            return Json(exists);
        }
    }
}
