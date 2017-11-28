using ProyectoDiginamo.Models.Data;
using ProyectoDiginamo.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoDiginamo.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop/Categories
        public ActionResult Categories()
        {

            //Declare list of models
            List<CategoryVM> categoryVmList;

            using (Db db = new Db())
            {
                // Init the list
                categoryVmList = db.Categories
                    .ToArray()
                    .OrderBy(x => x.Sorting)
                    .Select(x => new CategoryVM(x))
                    .ToList();
            }
            // Return view with list

            return View(categoryVmList);
        }

        // POST: Admin/Shop/AddNewCategory
        [HttpPost]
        public string AddNewCategory(string catName)
        {

            // declare id
            string id;

            using (Db db = new Db())
            {

                // check category name is unique
                if (db.Categories.Any(x => x.Name == catName))
                      return "titletaken";



                // Init DTO
                CategoryDTO dto = new CategoryDTO();


                // Add to DTO
                dto.Name = catName;
                dto.Slug = catName.Replace(" ", "-").ToLower();
                dto.Sorting = 100;

                //Save DTO
                db.Categories.Add(dto);
                db.SaveChanges();


                //Get the id
                id = dto.Id.ToString();

            }
            //Return id
            return id;


        }

    }
}