using DSA_Marathon.Web.Data;
using DSA_Marathon.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ExamEntity = DSA_Marathon.Web.Data.Entities.Exam;

namespace DSA_Marathon.Web.Pages.Exam
{
    public class StartModel : PageModel
    {
        private readonly QuizDbContext _context;

        public StartModel(QuizDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Username { get; set; } = "";

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                ModelState.AddModelError("", "Vui lòng nhập tên của bạn!");
                return Page();
            }

            // ✅ Tạo bài thi mới
            var exam = new ExamEntity
            {
                Username = Username,
                StartTime = DateTime.Now
            };
            _context.Exams.Add(exam);
            await _context.SaveChangesAsync();

            // ✅ Lấy ngẫu nhiên 50 câu hỏi
            var randomQuestions = await _context.Questions
                .OrderBy(q => Guid.NewGuid())
                .Take(50)
                .Include(q => q.BloomLevel)
                .ToListAsync();

            // ✅ Lưu vào Session
            HttpContext.Session.SetInt32("ExamId", exam.Id);
            HttpContext.Session.SetString("ExamQuestions",
                System.Text.Json.JsonSerializer.Serialize(randomQuestions));
            HttpContext.Session.SetString("Username", Username);

            return RedirectToPage("/Student/DoExam");
        }
    }
}
