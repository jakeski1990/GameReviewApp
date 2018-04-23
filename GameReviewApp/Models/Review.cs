using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GameReviewApp.Models
{
    public class Review
    {

        public int Id { get; set; }

        public string ReviewerName { get; set; }

        [Required]
        public string Content { get; set; }

        public int GameId { get; set; }

        [Required]
        [Range(1, 10,
            ErrorMessage = "Rating must be between 1 and 10")]
        [Display(Name ="Rating")]
        public int NumRating { get; set; }

    }
}