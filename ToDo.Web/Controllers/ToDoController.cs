﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ToDo.Web.Logic;
using ToDo.Web.Models;

namespace ToDo.Web.Controllers
{
    public class ToDoController : Controller
    {
        // GET: ToDo
        public async Task<ActionResult> Edit(Guid? id)
            ToDoItem item = await ServiceLogic.GetAsync<ToDoItem>(ConfigurationManager.AppSettings["ServiceUrl"] + "/" + id.ToString());

            if (item == null)
            return View(item);


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        {

            try
            {
                if (ModelState.IsValid)
                {
                    await ServiceLogic.PutAsync<bool>(ConfigurationManager.AppSettings["ServiceUrl"], item);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(item);
                }
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,Effort")]ToDoItem item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await ServiceLogic.PostAsync<Guid>(ConfigurationManager.AppSettings["ServiceUrl"], item);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Error");
            }
            return View(item);
        }

        public async Task<ActionResult> Delete(Guid? id)
        {

            ToDoItem item = await ServiceLogic.GetAsync<ToDoItem>(ConfigurationManager.AppSettings["ServiceUrl"] + "/" + id.ToString());

            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        public async Task<ActionResult> DeleteConfirmed(Guid? id)
        {
            if (id == null)

            await ServiceLogic.DeleteAsync<ToDoItem>(ConfigurationManager.AppSettings["ServiceUrl"] + "/" + id.ToString());
            return RedirectToAction("Index", "Home");
        }
    }
}