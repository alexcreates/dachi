using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Dojodachi.Controllers
{

    public class DojodachiControllers : Controller
    {
        [HttpGet("")]
        public ViewResult Index()
        {
            Check();
            ViewBag.happiness = HttpContext.Session.GetInt32("happiness");
            ViewBag.fullness = HttpContext.Session.GetInt32("fullness");
            ViewBag.energy = HttpContext.Session.GetInt32("energy");
            ViewBag.meals = HttpContext.Session.GetInt32("meals");
            ViewBag.Response = HttpContext.Session.GetString("Response");
            ViewBag.Image = HttpContext.Session.GetString("Image");
            return View("Index");
        }



        [Route("feed")]
        public IActionResult Feed()
        {
            string response = "";
            int? mealVal = HttpContext.Session.GetInt32("meals");
            int? fullnessVal = HttpContext.Session.GetInt32("fullness");
            
            Random r = new Random();
            int randRes = r.Next(1,5);

            if(mealVal < 1)
            {
                HttpContext.Session.SetString("Response", "not enough meals");
                return RedirectToAction("Index");
            }
            if(randRes > 1)
            {
                int randFull = r.Next(5,10);
                fullnessVal += randFull;
                HttpContext.Session.SetInt32("fullness", (int)fullnessVal);
                response = "You fed your dojodachi! Gained: " + randFull + " fullness...";
            }
            else if(randRes < 2)
            {
                response = "Your dojodachi did not like the meal";
            }
            mealVal --;
            
            HttpContext.Session.SetInt32("meals", (int)mealVal);
            HttpContext.Session.SetString("Response", response);
            HttpContext.Session.SetString("Image", "eat.jpg");
            return RedirectToAction("Index");
        }

        [Route("play")]
        public IActionResult Play()
        {
            int? happVal = HttpContext.Session.GetInt32("happiness");            
            int? energyVal = HttpContext.Session.GetInt32("energy");
            Random r = new Random();
            string response = "";
            energyVal -= 5;

            if(r.Next(1,4) > 1)
            {
                int inc = r.Next(5, 10);
                happVal += inc;
                response = "You played with your Dojodachi and gained " + inc + "happiness.";

            }
            else
            {
                response = "Your dojodachi is in a bad mood and didn't like being played with";
            }
            
            HttpContext.Session.SetInt32("happiness",(int)happVal);            
            HttpContext.Session.SetInt32("energy", (int)energyVal);            
            HttpContext.Session.SetString("Response", response);
            HttpContext.Session.SetString("Image", "play.jpg");
            return RedirectToAction("Index");
        }

        [Route("work")]
        public IActionResult Work()
        {
            Random r = new Random();
            int? energyVal = HttpContext.Session.GetInt32("energy");
            int? mealVal = HttpContext.Session.GetInt32("meals");

            energyVal -= 5;
            int mealEarn = r.Next(1,5);
            mealVal += mealEarn;

            HttpContext.Session.SetInt32("energy", (int)energyVal);
            HttpContext.Session.SetInt32("meals", (int)mealVal);
            HttpContext.Session.SetString("Response", "Earned " + mealEarn + " meals!");
            HttpContext.Session.SetString("Image", "work.jpg");            

            return RedirectToAction("Index");
        }

        [Route("sleep")]
        public IActionResult Sleep()
        {
            int? fullnessVal = HttpContext.Session.GetInt32("fullness");
            int? happVal = HttpContext.Session.GetInt32("happiness");
            int? energyVal = HttpContext.Session.GetInt32("energy");

            fullnessVal -= 5;
            happVal += 5;
            energyVal += 5;

            HttpContext.Session.SetInt32("fullness", (int)fullnessVal);
            HttpContext.Session.SetInt32("happiness", (int)happVal);
            HttpContext.Session.SetInt32("energy", (int)energyVal);               
            HttpContext.Session.SetString("Response", "Your Dojodachio has slept");
            HttpContext.Session.SetString("Image", "sleep.jpg");            

            return RedirectToAction("Index");
        }


        public IActionResult Check()
        {
            int? happiness = HttpContext.Session.GetInt32("happiness");
            int? fullness = HttpContext.Session.GetInt32("fullness");
            int? energy = HttpContext.Session.GetInt32("energy");
            int? meals = HttpContext.Session.GetInt32("meals");
            if(happiness == null)
            {
                happiness = 20;
                HttpContext.Session.SetInt32("happiness", (int)happiness);
            }
            if(fullness == null)
            {
                fullness = 20;
                HttpContext.Session.SetInt32("fullness", (int)fullness);
            }
            if(energy == null)
            {
                energy = 20;
                HttpContext.Session.SetInt32("energy", (int)energy);
            }
            if(meals == null)
            {
                meals = 20;
                HttpContext.Session.SetInt32("meals", (int)meals);
            }
            if((int)happiness > 99 && (int)fullness > 99 || (int)happiness < 1 && (int)fullness < 1)
            {
                ViewBag.Happiness = HttpContext.Session.GetInt32("happiness");
                ViewBag.Fullness = HttpContext.Session.GetInt32("fullness");
                if((int)happiness > 99 && (int)fullness > 99)
                    HttpContext.Session.SetString("response", "You've Won!");
                    HttpContext.Session.SetString("Image", "won.jpg");                    
                if((int)happiness < 1 && (int)fullness < 1)
                    HttpContext.Session.SetString("response", "Your Dojodachi has died... :(");
                    HttpContext.Session.SetString("Image", "ded.jpg");
                return View("Complete");
            }
            return RedirectToAction("Index");
        }

        public IActionResult Restart()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Check");
        }



        
    }
}