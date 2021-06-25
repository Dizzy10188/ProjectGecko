using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectGecko.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.RegularExpressions;
using MongoDB.Driver;

namespace ProjectGecko.Controllers
{
    public class UserController : Controller
    {
        public IActionResult ShowFeed()
        {
            return View();
        }
        public IActionResult ShowAccount(long userid)
        {
            Account user = Account.GetAccount(userid);
            ViewBag.Posts = DatabaseConnection.GetUserPosts(user.AccountID);
            ViewBag.CommNum = DatabaseConnection.GetCommissions(userid).Count();
            return View(user);
        }
        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateAccount(string DisplayName,
            string UserName,
            string PhoneNumber,
            string Email,
            string Password,
            IFormFile profileImage)
        {
            if (string.IsNullOrWhiteSpace(UserName) || Regex.Match(UserName, @"\s").Success)
            {
                ModelState.AddModelError("UserName", "UserName is Invalid. Username cannot be empty or have spaces within it");
                return Redirect("LogInSignUp");
            }
            else if (string.IsNullOrWhiteSpace(Email))
            {
                ModelState.AddModelError("Email", "Email is Required");
                return Redirect("LogInSignUp");
            }
            else if (string.IsNullOrWhiteSpace(Password))
            {
                ModelState.AddModelError("Password", "Password is Invaild");
                return Redirect("LogInSignUp");
            }
            else
            {
                bool userIsTaken = DatabaseConnection.GetAccount(UserName) != null;

                if (userIsTaken)
                {
                    ModelState.AddModelError("UserName", "UserName is taken");
                    return Redirect("LogInSignUp");
                }
                else if (Regex.Match(UserName, @"\s").Success)
                {
                    ModelState.AddModelError("UserName", "UserName may not contain spaces because it ruins file structure");
                    return Redirect("LogInSignUp");
                }
                else
                {
                    DisplayName = string.IsNullOrWhiteSpace(DisplayName) ? UserName : DisplayName;
                    if (ModelState.IsValid)
                    {
                        if (profileImage == null)
                        {
                            string pathForImage = "~/Images/Users/Default/defaultprofile.png";
                            Account account = new Account()
                            {
                                DisplayName = DisplayName,
                                UserName = UserName,
                                PhoneNumber = PhoneNumber,
                                Password = Password,
                                Email = Email,
                                ProfPicPath = pathForImage
                            };
                            DatabaseConnection.InsertAccount(account);
                            return Redirect("/");
                        }
                        else if (profileImage.ContentType.ToString() != "image/png" && profileImage.ContentType.ToString() != "image/jpeg" && profileImage.ContentType.ToString() != "image/jpg")
                        {
                            string pathForImage = "~/Images/Users/Default/defaultprofile.png";
                            Account account = new Account()
                            {
                                DisplayName = DisplayName,
                                UserName = UserName,
                                PhoneNumber = PhoneNumber,
                                Password = Password,
                                Email = Email,
                                ProfPicPath = pathForImage
                            };
                            DatabaseConnection.InsertAccount(account);
                            return Redirect("/");
                        }
                        else
                        {
                            //Grabs extension from image
                            var match = Regex.Match(profileImage.FileName, @"^.+(?<extension>\.[A-Za-z]+)$");
                            string ImageExtension = match.Groups["extension"].Value;
                            //constructs path for account image
                            string pathForImage = $"~/Images/Users/{UserName}/Profile/ProfilePicture" + ImageExtension;
                            //for local copy of image
                            string pathForCopy = $"wwwroot/Images/Users/{UserName}/Profile/ProfilePicture" + ImageExtension;

                            Directory.CreateDirectory($"wwwroot/Images/Users/{UserName}/Profile/");

                            //saves Image
                            using (FileStream stream = System.IO.File.OpenWrite(pathForCopy))
                            {
                                profileImage.CopyTo(stream);
                            }

                            Account account = new Account()
                            {
                                DisplayName = DisplayName,
                                UserName = UserName,
                                PhoneNumber = PhoneNumber,
                                Password = Password,
                                Email = Email,
                                AllowCommissions = "No",
                                ProfPicPath = pathForImage
                            };
                            DatabaseConnection.InsertAccount(account);
                            return Redirect("/");
                        }
                    }
                    return Redirect("LogInSignUp");
                }
            }
        }

