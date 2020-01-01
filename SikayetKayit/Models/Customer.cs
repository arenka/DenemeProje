using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SikayetKayit.Models
{
    public class Customer
    {
        public Customer()
        {
            this.Sikayets = new HashSet<Sikayet>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string SurName { get; set; }
        [Required,DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required,DataType(DataType.EmailAddress)]
        public string Eposta { get; set; }

        public int SikayetId { get; set; }
        public ICollection<Sikayet> Sikayets { get; set; }
    }
}