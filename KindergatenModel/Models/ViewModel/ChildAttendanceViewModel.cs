using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kindergaten.Models.ViewModel
{
    public class ChildAttendanceViewModel
    {
        public int ChildID { get; set; }
        public string FullName { get; set; }
        public int TotalAttendancesThisMonth { get; set; }
    }
}
