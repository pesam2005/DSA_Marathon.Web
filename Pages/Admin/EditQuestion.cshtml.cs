using DSA_Marathon.Web.Data;
using DSA_Marathon.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DSA_Marathon.Web.Pages.Admin
{
    public class EditQuestionModel : PageModel
    {
        private readonly QuizDbContext _context;
        public EditQuestionModel(QuizDbContext context) => _context = context;

        [BindProperty]
        public int QuestionId { get; set; }

        [BindProperty]
        public string QuestionContent { get; set; } = string.Empty;

        [BindProperty]
        public int SelectedBloomId { get; set; }

        [BindProperty]
        public List<AnswerInput> Answers { get; set; } = new();

        [BindProperty]
        public int CorrectIndex { get; set; }

        public List<BloomLevel> BloomLevels { get; set; } = new();

        public class AnswerInput
        {
            public string Content { get; set; } = string.Empty;
            public bool IsCorrect { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToPage("/Admin/Login");

            var question = await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
                return RedirectToPage("/Admin/Questions");

            QuestionId = question.Id;
            QuestionContent = question.Content;
            SelectedBloomId = question.BloomLevelId;

            Answers = question.Answers
                .Select(a => new AnswerInput { Content = a.Content, IsCorrect = a.IsCorrect })
                .ToList();

            BloomLevels = await _context.BloomLevels.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToPage("/Admin/Login");

            var question = await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == QuestionId);

            if (question == null)
                return RedirectToPage("/Admin/Questions");

            question.Content = QuestionContent;
            question.BloomLevelId = SelectedBloomId;

            _context.Answers.RemoveRange(question.Answers);
            await _context.SaveChangesAsync();

            for (int i = 0; i < Answers.Count; i++)
            {
                _context.Answers.Add(new Answer
                {
                    QuestionId = question.Id,
                    Content = Answers[i].Content,
                    IsCorrect = (i == CorrectIndex)
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("/Admin/Questions");
        }
    }
}
