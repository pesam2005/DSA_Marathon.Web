using System.ComponentModel.DataAnnotations.Schema;

namespace DSA_Marathon.Web.Data.Entities
{
    [Table("ExamResult")]
    public class ExamResult
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public Exam Exam { get; set; } = null!;
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public int ChosenAnswerId { get; set; }
        public bool IsCorrect { get; set; }
    }
}
