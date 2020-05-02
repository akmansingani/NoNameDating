using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Models
{
    [Serializable]
    public class TestTable
    {
        [Key]
        public int TestID { get; set; }

        public string TestName { get; set; }
    }
}
