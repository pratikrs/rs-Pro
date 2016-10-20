using MyDBFDemo.DataAccess.GenericRepository;
using MyDBFDemo.DataAccess.RepositoryPattern;
using MyDBFDemo.DbContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyDBFDemo.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Declaration
        UnitOfWork uow = new UnitOfWork();
        #endregion
        public ActionResult Index()
        {

            var studentRepository = uow.Repository<StudentRepository>();
            //StudentEntity _passenger = new StudentEntity();
            //_passenger.FirstName = "Sagar";
            //_passenger.LastName = "Patel";
            //_passenger.Phone = "9974888917";
            //_passenger.Email = "test@test.com";
            //_passenger.Gender = "M";
            //_passenger.CountryId = 1;
            //_passenger.StateId = 2;
            

            //studentRepository.AddPassenger(_passenger);

            //_passenger = new StudentEntity();
            //_passenger.FirstName = "Nirali";
            //_passenger.LastName = "Patel";
            //_passenger.Phone = "1234567890";
            //_passenger.Email = "test1@test.com";
            //_passenger.Gender = "F";
            //_passenger.CountryId = 1;
            //_passenger.StateId = 2;


            //studentRepository.AddPassenger(_passenger);

            var students = studentRepository.GetAllPassengers();

            if (students.Any())
            {
                var std = studentRepository.GetPassengerById(students.FirstOrDefault().Id);
                studentRepository.DeletePassengerById(students.FirstOrDefault().Id);
            }

            


            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}

//==========================
//using RafCompare.Entities.ProductEntities;
//using RafCompare.StateProvider;
//using Repository.Pattern;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace RafCompare.Controllers
//{
//    [AuthorizeWebForm]
//    [HandleError()]
//    public class BrandController : Controller
//    {
//        #region Declaration
//        UnitOfWork unitOfWork = new UnitOfWork();
//        #endregion

//        #region Actions
//        public ActionResult Create()
//        {
//            return View(new Brands());
//        }

//        [HttpPost]
//        public ActionResult Create(Brands model)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    unitOfWork.BeginTransaction();
//                    var brandRepo = unitOfWork.Repository<Brands>();

//                    if (brandRepo.GetAll(x => x.BrandName.ToLower() == model.BrandName.ToLower()).Count() > 0)
//                    {
//                        ModelState.AddModelError("BrandName", "Brand Name is already exist. You should enter unique brand name.");
//                        unitOfWork.Rollback();
//                        return View(model);
//                    }
//                    model.CreatedBy = SessionManager.Instance.CurrentUser.UserName;
//                    model.CreatedOn = DateTime.Now;
//                    brandRepo.Add(model);
//                    unitOfWork.SaveChanges();
//                    unitOfWork.Commit();
//                    return RedirectToAction("Brands");
//                }
//                catch (Exception)
//                {
//                    unitOfWork.Rollback();
//                    return View("Error");
//                }
//            }
//            return View(model);
//        }

//        public ActionResult Edit(int id)
//        {
//            Brands model = new Brands();

//            if (id > 0)
//            {
//                var brandRepo = unitOfWork.Repository<Brands>();
//                model = brandRepo.Get(x => x.BrandId == id);

//                if (model == null)
//                {
//                    ViewBag.BrandId = id;
//                    return View(model);
//                }
//            }
//            else
//            {
//                ViewBag.BrandId = id;
//            }

//            return View(model);
//        }

//        [HttpPost]
//        public ActionResult Edit(Brands model)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    unitOfWork.BeginTransaction();
//                    var brandRepo = unitOfWork.Repository<Brands>();

//                    if (brandRepo.GetAll(x => x.BrandName.ToLower() == model.BrandName.ToLower() && x.BrandId != model.BrandId).Count() > 0)
//                    {
//                        ModelState.AddModelError("BrandName", "Brand Name is already exist. You should enter unique brand name.");
//                        unitOfWork.Rollback();
//                        return View(model);
//                    }
//                    //var brandRepo1 = unitOfWork.Repository<Brands>();
//                    model.UpdatedBy = SessionManager.Instance.CurrentUser.UserName;
//                    model.UpdatedOn = DateTime.Now;
//                    brandRepo.Update(model);
//                    unitOfWork.SaveChanges();
//                    unitOfWork.Commit();
//                    return RedirectToAction("Brands");
//                }
//                catch (Exception)
//                {
//                    unitOfWork.Rollback();
//                    return View("Error");
//                }
//            }
//            return View(model);
//        }

//        public ActionResult Brands()
//        {
//            List<Brands> lstBrands = new List<Brands>();
//            var brandRepo = unitOfWork.Repository<Brands>();
//            lstBrands = brandRepo.GetAll().ToList();

//            var productRepo = unitOfWork.Repository<Products>();
//            var products = productRepo.GetAll().ToList();
//            return View(lstBrands);
//        }

//        [HttpPost]
//        public ActionResult Delete(int id)
//        {
//            try
//            {
//                if (id > 0)
//                {
//                    try
//                    {
//                        unitOfWork.BeginTransaction();
//                        var brandRepo = unitOfWork.Repository<Brands>();

//                        var brand = brandRepo.Get(x => x.BrandId == id);
//                        if (brand != null)
//                        {
//                            brandRepo.Delete(brand);
//                            unitOfWork.Commit();
//                        }
//                    }
//                    catch (Exception)
//                    {
//                        unitOfWork.Rollback();
//                        return View("Error");
//                    }
//                }

//                return RedirectToAction("Brands");
//            }
//            catch
//            {
//                return RedirectToAction("Brands");
//            }
//        }
//        #endregion

//    }
//}
//==============================
//namespace RafCompare.Entities.CompanyEntities
//{
//    public class CompanyContacts
//    {
//        [Key]
//        public int ContactId { get; set; }

//        [ForeignKey("Companies")]
//        public int CompanyId { get; set; }
//        public virtual Company Companies { get; set; }

//        [Required(ErrorMessage = "Contact Name cannot be empty.")]
//        [StringLength(50)]
//        public string ContactName { get; set; }

//        [StringLength(18)]
//        public string Mobile { get; set; }
//        //{
//        //    get { return Mobile; }
//        //    set
//        //    {
//        //        if (!string.IsNullOrEmpty(value))
//        //            Mobile = Regex.Replace(value, @"[^0-9.-]", "");
//        //        else
//        //        {
//        //            Mobile = value;
//        //        }
//        //    }
//        //}

//        [StringLength(15)]
//        public string Phone { get; set; }

//        [StringLength(50)]
//        [DataType(DataType.EmailAddress)]
//        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please provide a valid email address.")]
//        public string Email { get; set; }

//[StringLength(200)]
//        [DataType(DataType.Url)]
//        [RegularExpression(@"^http(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$", ErrorMessage = "Please provide a valid URL.")]
//        public string URL { get; set; }
//    }
//}
