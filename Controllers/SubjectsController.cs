using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using StudentPortal.Models.Entities;
using System.Threading.Tasks;

namespace StudentPortal.Controllers
{
    [Authorize]
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
            ViewData["ControllerName"] = "Subjects";
            return View("AddSubjects");
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddSubjectWithScheduleViewModel viewModel)
        {
            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
					ViewData["RecentForm"] = "Subjects";
					var existingSubject = dbContext.Subjects.Find(viewModel.Code);
                    if (existingSubject != null) {
                        ViewBag.AlertMessage = "Subject already exists!";
						return View("AddSubjects", viewModel);
					}

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

                    if (!string.IsNullOrWhiteSpace(viewModel.PreCategory) || !string.IsNullOrWhiteSpace(viewModel.PreCode))
                    {
                        var prerequisite = new SubjectPreq
                        {
                            SubjectCode = viewModel.Code,
                            PreCode = viewModel.PreCode,
                            Category = viewModel.PreCategory,
                        };
                        await dbContext.Prerequisites.AddAsync(prerequisite);
                        await dbContext.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
			ModelState.Clear();
			ViewBag.AlertMessage = "Subject successfully added!";
			return View("AddSubjects"); 
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var subjects = await dbContext.Subjects
                                        .Include(s => s.Prerequisites)
                                        .ToListAsync();

            return View(subjects);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            var subject = await dbContext.Subjects
                                 .Include(s => s.Prerequisites) 
                                 .FirstOrDefaultAsync(s => s.Code == Id);

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
            var subject = await dbContext.Subjects
                                         .Include(s => s.Prerequisites)
                                         .FirstOrDefaultAsync(s => s.Code == viewModel.Code);

            if (subject != null)
            {
                subject.Description = viewModel.Description;
                subject.Units = viewModel.Units;
                subject.Course = viewModel.Course;
                subject.Offering = viewModel.Offering;
                subject.Category = viewModel.Category;
                subject.Curriculum = viewModel.Curriculum;
                subject.Status = viewModel.Status;

                if (!string.IsNullOrWhiteSpace(viewModel.Prerequisites.PreCode) || !string.IsNullOrWhiteSpace(viewModel.Prerequisites.Category))
                {
                    if (subject.Prerequisites != null &&
                        subject.Prerequisites.PreCode != viewModel.Prerequisites.PreCode)
                    {
                        dbContext.Prerequisites.Remove(subject.Prerequisites);

                        subject.Prerequisites = new SubjectPreq
                        {
                            SubjectCode = viewModel.Code,
                            PreCode = viewModel.Prerequisites.PreCode,
                            Category = viewModel.Prerequisites.Category
                        };
                    }
                    else if (subject.Prerequisites == null)
                    {
                        subject.Prerequisites = new SubjectPreq
                        {
                            SubjectCode = viewModel.Code,
                            PreCode = viewModel.Prerequisites.PreCode,
                            Category = viewModel.Prerequisites.Category
                        };
                    }
                    else
                    {
                        subject.Prerequisites.Category = viewModel.Prerequisites.Category;
                    }
                }
                else
                {
                    if (subject.Prerequisites != null)
                    {
                        dbContext.Prerequisites.Remove(subject.Prerequisites);
                        subject.Prerequisites = null;
                    }
                }

                await dbContext.SaveChangesAsync();
                return RedirectToAction("List", "Subjects");
            }


            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Subject viewModel)
        {
            var subject = await dbContext.Subjects
                                 .Include(s => s.Prerequisites)
                                 .FirstOrDefaultAsync(x => x.Code == viewModel.Code);

            if (subject is not null)
            {
                if (subject.Prerequisites != null)
                {
                    dbContext.Prerequisites.Remove(subject.Prerequisites);
                }

                dbContext.Subjects.Remove(viewModel);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Subjects");
        }
    }
}
