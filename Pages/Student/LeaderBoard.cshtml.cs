using DSA_Marathon.Web.Data;
using DSA_Marathon.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ExamIndex = DSA_Marathon.Web.Data.Entities.Exam;
namespace DSA_Marathon.Web.Pages.Student
{
    public class LeaderBoardModel : PageModel
    {
        private readonly QuizDbContext _context;

        public LeaderBoardModel(QuizDbContext context)
        {
            _context = context;
        }

        public List<ExamIndex> Exams { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Lấy top 50 người có điểm cao nhất, sắp theo điểm và thời gian nộp
            Exams = await _context.Exams
                .Where(e => e.Score > 0)
                .OrderByDescending(e => e.Score)
                .ThenBy(e => e.EndTime)
                .Take(50)
                .ToListAsync();
        }
    }
}
