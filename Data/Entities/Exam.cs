using System.ComponentModel.DataAnnotations.Schema;

namespace DSA_Marathon.Web.Data.Entities
{
    [Table("Exam")] // 👈 Thêm dòng này để EF biết bảng tên là Exam (không phải Exams)
    public class Exam
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        [Column(TypeName = "datetime2")]
        public DateTime StartTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime EndTime { get; set; }

        public int Score { get; set; }
        public ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();
    }
}
