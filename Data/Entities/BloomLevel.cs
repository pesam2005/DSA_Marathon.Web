using System.ComponentModel.DataAnnotations.Schema;

namespace DSA_Marathon.Web.Data.Entities
{
    [Table("BloomLevel")]
    public class BloomLevel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
}
