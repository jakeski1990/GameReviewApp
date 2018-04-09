using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameReviewApp.Models
{
    public class GameDetailViewModel
    {
        public Game SelectedGame { get; set; }
        public Review ReviewModel { get; set; }

    }
}