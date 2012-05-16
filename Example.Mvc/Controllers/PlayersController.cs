﻿using System.Web.Mvc;
using Example.Model.Jobs;
using Simpler;

namespace Example.Mvc.Controllers
{
    public class PlayersController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var fetch = Job.New<FetchPlayers>();
            fetch.Run();
            var model = fetch.Out;

            return View(model);
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            var fetch = Job.New<FetchPlayer>();
            fetch.In.PlayerId = id;
            fetch.Run();
            var model = fetch.Out.Player;

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var fetch = Job.New<FetchPlayer>();
            fetch.In.PlayerId = id;
            fetch.Run();
            var model = fetch.Out.Player;

            return View(model);
        }

        [HttpPost]
        public ActionResult Update(UpdatePlayer.Input model)
        {
            if (!ModelState.IsValid)
            {
                var fetch = Job.New<FetchPlayer>();
                fetch.In.PlayerId = model.Player.PlayerId.GetValueOrDefault();
                fetch.Run();
                var editModel = fetch.Out;

                return View("Edit", editModel);
            }

            var update = Job.New<UpdatePlayer>();
            update.In.Player = model.Player;
            update.Run();

            return RedirectToAction("Show", new { id = model.Player.PlayerId });
        }
    }
}
