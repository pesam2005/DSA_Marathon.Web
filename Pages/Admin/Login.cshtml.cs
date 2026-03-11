using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DSA_Marathon.Web.Pages.Admin
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }

        public IActionResult OnPost()
        {
            if (Username == "thanhthai2310040" && Password == "05122005")
            {
                HttpContext.Session.SetString("AdminLoggedIn", "true");
                return RedirectToPage("/Admin/Questions");
            }

            ErrorMessage = "Sai tên đăng nhập hoặc mật khẩu!";
            return Page();
        }
    }
}
