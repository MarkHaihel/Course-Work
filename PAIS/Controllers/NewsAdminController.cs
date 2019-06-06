﻿using Microsoft.AspNetCore.Mvc;
using PAIS.Models;
using PAIS.Models.ViewModels;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace PAIS.Controllers
{
    [Authorize(Roles = "shop admin")]
    public class NewsAdminController : Controller
    {
        private INewsRepository repository;

        public NewsAdminController(INewsRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index() => View(repository.NewsRepo);

        public ViewResult Edit(int newsID)
        {
            News model = repository.NewsRepo
                   .FirstOrDefault(b => b.NewsID == newsID);

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(News model)
        {
            if (ModelState.IsValid)
            {
                repository.SaveNews(model);
                TempData["message"] = $"{model.Name} has been saved";
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(model);
            }
        }

        public ViewResult Create() => View("Edit", new News());

        [HttpPost]
        public IActionResult Delete(int newsID)
        {
            News deletedNews = repository.DeleteNews(newsID);

            if (deletedNews != null)
            {
                TempData["message"] = $"{deletedNews.Name} was deleted";
            }

            return RedirectToAction("Index");
        }
    }
}