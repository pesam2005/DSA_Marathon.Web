using DSA_Marathon.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DSA_Marathon.Web.Data
{
    public class QuizDbContext : DbContext
    {
        public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options) { }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<BloomLevel> BloomLevels { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }
    }
}
