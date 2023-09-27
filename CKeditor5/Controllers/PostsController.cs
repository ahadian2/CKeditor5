using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CKeditor5.Models;

namespace CKeditor5.Controllers
{
    public class PostsController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: Posts
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostID,PostTitle,PostContent")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostID,PostTitle,PostContent")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
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


        [HttpPost]
        public ActionResult MyUpload()
        {
            string FolderName = "/Images/upload";
            bool exists=System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(FolderName));
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(FolderName));
            }

            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string Loaction = FolderName;
                        string NewName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(file.FileName);
                        string Imagename = NewName + extension;
                        string ImageUrl = Loaction + "/" + Imagename;
                        string ImageUrlUpload = Path.Combine(Server.MapPath(Loaction), Imagename);
                        file.SaveAs(ImageUrlUpload);
                        return Json(new { uploaded = true, url = ImageUrl, JsonRequestBehavior.AllowGet });
                    }
                    return Json(new { uploaded = true, JsonRequestBehavior.AllowGet });
                }
                catch (Exception ex)
                {
                    return Json(new { uploaded = false, error = "خطایی روی داده است" + ex.Message, JsonRequestBehavior.AllowGet });
                }
            }
            else
            {
                return Json(new { uploaded = false, error = "تصویری برای آپلود ارسال نشده است", JsonRequestBehavior.AllowGet });
            }
        }

    }
}
