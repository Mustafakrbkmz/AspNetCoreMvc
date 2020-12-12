using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWithAngular.Controllers
{
    [Produces("application/json")]
    [Route("api/Customers")]
    public class CustomersController : Controller
    {
        public List<Customer> Get()
        {
            return new List<Customer>
            {
            new Customer { Id = 1, FirstName = "Mustafa", LastName = "Karabekmez" },
            new Customer { Id = 2, FirstName = "Merve", LastName = "Karabekmez" },
            new Customer { Id = 3, FirstName = "Eyyüp", LastName = "Karabekmez" }
            };
        }
    }
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}