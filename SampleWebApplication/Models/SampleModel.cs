using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SampleWebApplication.Models
{
    public class SampleModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(10,MinimumLength=1)]
        public string Name { get; set; }
        public string Note { get; set; }
    }
}