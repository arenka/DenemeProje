using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SikayetKayit.Models
{
    public class Sikayet
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime DateTime { get; set; }
        [Required, StringLength(100)]
        public string Description { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}