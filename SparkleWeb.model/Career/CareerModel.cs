using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleWeb.model.Career
{
    public class CareerData
    {
        [Key]
        public int CareerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Designation { get; set; }
        public string Experience { get; set; }
        public string Location { get; set; }
        public string Education { get; set; }
        public int SortOrder { get; set; }
        public Boolean? IsActive { get; set; }
        public Boolean? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
