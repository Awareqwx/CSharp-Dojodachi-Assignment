using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Dojodachi.Controllers
{
    public class DachiController : Controller
    {

        private static Random r = new Random();

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            Console.WriteLine("AAAAAAAAAAAAAAA");
            return View("Index");
        }

        [HttpGet]
        [Route("fetchdata")]
        public JsonResult Data()
        {
            if(!HttpContext.Session.TryGetValue("initialized", out var dummy))
            {
                initialize();
            }

            if(HttpContext.Session.GetInt32("happiness") <= 0) HttpContext.Session.SetInt32("happiness", 0);
            if(HttpContext.Session.GetInt32("fullness") <= 0) HttpContext.Session.SetInt32("fullness", 0);
            if(HttpContext.Session.GetInt32("energy") <= 0) HttpContext.Session.SetInt32("energy", 0);
            if(HttpContext.Session.GetInt32("meals") <= 0) HttpContext.Session.SetInt32("meals", 0);

            return Json(new {
                happiness = HttpContext.Session.GetInt32("happiness"),
                fullness = HttpContext.Session.GetInt32("fullness"),
                energy = HttpContext.Session.GetInt32("energy"),
                meals = HttpContext.Session.GetInt32("meals")
            });
        }

        [HttpGet]
        [Route("feed")]
        public JsonResult Feed()
        {
            if(HttpContext.Session.GetInt32("meals") < 1)
            {
                return Json(new {stats = Data(), message = "You don't have any food for your Dojodachi.", image = "Upset"});
            }
            if(HttpContext.Session.GetInt32("meals") < 1) return Data();
            HttpContext.Session.SetInt32("meals", (int) HttpContext.Session.GetInt32("meals") - 1);
            int rand = r.Next(4);
            int fed = 0;
            if(rand != 0)
            {
                fed = r.Next(5, 11);
                HttpContext.Session.SetInt32("fullness", (int) HttpContext.Session.GetInt32("fullness") + fed);
            }
            return Json(new {stats = Data(), message = "You fed your Dojodachi" + (rand != 0 ? $" and it gained {fed} fullness!" : ", but it didn't like it..."), image = rand != 0 ? "Happy" : "Angry"});
        }

        [HttpGet]
        [Route("play")]
        public JsonResult Play()
        {
            if(HttpContext.Session.GetInt32("energy") < 5)
            {
                return Json(new {stats = Data(), message = "Your Dojodachi is too tired to play.", image = "Upset"});
            }
            HttpContext.Session.SetInt32("energy", (int) HttpContext.Session.GetInt32("energy") - 5);
            int rand = r.Next(4);
            int happ = 0;
            if(rand != 0)
            {
                happ = r.Next(5, 11);
                HttpContext.Session.SetInt32("happiness", (int) HttpContext.Session.GetInt32("happiness") + happ);
            }
            return Json(new {stats = Data(), message = "You played with your Dojodachi" + (rand != 0 ? $" and it gained {happ} happiness!" : ", but it didn't like it..."), image = rand != 0 ? "Happy" : "Angry"});
        }

        [HttpGet]
        [Route("sleep")]
        public JsonResult Sleep()
        {
            HttpContext.Session.SetInt32("fullness", (int) HttpContext.Session.GetInt32("fullness") - 5);
            HttpContext.Session.SetInt32("happiness", (int) HttpContext.Session.GetInt32("happiness") - 5);
            HttpContext.Session.SetInt32("energy", (int) HttpContext.Session.GetInt32("energy") + 15);
            return Json(new {stats = Data(), message = "Your Dojodachi went to sleep and gained 15 energy.", image = "Sleep"});
        }

        [HttpGet]
        [Route("work")]
        public JsonResult Work()
        {
            if(HttpContext.Session.GetInt32("energy") < 5)
            {
                return Json(new {stats = Data(), message = "Your Dojodachi is too tired to work.", image = "Upset"});
            }
            HttpContext.Session.SetInt32("energy", (int) HttpContext.Session.GetInt32("energy") - 5);
            int meals = r.Next(1, 4);
            HttpContext.Session.SetInt32("meals", (int) HttpContext.Session.GetInt32("meals") + meals);
            return Json(new {stats = Data(), message = $"Your Dojodachi went to work and earned {meals} meals!", image = "Work"});
        }

        [HttpGet]
        [Route("reset")]
        public JsonResult Reset()
        {
            HttpContext.Session.Clear();
            return Data();
        }

        private void initialize()
        {
            HttpContext.Session.SetInt32("happiness", 20);
            HttpContext.Session.SetInt32("fullness", 20);
            HttpContext.Session.SetInt32("energy", 20);
            HttpContext.Session.SetInt32("meals", 3);
            HttpContext.Session.SetString("initialized", "true");
        }
    }
}