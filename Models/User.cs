using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Exam.Validations;

namespace Exam.Models
{
    public class User
    {
        [Key]
            public int UserId { get; set; }

            [Required(ErrorMessage="Please Enter First name")]
            public string FirstName {get;set;}

            [Required(ErrorMessage="Please Enter Last Name")]
            public string LastName {get;set;}

            [EmailAddress]
            [Required(ErrorMessage="Please Enter Email")]
            public string Email {get;set;}

            [DataType(DataType.Password)]
            [Required(ErrorMessage="Please Enter a Valid Password")]
            [UniquePassword]
            public string Password {get;set;}

            public DateTime CreatedAt {get;set;} = DateTime.Now;
            public DateTime UpdatedAt {get;set;} = DateTime.Now;

            // We use the NotMapped Annotation so that this variable doesn't end up in our database.
            [NotMapped]
            [Compare("Password")]
            [DataType(DataType.Password)]
            [Required(ErrorMessage="Confirm Password Required")]
            public string Confirm {get;set;}

            //Nav Prop One to Many- A user can plan many soccer matches
            public List<Event> MyEvents{get;set;}

            //Nav Prop Many to Many- A User can go to many Matches
            public List<People> Attending {get;set;}
    }
}