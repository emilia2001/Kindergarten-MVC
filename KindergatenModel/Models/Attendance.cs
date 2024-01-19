using System.ComponentModel.DataAnnotations;

namespace Kindergarten.Models
{
    public class Attendance
    {
        public int AttendanceID { get; set; }

        public int ChildID { get; set; }
        public Child? Child { get; set; }

        public DateTime Date { get; set; }

        [Display(Name = "Status")]
        public bool PresentStatus { get; set; }
    }
}
