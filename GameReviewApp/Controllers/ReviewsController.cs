using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GameReviewApp.Models;

namespace GameReviewApp.Controllers
{
    public class ReviewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reviews
        public ActionResult Index()
        {

            return View(BuildReviewListViewModelList(db.Reviews.ToList()));
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            ReviewListViewModel reviewListViewModel = BuildReviewListViewModel(review);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(reviewListViewModel);
        }

        // GET: Reviews/Create
        public ActionResult Create(int gameId)
        {
            Review review = new Review();
            review.GameId = gameId;


            return View(review);
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,Content,GameId,NumRating")] Review review, int GameId)
        {
            if (ModelState.IsValid)
            {
                db.Reviews.Add(review);
                db.SaveChanges();

                Game game = db.Games.Find(GameId);

                game.Reviews = db.Reviews.Where(r => r.GameId == game.Id).ToList();
                game.Rating = (game.Reviews.Sum(r => r.NumRating)) / game.Reviews.Count;
                db.SaveChanges();

                int? id = GameId;
                return RedirectToAction("Details", "Games", new { id });
            }

            return View(review);
        }

        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            ReviewListViewModel reviewListViewModel = BuildReviewListViewModel(review);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(reviewListViewModel);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,Content,GameId,NumRating")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(review);
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            ReviewListViewModel reviewListViewModel = BuildReviewListViewModel(review);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(reviewListViewModel);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
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

        [NonAction]
        private ReviewListViewModel BuildReviewListViewModel(Review review)
        {
            //var chosenGame = db.Games.Where(g => g.Id == review.GameId);
            var gameNames = db.Games.ToDictionary(g => g.Id, g => g.Name);
            ReviewListViewModel model = new ReviewListViewModel();
            model.ID = review.Id;
            model.UserId = review.UserId;
            model.Content = review.Content;
            model.GameId = review.GameId;
            model.GameName = gameNames[review.GameId];
            //model.GameName = chosenGame.First().Name;
            model.NumRating = review.NumRating;

            return model;


        }

        [NonAction]
        private List<ReviewListViewModel> BuildReviewListViewModelList(List<Review> reviews)
        {
            List<ReviewListViewModel> reviewListViewModel = new List<ReviewListViewModel>();

            foreach (var review in reviews)
            {
               reviewListViewModel.Add(BuildReviewListViewModel(review));
            }


            return reviewListViewModel;
        }
        
    }
}
