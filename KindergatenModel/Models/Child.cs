using Kindergarten.Models;
using System.ComponentModel.DataAnnotations;

public class Child
{
    public int ChildID { get; set; }

    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    public string FullName
    {
        get { return $"{LastName} {FirstName}"; }
    }

    [Display(Name = "Date of Birth")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Display(Name = "Contact Number")]
    public string ParentContact { get; set; }

    [Display(Name = "Group")]
    public int? ClassID { get; set; }
    [Display(Name = "Group")]
    public Class? Class { get; set; }

    public List<Attendance>? Attendances { get; set; }
}
