using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMvc2.Introduction.Entities;
using AspNetCoreMvc2.Introduction.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMvc2.Introduction.Controllers
{
    public class SessionDemoController : Controller
    {
        public string Index()
        {
            HttpContext.Session.SetInt32("age", 25);
            HttpContext.Session.SetString("name", "Mustafa");
            HttpContext.Session.SetObject("student",new Student { Email = "karabekmezmustafa@gmail.com", FirstName="Mustafa", LastName="Karabekmez",Id=1 }); //student adında key verdik ve bir student oluşturduk.
            return "Session Created";

        }

        public string GetSessions()
        {
            return String.Format("Hello {0}, you are {1}.Student is{2}", HttpContext.Session.GetString("name"), 
                HttpContext.Session.GetInt32("age"),
                HttpContext.Session.GetObject<Student>("student").FirstName);//student değeri için Student tipinde bir nesne istedik.
        }
    }
}