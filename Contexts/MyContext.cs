using Exam.Models;
using Microsoft.EntityFrameworkCore;

namespace Exam.Contexts
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options){}
        public DbSet<User> Users {get;set;}
        public DbSet<Event> Events{get;set;}
        public DbSet<People> Peoples{get;set;}
    }
}