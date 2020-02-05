using SikayetKayit.Extensions;
using SikayetKayit.Models;
using SikayetKayit.Models.Data;
using SikayetKayit.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Filter = SikayetKayit.Models.ViewModel.Filter;

namespace SikayetKayit.Controllers
{
    public class HomeController : Controller
    {
        DataContext dataContext = new DataContext();
        // GET: Home
        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel loginViewModel)
        {

            if (ModelState.IsValid)
            {
                User user = dataContext.User.FirstOrDefault(x => x.Username == loginViewModel.Username && x.Password == loginViewModel.Password);
                if (user == null)
                {
                    TempData["Uyari"] = "Kullanıcı adı veya şifre hatalı...";
                    return View();
                }

                return RedirectToAction("List");

            }
            return View();

        }

        public ActionResult List()
        {
            return View(dataContext.Sikayet.ToList());
        }

        public ActionResult Pagination(Filter filter, int page = 1, int pageSize = 3)
        {
            var query = dataContext.Sikayet
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(w => w.Title.StartsWith(filter.Title));
            }

            var model = query.OrderByDescending(o => o.Id).GetPaged(page, pageSize);
            return View(model);            
        }

        public ActionResult CustomerSearch(string q)
        {
            var customer = dataContext.Customer.Where(x => x.Eposta.Contains(q) || x.Phone.Contains(q));
            return Json(customer, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SikayetDetay(int? id)
        {
            if (id == null)
            {
                new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sikayet sikayet = dataContext.Sikayet.FirstOrDefault(x => x.Id == id);
            if (sikayet == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialDetails", sikayet);
        }

        [HttpPost]
        public ActionResult Edit(int? id, string text)
        {
            if (id == null)
            {
                new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sikayet sikayet = dataContext.Sikayet.FirstOrDefault(x => x.Id == id);


            if (text.Length < 100)
            {
                sikayet.Description = text;
                var updated = dataContext.Set<Sikayet>().Attach(sikayet);
                dataContext.Entry<Sikayet>(updated).State = System.Data.Entity.EntityState.Modified;
                dataContext.SaveChanges();

                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                sikayet.Description = sikayet.Description;
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sikayet sikayet = dataContext.Sikayet.FirstOrDefault(x => x.Id == id);
            if (sikayet == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialDelete", sikayet);
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult Deleted(int? id)
        {
            if (id == null)
            {
                new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sikayet sikayet = dataContext.Sikayet.FirstOrDefault(x => x.Id == id);


            var deleted = dataContext.Set<Sikayet>().Attach(sikayet);
            dataContext.Entry<Sikayet>(deleted).State = System.Data.Entity.EntityState.Deleted;
            dataContext.SaveChanges();

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult People()
        {
            return View();
        }
        [HttpPost]
        public ActionResult People(PeopleViewModel model)
        {   
            return View();
        }
       
        public ActionResult PeopleJ(PeopleViewModel model)
        {
            return View();
        }
    }
}