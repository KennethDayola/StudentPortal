using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentPortal.Data;
using StudentPortal.Models;
using StudentPortal.Models.Entities;
using System.Runtime.InteropServices;

namespace StudentPortal.Controllers
{
	public class StudentsController : Controller
	{
		private readonly ApplicationDbContext dbContext;

		public StudentsController(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		[HttpGet]
		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddStudentViewModel viewModel)
		{
			using (var transaction = await dbContext.Database.BeginTransactionAsync())
			{
				try
				{
					await dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Students ON");

					var existingStudent = dbContext.Students.Find(viewModel.Id);
					if (existingStudent != null)
					{
						ViewBag.AlertMessage = "Student ID already exists!";
						return View(viewModel);
					}

					var student = new Student
					{
						Id = viewModel.Id, 
						FirstName = viewModel.FirstName,
                        MiddleName = string.IsNullOrWhiteSpace(viewModel.MiddleName) ? "-" : viewModel.MiddleName,
                        LastName = viewModel.LastName,
						Course = viewModel.Course,
						Remarks = viewModel.Remarks,
						Year = viewModel.Year,
					};

					await dbContext.Students.AddAsync(student);
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
			ViewBag.AlertMessage = "You have been successfully enrolled!";
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> List()
		{
			var students = await dbContext.Students.ToListAsync();

			return View(students);
		}

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var student = await dbContext.Students.FindAsync(Id);

            if (student == null)
            {
                return NotFound();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Edit", student);
            }

            return View(student);
        }

        [HttpPost]
		public async Task<IActionResult> Edit(Student viewModel)
		{
			var student = await dbContext.Students.FindAsync(viewModel.Id);

			if (student is not null)
			{
				student.FirstName = viewModel.FirstName;
				student.MiddleName = viewModel.MiddleName;
				student.LastName = viewModel.LastName;
				student.Course = viewModel.Course;
				student.Remarks = viewModel.Remarks;
                student.Year = viewModel.Year;
                student.Status = viewModel.Status;

				await dbContext.SaveChangesAsync();
			}

			return RedirectToAction("List", "Students");
		}

		[HttpPost]
		public async Task<IActionResult> Delete(Student viewModel)
		{
			var student = await dbContext.Students.
				AsNoTracking().
				FirstOrDefaultAsync(x => x.Id == viewModel.Id);

			if (student is not null)
			{
				dbContext.Students.Remove(viewModel);
				await dbContext.SaveChangesAsync();
			}

			return RedirectToAction("List", "Students");
		}
	}
}
