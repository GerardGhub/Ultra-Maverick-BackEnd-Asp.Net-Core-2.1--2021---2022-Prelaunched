using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcTaskManager.Identity;
using MvcTaskManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Controllers
{
  public class RoleModulesController : Controller
  {
    private readonly ApplicationDbContext db;
    
    public RoleModulesController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/RoleModules/{RoleId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<ActionResult<RoleModules>> GetByRoleID(int RoleId)
    {





      var result = await (from Parents in db.UserRoles
                          join User in db.Users on Parents.user_id equals User.User_Identity
                          join Department in db.Department on Parents.department_id equals Department.department_id

                          where Parents.mrs_id == Parents.mrs_id
                            && Parents.is_approved_by == null
                            && Parents.is_active.Equals(true)



                          select new
                          {
                            Parents.mrs_id,
                            Parents.mrs_req_desc,
                            Parents.mrs_requested_date,
                            Parents.mrs_requested_by,
                            Parents.department_id,
                            Department.department_name,
                            Parents.is_cancel_by,
                            Parents.is_cancel_reason,
                            Parents.is_cancel_date,
                            Parents.is_active,
                            Parents.is_approved_by,
                            Parents.is_approved_date,
                            Parents.updated_by,
                            Parents.updated_date,
                            Parents.is_prepared,
                            Parents.is_for_validation,
                            Parents.user_id,
                            Parents.mrs_date_needed,
                            User.First_approver_id,
                            User.First_approver_name,
                            User.Second_approver_id,
                            User.Second_approver_name,
                            User.Third_approver_id,
                            User.Third_approver_name,
                            User.Fourth_approver_id,
                            User.Fourth_approver_name,
                            total_request_count = (from Childs in db.Material_request_logs
                                                   where Parents.mrs_id == Childs.mrs_id
                                                   && Childs.is_active.Equals(true)
                                                   select Parents).Count(),



                            material_request_logs =
                                             from Childs in db.Material_request_logs
                                             where Parents.mrs_id == Childs.mrs_id && Childs.is_active.Equals(true)
                                             select new
                                             {
                                               Childs.id,
                                               Childs.mrs_id,
                                               Childs.mrs_item_code,
                                               Childs.mrs_item_description,
                                               Childs.mrs_order_qty,
                                               Childs.mrs_uom,
                                               Childs.mrs_served_qty,
                                               Childs.mrs_remarks,
                                               Childs.mrs_date_requested,
                                               Childs.is_active,
                                               Childs.is_prepared,
                                               Childs.is_prepared_date,
                                               Childs.is_prepared_by,
                                               Childs.is_wh_checker_cancel



                                             }
                          })

                      .ToListAsync();

      return Ok(result);




    }




    }



  
}
