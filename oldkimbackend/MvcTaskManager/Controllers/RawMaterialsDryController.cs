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
        //List<tblLocation> tblRejectedStatuses = await db.Location.ToListAsync();
        //return Ok(tblRejectedStatuses);
        List<RawMaterialsDry> departments = await db.Raw_Materials_Dry.Where(temp => temp.is_active.Equals(true)).ToListAsync();
        List<RawMaterialsDryViewModels> rawmatsViewModel = new List<RawMaterialsDryViewModels>();
        foreach (var dept in departments)
        {

        rawmatsViewModel.Add(new RawMaterialsDryViewModels()
          {
            Item_id = dept.item_id,
            Item_code = dept.item_code,
            Item_description = dept.item_description,
            Item_class = dept.item_class,
            Major_category = dept.major_category,
            Sub_category = dept.sub_category,
            Primary_unit = dept.primary_unit,
            Conversion = dept.conversion,
            Item_type = dept.item_type,
            Is_active = dept.is_active

          });
        }
        return Ok(rawmatsViewModel);


      }



    }
}
