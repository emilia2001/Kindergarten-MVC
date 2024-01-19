// Activity model class
using System.ComponentModel.DataAnnotations;

public class KActivity
{
    public int KActivityID { get; set; }

    [Display(Name = "Name")]
    public string KActivityName { get; set; }

    [Display(Name = "Start time")]
    public DateTime Date { get; set; }

    [Display(Name = "Duration")]
    public TimeSpan Time { get; set; }

    // Navigation property for the many-to-many relationship with Classes
    public List<ClassActivity>? ClassActivities { get; set; }
}
