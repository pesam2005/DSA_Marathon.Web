using DSA_Marathon.Web.Data;
using DSA_Marathon.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DSA_Marathon.Web.Pages.Student
{
    public class DoExamModel : PageModel
    {
        private readonly QuizDbContext _context;
        public DoExamModel(QuizDbContext context)
        {
            _context = context;
        }

        public List<Question> Questions { get; set; } = new();

        [BindProperty]
        public Dictionary<int, int> SelectedAnswers { get; set; } = new(); // QuestionId → AnswerId

        public async Task OnGetAsync()
        {
            // ✅ Lấy ngẫu nhiên 50 câu hỏi (hoặc ít hơn nếu DB < 50)
            Questions = await _context.Questions
                .Include(q => q.Answers)
                .OrderBy(q => Guid.NewGuid())
                .Take(50)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var examId = HttpContext.Session.GetInt32("ExamId");
            if (examId == null)
                return RedirectToPage("/Student/Start");

            var exam = await _context.Exams
                .Include(e => e.ExamResults)
                .FirstOrDefaultAsync(e => e.Id == examId);

            if (exam == null)
                return RedirectToPage("/Student/Start");

            // ✅ Lấy lại danh sách câu hỏi từ session
            var json = HttpContext.Session.GetString("ExamQuestions");
            var questions = JsonSerializer.Deserialize<List<Question>>(json) ?? new();

            int correctCount = 0;

            foreach (var q in questions)
            {
                if (SelectedAnswers.TryGetValue(q.Id, out int answerId))
                {
                    var answer = await _context.Answers
                        .FirstOrDefaultAsync(a => a.Id == answerId);

                    if (answer != null)
                    {
                        bool isCorrect = answer.IsCorrect;
                        exam.ExamResults.Add(new ExamResult
                        {
                            ExamId = exam.Id,
                            QuestionId = q.Id,
                            ChosenAnswerId = answerId,
                            IsCorrect = isCorrect
                        });

                        if (isCorrect) correctCount++;
                    }
                }
            }

            // ✅ Cập nhật thời gian & điểm số
            exam.EndTime = DateTime.Now;
            exam.Score = correctCount; // Mỗi câu đúng = 2 điểm

            await _context.SaveChangesAsync();

            return RedirectToPage("/Student/Result", new { score = exam.Score });
        }

    }
}
