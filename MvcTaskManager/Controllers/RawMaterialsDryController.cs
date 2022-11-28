using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcTaskManager.Identity;
using MvcTaskManager.Models;
using MvcTaskManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Controllers
{
  public class RawMaterialsDryController : Controller
  {

      private ApplicationDbContext db;
    public RawMaterialsDryController(ApplicationDbContext db)
      {
        this.db = db;
      }

      [HttpGet]
      [Route("api/rawmaterials")]
      [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

      public async Task<IActionResult> Get()
      {
      
        List<RawMaterialsDry> RM = await db.Raw_Materials_Dry.Where(temp => temp.is_active.Equals(true)).ToListAsync();
        List<RawMaterialsDryViewModels> rawmatsViewModel = new List<RawMaterialsDryViewModels>();
        foreach (var items in RM)
        {

        rawmatsViewModel.Add(new RawMaterialsDryViewModels()
          {
            Item_id = items.item_id,
            Item_code = items.item_code,
            Item_description = items.item_description,
            Item_class = items.item_class,
            Major_category = items.major_category.ToString(),
            Sub_category = items.sub_category,
            Primary_unit = items.primary_unit,
            Conversion = items.conversion,
            Item_type = items.item_type,
            Is_active = items.is_active
          });
        }
        return Ok(rawmatsViewModel);


      }



    }
}
