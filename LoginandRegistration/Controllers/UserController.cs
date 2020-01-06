using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using LoginandRegistration.Models;


namespace LoginandRegistration.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified,ActivationCode")] tblUser user)
        {
            bool Status = false;
            string Message = "";
            //Validation
            if (ModelState.IsValid)
            {
                //Check User Exists or Not
                var isExist = IsEmailExist(user.Email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email Already Exists");
                    return View(user);
                }
                //Activation Code
                user.ActivationCode = Guid.NewGuid();

                //Password Hashing
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                user.IsEmailVerified = false;
                using (basicEntities db = new basicEntities())
                {
                    db.tblUsers.Add(user);
                    db.SaveChanges();

                    //Sending Verification Link
                    SendVerificationLinkEmail(user.Email, user.ActivationCode.ToString());
                    Message= "Registration successfully done. Account activation link " +
                " has been sent to your email id:" + user.Email;
                    Status = true;
                }
            }
            else
            {
                Message = "Invalid Request";
            }
            ViewBag.Status = Status;
            ViewBag.Message = Message;
            return View(user);
        }

        [NonAction]
        public bool IsEmailExist(string email)
        {
            using (basicEntities db = new basicEntities())
            {
                var v = db.tblUsers.Where(a => a.Email == email).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public void SendVerificationLinkEmail(string email, string activation)
        {
            var verifyUrl = "/User/VerifyAccount" + activation;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("abibhandari7@gmail.com", "Abinash Bhandari");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "abinash727";

            string subject = "Your Account Has Been Created Successfully!!!";
            string body = "<br/>We are excited to tell you that your Dotnet Awesome account is" +
                " successfully created. Please click on the below link to verify your account" +
                " <br/><br/><a href='" + link + "'>" + link + "</a> ";

            var SMTP = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var Message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            SMTP.Send(Message);
        }
    }
}