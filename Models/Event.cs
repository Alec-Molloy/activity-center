using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Exam.Validations;

namespace Exam.Models
{
    public class Event
    {
        [Key]
        public int EventId{get;set;}

        [Required(ErrorMessage = "Event Title is Required")]
        public string Title {get;set;}

        [Required(ErrorMessage="Date is Required")]
        [DataType(DataType.Date)]
        [NoPastDate]
        public DateTime Date{get;set;}

        [Required(ErrorMessage="Time is Required")]
        [DataType(DataType.Time)]
        public DateTime Time{get;set;}

        [Required(ErrorMessage="Duration is Required")]
        public int Duration{get;set;}

        [Required(ErrorMessage="Description is Required")]
        public string Description{get;set;}
        
        public DateTime CreatedAt{get;set;}= DateTime.Now;
        
        public DateTime UpdatedAt{get;set;}= DateTime.Now;
        public int UserId{get;set;}
        public User Organizer{get;set;}
        public List<People> Participants{get;set;}
    }
}