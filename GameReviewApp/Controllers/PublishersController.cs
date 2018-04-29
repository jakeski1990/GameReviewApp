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

namespace GameReviewApp.Controllers
{
    public class PublishersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Publishers
        public ActionResult Index(string sortOrder)
        {

            ICollection<Publisher> publisherList = db.Publishers.ToList();
            IOrderedEnumerable<Publisher> sortedPublishers;


            switch (sortOrder)
            {
                case "PublisherName":
                    sortedPublishers = publisherList.OrderBy(p => p.Name);
                    break;
                case "Founding":
                    sortedPublishers = publisherList.OrderBy(p => p.DateFounded);
                    break;
                default:
                    sortedPublishers = publisherList.OrderBy(p => p.Id);
                    break;
            }


            return View(sortedPublishers);
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexSearch(string searchCriteria)
        {
            IEnumerable<Publisher> publisherList = db.Publishers.ToList() as IList<Publisher>;

            //Search Function
            if (searchCriteria != null)
            {
                publisherList = publisherList.Where(g => g.Name.ToUpper().Contains(searchCriteria.ToUpper()));
            }


            return View(publisherList);
        }


        [AuthorizeOrRedirectAttribute(Roles = "Admin,Moderator")]
        public ActionResult AdminIndex(string sortOrder)
        {

            ICollection<Publisher> publisherList = db.Publishers.ToList();
            IOrderedEnumerable<Publisher> sortedPublishers;


            switch (sortOrder)
            {
                case "PublisherName":
                    sortedPublishers = publisherList.OrderBy(p => p.Name);
                    break;
                case "Founding":
                    sortedPublishers = publisherList.OrderBy(p => p.DateFounded);
                    break;
                default:
                    sortedPublishers = publisherList.OrderBy(p => p.Id);
                    break;
            }


            return View(sortedPublishers);
        }

        //[HttpPost]
        //[ActionName("AdminIndex")]
        //public ActionResult AdminIndexSearch(string searchCriteria)
        //{
        //    IEnumerable<Publisher> publisherList = db.Publishers.ToList() as IList<Publisher>;

        //    //Search Function
        //    if (searchCriteria != null)
        //    {
        //        publisherList = publisherList.Where(g => g.Name.ToUpper().Contains(searchCriteria.ToUpper()));
        //    }


        //    return View(publisherList);
        //}


        // GET: Publishers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publisher publisher = db.Publishers.Find(id);
            if (publisher == null)
            {
                return HttpNotFound();
            }
            publisher.Games = db.Games.Where(r => r.Publisher == publisher.Name).ToList();


            return View(publisher);
        }

        // GET: Publishers/Create
        [AuthorizeOrRedirectAttribute(Roles = "Admin,Moderator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Publishers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Logo,DateFounded")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                db.Publishers.Add(publisher);
                db.SaveChanges();
                return RedirectToAction("AdminIndex");
            }

            return View(publisher);
        }

        // GET: Publishers/Edit/5
        [AuthorizeOrRedirectAttribute(Roles = "Admin,Moderator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publisher publisher = db.Publishers.Find(id);
            if (publisher == null)
            {
                return HttpNotFound();
            }
            return View(publisher);
        }

        // POST: Publishers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Logo,DateFounded")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(publisher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AdminIndex");
            }
            return View(publisher);
        }

        // GET: Publishers/Delete/5
        [AuthorizeOrRedirectAttribute(Roles = "Admin,Moderator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publisher publisher = db.Publishers.Find(id);
            if (publisher == null)
            {
                return HttpNotFound();
            }
            return View(publisher);
        }

        // POST: Publishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Publisher publisher = db.Publishers.Find(id);
            db.Publishers.Remove(publisher);
            db.SaveChanges();
            return RedirectToAction("AdminIndex");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
