using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleWeb.model.Career
{
    public class CareerDataViewModels
    {
        public int CareerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Designation { get; set; }
        public string Experience { get; set; }
        public string Location { get; set; }
        public string Education { get; set; }
        public virtual int SortOrder { get; set; }
        public Boolean? IsActive { get; set; }
    }
}
