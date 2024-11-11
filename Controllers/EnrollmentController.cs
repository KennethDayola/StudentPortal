using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using StudentPortal.Models.Entities;
using System.Reflection.Emit;

namespace StudentPortal.Controllers
{
    [Authorize]
    public class EnrollmentController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public EnrollmentController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var enrollmentHeaders = await dbContext.EnrollmentHeaders.ToListAsync();
            return View(enrollmentHeaders);
        }

        [HttpGet]
        public async Task<IActionResult> DetailList(int studId = 23239916)
        {
            var detailList = await dbContext.EnrollmentDetails
                .Include(e => e.EnrollmentHeader)
                .ThenInclude(eh => eh.Student)                  
                .Include(e => e.SubjectSchedule)
                .ThenInclude(s => s.Subject)
                .Where(e => e.StudId == studId)
                .ToListAsync();

            if (!detailList.Any())
            {
                return NotFound();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("DetailList", detailList);
            }

            return View(detailList);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(string edpCode, string newStatus)
        {
            var detail = await dbContext.EnrollmentDetails
                .FirstOrDefaultAsync(e => e.EDPCode == edpCode);

            if (detail == null)
            {
                return NotFound();
            }

            detail.Status = newStatus;
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DetailDelete(int studentId, string edpCode, int units)
        {
            var enrollmentDetail = await dbContext.EnrollmentDetails
                .Include(e => e.EnrollmentHeader)
                .FirstOrDefaultAsync(e => e.StudId == studentId && e.EDPCode == edpCode);

            if (enrollmentDetail == null)
            {
                return NotFound();
            }

            enrollmentDetail.EnrollmentHeader.TotalUnits -= units;
            await dbContext.SaveChangesAsync();

            dbContext.EnrollmentDetails.Remove(enrollmentDetail);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var enrollmentHeader = await dbContext.EnrollmentHeaders.FindAsync(Id);

            if (enrollmentHeader == null)
            {
                return NotFound();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Edit", enrollmentHeader);
            }

            return View(enrollmentHeader);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(EnrollmentHeader viewModel)
        {
            var enrollmentHeader = await dbContext.EnrollmentHeaders.FindAsync(viewModel.StudId);

            if (enrollmentHeader is not null)
            {
                enrollmentHeader.EnrollDate = viewModel.EnrollDate;
                enrollmentHeader.SchoolYear = viewModel.SchoolYear;
                enrollmentHeader.Encoder = viewModel.Encoder;
                enrollmentHeader.TotalUnits = viewModel.TotalUnits;
                enrollmentHeader.Status = viewModel.Status;

                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Enrollment");
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(EnrollmentHeader enrollmentHeader)
        {
            enrollmentHeader.Status = "EN";
            foreach (var detail in enrollmentHeader.EnrollmentDetails)
            {
                detail.Status = "WI";
                detail.StudId = enrollmentHeader.StudId;
            }
            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var existingHeader = await dbContext.EnrollmentHeaders
                        .FirstOrDefaultAsync(h => h.StudId == enrollmentHeader.StudId);

                    if (existingHeader == null)
                    {
                        await dbContext.EnrollmentHeaders.AddAsync(enrollmentHeader);
                        await dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        existingHeader.EnrollDate = enrollmentHeader.EnrollDate;
                        existingHeader.SchoolYear = enrollmentHeader.SchoolYear;
                        existingHeader.Encoder = enrollmentHeader.Encoder;
                        existingHeader.TotalUnits += enrollmentHeader.TotalUnits;
                        existingHeader.Status = "EN"; 
                        dbContext.EnrollmentHeaders.Update(existingHeader);
                    }

                    foreach (var detail in enrollmentHeader.EnrollmentDetails)
                    {
                        var existingDetail = await dbContext.EnrollmentDetails
                            .FirstOrDefaultAsync(d => d.StudId == detail.StudId && d.EDPCode == detail.EDPCode);

                        if (existingDetail == null) 
                        {
                            await dbContext.EnrollmentDetails.AddAsync(detail);

                            var existingSubjectSchedule = await dbContext.SubjectSchedules
                                .FirstOrDefaultAsync(s => s.EDPCode == detail.EDPCode);

                            if (existingSubjectSchedule != null)
                            {
                                existingSubjectSchedule.ClassSize += 1;
                                dbContext.SubjectSchedules.Update(existingSubjectSchedule);
                            }
                        }
                    }

                    await dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();

                    ModelState.Clear();
                    ViewBag.AlertMessage = "You have been enrolled successfully!";
                    return View();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                    Exception inner = ex.InnerException;
                    while (inner != null)
                    {
                        Console.WriteLine("Inner exception: " + inner.Message);
                        Console.WriteLine("Inner stack trace: " + inner.StackTrace);
                        inner = inner.InnerException;
                    }

                    await transaction.RollbackAsync();
                    ViewBag.AlertMessage = ex.Message;
                    ViewBag.ExceptionMsg = ex.Message + (ex.InnerException?.Message ?? "");
                    return View();
                }
            }
        }

        [HttpPost]
        public JsonResult ValidateSubjectAdditions([FromBody] List<string> enrollmentDetails)
        {
            bool isDuplicateFound = false;
            List<string> duplicateEntries = new List<string>();

            bool requisiteMet = true;
            string requisiteMessage = "You do not have the ";

            for (int i = 0; i < enrollmentDetails.Count; i += 3)
            {
                string edpCode = enrollmentDetails[i];

                int studId = int.Parse(enrollmentDetails[i + 1]);

                string subjCode = enrollmentDetails[i + 2];

                var existingEntry = dbContext.EnrollmentDetails
                    .FirstOrDefault(e => e.StudId == studId && e.EDPCode == edpCode);

                if (existingEntry != null)
                {
                    isDuplicateFound = true;
                    duplicateEntries.Add(edpCode);
                }

                var subject = dbContext.Subjects
                    .Include(s => s.Prerequisites)
                    .FirstOrDefault(s => s.Code == subjCode);

                if (subject != null && subject.Prerequisites != null)
                {
                    if (subject.Prerequisites.Category == "PR")
                    {
                        requisiteMet = dbContext.EnrollmentDetails
                            .Any(e => e.StudId == studId && e.SubjectCode == subject.Prerequisites.PreCode);

                        if (!requisiteMet)
                        {
                            requisiteMessage += "Pre-Requisite subject for " + subjCode + ": " + subject.Prerequisites.PreCode;
                            break;
                        }
                    }
                    else if (subject.Prerequisites.Category == "CR")
                    {
                        requisiteMet = enrollmentDetails
                           .Where((e, index) => index % 3 == 2)
                           .Any(e => e == subject.Prerequisites.PreCode);

                        if (!requisiteMet)
                        {
                            requisiteMessage += "Co-Requisite subject for " + subjCode + ": " + subject.Prerequisites.PreCode;
                            break;
                        }
                    }
                }
            }

            if (isDuplicateFound)
            {
                return Json(new { success = false, message = "You are already enrolled in the following EDP codes: " + string.Join(", ", duplicateEntries) });
            }

            if (!requisiteMet)
            {
                return Json(new { success = false, message = requisiteMessage });
            }

            return Json(new { success = true });
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

        [HttpGet]
        public JsonResult ValidateEDPCode(string edpCode)
        {
            var subjectSchedule = dbContext.SubjectSchedules
                 .Where(ss => ss.EDPCode == edpCode)
                 .Select(ss => new
                 {
                     ss.EDPCode,
                     ss.SubjectCode,
                     ss.StartTime,
                     ss.EndTime,
                     ss.Days,
                     ss.Room,
                     ss.MaxSize,
                     ss.ClassSize
                 })
                 .FirstOrDefault();

            if (subjectSchedule == null)
            {
                return Json(new { success = false, message = "EDP Code not found" });
            }

            var subject = dbContext.Subjects
                .Where(s => s.Code == subjectSchedule.SubjectCode)
                .Select(s => new
                {
                    s.Units
                })
                .FirstOrDefault();

            if (subject == null)
            {
                return Json(new { success = false, message = "Units of EDP Code not found" });
            }

            return Json(new
            {
                success = true,
                message = "EDP Code found successfully",
                dataObject = new
                {
                    subjectSchedule,
                    Units = subject.Units
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EnrollmentHeader viewModel)
        {
            var enrollment = await dbContext.EnrollmentHeaders
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.StudId == viewModel.StudId);

            if (enrollment is not null)
            {
                dbContext.EnrollmentHeaders.Remove(viewModel);
                await dbContext.SaveChangesAsync();
                ViewBag.AlertMessage = "Enrollment record has been successfully deleted!";
            }

            return RedirectToAction("List", "Enrollment");
        }
    }
}
