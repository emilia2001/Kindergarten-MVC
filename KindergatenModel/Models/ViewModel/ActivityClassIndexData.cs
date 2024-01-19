using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kindergaten.Models.ViewModel
{
    public class ActivityClassIndexData
    {
        public ActivityClassIndexData() { }
        public IEnumerable<KActivity> Activities { get; set; }
        public IEnumerable<Class> Classes { get; set; }
    }
}
