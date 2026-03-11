using DSA_Marathon.Web.Data;
using DSA_Marathon.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DSA_Marathon.Web.Pages.Student
{
    public class ResultModel : PageModel
    {
        private readonly QuizDbContext _context;

        public ResultModel(QuizDbContext context)
        {
            _context = context;
        }

        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }

        public Dictionary<string, int> BloomCorrect { get; set; } = new();
        public Dictionary<string, int> BloomTotal { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? score = null)
        {
            var examId = HttpContext.Session.GetInt32("ExamId");
            if (examId == null)
                return RedirectToPage("/Student/Start");

            var exam = await _context.Exams
                .Include(e => e.ExamResults)
                    .ThenInclude(er => er.Question)
                        .ThenInclude(q => q.BloomLevel)
                .FirstOrDefaultAsync(e => e.Id == examId);

            if (exam == null)
                return RedirectToPage("/Student/Start");

            Score = exam.Score;
            TotalQuestions = exam.ExamResults.Count;
            CorrectAnswers = exam.ExamResults.Count(er => er.IsCorrect);

            // ✅ Tính kết quả theo Bloom level
            var grouped = exam.ExamResults
                .GroupBy(er => er.Question.BloomLevel.Name)
                .Select(g => new
                {
                    Bloom = g.Key,
                    Correct = g.Count(x => x.IsCorrect),
                    Total = g.Count()
                });

            foreach (var g in grouped)
            {
                BloomCorrect[g.Bloom] = g.Correct;
                BloomTotal[g.Bloom] = g.Total;
            }

            return Page();
        }
    }
}
