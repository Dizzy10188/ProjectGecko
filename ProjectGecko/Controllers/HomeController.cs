using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ProjectGecko.Models;

namespace ProjectGecko.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Posts = DatabaseConnection.GetAllPosts(true);

            ////connect to mongodb
            //var mongoClient = new MongoClient("mongodb+srv://admin:password1234@test-un7p6.azure.mongodb.net/test?retryWrites=true&w=majority").GetDatabase("AccountDB");
            ////return user by id
            //ViewBag.Posts = mongoClient.GetCollection<Post>("Posts").Find(x => true).ToList();
            Account account = Account.GetAccount(Request.Cookies["LoggedUser"]);
            if (account != null)
            {
                return View(account);
            }
            return View();
        }

        [HttpGet]
        public IActionResult LogInSignUp()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Search()
        {
            ViewBag.Posts = null;
            ViewBag.Accounts = null;
            return View();
        }

        [HttpPost]
        public IActionResult Search(string SearchBar)
        {
            ViewBag.Posts = DatabaseConnection.FindPosts(SearchBar);
            ViewBag.Accounts = DatabaseConnection.FindAccounts(SearchBar);
            return View();
        }
    }
}