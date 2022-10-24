using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcTaskManager.Identity;
using MvcTaskManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MvcTaskManager.Controllers
{
  public class MainMenusController : Controller
  {
    private ApplicationDbContext db;

    public MainMenusController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/MainMenus")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get()
    {
      List<MainMenus> tblLabProc = await db.Main.ToListAsync();
      return Ok(tblLabProc);
    }




  }


}
