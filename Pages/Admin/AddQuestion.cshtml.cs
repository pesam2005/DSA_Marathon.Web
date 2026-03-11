using DSA_Marathon.Web.Data;
using DSA_Marathon.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DSA_Marathon.Web.Pages.Admin
{
    public class AddQuestionModel : PageModel
    {
        private readonly QuizDbContext _context;
        public AddQuestionModel(QuizDbContext context) => _context = context;

        [BindProperty]
        public string QuestionContent { get; set; } = string.Empty;

        [BindProperty]
        public int? SelectedBloomId { get; set; }

        [BindProperty]
        public List<AnswerInput> Answers { get; set; } = new();

        [BindProperty]
        public int CorrectIndex { get; set; }

        public List<BloomLevel> BloomLevels { get; set; } = new();

        public class AnswerInput
        {
            public string Content { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToPage("/Admin/Login");

            BloomLevels = await _context.BloomLevels.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToPage("/Admin/Login");

            if (string.IsNullOrWhiteSpace(QuestionContent) || !SelectedBloomId.HasValue)
            {
                ModelState.AddModelError("", "Vui lòng nhập đủ thông tin!");
                BloomLevels = await _context.BloomLevels.ToListAsync();
                return Page();
            }

            var question = new Question
            {
                Content = QuestionContent,
                BloomLevelId = SelectedBloomId.Value
            };
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            for (int i = 0; i < Answers.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(Answers[i].Content))
                {
                    var ans = new Answer
                    {
                        QuestionId = question.Id,
                        Content = Answers[i].Content,
                        IsCorrect = (i == CorrectIndex)
                    };
                    _context.Answers.Add(ans);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("/Admin/Questions");
        }
    }
}
