namespace Exam.Models
{
    public class People
    {
        public int PeopleId{get;set;}
        public int UserId{get;set;}
        public int EventId{get;set;}
        public User participant{get;set;}
        public Event Event{get;set;}
    }
}