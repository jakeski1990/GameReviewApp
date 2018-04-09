using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GameReviewApp.Models
{
    public class ReviewListViewModel
    {
        public int ID { get; set; }

        //TODO: Later replace with UserName
        public int UserId { get; set; }

        [Required]
        public string Content { get; set; }

        public int GameId { get; set; }

        public string GameName { get; set; }

        [Required]
        [Range(1, 10,
            ErrorMessage = "Rating must be between 1 and 10")]
        public int NumRating { get; set; }


    }
}