// Class model class
using Kindergarten.Models;
using System.ComponentModel.DataAnnotations;

public class Class
{
    public int ClassID { get; set; }

    [Display(Name = "Name")]
    public string? ClassName { get; set; }

    [Display(Name = "Teacher")]
    public int? TeacherID { get; set; }
    public Teacher? Teacher { get; set; }

    public int Capacity { get; set; }

    // Navigation property for the associated Children
    public List<Child>? Children { get; set; }

    // Navigation property for the many-to-many relationship with Activities
    public List<ClassActivity>? ClassActivities { get; set; }
}
