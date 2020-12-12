using Microsoft.AspNetCore.Mvc;
using System;

namespace AspNetCoreMvc2.Introduction.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        [Route("")]
        [Route("Save")]
        [Route("~/save")]
        public string Save()
        {
            return "Saved";
        }

        [Route("Delete/{id?}")]
        public string Delete(int id = 0)
        {
            return String.Format("Deleted {0}", id);
        }


        [Route("Update")]

        public string Update()
        {
            return "Updated";
        }

    }
}