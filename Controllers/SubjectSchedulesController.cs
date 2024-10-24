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
					ViewData["RecentForm"] = "SubjectSchedules";
					var existingSchedule = dbContext.SubjectSchedules.Find(viewModel.EDPCode);
					if (existingSchedule != null)
					{
						ViewBag.AlertMessage = "EDP Code already exists!";
						return View("AddSubjects", viewModel);
					}
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
            ModelState.Clear();
			ViewBag.AlertMessage = "Schedule successfully added!";
			return View("AddSubjects");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var subjectSchedules = await dbContext.SubjectSchedules.ToListAsync();
            return View(subjectSchedules);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            var subjectSchedule = await dbContext.SubjectSchedules.FindAsync(Id);

            if (subjectSchedule == null)
            {
                return NotFound();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Edit", subjectSchedule);
            }

            return View(subjectSchedule);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SubjectSchedule viewModel)
        {
            var subjectSchedule = await dbContext.SubjectSchedules.FindAsync(viewModel.EDPCode);

            if (subjectSchedule is not null)
            {
                subjectSchedule.EDPCode = viewModel.EDPCode;
                subjectSchedule.SubjectCode = viewModel.SubjectCode;
                subjectSchedule.StartTime = viewModel.StartTime;
                subjectSchedule.EndTime = viewModel.EndTime;
                subjectSchedule.Days = viewModel.Days;
                subjectSchedule.Room = viewModel.Room;
                subjectSchedule.MaxSize = viewModel.MaxSize;
                subjectSchedule.ClassSize = viewModel.ClassSize;
                subjectSchedule.Status = viewModel.Status;
                subjectSchedule.XM = viewModel.XM;
                subjectSchedule.Section = viewModel.Section;
                subjectSchedule.Year = viewModel.Year;

                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "SubjectSchedules");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SubjectSchedule viewModel)
        {
            var subjectSchedule = await dbContext.SubjectSchedules
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.EDPCode == viewModel.EDPCode);

            if (subjectSchedule is not null)
            {
                dbContext.SubjectSchedules.Remove(viewModel);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "SubjectSchedules");
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
