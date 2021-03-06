﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GameReviewApp.CustomAttribute;
using GameReviewApp.Models;
using Microsoft.AspNet.Identity;

namespace GameReviewApp.Controllers
{
    public class GamesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Random rnd = new Random();

        // GET: Games
        [HttpGet]
        public ActionResult Index(string sortOrder)
        {
            ICollection<Game> gameList = db.Games.ToList();
            IOrderedEnumerable<Game> sortedGames;
            ViewBag.Platforms = Enum.GetValues(typeof(Platform)) as IList<Platform>;

            foreach (var game in gameList)
            {
                game.Reviews = db.Reviews.Where(r => r.GameId == game.Id).ToList();
                if (game.Reviews.Count != 0)
                {
                    game.Rating = (game.Reviews.Sum(r => r.NumRating)) / game.Reviews.Count;
                }

            }
            db.SaveChanges();


            switch (sortOrder)
            {
                case "Name":
                    sortedGames = gameList.OrderBy(g => g.Name);
                    break;
                case "Rating":
                    sortedGames = gameList.OrderBy(g => g.Rating);
                    break;
                default:
                    sortedGames = gameList.OrderBy(g => g.Id);
                    break;
            }

            IList<News> newsList = db.News.ToList();
            int ranNum = rnd.Next(newsList.Count);
            News selectedNews = newsList[ranNum];
            ViewBag.SelectedNews = selectedNews;

            return View(sortedGames);
        }

        [HttpPost]
        public ActionResult Index(string searchCriteria, string platformFilter)
        {
            IEnumerable<Game> gameList = db.Games.ToList() as IList<Game>;

            ViewBag.Platforms = Enum.GetValues(typeof(Platform)) as IList<Platform>;
            ViewBag.platformChoice = platformFilter;

            //Search Function
            if (searchCriteria != null)
            {
                gameList = gameList.Where(g => g.Name.ToUpper().Contains(searchCriteria.ToUpper()));
            }

            //Filter function
            if (platformFilter != "" || platformFilter == null)
            {
                Enum.TryParse(platformFilter, out Platform convertedFilter);

                gameList = gameList.Where(g => g.Platform == convertedFilter);
            }


            return View(gameList);
        }



        // GET: Games/Details/5
        public ActionResult Details(int? id, string sortOrder)
        {
            ICollection<Review> reviews = db.Reviews.ToList();
            IOrderedEnumerable<Review> sortedReviews; 


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Game game = db.Games.Find(id);

            if (game == null)
            {
                return HttpNotFound();
            }

            game.Reviews = db.Reviews.Where(r => r.GameId == game.Id).ToList();

            switch (sortOrder)
            {
                case "RatingDesc":
                    sortedReviews = reviews.OrderByDescending(r => r.NumRating);
                    game.Reviews = sortedReviews.Where(r => r.GameId == game.Id).ToList();
                    break;
                case "RatingAsc":
                    sortedReviews = reviews.OrderBy(r => r.NumRating);
                    game.Reviews = sortedReviews.Where(r => r.GameId == game.Id).ToList();
                    break;
                default:
                    game.Reviews = db.Reviews.Where(r => r.GameId == game.Id).ToList();
                    break;
            }



            Review review = new Review();
            review.GameId = game.Id;

            var detailModel = new GameDetailViewModel {SelectedGame = game, ReviewModel = review };

            string reviewerName = User.Identity.GetUserName();
            ViewBag.ReviewerName = reviewerName;

            return View(detailModel);
        }

        [HttpPost]
        public ActionResult Details([Bind(Include = "SelectedGame,ReviewModel")] GameDetailViewModel detailModel)
        {
            //SelectedGame coming up null
            if (ModelState.IsValid)
            {
                db.Reviews.Add(detailModel.ReviewModel);
                db.SaveChanges();

                Game game = db.Games.Find(detailModel.ReviewModel.GameId);


                game.Reviews = db.Reviews.Where(r => r.GameId == game.Id).ToList();
                game.Rating = (game.Reviews.Sum(r => r.NumRating)) / game.Reviews.Count;
                db.SaveChanges();

                int? id = detailModel.ReviewModel.GameId;

                string userName = User.Identity.GetUserName();
                ApplicationUser user = db.Users.Where(u => u.UserName == userName).ToList().FirstOrDefault();
                user.ReviewCount += 1;
                db.SaveChanges();

                return RedirectToAction("Details", "Games", new { id });
            }

            return RedirectToAction("Index", "Games"); //View(detailModel); //RedirectToAction("Create", "Reviews", new { gameId });
        }



        // GET: Games/Create
        [AuthorizeOrRedirectAttribute(Roles = "Admin,Moderator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Release,Price,Platform,Publisher,Thumbnail,BuyLink")] Game game)
        {
            if (ModelState.IsValid)
            {
                db.Games.Add(game);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(game);
        }

        // GET: Games/Edit/5
        [AuthorizeOrRedirectAttribute(Roles = "Admin,Moderator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Release,Price,Platform,Publisher,Thumbnail,BuyLink")] Game game)
        {
            if (ModelState.IsValid)
            {
                db.Entry(game).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(game);
        }

        // GET: Games/Delete/5
        [AuthorizeOrRedirectAttribute(Roles = "Admin,Moderator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Game game = db.Games.Find(id);
            db.Games.Remove(game);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        [AuthorizeOrRedirectAttribute(Roles = "Admin,Moderator")]
        public ActionResult AdminIndex(string sortOrder)
        {
            ICollection<Game> gameList = db.Games.ToList();
            IOrderedEnumerable<Game> sortedGames;
            ViewBag.Platforms = Enum.GetValues(typeof(Platform)) as IList<Platform>;

            foreach (var game in gameList)
            {
                game.Reviews = db.Reviews.Where(r => r.GameId == game.Id).ToList();
                if (game.Reviews.Count != 0)
                {
                    game.Rating = (game.Reviews.Sum(r => r.NumRating)) / game.Reviews.Count;
                }

            }
            db.SaveChanges();


            switch (sortOrder)
            {
                case "Name":
                    sortedGames = gameList.OrderBy(g => g.Name);
                    break;
                case "Rating":
                    sortedGames = gameList.OrderBy(g => g.Rating);
                    break;
                default:
                    sortedGames = gameList.OrderBy(g => g.Id);
                    break;
            }


            return View(sortedGames);
        }

        [HttpPost]
        public ActionResult AdminIndex(string searchCriteria, string platformFilter)
        {
            IEnumerable<Game> gameList = db.Games.ToList() as IList<Game>;

            ViewBag.Platforms = Enum.GetValues(typeof(Platform)) as IList<Platform>;
            ViewBag.platformChoice = platformFilter;

            //Search Function
            if (searchCriteria != null)
            {
                gameList = gameList.Where(g => g.Name.ToUpper().Contains(searchCriteria.ToUpper()));
            }

            //Filter function
            if (platformFilter != "" || platformFilter == null)
            {
                Enum.TryParse(platformFilter, out Platform convertedFilter);

                gameList = gameList.Where(g => g.Platform == convertedFilter);
            }


            return View(gameList);
        }


    }
}
