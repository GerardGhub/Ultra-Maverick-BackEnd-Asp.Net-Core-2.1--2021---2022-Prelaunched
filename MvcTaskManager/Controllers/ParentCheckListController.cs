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
  public class ParentCheckListController : Controller
  {

    private ApplicationDbContext db;
    public ParentCheckListController(ApplicationDbContext db)
    {
      this.db = db;
    }



    [HttpPut]
    [Route("api/parent_checklist")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<ParentCheckList>> Put([FromBody] ParentCheckListReturnEagerLoading parentRequestParam)
    {

      var ParentCheckListDataInfo = await db.Parent_checklist
        .Where(temp => temp.parent_chck_details == parentRequestParam.parent_chck_details).ToListAsync();

      if (ParentCheckListDataInfo.Count > 0)
      {
        return BadRequest(new { message = "You already have a duplicate request check the data to proceed" });
      }

      ParentCheckList existingDataStatus = await db.Parent_checklist.Where(temp => temp.parent_chck_id == parentRequestParam.parent_chck_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.parent_chck_details = parentRequestParam.parent_chck_details;
        existingDataStatus.updated_at = DateTime.Now.ToString();
        existingDataStatus.updated_by = parentRequestParam.updated_by;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }

    [HttpPut]
    [Route("api/parent_checklist/deactivate")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<ParentCheckList>> PutDeactivate([FromBody] ParentCheckListReturnEagerLoading parentRequestParam)
    {
      ParentCheckList existingDataStatus = await db.Parent_checklist.Where(temp => temp.parent_chck_id == parentRequestParam.parent_chck_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.is_active = false;
        existingDataStatus.deactivated_at = DateTime.Now.ToString();
        existingDataStatus.deactivated_by = parentRequestParam.deactivated_by;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }


    [HttpPut]
    [Route("api/parent_checklist/activate")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<ParentCheckList>> PutActivate([FromBody] ParentCheckListReturnEagerLoading parentRequestParam)
    {

      ParentCheckList existingDataStatus = await db.Parent_checklist.Where(temp => temp.parent_chck_id == parentRequestParam.parent_chck_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.is_active = true;
        existingDataStatus.deactivated_at = null;
        existingDataStatus.deactivated_by = null;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }



    [HttpPost]
    [Route("api/parent_checklist")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Post([FromBody] ParentCheckList parentRequestParam)
    {

      if (parentRequestParam.parent_chck_details == null || parentRequestParam.parent_chck_details == ""
        || parentRequestParam.parent_chck_added_by == null || parentRequestParam.parent_chck_added_by == "")
      {
        return BadRequest(new { message = "Fill up the required fields" });
      }

      var ParentCheckListDataInfo = await db.Parent_checklist.Where(temp => temp.parent_chck_details == parentRequestParam.parent_chck_details
      ).ToListAsync();

      if (ParentCheckListDataInfo.Count > 0)
      {
        return BadRequest(new { message = "You already have a duplicate request check the data to proceed" });
      }


      db.Parent_checklist.Add(parentRequestParam);
      await db.SaveChangesAsync();

      ParentCheckList existingProject = await db.Parent_checklist.Where(temp => temp.parent_chck_details == parentRequestParam.parent_chck_details).FirstOrDefaultAsync();

      ParentCheckListViewModel ParentViewModel = new ParentCheckListViewModel()
      {

        Parent_chck_id = existingProject.parent_chck_id,
        Parent_chck_details = existingProject.parent_chck_details,
        Parent_chck_added_by = existingProject.parent_chck_added_by,
        Parent_chck_date_added = existingProject.parent_chck_date_added,
        Is_active = existingProject.is_active
      };

      return Ok(ParentViewModel);

    }




    [HttpGet]
    [Route("api/parent_checklist/{parent_identity}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<IActionResult> GetParentCheckListById(int parent_identity)
    {

      List<ParentCheckList> AllParentData = await db.Parent_checklist.Where(temp => temp.parent_chck_id == parent_identity).ToListAsync();


      List<ParentCheckListViewModel> MaterialRequestViewModel = new List<ParentCheckListViewModel>();

      if (AllParentData.Count > 0)
      {

      }
      else
      {
        return NoContent();
      }

      foreach (var material in AllParentData)
      {

        MaterialRequestViewModel.Add(new ParentCheckListViewModel()
        {
          Parent_chck_id = material.parent_chck_id,
          Parent_chck_details = material.parent_chck_details,
          Parent_chck_added_by = material.parent_chck_added_by,
          Parent_chck_date_added = material.parent_chck_date_added,
          Is_active = material.is_active


        });
      }
      return Ok(MaterialRequestViewModel);


    }




    [HttpGet]
    [Route("api/parent_dynamic_checklist")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]


    public async Task<ActionResult<ParentCheckList>> GetDynamicChecklist()
    {

      //var TotalPartialReceiving = await db.ProjectsPartialPo.Where(temp => temp.Active.Equals(true)).ToListAsync();

      //return BadRequest(TotalPartialReceiving);

      var result = await (from ParentCheckList in db.Parent_checklist
                          //join User in db.Users on Parents.user_id equals User.User_Identity
                          //join Department in db.Department on Parents.department_id equals Department.department_id

                          where ParentCheckList.parent_chck_id == ParentCheckList.parent_chck_id
                      
                            && ParentCheckList.is_active.Equals(true)



                          select new
                          {
                            ParentCheckList.parent_chck_id,
                            ParentCheckList.parent_chck_details,
                            ParentCheckList.parent_chck_added_by,
                            ParentCheckList.parent_chck_date_added,
                            ParentCheckList.is_active,

                            childCheckLists =
                                             from Child_Checklist in db.Child_checklist
                                             where ParentCheckList.parent_chck_id == Child_Checklist.parent_chck_id && Child_Checklist.is_active.Equals(true)
                                             select new
                                             {
                                               Child_Checklist.cc_id,
                                               Child_Checklist.cc_description,
                                               Child_Checklist.cc_parent_key,
                                               Child_Checklist.parent_chck_details,
                                               Child_Checklist.parent_chck_id,                                         
                                               Child_Checklist.cc_added_by,
                                               Child_Checklist.cc_date_added,
                                               Child_Checklist.is_active,
                           grandChildCheckLists =
                                              from GrandChildChecklist in db.Grandchild_checklist
                                              where Child_Checklist.cc_id == GrandChildChecklist.cc_id && GrandChildChecklist.is_active.Equals(true)
                                                  select new
                                                  {
                                                    GrandChildChecklist.gc_id,
                                                    GrandChildChecklist.cc_id,
                                                    GrandChildChecklist.gc_child_key,
                                                    GrandChildChecklist.parent_chck_details,
                                                    GrandChildChecklist.parent_chck_id,
                                                    GrandChildChecklist.gc_added_by,
                                                    GrandChildChecklist.is_active,
                                                    GrandChildChecklist.gc_description,
                                                    GrandChildChecklist.is_manual,
                         checkListParameters =
                             from CheckListParams in db.Checklist_paramaters
                             where GrandChildChecklist.gc_id == CheckListParams.Gc_id && CheckListParams.Is_active.Equals(true)
                             select new
                             {
                               CheckListParams.Cp_params_id,
                               CheckListParams.Cp_description,
                               CheckListParams.Gc_id,
                               CheckListParams.Cp_gchild_key,
                               CheckListParams.Parent_chck_details,
                               CheckListParams.Cp_added_by,
                               CheckListParams.Cp_date_added,
                               CheckListParams.Is_active,
                               CheckListParams.Parent_chck_id,
                               CheckListParams.Cp_status,
                               CheckListParams.Manual_description
                            
                             }

                             }

                                                  }
                          })

                    .ToListAsync();

      return Ok(result);


    }

    [HttpGet]
    [Route("api/parent_dynamic_checklist_per_identity/{ProjectID}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]


    public async Task<ActionResult<ParentCheckList>> GetDynamicChecklistPerProjectID(int ProjectID)
    {

      var result = await (from Parent in db.Parent_checklist
                          where Parent.is_active.Equals(true)
                          //and
                          select new
                          {
                            ParentCheckList = from Parents in db.Parent_checklist
                                              where Parent.parent_chck_id == Parents.parent_chck_id && Parent.is_active.Equals(true)
                                              select new
                                              {
                                                Parents.parent_chck_id,
                                                Parents.parent_chck_details,
                                                Parents.is_active,



                                                childCheckLists =
                                                   from Childs in db.Child_checklist
                                                   where Parent.parent_chck_id == Childs.parent_chck_id && Parent.is_active.Equals(true)
                                                   select new
                                                   {
                                                     Childs.cc_id,
                                                     Childs.cc_description,
                                                     Childs.parent_chck_id,
                                                     Childs.parent_chck_details,
                                                     Childs.is_active,
                                                     grandChildCheckLists = from GrandChilds in db.Grandchild_checklist
                                                                            where Childs.cc_id == GrandChilds.cc_id && Childs.is_active.Equals(true)
                                                                            select new
                                                                            {
                                                                              GrandChilds.gc_id,
                                                                              GrandChilds.gc_description,
                                                                              GrandChilds.cc_id,
                                                                              GrandChilds.parent_chck_id,
                                                                              GrandChilds.parent_chck_details,
                                                                              GrandChilds.is_active,
                                                                              GrandChilds.is_manual,
                                                                              dynamicChecklistLoggers =
                                       from dynamicChecklistLoggers in db.Dynamic_checklist_logger
                                             where GrandChilds.gc_id == dynamicChecklistLoggers.gc_id && dynamicChecklistLoggers.ProjectID == ProjectID
                                             select new
                                             {
                                               dynamicChecklistLoggers.id,
                                               dynamicChecklistLoggers.manual_description,
                                               dynamicChecklistLoggers.ProjectID,
                                               dynamicChecklistLoggers.parent_chck_id,
                                               dynamicChecklistLoggers.parent_desc,
                                               dynamicChecklistLoggers.gc_id,
                                               dynamicChecklistLoggers.grand_child_desc,
                                               dynamicChecklistLoggers.cp_params_id,
                                               dynamicChecklistLoggers.cp_status,
                                               dynamicChecklistLoggers.cp_description

                                             }

                                                                            },
                                                     checkListParameters =
                                                   from Params in db.Checklist_paramaters
                                                   where Parent.parent_chck_id == Params.Parent_chck_id && Params.Is_active.Equals(true)
                                                   select new
                                                   {
                                                     Params.Cp_params_id,
                                                     Params.Cp_description,
                                                     Params.Parent_chck_id,
                                                     Params.Parent_chck_details,
                                                     Params.Gc_id,
                                                     Params.Is_active,
                                                     Params.Cp_status,
                                                     Params.Manual_description,
                                                     dynamicChecklistLoggers =
                                                   from dynamicChecklistLoggers in db.Dynamic_checklist_logger
                                                   where Params.Cp_params_id == dynamicChecklistLoggers.cp_params_id && dynamicChecklistLoggers.ProjectID == ProjectID
                                                   select new
                                                   {
                                                     dynamicChecklistLoggers.id,
                                                     dynamicChecklistLoggers.manual_description,
                                                     dynamicChecklistLoggers.ProjectID,
                                                     dynamicChecklistLoggers.parent_chck_id,
                                                     dynamicChecklistLoggers.parent_desc,
                                                     dynamicChecklistLoggers.gc_id,
                                                     dynamicChecklistLoggers.grand_child_desc,
                                                     dynamicChecklistLoggers.cp_params_id,
                                                     dynamicChecklistLoggers.cp_status,
                                                     dynamicChecklistLoggers.cp_description
                                                   }


                                                   }//child





                                   }
                                              }

                                              //};
                          }).ToListAsync();

      return Ok(result);





      //   var DynamicCheckList = await db.Parent_checklist.Where(d => d.is_active.Equals(true)
      //)
      //     .Include(a => a.ChildCheckLists)

      //     //new
      //     .ThenInclude(a1 => a1.GrandChildCheckLists)
      //     .ThenInclude(a2 => a2.DynamicChecklistLoggers)

      //     .Include(b => b.CheckListParameters)
      //     .ThenInclude(b1 => b1.DynamicChecklistLoggers)

      //     //.Where(d => d.is_active.Equals(true))
      //     .FirstOrDefaultAsync();

      //   if (DynamicCheckList == null)
      //   {
      //     return NotFound();
      //   }


      //   return Ok(DynamicCheckList);





    }




    [HttpGet]
    [Route("api/parent_dynamic_checklist_all")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]


    public async Task<ActionResult<ParentCheckList>> GetDynamicChecklistAllAnswer()
    {

      var result = await (from Parent in db.Parent_checklist
                          where Parent.is_active.Equals(true)
                          //and
                          select new
                          {
                            ParentCheckList = from Parents in db.Parent_checklist
                                              where Parent.parent_chck_id == Parents.parent_chck_id && Parent.is_active.Equals(true)
                                              select new
                                              {
                                                Parents.parent_chck_id,
                                                Parents.parent_chck_details,
                                                Parents.is_active,



                                                childCheckLists =
                                                   from Childs in db.Child_checklist
                                                   where Parent.parent_chck_id == Childs.parent_chck_id && Parent.is_active.Equals(true)
                                                   select new
                                                   {
                                                     Childs.cc_id,
                                                     Childs.cc_description,
                                                     Childs.parent_chck_id,
                                                     Childs.parent_chck_details,
                                                     Childs.is_active,
                                                     grandChildCheckLists = from GrandChilds in db.Grandchild_checklist
                                                                            where Childs.cc_id == GrandChilds.cc_id && Childs.is_active.Equals(true)
                                                                            select new
                                                                            {
                                                                              GrandChilds.gc_id,
                                                                              GrandChilds.gc_description,
                                                                              GrandChilds.cc_id,
                                                                              GrandChilds.parent_chck_id,
                                                                              GrandChilds.parent_chck_details,
                                                                              GrandChilds.is_active,
                                                                              dynamicChecklistLoggers =
                                       from dynamicChecklistLoggers in db.Dynamic_checklist_logger
                                       where GrandChilds.gc_id == dynamicChecklistLoggers.gc_id && GrandChilds.is_active.Equals(true)
                                       select new
                                       {
                                         dynamicChecklistLoggers.id,
                                         dynamicChecklistLoggers.manual_description,
                                         dynamicChecklistLoggers.ProjectID,
                                         dynamicChecklistLoggers.parent_chck_id,
                                         dynamicChecklistLoggers.parent_desc,
                                         dynamicChecklistLoggers.gc_id,
                                         dynamicChecklistLoggers.grand_child_desc,
                                         dynamicChecklistLoggers.cp_params_id,
                                         dynamicChecklistLoggers.cp_status
                                       }

                                                                            },
                                                     checkListParameters =
                                                   from Params in db.Checklist_paramaters
                                                   where Parent.parent_chck_id == Params.Parent_chck_id && Parent.is_active.Equals(true)
                                                   select new
                                                   {
                                                     Params.Cp_params_id,
                                                     Params.Cp_description,
                                                     Params.Parent_chck_id,
                                                     Params.Parent_chck_details,
                                                     Params.Gc_id,
                                                     Params.Is_active,
                                                     Params.Cp_status,
                                                     Params.Manual_description,
                                                     dynamicChecklistLoggers =
                                                   from dynamicChecklistLoggers in db.Dynamic_checklist_logger
                                                   where Params.Cp_params_id == dynamicChecklistLoggers.cp_params_id 
                                                   select new
                                                   {
                                                     dynamicChecklistLoggers.id,
                                                     dynamicChecklistLoggers.manual_description,
                                                     dynamicChecklistLoggers.ProjectID,
                                                     dynamicChecklistLoggers.parent_chck_id,
                                                     dynamicChecklistLoggers.parent_desc,
                                                     dynamicChecklistLoggers.gc_id,
                                                     dynamicChecklistLoggers.grand_child_desc,
                                                     dynamicChecklistLoggers.cp_params_id,
                                                     dynamicChecklistLoggers.cp_status
                                                   }


                                                   }//child





                                                   }
                                              }

                                              //};
                          }).ToListAsync();

      return Ok(result);









    }





    [HttpGet]
    [Route("api/parent_checklist")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<IActionResult> GetParentCheckList()
    {

      List<ParentCheckList> AllParentData = await db.Parent_checklist.ToListAsync();


      List<ParentCheckListViewModel> MaterialRequestViewModel = new List<ParentCheckListViewModel>();

      if (AllParentData.Count > 0)
      {

      }
      else
      {
        return NoContent();
      }

      foreach (var material in AllParentData)
      {

        MaterialRequestViewModel.Add(new ParentCheckListViewModel()
        {
          Parent_chck_id = material.parent_chck_id,
          Parent_chck_details = material.parent_chck_details,
          Parent_chck_added_by = material.parent_chck_added_by,
          Parent_chck_date_added = material.parent_chck_date_added,         
          Is_active = material.is_active


        });
      }
      return Ok(MaterialRequestViewModel);


    }
  }








  ///
}

