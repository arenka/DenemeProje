using Newtonsoft.Json;
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

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Pagination(Filter filter, int page = 1, int pageSize = 10)
        {
            var query = dataContext.Sikayet
            .AsQueryable();

            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(w => w.Title.Contains(filter.Title) ||
                                     w.Description.Contains(filter.Title) ||
                                     w.Customer.Name.Contains(filter.Title) ||
                                     w.Customer.Phone.Contains(filter.Title) ||
                                     w.Customer.SurName.Contains(filter.Title));
            }

            var model = query.OrderByDescending(o => o.Id).GetPaged(page, pageSize);
            return View(model);
        }

        public JsonResult PaginationSearch(string SearchValue)
        {
            List<Sikayet> SikayetList = new List<Sikayet>();
            SikayetList = dataContext.Sikayet.Where(s => s.Title.Contains(SearchValue) || SearchValue == null).ToList();
            return Json(SikayetList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SikayetSearch(string q)
        {
            var customer = dataContext.Sikayet.Where(x => x.Title.Contains(q));
            return Json(customer, JsonRequestBehavior.AllowGet);
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

        public ActionResult PeoplePar(PeopleViewModel model)
        {
            return View();
        }

        public ActionResult AutoTextbox()
        {
            return View();
        }

        public JsonResult GetSearchValue(string search)
        {

            List<Sikayet> allSearch = dataContext.Sikayet.Where(x => x.Title.Contains(search)).ToList().Select(x => new Sikayet
            {
                Id = x.Id,
                Title = x.Title
                //Customer = x.Customer,
                //CustomerId = x.CustomerId,
                //DateTime = x.DateTime,
                //Description = x.Description
            }).ToList();
            return new JsonResult { Data = allSearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

        public ActionResult AutoDdl()
        {

            return View();
        }

        public ActionResult Deneme()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Captcha()
        {
            var response = Request["g-recaptcha-response"];
            const string secret = "secretKey";
            //Kendi Secret keyinizle değiştirin.

            var client = new WebClient();
            var reply =
                client.DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaModel>(reply);

            if (!captchaResponse.Success)
                TempData["Message"] = "Lütfen güvenliği doğrulayınız.";
            else
                TempData["Message"] = "Güvenlik başarıyla doğrulanmıştır.";
            return View("Deneme");
        }
    }
}