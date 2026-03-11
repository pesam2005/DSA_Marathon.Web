using System.ComponentModel.DataAnnotations.Schema;

namespace DSA_Marathon.Web.Data.Entities
{
    [Table("Answer")]
    public class Answer
    {
        public int Id { get; set; }
        public string Content { get; set; } = "";
        public bool IsCorrect { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
    }
}
