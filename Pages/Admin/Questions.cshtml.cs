using DSA_Marathon.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DSA_Marathon.Web.Data;
namespace DSA_Marathon.Web.Pages.Admin
{
    public class QuestionsModel : PageModel
    {
        private readonly QuizDbContext _context;
        public QuestionsModel(QuizDbContext context) => _context = context;

        public List<Question> Questions { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToPage("/Admin/Login");

            Questions = await _context.Questions
                .Include(q => q.BloomLevel)
                .Include(q => q.Answers)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToPage("/Admin/Login");

            var question = await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question != null)
            {
                _context.Answers.RemoveRange(question.Answers);
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
