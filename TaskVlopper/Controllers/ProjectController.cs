﻿using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskVlopper.Helpers;
using TaskVlopper.Base.Logic;
using TaskVlopper.Base.Repository;
using TaskVlopper.Models;
using TaskVlopper.ServiceLocator;

namespace TaskVlopper.Controllers
{
    public class ProjectController : Controller
    {
        // GET: Project
        [HttpGet]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                using (IUnityContainer container = UnityConfig.GetConfiguredContainer())
                {
                    var repository = container.Resolve<IProjectRepository>();
                    var viewModel = new ProjectsViewModel(repository.GetAll().ToList());

                    return Json(viewModel, JsonRequestBehavior.AllowGet);
                }
            }
            Response.StatusCode = (int)HttpErrorCode.Forbidden;
            return View("Error");
        }

        // GET: Project/Details/5
        public ActionResult Details(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                using (IUnityContainer container = UnityConfig.GetConfiguredContainer())
                {
                    var repository = container.Resolve<IProjectRepository>();
                    var viewModel = new ProjectViewModel(repository.GetAll().ToList().Find(p => p.ID == id));

                    return Json(viewModel, JsonRequestBehavior.AllowGet);
                }
            }
            Response.StatusCode = (int)HttpErrorCode.Forbidden;
            return View("Error");
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                return PartialView("ModelEditor");
            }
            Response.StatusCode = (int)HttpErrorCode.Forbidden;
            return View("Error");
        }

        // POST: Project/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return Json(HttpNotFound());
            }
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                using (IUnityContainer container = UnityConfig.GetConfiguredContainer())
                {
                    var repository = container.Resolve<IProjectRepository>();
                    var viewModel = new ProjectViewModel(repository.GetAll().ToList().Find(p => p.ID == id));

                    return View("ModelEditor", viewModel);
                }
            }
            Response.StatusCode = (int)HttpErrorCode.Forbidden;
            return View("Error");

        }

        // POST: Project/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                Response.StatusCode = (int)HttpErrorCode.InternalServerError;
                return View("Error");
            }
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int id)
        {
            return Details(id);
        }

        // POST: Project/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return Json(HttpNotFound());
            }
        }
    }
}
