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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddSubjectViewModel viewModel)
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
                        Status = viewModel.Status,
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

            return View(); 
        }
    }
}