        [HttpGet]
        public IActionResult RequestCommission()
        {
            string CommIdStr = Request.Query["CommissioneeId"];
            if (CommIdStr != null)
            {
                ViewBag.CommissioneeId = long.Parse(CommIdStr);
                return View();
            }
            else
            {
                return Redirect("/");
            }
        }

        [HttpPost]
        public IActionResult RequestCommission(long commissioneeID,
            IFormFile imagePath1,
            IFormFile imagePath2,
            IFormFile imagePath3,
            IFormFile imagePath4,
            IFormFile imagePath5,
            string description)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                ModelState.AddModelError("Description", "Please enter a description");
                return View();
            }
            else
            {
                if (imagePath1 != null || imagePath2 != null || imagePath3 != null || imagePath4 != null || imagePath5 != null)
                {
                    Commission newCommission = new Commission();
                    Account AccountPoster = Account.GetAccount(Request.Cookies["LoggedUser"]);
                    List<IFormFile> imagePaths = new List<IFormFile>();
                    //string[] pathList = new string[0];
                    if (imagePath1 != null)
                    {
                        imagePaths.Add(imagePath1);
                    }
                    if (imagePath2 != null)
                    {
                        imagePaths.Add(imagePath2);
                    }
                    if (imagePath3 != null)
                    {
                        imagePaths.Add(imagePath3);
                    }
                    if (imagePath4 != null)
                    {
                        imagePaths.Add(imagePath4);
                    }
                    if (imagePath5 != null)
                    {
                        imagePaths.Add(imagePath5);
                    }
                    string[] pathList = new string[imagePaths.Count()];
                    int i = 0;
                    foreach (var item in imagePaths)
                    {

                        var match = Regex.Match(item.FileName, @"^.+(?<extension>\.[A-Za-z]+)$");
                        string ImageExtension = match.Groups["extension"].Value;
                        string imageName = Regex.Replace(item.FileName, @"\s", "+");
                        string pathForImage = $"~/Images/Users/{AccountPoster.UserName}/Commissions/{AccountPoster.AccountID}/{commissioneeID}/{newCommission.CommissionID}/Image" + i + ImageExtension;
                        //for local copy of image
                        string pathForCopy = $"wwwroot/Images/Users/{AccountPoster.UserName}/Commissions/{AccountPoster.AccountID}/{commissioneeID}/{newCommission.CommissionID}/Image" + i + ImageExtension;
                        Directory.CreateDirectory($"wwwroot/Images/Users/{AccountPoster.UserName}/Commissions/{AccountPoster.AccountID}/{commissioneeID}/{newCommission.CommissionID}/");
                        pathList[i] = pathForImage;
                        using (FileStream stream = System.IO.File.OpenWrite(pathForCopy))
                        {
                            item.CopyTo(stream);
                        }

                        i++;
                    }
                    newCommission.CommissionerID = AccountPoster.AccountID;
                    newCommission.CommissioneeID = commissioneeID;
                    newCommission.ImagePaths = pathList;
                    newCommission.Description = description;

                    DatabaseConnection.InsertCommision(newCommission);

                    return Redirect("ShowFeed");
                }
                else
                {
                    return View();
                }
            }
        }

        [HttpPost]
        public IActionResult LogIn(string UserName, string Password)
        {
            Account LoginAttempt = Account.GetAccount(UserName);
            if (LoginAttempt == null)
            {
                ModelState.AddModelError("UserName", "Username Entered was incorrect");
                ModelState.AddModelError("Password", "Password Entered was incorrect");
                return Redirect("LogInSignUp");
            }
            else if (LoginAttempt.Password == Password)
            {
                Response.Cookies.Append("LoggedUser", LoginAttempt.UserName);
                return Redirect("/");
            }
            else
            {
                ModelState.AddModelError("Password", "Password Entered was incorrect");
                return Redirect("LogInSignUp");
            }
        }

        [HttpPost]
        public IActionResult Comment(string CommentText, string postId, string user)
        {
            Post p = Post.GetPost(long.Parse(postId));
            Comment c = new Comment() { CommentPoster = long.Parse(user), Text = CommentText };
            p.Comments.Add(c);
            DatabaseConnection.UpdatePost(p);
            return Redirect($"/{postId}/showpost");
        }

        [HttpGet]
        public IActionResult UserCommissions(long userid)
        {
            Account user = Account.GetAccount(userid);
            ViewBag.Commissions = DatabaseConnection.GetCommissionsForUser(user.AccountID);
            return View();
        }

        [HttpGet]
        public IActionResult EditAccount(long userid)
        {
            Account user = Account.GetAccount(userid);
            return View(user);
        }

        [HttpPost]
        public IActionResult EditAccount(long userID,
            string DisplayName,
            string Biography,
            string PhoneNumber,
            string Email,
            string Password,
            string PayPal,
            string AllowCommissions,
            byte UserCommLimit,
            IFormFile CommissionPricesImage,
            IFormFile profileImage)
        {
            Account user = Account.GetAccount(userID);

            var filter = Builders<Account>.Filter.Eq("Id", user._id);

            if(UserCommLimit != 0)
            {
                user.CommissionLimit = UserCommLimit;

            }

            if (string.IsNullOrWhiteSpace(DisplayName) != true)
            {
                user.DisplayName = DisplayName;
                //var updateDef = Builders<Account>.Update.Set("DisplayName", user.DisplayName);
                //DatabaseConnection.UpdateAccount(filter, updateDef);
            }
            if (string.IsNullOrWhiteSpace(Biography) != true)
            {
                user.Biography = Biography;
                //var updateDef = Builders<Account>.Update.Set("Biography", user.Biography);
                //DatabaseConnection.UpdateAccount(filter, updateDef);
            }
            if (string.IsNullOrWhiteSpace(PhoneNumber) != true)
            {
                user.PhoneNumber = PhoneNumber;
            }
            if (string.IsNullOrWhiteSpace(Email) != true)
            {
                user.Email = Email;
            }
            if (string.IsNullOrWhiteSpace(Password) != true)
            {
                user.Password = Password;
            }
            if (string.IsNullOrWhiteSpace(PayPal) != true)
            {
                user.PayPal = PayPal;
            }
            if (string.IsNullOrWhiteSpace(AllowCommissions) != true)
            {
                user.AllowCommissions = AllowCommissions;
            }
            if (CommissionPricesImage != null)
            {
                var match = Regex.Match(CommissionPricesImage.FileName, @"^.+(?<extension>\.[A-Za-z]+)$");
                string ImageExtension = match.Groups["extension"].Value;
                string pathForImage = $"~/Images/Users/{user.UserName}/Profile/CommissionPicture" + ImageExtension;
                string pathForCopy = $"wwwroot/Images/Users/{user.UserName}/Profile/CommissionPicture" + ImageExtension;
                using (FileStream stream = System.IO.File.OpenWrite(pathForCopy))
                {
                    CommissionPricesImage.CopyTo(stream);
                }
                user.CommissionPricesImage = pathForImage;
            }
            if (profileImage != null)
            {
                var match = Regex.Match(profileImage.FileName, @"^.+(?<extension>\.[A-Za-z]+)$");
                string ImageExtension = match.Groups["extension"].Value;
                string pathForImage = $"~/Images/Users/{user.UserName}/Profile/ProfilePicture" + ImageExtension;
                string pathForCopy = $"wwwroot/Images/Users/{user.UserName}/Profile/ProfilePicture" + ImageExtension;
                using (FileStream stream = System.IO.File.OpenWrite(pathForCopy))
                {
                    profileImage.CopyTo(stream);
                }
                user.ProfPicPath = pathForImage;
            }
            DatabaseConnection.UpdateAccount(user);
            return RedirectToAction("ShowAccount", "User");
        }
    }
}