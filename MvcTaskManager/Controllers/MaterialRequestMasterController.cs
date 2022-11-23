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
    [Route("api/material_request_master/mrs_orders")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetStoreOrders()
    {

      //List<MaterialRequestMaster> MRSParent = await db.Material_request_master.Where(x => x.Is_wh_checker_approval.Equals(true)).ToListAsync();

      //if (MRSParent.Count > 0)
      //{


      List<MaterialRequestMaster> obj = new List<MaterialRequestMaster>();
      var results = (from a in db.Material_request_master
                     join b in db.Material_request_logs on a.mrs_id equals b.mrs_id
                     join c in db.Department on a.department_id equals c.department_id

                     where
                     a.mrs_id == b.mrs_id &&
                     b.is_active.Equals(true) &&
                     a.is_for_validation.Contains("0") &&
                     a.is_approved_by != null &&
                     a.is_wh_sup_approval.Equals(true) &&
                     a.is_active.Equals(true) &&
                     a.is_prepared.Equals(false)
                     || a.Force_prepared_status != null
                     //|| b.is_wh_checker_cancel.Contains("1")

                     group a by new
                     {
                       a.mrs_id,
                       a.Is_wh_sup_approval_date,
                       a.department_id,
                       c.department_name,
                       a.mrs_req_desc,
                       a.mrs_requested_date,
                       a.mrs_requested_by,
                       a.is_approved_date,
                       a.is_active,
                       a.Is_wh_preparation_date,
                       TotalItems = b.is_active

                     } into total

                     select new

                     {

                       total.Key.mrs_id,
                       is_approved_prepa_date = total.Key.Is_wh_preparation_date,
                       department_id = total.Key.department_id,
                       department_name = total.Key.department_name,
                       mrs_req_desc = total.Key.mrs_req_desc,
                       mrs_requested_date = total.Key.mrs_requested_date,
                       mrs_requested_by = total.Key.mrs_requested_by,
                       is_active = total.Key.is_active,
                       TotalItems = total.Sum(x => Convert.ToInt32(total.Key.TotalItems)) + (from Order in db.Material_request_logs
                                                                                             where total.Key.mrs_id == Order.mrs_id
                                                                                             && total.Key.mrs_requested_date == Order.mrs_date_requested
                                                                                             && Order.is_active.Equals(false)
                                                                                             && Order.is_wh_checker_cancel.Contains("1")
                                                                                             select Order).Count(),
                       TotalPreparedItems = (from Order in db.Material_request_logs
                                             where Order.mrs_date_requested == total.Key.mrs_requested_date


                                             && total.Key.mrs_id == Order.mrs_id
                                             && Order.is_active.Equals(true)
                                             && Order.is_prepared.Equals(true)
                                             select Order).Count() - (from Order in db.Material_request_logs
                                                                      where total.Key.mrs_id == Order.mrs_id
                                                                      && total.Key.mrs_requested_date == Order.mrs_date_requested
                                                                      && Order.is_active.Equals(true)
                                                                      && Order.is_wh_checker_cancel.Contains("1")

                                                                      select Order).Count(),
                       TotalRejectItems = (from Order in db.Material_request_logs
                                           where total.Key.mrs_id == Order.mrs_id
                                           && total.Key.mrs_requested_date == Order.mrs_date_requested
                                           && Order.is_active.Equals(false)
                                           && Order.is_wh_checker_cancel.Contains("1")
                                           select Order).Count()
                     }



                    );
      //return Ok(results);



      var GetAllPreparedItems = await results.Where(x => x.TotalItems != x.TotalPreparedItems
      && x.is_active.Equals(true)
      || x.TotalPreparedItems != 0).ToListAsync();



      var result = await System.Threading.Tasks.Task.Run(() =>
      {
        return Ok(GetAllPreparedItems);
      });

      if (GetAllPreparedItems.Count() > 0)
      {
        return (result);
      }
      else
      {

        return NoContent();
      }

    }




    [HttpGet]
    [Route("api/material_request_master/mrs_orders/cancelled")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetCancelledOrders()
    {




      List<MaterialRequestMaster> obj = new List<MaterialRequestMaster>();
      var results = (from a in db.Material_request_master
                     join b in db.Material_request_logs on a.mrs_id equals b.mrs_id
                     join c in db.Department on a.department_id equals c.department_id

                     where
                     a.mrs_id == b.mrs_id &&
                     b.is_active.Equals(false) &&
                     a.is_for_validation.Contains("0") &&
                     a.is_approved_by != null &&
                     a.is_wh_sup_approval.Equals(true) &&
                     a.is_active.Equals(true) &&
                     a.is_prepared.Equals(false)
                     || a.Is_wh_checker_cancel != null
                     || a.Force_prepared_status != null
                     //|| b.is_wh_checker_cancel.Contains("1")

                     group a by new
                     {
                       a.mrs_id,
                       a.Is_wh_sup_approval_date,
                       a.department_id,
                       c.department_name,
                       a.mrs_req_desc,
                       a.mrs_requested_date,
                       a.mrs_requested_by,
                       a.is_approved_date,
                       a.is_active,
                       a.Is_wh_preparation_date,
                       TotalItems = b.is_active,
                       a.Is_wh_checker_cancel

                     } into total

                     select new

                     {

                       total.Key.mrs_id,
                       is_approved_prepa_date = total.Key.Is_wh_preparation_date,
                       department_id = total.Key.department_id,
                       department_name = total.Key.department_name,
                       mrs_req_desc = total.Key.mrs_req_desc,
                       mrs_requested_date = total.Key.mrs_requested_date,
                       mrs_requested_by = total.Key.mrs_requested_by,
                       is_active = total.Key.is_active,
                       is_wh_checker_cancel = total.Key.Is_wh_checker_cancel,
                       TotalItems = total.Sum(x => Convert.ToInt32(total.Key.TotalItems)) + (from Order in db.Material_request_logs
                                                                                             where total.Key.mrs_id == Order.mrs_id
                                                                                             && total.Key.mrs_requested_date == Order.mrs_date_requested
                                                                                             && Order.is_active.Equals(false)
                                                                                             && Order.is_wh_checker_cancel.Contains("1")
                                                                                             select Order).Count(),
                       TotalPreparedItems = (from Order in db.Material_request_logs
                                             where Order.mrs_date_requested == total.Key.mrs_requested_date


                                             && total.Key.mrs_id == Order.mrs_id
                                             && Order.is_active.Equals(true)
                                             && Order.is_prepared.Equals(true)
                                             select Order).Count() - (from Order in db.Material_request_logs
                                                                      where total.Key.mrs_id == Order.mrs_id
                                                                      && total.Key.mrs_requested_date == Order.mrs_date_requested
                                                                      && Order.is_active.Equals(true)
                                                                      && Order.is_wh_checker_cancel.Contains("1")

                                                                      select Order).Count(),
                       TotalRejectItems = (from Order in db.Material_request_logs
                                           where total.Key.mrs_id == Order.mrs_id
                                           && total.Key.mrs_requested_date == Order.mrs_date_requested
                                           && Order.is_active.Equals(false)
                                           && Order.is_wh_checker_cancel.Contains("1")
                                           select Order).Count()
                     }



                    );
      //return Ok(results);



      var GetAllPreparedItems = await results.Where(x => x.TotalItems != x.TotalPreparedItems
      && x.is_active.Equals(true)
      || x.TotalPreparedItems != 0).ToListAsync();



      var result = await System.Threading.Tasks.Task.Run(() =>
      {
        return Ok(GetAllPreparedItems);
      });

      if (GetAllPreparedItems.Count() > 0)
      {
        return (result);
      }
      else
      {

        return NoContent();
      }

    }

    [HttpGet]
    [Route("api/material_request_master/distinct_mrs_orders")]

    public async Task<ActionResult> GetDistinctPreparedOrders()
    {
List<MaterialRequestMaster> obj = new List<MaterialRequestMaster>();
      var results = (from a in db.Material_request_master
                     join b in db.Material_request_logs on a.mrs_id equals b.mrs_id
                     join c in db.Department on a.department_id equals c.department_id

                     where
                     a.mrs_id == b.mrs_id &&
                     b.is_active.Equals(true) &&

                     a.is_for_validation.Contains("0") &&
                     a.is_approved_by != null &&
                     a.is_wh_sup_approval.Equals(true) &&
                     a.is_active.Equals(true) &&
                     a.is_prepared.Equals(true)
                    && a.Is_wh_checker_approval.Equals(false)
                    && a.Is_wh_checker_cancel == null
                     || a.Force_prepared_status != null


                     group a by new
                     {
                       a.mrs_id,
                       a.Is_wh_sup_approval_date,
                       a.department_id,
                       c.department_name,
                       a.mrs_req_desc,
                       a.mrs_requested_date,
                       a.mrs_requested_by,
                       a.is_approved_date,
                       b.is_active,
                       a.Is_wh_preparation_date,
                       TotalItems = b.is_active,
                       a.Is_wh_checker_approval,
                       a.Is_wh_checker_approval_by,
                       a.Is_wh_checker_approval_date


                     } into total

                     select new

                     {
                       Id = total.Key.mrs_id,
                       is_approved_prepa_date = total.Key.Is_wh_preparation_date,
                       department_id = total.Key.department_id,
                       department_name = total.Key.department_name,
                       mrs_req_desc = total.Key.mrs_req_desc,
                       mrs_requested_date = total.Key.mrs_requested_date,
                       mrs_requested_by = total.Key.mrs_requested_by,
                       is_active = total.Key.is_active,
                       is_wh_checker_approval = total.Key.Is_wh_checker_approval,
                       is_wh_checker_approval_by = total.Key.Is_wh_checker_approval_by,
                       is_wh_checker_approval_date = total.Key.Is_wh_checker_approval_date,
                       total_state_repack = total.Sum(x => Convert.ToInt32(total.Key.TotalItems)),
                       TotalPreparedItems = (from Order in db.Material_request_logs
                                             where 
                                             Order.mrs_date_requested == total.Key.mrs_requested_date
                                             && total.Key.mrs_id == Order.mrs_id
                                             && Order.is_active.Equals(true)
                                             && Order.is_prepared.Equals(true)

                                             select Order).Count() + (from Order in db.Material_request_logs
                                                                      where total.Key.mrs_id == Order.mrs_id
                                                                      && Order.mrs_date_requested == total.Key.mrs_requested_date                        
                                                                      && Order.is_active.Equals(false)
                                                                      && Order.is_wh_checker_cancel.Contains("1")
                                                                      select Order).Count()
       
                     }

                    );

      var GetAllPreparedItems = await results.Where(x => x.total_state_repack == x.TotalPreparedItems).ToListAsync();

      var result = await System.Threading.Tasks.Task.Run(() =>
      {
        return Ok(GetAllPreparedItems);
      });

      if (GetAllPreparedItems.Count() > 0)
      {
        return (result);
      }
      else
      {

        return NoContent();
      }

    }



    [HttpGet]
    [Route("api/material_request_master/dispatching")]

    public async Task<ActionResult> GetDispatchingPreparedOrders()
    {
      List<MaterialRequestMaster> obj = new List<MaterialRequestMaster>();
      var results = (from a in db.Material_request_master
                     join b in db.Material_request_logs on a.mrs_id equals b.mrs_id
                     join c in db.Department on a.department_id equals c.department_id

                     where
                     a.mrs_id == b.mrs_id &&
                     b.is_active.Equals(true) &&

                     a.is_for_validation.Contains("0") &&
                     a.is_approved_by != null &&
                     a.is_wh_sup_approval.Equals(true) &&
                     a.is_active.Equals(true) &&
                     a.is_prepared.Equals(true)
                      && a.Is_wh_checker_approval.Equals(true)

                     || a.Force_prepared_status != null

                     group a by new
                     {
                       a.mrs_id,
                       a.Is_wh_sup_approval_date,
                       a.department_id,
                       c.department_name,
                       a.mrs_req_desc,
                       a.mrs_requested_date,
                       a.mrs_requested_by,
                       a.is_approved_date,
                       b.is_active,
                       a.Is_wh_preparation_date,
                       TotalItems = b.is_active,
                       a.Is_wh_checker_approval,
                       a.Is_wh_checker_approval_by,
                       a.Is_wh_checker_approval_date,
                       a.Wh_checker_move_order_no


                     } into total

                     select new

                     {
                       Id = total.Key.mrs_id,
                       is_approved_prepa_date = total.Key.Is_wh_preparation_date,
                       department_id = total.Key.department_id,
                       department_name = total.Key.department_name,
                       mrs_req_desc = total.Key.mrs_req_desc,
                       mrs_requested_date = total.Key.mrs_requested_date,
                       mrs_requested_by = total.Key.mrs_requested_by,
                       is_active = total.Key.is_active,
                       is_wh_checker_approval = total.Key.Is_wh_checker_approval,
                       is_wh_checker_approval_by = total.Key.Is_wh_checker_approval_by,
                       is_wh_checker_approval_date = total.Key.Is_wh_checker_approval_date,
                       wh_checker_move_order_no = total.Key.Wh_checker_move_order_no,
                       total_state_repack = total.Sum(x => Convert.ToInt32(total.Key.TotalItems)),
                       TotalPreparedItems = (from Order in db.Material_request_logs
                                             where
                                             Order.mrs_date_requested == total.Key.mrs_requested_date
                                             && total.Key.mrs_id == Order.mrs_id
                                             && Order.is_active.Equals(true)
                                             && Order.is_prepared.Equals(true)

                                             select Order).Count() + (from Order in db.Material_request_logs
                                                                      where total.Key.mrs_id == Order.mrs_id
                                                                      && Order.mrs_date_requested == total.Key.mrs_requested_date
                                                                      && Order.is_active.Equals(false)
                                                                      && Order.is_wh_checker_cancel.Contains("1")
                                                                      select Order).Count()

                     }

                    );

      var GetAllPreparedItems = await results.Where(x => x.total_state_repack == x.TotalPreparedItems).ToListAsync();

      var result = await System.Threading.Tasks.Task.Run(() =>
      {
        return Ok(GetAllPreparedItems);
      });

      if (GetAllPreparedItems.Count() > 0)
      {
        return (result);
      }
      else
      {

        return NoContent();
      }

    }



    [HttpGet]
    [Route("api/material_request_master/{user_id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<ActionResult<MaterialRequestMaster>> GetByUserID(int user_id)
    {


      string UserRole = "";
      bool IsRequestor = false;
      int ApproverOne = 0;
      int ApproverTwo = 0;
      int ApproverThree = 0;
      int ApproverFour = 0;
 

      var CheckifUserIsAdmin = await (from User in db.Users
                                        
                                      where User.User_Identity == user_id
                                      select new
                                      {
                                        UserRole = User.UserRole,
                                        ApproverOne = User.First_approver_id,
                                        ApproverTwo = User.Second_approver_id,
                                        ApproverThree = User.Third_approver_id,
                                        ApproverFour = User.Fourth_approver_id,
                                        IsRequestor = User.Requestor
                                      })

                       .ToListAsync();

      foreach (var item in CheckifUserIsAdmin)
      {
        UserRole = item.UserRole;
        ApproverOne = (int)item.ApproverOne;
        ApproverTwo = (int)item.ApproverTwo;
        ApproverThree = (int)item.ApproverThree;
        ApproverFour = (int)item.ApproverFour;
        IsRequestor = item.IsRequestor;


      }
      //return BadRequest(ApproverOne);
      //if (ApproverOne == user_id)
      //{
      //  SelectedApprover = 1;
      //  return BadRequest("1");
      //}

      //if (ApproverTwo == user_id)
      //{
      //  SelectedApprover = 2;
      //  return BadRequest("2");
      //}

      //if (ApproverThree == user_id)
      //{
      //  SelectedApprover = 3;
      //  return BadRequest("3");
      //}
      //if (ApproverFour == user_id)
      //{
      //  SelectedApprover = 4;
      //  return BadRequest("4");
      //}


      if (UserRole == "Admin")
      {

        var result = await (from Parents in db.Material_request_master
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
      else if(IsRequestor == true)
      {
        var result = await (from Parents in db.Material_request_master
                            join User in db.Users on Parents.user_id equals User.User_Identity
                            join Department in db.Department on Parents.department_id equals Department.department_id

                            where Parents.mrs_id == Parents.mrs_id

                              && Parents.user_id == user_id
                              //|| User.First_approver_id == user_id
                              //|| User.Second_approver_id == user_id
                              //|| User.Third_approver_id == user_id
                              //|| User.Fourth_approver_id == user_id

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
      else
      {
        //return NoContent();
        var result = await (from Parents in db.Material_request_master
                            join User in db.Users on Parents.user_id equals User.User_Identity
                            join Department in db.Department on Parents.department_id equals Department.department_id

                            where Parents.mrs_id == Parents.mrs_id
                               && Parents.is_active.Equals(true)

                              && Parents.is_approved_by == null

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

        var showActivatedData = result.Where(temp => temp.is_active.Equals(true) && temp.is_approved_by == null).ToList();
        return Ok(showActivatedData);
        
      }



    







    }





    [HttpGet]
    [Route("api/material_request_master/approved/{user_id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<ActionResult<MaterialRequestMaster>> GetApproved(int user_id)
    {

  
      string UserRole = "";
      bool IsRequestor = false;


      var CheckifUserIsAdmin = await (from User in db.Users

                                      where User.User_Identity == user_id
                                      select new
                                      {
                                        UserRole = User.UserRole,
                                        IsRequestor = User.Requestor
                                      })

                       .ToListAsync();

      foreach (var item in CheckifUserIsAdmin)
      {
        UserRole = item.UserRole;
        IsRequestor = item.IsRequestor;
      }

      //return BadRequest(IsRequestor);

      if (UserRole == "Admin")
      {
        //user_id = 0;
        var result = await (from Parents in db.Material_request_master
                            join User in db.Users on Parents.user_id equals User.User_Identity
                            join Department in db.Department on Parents.department_id equals Department.department_id

                            where Parents.mrs_id == Parents.mrs_id
                              && Parents.is_approved_by != null
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
      else if(IsRequestor == true)
      {
        //Start
        var result = await (from Parents in db.Material_request_master
                            join User in db.Users on Parents.user_id equals User.User_Identity
                            join Department in db.Department on Parents.department_id equals Department.department_id

                            where Parents.mrs_id == Parents.mrs_id

                              && Parents.user_id == user_id
                 

                              && Parents.is_active.Equals(true)
                                        && Parents.is_approved_by != null

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


        //End

      }


      else
      {
      

        var result = await (from Parents in db.Material_request_master
                            join User in db.Users on Parents.user_id equals User.User_Identity
                            join Department in db.Department on Parents.department_id equals Department.department_id

                            where Parents.mrs_id == Parents.mrs_id
                            
                              && Parents.user_id == user_id
                                                    && Parents.is_approved_by != null
                                                    &&  Parents.is_active.Equals(true)
                                                    
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

        var getApprovedData = result.Where(temp => temp.is_approved_by != null).ToList();

        return Ok(getApprovedData);
      }


    }







    [HttpGet]
    [Route("api/material_request_master/cancelled/{user_idx}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<ActionResult<MaterialRequestMaster>> GetCancelData(int user_idx)
    {


      string UserRole = "";
      bool IsRequestor = false;

      var CheckifUserIsAdmin = await (from User in db.Users

                                      where User.User_Identity == user_idx
                                      select new
                                      {
                                        UserRole = User.UserRole,
                                        IsRequestor = User.Requestor
                                      })

                       .ToListAsync();

      foreach (var item in CheckifUserIsAdmin)
      {
        UserRole = item.UserRole;
        IsRequestor = item.IsRequestor;
      }


      if (UserRole == "Admin")
      {
        //user_id = 0;
        var result = await (from Parents in db.Material_request_master
                            join User in db.Users on Parents.user_id equals User.User_Identity
                            join Department in db.Department on Parents.department_id equals Department.department_id

                            where Parents.mrs_id == Parents.mrs_id
                              //&& Parents.is_approved_by != null
                              && Parents.is_active.Equals(false)
               


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
        if (result.Count > 0)
        {
          
        }
        else
        {
          return NoContent();
        }

        return Ok(result);
      }
      else if (IsRequestor == true)
      {
        var result = await (from Parents in db.Material_request_master
                            join User in db.Users on Parents.user_id equals User.User_Identity
                            join Department in db.Department on Parents.department_id equals Department.department_id

                            where Parents.mrs_id == Parents.mrs_id
                            //&& Parents.is_approved_by != null

                            && Parents.user_id == user_idx

                            && Parents.is_active.Equals(false)

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

        if (result.Count > 0)
        {

        }
        else
        {
          return NoContent();
        }

        return Ok(result);
      }


      else
      {
        //return NoContent();
        var result = await (from Parents in db.Material_request_master
                            join User in db.Users on Parents.user_id equals User.User_Identity
                            join Department in db.Department on Parents.department_id equals Department.department_id

                            where Parents.mrs_id == Parents.mrs_id
                              //&& Parents.is_approved_by != null
                         
                            && Parents.user_id == user_idx
                                    && Parents.is_active.Equals(false)
                 
                            || User.First_approver_id == user_idx
                            || User.Second_approver_id == user_idx
                            || User.Third_approver_id == user_idx
                            || User.Fourth_approver_id == user_idx
                    

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

        if (result.Count > 0)
        {

        }
        else
        {
          return NoContent();
        }


        var FiltertheCancelData = result.Where(temp => temp.is_active.Equals(false)).ToList();

        return Ok(FiltertheCancelData);

      }


    }







    [HttpGet]
    [Route("api/material_request_master/search/{searchtext}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> SearchPerItem(int searchText)
    {


      List<MaterialRequestLogs> projects = null;

      int MrisID = searchText;
      //if (searchBy == "store_name")

      projects = await db.Material_request_logs.Where(temp => temp.is_active.Equals(true)
      && temp.mrs_id == MrisID
      && temp.is_wh_checker_cancel == null
      && temp.prepared_allocated_qty != null
      ).ToListAsync();


      return Ok(projects);

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
          //item.mrs_date_needed = MRSParams.mrs_date_needed;
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
    [Route("api/material_request_master/wh_checker_cancel")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<MaterialRequestMaster>> PutPreparationl([FromBody] MaterialRequestMaster MRSParams)
    {


      MaterialRequestMaster existingDataStatus = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).FirstOrDefaultAsync();

      var allToBeUpdated = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).ToListAsync();
      if (existingDataStatus != null)
      {
        foreach (var item in allToBeUpdated)
        {

          item.Is_wh_checker_cancel = "1";
          item.Is_wh_checker_cancel_by = MRSParams.Is_wh_checker_cancel_by;
          item.Is_wh_checker_cancel_reason = MRSParams.Is_wh_checker_cancel_reason;
          item.Is_wh_checker_cancel_date = DateTime.Now;
          item.Is_return_date = null;
          item.Is_return_by = null;

        }

        await db.SaveChangesAsync();

        var MaterialLogsPrepared = await db.Material_request_logs.Where(temp => temp.mrs_id == MRSParams.mrs_id).ToListAsync();
        if (MaterialLogsPrepared != null)
        {
          foreach (var item in MaterialLogsPrepared)
          {
            item.is_active = false;
            item.is_wh_checker_cancel = "1";
            item.cancel_reason = MRSParams.Is_wh_checker_cancel_reason;
            item.deactivated_by = MRSParams.Is_wh_checker_cancel_by;
            item.deactivated_date = DateTime.Now.ToString();
          }
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
    [Route("api/material_request_master/wh_checker_cancel/return")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<MaterialRequestMaster>> PutPreparationReturn([FromBody] MaterialRequestMaster MRSParams)
    {


      MaterialRequestMaster existingDataStatus = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).FirstOrDefaultAsync();

      var allToBeUpdated = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).ToListAsync();
      if (existingDataStatus != null)
      {
        foreach (var item in allToBeUpdated)
        {

          item.Is_wh_checker_cancel = null;
          item.Is_wh_checker_cancel_by = null;
          item.Is_wh_checker_cancel_reason = null;
          item.Is_wh_checker_cancel_date = null;

          item.Is_return_date = DateTime.Now.ToString();
          item.Is_return_by = MRSParams.Is_wh_checker_cancel_by;
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
    [Route("api/material_request_master/wh_checker_approval")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<MaterialRequestMaster> PutforWhCheckerApproval([FromBody] MaterialRequestMaster MRSParams)
    {
      MaterialRequestMaster existingDataStatus = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).FirstOrDefaultAsync();

      var allToBeUpdated = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).ToListAsync();
      if (existingDataStatus != null)
      {
        foreach (var item in allToBeUpdated)
        {

          item.Is_wh_checker_approval_by = MRSParams.Is_wh_checker_approval_by;
          item.Is_wh_checker_approval_date = DateTime.Now;
          item.Is_wh_checker_approval = true;
          item.Wh_checker_move_order_no = MRSParams.Wh_checker_move_order_no;
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
