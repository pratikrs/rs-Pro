using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UOWFirstDemo.DataAccess.GenericRepositiry;
using UOWFirstDemo.DataAccess.RepositoryPattern;
using UOWFirstDemo.Entities;

namespace UOWFirstDemo.Controllers
{
    public class HomeController : Controller
    {
        #region Declaration
        UnitOfWork uow = new UnitOfWork();
        #endregion
        public ActionResult Index()
        {
            var studentRepository = uow.Repository<StudentRepository>();

            #region Insert
            //var _student = new StudentModel();
            //_student.FirstName = "fff";
            //_student.LastName = "lll";
            //_student.Phone = "2135648790";
            //_student.Email = "2@2.com";
            //_student.CountryId = 0;
            //_student.StateId = 0;
            //_student.ContactInfoes.Add(new ContactModel { Phone1 = "4564564560" });
            //_student.AddressInfoes.Add(new AddressModel { Address1 = "Add2", Address2 = "add4" });

            //studentRepository.InsertStudent(_student);
            #endregion

            #region GetById
            //var student = studentRepository.GetStudentById(2);
            #endregion

            #region update
            //var model = studentRepository.GetStudentById(3);
            //model.Email = "test@test.com";
            //studentRepository.UpdateStudent(model);
            #endregion

            #region Delete
            studentRepository.DeleteStudent(4);
            #endregion

            var stds = studentRepository.GetAllPassengers();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}