using System;
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
    public class ReviewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reviews
        [AuthorizeOrRedirectAttribute(Roles = "Admin,Moderator")]
        public ActionResult Index(string sortOrder)
        {
            ICollection<ReviewListViewModel> modelList = BuildReviewListViewModelList(db.Reviews.ToList());
            IOrderedEnumerable<ReviewListViewModel> sortedReviews;


            switch (sortOrder)
            {
                case "ReviewerName":
                    sortedReviews = modelList.OrderBy(m => m.ReviewerName);
                    break;
                case "GameName":
                    sortedReviews = modelList.OrderBy(g => g.GameName);
                    break;
                default:
                    sortedReviews = modelList.OrderBy(g => g.ID);
                    break;
            }

            this.Session["_ReviewReturn"] = "review";

            return View(sortedReviews);
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexSearch(string searchCriteria)
        {
            IEnumerable<ReviewListViewModel> modelList = BuildReviewListViewModelList(db.Reviews.ToList()) as IList<ReviewListViewModel>;


            //Search Function
            if (searchCriteria != null)
            {
                modelList = modelList.Where(g => g.Content.ToUpper().Contains(searchCriteria.ToUpper()));
            }


            return View(modelList);
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
        public ActionResult Create([Bind(Include = "Id,ReviewerName,Content,GameId,NumRating")] Review review, int GameId)
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
        //[AuthorizeOrRedirectAttribute(Roles = "Admin,Moderator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);

            if (User.IsInRole("Admin") || User.IsInRole("Moderator")) ;
            else if (User.Identity.GetUserName() == review.ReviewerName) ;
            else return RedirectToAction("AccessDenied", "Error");

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
        public ActionResult Edit([Bind(Include = "Id,ReviewerName,Content,GameId,NumRating")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                if (this.Session["_ReviewReturn"].ToString() == "manage")
                {
                    return RedirectToAction("Index", "Manage");
                }

                return RedirectToAction("Index", "Reviews");
            }
            return View(review);
        }

        // GET: Reviews/Delete/5
        //[AuthorizeOrRedirectAttribute(Roles = "Admin,Moderator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);

            if (User.IsInRole("Admin") || User.IsInRole("Moderator"));
            else if (User.Identity.GetUserName() == review.ReviewerName);
            else return RedirectToAction("AccessDenied", "Error");
            
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

            if (this.Session["_ReviewReturn"].ToString() == "manage")
            {
                return RedirectToAction("Index","Manage");
            }

            return RedirectToAction("Index", "Reviews");
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
            model.ReviewerName = review.ReviewerName;
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
