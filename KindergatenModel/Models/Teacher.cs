using System.ComponentModel.DataAnnotations;

namespace Kindergarten.Models
{
    public class Teacher
    {
        public int TeacherID { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Phone number")]
        public string ContactNumber { get; set; }

        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }
    }
}
