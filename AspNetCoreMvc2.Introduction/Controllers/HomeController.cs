using AspNetCoreMvc2.Introduction.Entities;
using AspNetCoreMvc2.Introduction.Filters;
using AspNetCoreMvc2.Introduction.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace AspNetCoreMvc2.Introduction.Controllers
{
    public class HomeController : Controller
    {
        public string Index()
        {
            return "Merhaba Dunya";
        }

        [HandleException(ViewName ="DivideByZeroError",ExceptionType=typeof(DivideByZeroException))] // Hata tipine göre error sayfaasına gönder
        public ViewResult Index2()
        {
            throw new SecurityException();
            return View();
        }
        public ViewResult Index3()
        {
            List<Employee> employees = new List<Employee>
            {
                new Employee{Id=1,FirstName="Eyyüp",LastName="Karabekmez",CityId=5},
                new Employee{Id=2,FirstName="Mustafa",LastName="Karabekmez",CityId=4},
                new Employee{Id=3,FirstName="Merve",LastName="Karabekmez",CityId=44}
            };
            List<string> cities = new List<string> { "İstanbul", "Ankara" };
            var model = new EmployeeListViewModel
            {
                Employees = employees,
                Cities = cities
            };
            return View(employees);
        }
        public IActionResult Index4()
        {
            return StatusCode(200);
        }
        public IActionResult Index5()
        {
            return StatusCode(400);
        }
        public JsonResult Index9()
        {
            List<Employee> employees = new List<Employee>
            {
                new Employee{Id=1,FirstName="Eyyüp",LastName="Karabekmez",CityId=5},
                new Employee{Id=2,FirstName="Mustafa",LastName="Karabekmez",CityId=4},
                new Employee{Id=3,FirstName="Merve",LastName="Karabekmez",CityId=44}
            };
            return Json(employees);
        }
        public IActionResult RazorDemo()
        {
            List<Employee> employees = new List<Employee>
            {
                new Employee{Id=1,FirstName="Eyyüp",LastName="Karabekmez",CityId=5},
                new Employee{Id=2,FirstName="Mustafa",LastName="Karabekmez",CityId=4},
                new Employee{Id=3,FirstName="Merve",LastName="Karabekmez",CityId=44}
            };
            List<string> cities = new List<string> { "İstanbul", "Ankara" };
            var model = new EmployeeListViewModel
            {
                Employees = employees,
                Cities = cities
            };
            return View(model);
        }

        public JsonResult Index10(string key)
        {
            List<Employee> employees = new List<Employee>
            {
                new Employee{Id=1,FirstName="Eyyüp",LastName="Karabekmez",CityId=6},
                new Employee{Id=2,FirstName="Mustafa",LastName="Karabekmez",CityId=44},
                new Employee{Id=3,FirstName="Merve",LastName="Karabekmez",CityId=44}
            };
            if (String.IsNullOrEmpty(key))
            {
                return Json(employees);

            }
            var result = employees.Where(e => e.FirstName.ToLower().Contains(key));

            return Json(result);
        }

        public ViewResult EmployeeForm()
        {
            return View();
        }

        //public string RouteData(int id)
        //{
        //    return id.ToString();
        //}
    }
}