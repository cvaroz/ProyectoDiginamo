using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoDiginamo.Models.Data
{
    [Table("tblPages")]
    public class PageDTO
    {
        [Key]
        public int Id { get; set; }
        public String Title { get; set; }
        public String Slug { get; set; }
        public String Body { get; set; }
        public int Sorting { get; set; }
        public bool HasSidebar { get; set; }


    }
}