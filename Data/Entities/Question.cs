
using System.ComponentModel.DataAnnotations.Schema;

namespace DSA_Marathon.Web.Data.Entities
{
    [Table("Question")]
    public class Question
    {
        public int Id { get; set; }
        public string Content { get; set; } = "";
        public int BloomLevelId { get; set; }
        public BloomLevel BloomLevel { get; set; } = null!;
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}
