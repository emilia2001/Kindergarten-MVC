using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kindergaten.Models.ViewModel
{
    public class EnrolledClassData
        
    {
        public int ClassID { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool IsEnrolled { get; set; }

    }
}
