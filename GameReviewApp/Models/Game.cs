using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GameReviewApp.Models
{
    public class Game
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Release { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public Platform Platform { get; set; }

        [Required]
        public string Publisher { get; set; }


        public string Thumbnail { get; set; }


        public string BuyLink { get; set; }

        public double Rating { get; set; }



        public ICollection<Review> Reviews { get; set; }

    }
}