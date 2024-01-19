// ClassActivity model class
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

public class ClassActivity
{
    public int ClassID { get; set; }
    public Class Class { get; set; }

    public int KActivityID { get; set; }
    [Display(Name = "Activity")]
    public KActivity KActivity { get; set; }
}
