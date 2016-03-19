using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using AarhusWebDevCoop.ViewModels;
using System.Net.Mail;
using Umbraco.Core.Models;



namespace AarhusWebDevCoop.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {
        // GET: ContactFormSurface
        
        public ActionResult Index()
        {
            return PartialView("ContactForm", new ContactForm());
        }

        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model)
        {
           
            
                if (!ModelState.IsValid) { return CurrentUmbracoPage(); }
                // send mail

                IContent comment = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "Message");

                comment.SetValue("cname", model.Name);
                comment.SetValue("email", model.Email);
                comment.SetValue("subject", model.Subject);
                comment.SetValue("message", model.Message);

                //Save
                Services.ContentService.Save(comment);

            MailMessage message = new MailMessage();
            message.To.Add("umbracotest12@gmail.com");
            message.Subject = model.Subject;
            message.From = new MailAddress(model.Email, model.Name);
            message.Body = model.Message;

            using (SmtpClient smtp = new SmtpClient())
            {

                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("umbracotest12@gmail.com", "test1234567890");
                smtp.EnableSsl = true;

                smtp.Send(message);
                TempData["success"] = true;
            }

            
            
            return RedirectToCurrentUmbracoPage();
        }
    }
}