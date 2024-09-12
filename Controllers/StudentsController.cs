using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
					// Enable IDENTITY_INSERT
					await dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Students ON");

					var student = new Student
					{
						Id = viewModel.Id, // Explicit value for Id
						FirstName = viewModel.FirstName,
						MiddleName = viewModel.MiddleName,
						LastName = viewModel.LastName,
						Course = viewModel.Course,
						Remarks = viewModel.Remarks,
					};

					await dbContext.Students.AddAsync(student);
					await dbContext.SaveChangesAsync();


					// Commit the transaction
					await transaction.CommitAsync();
				}
				catch (Exception ex)
				{
					// Rollback if something goes wrong
					await transaction.RollbackAsync();
					throw;
				}
			}

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
