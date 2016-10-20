using LeadManagement.Common;
using LeadManagement.Entities.CategoryEntities;
using LeadManagement.Entities.CompanyEntities;
using LeadManagement.Models.Category;
using LeadManagement.Models.Company;
using Repository.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LeadManagement.Controllers
{
    public class CategoryController : Controller
    {
        #region Declaration
        UnitOfWork uow = new UnitOfWork();
        [HttpGet]
        #endregion

        #region Actions
        public JsonResult Get()
        {
            Category category = new Category();
            CompanyBranch company = new CompanyBranch();
            CategoryGetResponse outPut = new CategoryGetResponse();
            var companyBranchRepository = uow.Repository<CompanyBranchRepository>();
            var branches = companyBranchRepository.GetAll();
            var companyRepository = uow.Repository<CompanyRepository>();
            var companys = companyRepository.GetAll();
            var categoryRepository = uow.Repository<CategoryRepository>();
            var categoryData = categoryRepository.GetAllCategory();

            var resultData = (from ld in categoryData
                              join bc in branches on ld.CompanyBranchId equals bc.CompanyBranchId
                              join cp in companys on bc.CompanyId equals cp.CompanyId
                              select new
                              {
                                  ld.CategoryId,
                                  ld.CategoryName,
                                  ld.CompanyBranchId,
                                  bc.BranchName,
                                  ld.CompanyId,
                                  cp.CompanyName
                              }).ToList();
            foreach (var item in resultData)
            {
                outPut.categoryList.Add(new CategoryList()
                {
                    CategoryId = item.CategoryId,
                    CategoryName = item.CategoryName,
                    BranchName = item.BranchName,
                    CompanyBranchId = item.CompanyBranchId,
                    CompanyId = item.CompanyId,
                    CompanyName=item.CompanyName,
                });
            }
            foreach (var item in branches)
            {
                outPut.Branches.Add(new CompanyBranchList()
                {
                    BranchName = item.BranchName,
                    CompanyBranchId = item.CompanyBranchId
                });
            }
            foreach (var item in companys)
            {
                outPut.companys.Add(new CompanyList()
                {
                    CompanyId = item.CompanyId,
                    CompanyName = item.CompanyName,
                });
            }
            return Json(outPut, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Add(CategoryModel model)
        {
            Category category = new Category();
            var categoryRepository = uow.Repository<CategoryRepository>();
            var companyBranchRepository = uow.Repository<CompanyBranchRepository>();
            var companyBranchData = companyBranchRepository.GetAll(x => x.CompanyBranchId == model.CompanyBranchId).ToList();
            ResponseOutput<CategoryModel> output = new ResponseOutput<CategoryModel>();
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToList();
                var errorOutput = new ResponseOutput<List<string>>();
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Response.StatusDescription = "Internal server ereor";
                errorOutput.Data = errors;
                return Json(errorOutput, JsonRequestBehavior.AllowGet);
            }
            if (categoryRepository.GetAll(x => x.CategoryName.ToLower() == model.CategoryName.ToLower() && x.CategoryId!= model.CategoryId).Count() > 0)
            {
                var errorOutput = new ResponseOutput<List<string>>();
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Response.StatusDescription = "Internal server error";
                errorOutput.Data = new List<string>();
                errorOutput.Data.Add("Category name already exist. You should enter unique category name");
                return Json(errorOutput, JsonRequestBehavior.AllowGet);
            }
            category.CategoryName = model.CategoryName;
            category.CompanyBranchId = model.CompanyBranchId;
            category.CompanyId = model.CompanyId;
            categoryRepository.AddCategory(category);
            Response.StatusCode = (int)HttpStatusCode.Created;
            Response.StatusDescription = "Created";
            output.Data = model;
            output.Message = "Data saved successfully.";
            return Json(output, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Edit(int id)
        {
            var output = new ResponseOutput<CategoryModel>();
            var categoryRepository = uow.Repository<CategoryRepository>();
            var companyRepository = uow.Repository<CompanyRepository>();
            var companyBranchRepository = uow.Repository<CompanyBranchRepository>();

            var categoryData = categoryRepository.GetAllCategoryById(id);
            var companyData = companyRepository.Get(x => x.CompanyId == categoryData.CompanyId);
            var companyBranchData = companyBranchRepository.Get(x => x.CompanyBranchId == categoryData.CompanyBranchId);
            Response.StatusCode = (int)HttpStatusCode.OK;

            CategoryModel categoryModel = new CategoryModel();
            categoryModel.CategoryName = categoryData.CategoryName;
            categoryModel.CategoryId =id;
            categoryModel.CompanyBranchId = categoryData.CompanyBranchId;
            categoryModel.CompanyId = categoryData.CompanyId;
            categoryModel.CompanyName = companyData.CompanyName;
            categoryModel.CompanyBranchName = companyBranchData.BranchName;
            output.Data = categoryModel;
            return Json(output, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(CategoryModel model)
        {
            var categoryRepository = uow.Repository<CategoryRepository>();
            ResponseOutput<CategoryModel> output = new ResponseOutput<CategoryModel>();
            var categoryData = categoryRepository.GetAllCategoryById(model.CategoryId);
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToList();
                var errorOutput = new ResponseOutput<List<string>>();
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Response.StatusDescription = "Internal server ereor";
                errorOutput.Data = errors;
                return Json(errorOutput, JsonRequestBehavior.AllowGet);
            }
            if (categoryRepository.GetAll(x => x.CategoryName.ToLower() == model.CategoryName.ToLower() && x.CategoryId != model.CategoryId).Count() > 0)
            {
                var errorOutput = new ResponseOutput<List<string>>();
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Response.StatusDescription = "Internal server error";
                errorOutput.Data = new List<string>();
                errorOutput.Data.Add("Category name already exist. You should enter unique category name");
                return Json(errorOutput, JsonRequestBehavior.AllowGet);
            }
            if (categoryData != null)
            {
                categoryData.CategoryName = model.CategoryName;
                categoryData.CompanyBranchId = model.CompanyBranchId;
                categoryData.CompanyId = model.CompanyId;
                uow.SaveChanges();
                output.Data = model;
                output.Message = "Data updated sucessfully";
            }
            return Json(output, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int id)
        {
            var categoryRepository = uow.Repository<CategoryRepository>();
            var categoryData = categoryRepository.GetAllCategoryById(id);
            ResponseOutput<Category> output = new ResponseOutput<Category>();
            if (categoryData != null)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Response.StatusDescription = "Internal server error";
                categoryRepository.DeleteCategoryById(id);
                output.Data = categoryData;
                output.Message = "Data delete successfully.";
            }
            return Json(output, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetBranchData(int companyId)
        {
            var companyBranchRepository = uow.Repository<CompanyBranchRepository>();
            CategoryGetResponse outPut = new CategoryGetResponse();
            var branchData = companyBranchRepository.GetAll(x => x.CompanyId == companyId).ToList();
            foreach (var item in branchData)
            {
                outPut.Branches.Add(new CompanyBranchList()
                {
                    BranchName = item.BranchName,
                    CompanyBranchId = item.CompanyBranchId,
                });
            }
            return Json(outPut, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
