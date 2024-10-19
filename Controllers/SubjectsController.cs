using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using StudentPortal.Models.Entities;
using System.Threading.Tasks;

namespace StudentPortal.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public SubjectsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View("AddSubjects");
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddSubjectWithScheduleViewModel viewModel)
        {
            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var subject = new Subject
                    {
                        Code = viewModel.Code,
                        Description = viewModel.Description,
                        Units = viewModel.Units,
                        Offering = viewModel.Offering,
                        Category = viewModel.Category,
                        Status = "AC",
                        Course = viewModel.Course,
                        Curriculum = viewModel.Curriculum
                    };

                    await dbContext.Subjects.AddAsync(subject);
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

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var subjects = await dbContext.Subjects.ToListAsync();

            return View(subjects);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            var subject = await dbContext.Subjects.FindAsync(Id);

            if (subject == null)
            {
                return NotFound();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Edit", subject);
            }

            return View(subject);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Subject viewModel)
        {
            var subject = await dbContext.Subjects.FindAsync(viewModel.Code);

            if (subject is not null)
            {
                subject.Code = viewModel.Code;
                subject.Description = viewModel.Description;
                subject.Units = viewModel.Units;
                subject.Course = viewModel.Course;
                subject.Offering = viewModel.Offering;
                subject.Category = viewModel.Category;
                subject.Curriculum = viewModel.Curriculum;
                subject.Status = viewModel.Status;

                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Subjects");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Subject viewModel)
        {
            var subject = await dbContext.Subjects.
                AsNoTracking().
                FirstOrDefaultAsync(x => x.Code == viewModel.Code);

            if (subject is not null)
            {
                dbContext.Subjects.Remove(viewModel);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Subjects");
        }
    }
}
