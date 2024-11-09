using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
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
                        existingHeader.TotalUnits = enrollmentHeader.TotalUnits;
                        existingHeader.Status = "EN"; 
                        dbContext.EnrollmentHeaders.Update(existingHeader);
                    }

                    await dbContext.EnrollmentDetails.AddRangeAsync(enrollmentHeader.EnrollmentDetails);
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

                    await transaction.RollbackAsync();
                    ViewBag.AlertMessage = ex.Message;
                    return View(); 
                }
            }
        }

        [HttpPost]
        public JsonResult IsStudentInEdpCode([FromBody] List<string> enrollmentDetails)
        {
            bool isDuplicateFound = false;
            List<string> duplicateEntries = new List<string>();

            for (int i = 0; i < enrollmentDetails.Count; i += 2)
            {
                string edpCode = enrollmentDetails[i];

                int studId = int.Parse(enrollmentDetails[i + 1]);

                var existingEntry = dbContext.EnrollmentDetails
                    .FirstOrDefault(e => e.StudId == studId && e.EDPCode == edpCode);

                if (existingEntry != null)
                {
                    isDuplicateFound = true;
                    duplicateEntries.Add(edpCode);
                }
            }

            if (isDuplicateFound)
            {
                return Json(new { success = false, message = "You are already enrolled in the following EDP codes: " + string.Join(", ", duplicateEntries) });
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
    }
}
