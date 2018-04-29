using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GameReviewApp.Models
{
    public class Publisher
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Logo { get; set; }

        public DateTime DateFounded { get; set; }

        public ICollection<Game> Games { get; set; }
    }
}