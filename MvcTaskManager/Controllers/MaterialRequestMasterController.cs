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
  public class MaterialRequestMasterController : Controller
  {
    private ApplicationDbContext db;
    public MaterialRequestMasterController(ApplicationDbContext db)
    {
      this.db = db;
    }




    [HttpGet]
    [Route("api/material_request_master/{user_id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<ActionResult<MaterialRequestMaster>> Get(int user_id)
    {

      //List<MaterialRequestLogs> allmrs = await db.Material_request_logs.Where(temp => temp.is_active.Equals(true) && temp.mrs_id.ToString().Contains(transact_no_passed_by)).ToListAsync();



      var CheckifUserIsAdmin = await (from User in db.Users
                          //join User in db.Users on Parents.user_id equals User.User_Identity
                          //join Department in db.Department on Parents.department_id equals Department.department_id
                  where User.UserRole == "Admin"
      select new
        {
        User.UserName
        }).ToListAsync();



      if (CheckifUserIsAdmin.Count > 0)
      {
        //user_id = 0;
        var result = await (from Parents in db.Material_request_master
                            join User in db.Users on Parents.user_id equals User.User_Identity
                            join Department in db.Department on Parents.department_id equals Department.department_id

                            where Parents.mrs_id == Parents.mrs_id
                              && Parents.is_approved_by == null
                              && Parents.is_active.Equals(true)
                              //&& Parents.user_id == user_id
                              //|| User.First_approver_id == user_id
                              // || User.Second_approver_id == user_id
                              //  || User.Third_approver_id == user_id
                              //   || User.Fourth_approver_id == user_id


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
      else
      {
        //return NoContent();
        var result = await (from Parents in db.Material_request_master
                            join User in db.Users on Parents.user_id equals User.User_Identity
                            join Department in db.Department on Parents.department_id equals Department.department_id

                            where Parents.mrs_id == Parents.mrs_id
                              && Parents.is_approved_by == null
                              && Parents.is_active.Equals(true)
                              && Parents.user_id == user_id
                              || User.First_approver_id == user_id
                               || User.Second_approver_id == user_id
                                || User.Third_approver_id == user_id
                                 || User.Fourth_approver_id == user_id


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





    [HttpGet]
    [Route("api/material_request_master/approved/{user_id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<ActionResult<MaterialRequestMaster>> GetApproved(int user_id)
    {

      //List<MaterialRequestLogs> allmrs = await db.Material_request_logs.Where(temp => temp.is_active.Equals(true) && temp.mrs_id.ToString().Contains(transact_no_passed_by)).ToListAsync();



      var CheckifUserIsAdmin = await (from User in db.Users
                                        //join User in db.Users on Parents.user_id equals User.User_Identity
                                        //join Department in db.Department on Parents.department_id equals Department.department_id
                                      where User.UserRole == "Admin"
                                      select new
                                      {
                                        User.UserName
                                      }).ToListAsync();



      if (CheckifUserIsAdmin.Count > 0)
      {
        //user_id = 0;
        var result = await (from Parents in db.Material_request_master
                            join User in db.Users on Parents.user_id equals User.User_Identity
                            join Department in db.Department on Parents.department_id equals Department.department_id

                            where Parents.mrs_id == Parents.mrs_id
                              && Parents.is_approved_by != null
                              && Parents.is_active.Equals(true)
                            //&& Parents.user_id == user_id
                            //|| User.First_approver_id == user_id
                            // || User.Second_approver_id == user_id
                            //  || User.Third_approver_id == user_id
                            //   || User.Fourth_approver_id == user_id


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
      else
      {
        //return NoContent();
        var result = await (from Parents in db.Material_request_master
                            join User in db.Users on Parents.user_id equals User.User_Identity
                            join Department in db.Department on Parents.department_id equals Department.department_id

                            where Parents.mrs_id == Parents.mrs_id
                              && Parents.is_approved_by != null
                              && Parents.is_active.Equals(true)
                              && Parents.user_id == user_id
                              || User.First_approver_id == user_id
                               || User.Second_approver_id == user_id
                                || User.Third_approver_id == user_id
                                 || User.Fourth_approver_id == user_id


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



    [HttpGet]
    [Route("api/material_request_master/cancel")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<IActionResult> GetCancelMRS()
    {

      List<MaterialRequestMaster> allmrs = await db.Material_request_master.Where(temp => temp.is_active.Equals(false)).ToListAsync();
      List<MaterialRequestMasterViewModel> MaterialRequestViewModel = new List<MaterialRequestMasterViewModel>();
      foreach (var material in allmrs)
      {

        MaterialRequestViewModel.Add(new MaterialRequestMasterViewModel()
        {
          Mrs_id = material.mrs_id,
          Mrs_req_desc = material.mrs_req_desc,
          Mrs_requested_by = material.mrs_requested_by,
          Mrs_requested_date = material.mrs_requested_date,
          Department_id = material.department_id,
          Is_cancel_by = material.is_cancel_by,
          Is_cancel_reason = material.is_cancel_reason,
          Is_cancel_date = material.is_cancel_date,
          Is_active = material.is_active,
          Is_approved_by = material.is_approved_by,
          Is_approved_date = material.is_approved_date,
          User_id = material.user_id,
          Is_prepared = material.is_prepared


        });
      }
      return Ok(MaterialRequestViewModel);


    }



    [HttpPut]
    [Route("api/material_request_master/deactivate")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<MaterialRequestMaster> PutCancelAll([FromBody] MaterialRequestMaster MRSParams)
    {
      MaterialRequestMaster existingDataStatus = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).FirstOrDefaultAsync();

      var allToBeUpdated = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).ToListAsync();
      if (existingDataStatus != null)
      {
        foreach (var item in allToBeUpdated)
        {

          item.is_active = false;
          item.is_cancel_reason = MRSParams.is_cancel_reason;
          item.is_cancel_by = MRSParams.is_cancel_by;
          item.is_cancel_date = DateTime.Now.ToString("M/d/yyyy");

        }

        await db.SaveChangesAsync();
        return existingDataStatus;

      }
      else
      {
        return null;
      }
    }



    [HttpPut]
    [Route("api/material_request_master")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<MaterialRequestMaster>> PutUpdateAll([FromBody] MaterialRequestMaster MRSParams)
    {

      var CheckParametersKey =
        await db.Material_request_master.Where(temp => temp.mrs_req_desc.ToString() == MRSParams.mrs_req_desc).ToListAsync();

      if (CheckParametersKey.Count > 0)
      {
        return BadRequest(new { message = "You already have a duplicate request check the data to proceed" });
      }
 



      MaterialRequestMaster existingDataStatus = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).FirstOrDefaultAsync();

      var allToBeUpdated = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).ToListAsync();
      if (existingDataStatus != null)
      {
        foreach (var item in allToBeUpdated)
        {

          item.mrs_req_desc = MRSParams.mrs_req_desc;
          item.updated_by = MRSParams.updated_by;
          item.mrs_date_needed = MRSParams.mrs_date_needed;
          item.updated_date = DateTime.Now.ToString("M/d/yyyy");

        }

        await db.SaveChangesAsync();
        return existingDataStatus;

      }
      else
      {
        return null;
      }
    }







    [HttpPut]
    [Route("api/material_request_master/approve")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<MaterialRequestMaster> PutforApprove([FromBody] MaterialRequestMaster MRSParams)
    {
      MaterialRequestMaster existingDataStatus = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).FirstOrDefaultAsync();

      var allToBeUpdated = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).ToListAsync();
      if (existingDataStatus != null)
      {
        foreach (var item in allToBeUpdated)
        {

          item.is_approved_by = MRSParams.is_approved_by;
          item.is_approved_date = DateTime.Now.ToString("M/d/yyyy");

        }

        await db.SaveChangesAsync();
        return existingDataStatus;

      }
      else
      {
        return null;
      }
    }






    [HttpPut]
    [Route("api/material_request_master/dis-approve")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<MaterialRequestMaster> PutDisApprove([FromBody] MaterialRequestMaster MRSParams)
    {
      MaterialRequestMaster existingDataStatus = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).FirstOrDefaultAsync();

      var allToBeUpdated = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).ToListAsync();
      if (existingDataStatus != null)
      {
        foreach (var item in allToBeUpdated)
        {

     
          item.is_cancel_by = MRSParams.is_cancel_by;
          item.is_cancel_date = DateTime.Now.ToString("M/d/yyyy");
          item.is_cancel_reason = MRSParams.is_cancel_reason;
          item.is_approved_by = null;
          item.is_approved_date = null;
          item.is_active = false;

        }

        await db.SaveChangesAsync();
        return existingDataStatus;

      }
      else
      {
        return null;
      }
    }


    [HttpPut]
    [Route("api/material_request_master/activate")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<MaterialRequestMaster> PutReturnAll([FromBody] MaterialRequestMaster MRSParams)
    {
      MaterialRequestMaster existingDataStatus = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).FirstOrDefaultAsync();

      var allToBeUpdated = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).ToListAsync();
      if (existingDataStatus != null)
      {
        foreach (var item in allToBeUpdated)
        {

          item.is_active = true;
          item.is_cancel_reason = null;
          item.is_cancel_by = null;
          item.is_cancel_date = null;

        }

        await db.SaveChangesAsync();
        return existingDataStatus;

      }
      else
      {
        return null;
      }
    }



    [HttpPost]
    [Route("api/material_request_master")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Post([FromBody] MaterialRequestMaster materialRequest)
    {



      var RawDataInfo = await db.Material_request_master.Where(temp => temp.mrs_requested_date == materialRequest.mrs_requested_date
      && temp.mrs_req_desc == materialRequest.mrs_req_desc && materialRequest.is_active.Equals(true)).ToListAsync();

      if (RawDataInfo.Count > 0)
      {
        return BadRequest(new { message = "You already have a duplicate request check the data to proceed" });
      }
   

        db.Material_request_master.Add(materialRequest);
        await db.SaveChangesAsync();
      
        MaterialRequestMaster existingProject = await db.Material_request_master.Where(temp => temp.mrs_id == materialRequest.mrs_id).FirstOrDefaultAsync();

      


      MaterialRequestMasterViewModel MRISViewModel = new MaterialRequestMasterViewModel()
      {

        Mrs_id = existingProject.mrs_id,
        Mrs_req_desc = existingProject.mrs_req_desc,
        Mrs_requested_by = existingProject.mrs_requested_by,
        Mrs_date_needed = existingProject.mrs_date_needed,
        Mrs_requested_date = DateTime.Now.ToString(),
        Department_id = existingProject.department_id,
        Is_active = true,
        User_id = existingProject.user_id


      };

      return Ok(MRISViewModel);

    }




  }





}
