using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcTaskManager.Identity;
using MvcTaskManager.Models;
using MvcTaskManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Http.Headers;

namespace MvcTaskManager.Controllers
{
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public class tblDryWHReceivingController : Controller
  {
    private ApplicationDbContext db;
    private IHostingEnvironment _hostingEnvironment;
    public int LaboratoryAging { get; set; }
    public int dayDiffExpiryDaysAging {get; set;}

    public tblDryWHReceivingController(ApplicationDbContext db, IHostingEnvironment hostingEnvironment)
    {
      this.db = db;
      _hostingEnvironment = hostingEnvironment;
    }



    [HttpGet]
    [Route("api/DryWareHouseReceivingForLabTest/LabResultWithAccessCode")]
    public async Task<IActionResult> GetLabResultWithAccessCode()
    {
      var GetResultWithAccessCode = db.Dry_wh_lab_test_req_logs

        .Include(x => x.DryWareHouseReceiving).ToList()
        .GroupBy(d => new {
          d.Lab_access_code,
          d.Add_access_code_by,
          d.add_access_code_date,
          d.Is_active,
          d.Qa_supervisor_is_approve_status,
          d.Tsqa_Approval_Status
        })

        .Where(temp => temp.Key.Is_active.Equals(true)
        && temp.Key.Qa_supervisor_is_approve_status.Equals(true)
        && temp.Key.Tsqa_Approval_Status.Equals(true)
        && temp.Key.Lab_access_code != String.Empty)

             .Select(g => new
             {
               Lab_access_code = g.Key.Lab_access_code,
               Add_access_code_by = g.Key.Add_access_code_by,
               Add_access_code_date = g.Key.add_access_code_date,
               Is_Active = g.Key.Is_active,
               Qa_supervisor_is_approve_status = g.Key.Qa_supervisor_is_approve_status,
               Tsqa_Approval_Status = g.Key.Tsqa_Approval_Status,
               TotalItems = g.Sum(a => Convert.ToInt32(a.Is_active.Equals(true)))
             }).ToList();
      return Ok(GetResultWithAccessCode);
    }


    [HttpGet]
    [Route("api/DryWareHouseReceivingForLabTest/SearchLabResultWithAccessCode/{LabAccessCode}")]
    public async Task<IActionResult> GetSearchLabResultWithAccessCode(int LabAccessCode)
    {
      string LabCode = LabAccessCode.ToString();
      List<DryWhLabTestReqLogs> projects = null;
      projects = await db.Dry_wh_lab_test_req_logs.Include("DryWareHouseReceiving")
        .Where(temp => temp.Is_active.Equals(true)
          && temp.Qa_supervisor_is_approve_status.Equals(true)
          && temp.Tsqa_Approval_Status.Equals(true)
          //&& temp.lab_result_received_by != null
          && temp.Lab_access_code != null
          && temp.Lab_access_code.Contains(LabCode)

      ).ToListAsync();


      List<DryWhLabTestReqLogsViewModel> WarehouseReceivingContructor = new List<DryWhLabTestReqLogsViewModel>();
      foreach (var project in projects)
      {
        int LaboratoryAging = ((TimeSpan)(project.DryWareHouseReceiving.Qa_approval_date - project.Lab_request_date)).Days;
        int dayDiffExpiryDaysAging = (project.DryWareHouseReceiving.Lab_exp_date_extension - project.Bbd).Days;
        string LabStatus = "";
        if (project.DryWareHouseReceiving.Lab_status == null)
        {

          LabStatus = "LAB RECEIVED";
        }
        else
        {
          LabStatus = project.DryWareHouseReceiving.Lab_status;
        }

        WarehouseReceivingContructor.Add(new DryWhLabTestReqLogsViewModel()
        {

          Lab_req_id = project.Lab_req_id,
          Item_code = project.Item_code,
          Item_desc = project.Item_desc,
          Category = project.Category,
          Qty_received = project.Qty_received,
          Remaining_qty = project.Remaining_qty,
          Days_to_expired = dayDiffExpiryDaysAging.ToString(),
          Lab_status = LabStatus,
          Historical = project.Historical,
          Aging = project.Aging,
          Remarks = project.Remarks,
          Fk_receiving_id = project.Fk_receiving_id,
          Is_active = project.Is_active,
          Added_by = project.Added_by,
          Date_added = project.Date_added.ToString("MM/dd/yyyy"),
          Qa_approval_by = project.Qa_approval_by,
          Qa_approval_status = project.Qa_approval_status,
          Qa_approval_date = project.DryWareHouseReceiving.Qa_approval_date.ToString(),

          Lab_result_released_by = project.Lab_result_released_by,
          Lab_result_released_date = project.Lab_result_released_date,
          Lab_result_remarks = project.Lab_result_remarks,
          Lab_sub_remarks = project.Lab_sub_remarks,
          Lab_exp_date_extension = project.DryWareHouseReceiving.Lab_exp_date_extension.ToString("MM/dd/yyyy"),
          Laboratory_procedure = project.Laboratory_procedure,
          Lab_request_date = project.DryWareHouseReceiving.Lab_request_date,
          Lab_result_received_by = project.Lab_result_received_by,
          Lab_result_received_date = project.Lab_result_received_date,
          Lab_request_by = project.Lab_request_by,
          Is_received_status = project.Is_received_status,

          Po_date = project.Po_date,
          Po_number = project.Po_number,
          Pr_date = project.Pr_date,
          Pr_no = project.Pr_number,

          Lab_access_code = project.Lab_access_code,
          Bbd = project.Bbd.ToString("MM/dd/yyyy"),
          Lab_approval_aging_days = LaboratoryAging,
          Client_requestor = project.DryWareHouseReceiving.Client_requestor,
          Supplier = project.DryWareHouseReceiving.Supplier,

          Qa_supervisor_is_approve_status = project.Qa_supervisor_is_approve_status,
          Qa_supervisor_is_approve_by = project.Qa_supervisor_is_approve_by,
          Qa_supervisor_is_approve_date = project.Qa_supervisor_is_approve_date,


          Qa_supervisor_is_cancelled_status = project.Qa_supervisor_is_cancelled_status,
          Qa_supervisor_is_cancelled_by = project.Qa_supervisor_is_cancelled_by,
          Qa_supervisor_is_cancelled_date = project.Qa_supervisor_is_cancelled_date,
          Qa_supervisor_cancelled_remarks = project.Qa_supervisor_cancelled_remarks,

          Tsqa_Approval_By = project.Tsqa_Approval_By,
          Tsqa_Approval_Date = project.Tsqa_Approval_Date.ToString(),
          Tsqa_Approval_Status = project.Tsqa_Approval_Status,
          Sample_Qty = project.Sample_Qty

        });
      }

      return Ok(WarehouseReceivingContructor);



    }


    [HttpGet]
    [Route("api/DryWareHouseReceivingForLabTest")]
    public async Task<IActionResult> GetForLabTest()
    {
      string StringSequence = "3";

      List<DryWareHouseReceiving> TblDryWhReceiving = await db.TblDryWHReceiving.ToListAsync();

      if (TblDryWhReceiving.Count > 0)
      {

      }
      else
      {
        return Ok("No Data Found1");
      }

      List<DryWhLabTestReqLogs> DryWhLabTestReqLogs = await db.Dry_wh_lab_test_req_logs
        .Where(temp => temp.Is_active.Equals(true)).ToListAsync();

      if (DryWhLabTestReqLogs.Count > 0)
      {

      }
      else
      {
        return Ok("No Data Found");
      }


      List<DryWareHouseReceiving> obj = new List<DryWareHouseReceiving>();
      var results = (from a in db.TblDryWHReceiving
                     join b in db.Store_Preparation_Logs on a.id equals b.Prepa_Source_Key into ps
                     from b in ps.DefaultIfEmpty()
                     join c in db.Dry_wh_lab_test_req_logs on a.id equals c.Fk_receiving_id into pc
                     from c in  pc.DefaultIfEmpty()

                     where
                     (b.Is_active.Equals(true) ||
                     c.Tsqa_Approval_Status.Equals(false)) &&
                     a.Is_active == 1 &&
                     a.Lab_request_by != null
                     &&
                     a.Lab_result_released_by == null
                     !=
                     a.Qa_approval_status.Contains(StringSequence)


                     group a by new
                     {
                       a.id,
                       a.lab_access_code,
                       a.Item_code,
                       a.Item_description,
                       a.Category,
                       a.Is_active,
                       a.Uom,
                       a.Po_number,
                       a.Po_date,
                       a.pr_no,
                       a.pr_date,
                       a.Supplier,
                       b.Prepa_Allocated_Qty,
                       a.Qty_received,
                       a.Lab_status,
                       a.Historical_lab_transact_count,
                       a.Client_requestor,
                       a.Lab_exp_date_extension,
                       a.Lab_request_date,
                       a.Lab_request_by,
                       a.Qa_approval_status,
                       a.Qa_approval_by,
                       a.Qa_approval_date,
                       a.Lab_result_released_by,
                       a.Lab_result_released_date,
                       a.Lab_result_remarks,
                       a.Lab_sub_remarks,
                       a.Lab_approval_aging_days,
                       a.Laboratory_procedure,
                       a.Lab_cancel_by,
                       a.Lab_cancel_date,
                       a.Lab_cancel_remarks,
                       a.Sample_Qty
                     } into total

                     select new

                     {
                       total.Key.id,
                       total.Key.lab_access_code,
                       total.Key.Item_code,
                       total.Key.Item_description,
                       total.Key.Category,
                       total.Key.Po_number,
                       total.Key.Po_date,
                       total.Key.pr_no,
                       total.Key.pr_date,
                       total.Key.Supplier,
                       total.Key.Uom,
                       total.Key.Is_active,
                       total.Key.Lab_status,
                       total.Key.Lab_request_date,
                       total.Key.Lab_request_by,
                       total.Key.Historical_lab_transact_count,
                       total.Key.Client_requestor,
                       total.Key.Qa_approval_status,
                       total.Key.Qa_approval_by,
                       total.Key.Qa_approval_date,
                       total.Key.Lab_result_released_by,
                       total.Key.Lab_result_released_date,
                       total.Key.Lab_result_remarks,
                       total.Key.Lab_sub_remarks,
                       total.Key.Lab_exp_date_extension,
                       //total.Key.Lab_approval_aging_days,

                       //Lab_approval_aging_days = (DateTime.Now - total.Key.Qa_approval_date).Days,
                       total.Key.Laboratory_procedure,
                       total.Key.Lab_cancel_by,
                       total.Key.Lab_cancel_date,
                       total.Key.Lab_cancel_remarks,
                       sample_qty = total.Key.Sample_Qty,
                       qty_received = Convert.ToInt32(total.Key.Qty_received) - total.Sum(x => Convert.ToInt32(total.Key.Prepa_Allocated_Qty)),
                       expiry_days_aging = (total.Key.Lab_exp_date_extension - DateTime.Now).Days
                     }
                    );
      var result = await System.Threading.Tasks.Task.Run(() =>
      {
        return Ok(results);
      });
      if (results.Count() > 0)
      {
        return (result);
      }
      else
      {
        return NoContent();
      }
    }






    [HttpPost]
    [Route("api/DryWareHouseReceivingForLabTest/AttachResult")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Post([FromBody] ParentCheckList parentRequestParam)
    {

      //if (parentRequestParam.parent_chck_details == null || parentRequestParam.parent_chck_details == ""
      //  || parentRequestParam.parent_chck_added_by == null || parentRequestParam.parent_chck_added_by == "")
      //{
      //  return BadRequest(new { message = "Fill up the required fields" });
      //}

      //var ParentCheckListDataInfo = await db.Parent_checklist.Where(temp => temp.parent_chck_details == parentRequestParam.parent_chck_details
      //).ToListAsync();

      //if (ParentCheckListDataInfo.Count > 0)
      //{
      //  return BadRequest(new { message = "You already have a duplicate request check the data to proceed" });
      //}


      //db.Parent_checklist.Add(parentRequestParam);
      //await db.SaveChangesAsync();

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
    [Route("api/DryWareHouseReceivingForLabTest/NearlyExpiry")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetItemNearlyExpiry()
    {
      List<DryWareHouseReceiving> projects = await db.TblDryWHReceiving.Include("TblNearlyExpiryMgmtModel").Where(temp => temp.Is_active == 1
      && temp.FK_Sub_Category_IsExpirable == 1
      && (temp.Lab_exp_date_extension - DateTime.Now).Days < temp.TblNearlyExpiryMgmtModel.p_nearly_expiry_desc
      ).ToListAsync();


      List<DryWareHouseReceivingViewModelNearlyExpiry> projectsViewModel = new List<DryWareHouseReceivingViewModelNearlyExpiry>();
      foreach (var project in projects)
      {
        //int dayDiff = (project.Expiration_date_string - DateTime.Now).Days;
        int dayDiffExpiryDaysAging = (project.Lab_exp_date_extension - DateTime.Now).Days;

        int LaboratoryAging = ((TimeSpan)(project.Qa_approval_date - DateTime.Now)).Days;
        projectsViewModel.Add(new DryWareHouseReceivingViewModelNearlyExpiry()
        {
          Id = project.id,
          Item_code = project.Item_code,
          Item_description = project.Item_description,
          Qty_received = project.Qty_received,
          Lab_exp_date_extension = project.Lab_exp_date_extension.ToString("MM/dd/yyyy"),
          Expiry_days_aging = dayDiffExpiryDaysAging,
          Standard_Expiry_Days = project.TblNearlyExpiryMgmtModel.p_nearly_expiry_desc.ToString(),
          RemainingQty = project.Qty_received,
          //DaysBeforeExpired = dayDiff

        });
      }
      return Ok(projectsViewModel);
    }



    [HttpGet]
    [Route("api/DryWareHouseReceivingForLabTest/LabResult")]
    public async Task<IActionResult> GetLabResult()
    {
      List<DryWhLabTestReqLogs> projects = null;
      projects = await db.Dry_wh_lab_test_req_logs.Include("DryWareHouseReceiving")
        .Where(temp => temp.Is_active.Equals(true)
        && temp.Qa_supervisor_is_approve_status.Equals(true)
        && temp.Tsqa_Approval_Status.Equals(true)
        && temp.Lab_result_received_by == null
        && temp.Lab_access_code == String.Empty
      ).ToListAsync();

      List<DryWhLabTestReqLogsViewModel> WarehouseReceivingContructor = new List<DryWhLabTestReqLogsViewModel>();
      foreach (var project in projects)
      {
        if (project != null)
        {
          int LaboratoryAging = ((TimeSpan)(project.DryWareHouseReceiving.Qa_approval_date - project.Lab_request_date)).Days;
          int dayDiffExpiryDaysAging = (project.DryWareHouseReceiving.Lab_exp_date_extension - project.Bbd).Days;
        }

        string LabStatus = "";
        if (project.DryWareHouseReceiving.Lab_status == null)
        {

          LabStatus = "LAB RECEIVED";
        }
        else
        {
          LabStatus = project.DryWareHouseReceiving.Lab_status;
        }

        WarehouseReceivingContructor.Add(new DryWhLabTestReqLogsViewModel()
        {

          Lab_req_id = project.Lab_req_id,
          Item_code = project.Item_code,
          Item_desc = project.Item_desc,
          Category = project.Category,
          Qty_received = project.Qty_received,
          Remaining_qty = project.Remaining_qty,
          Days_to_expired = dayDiffExpiryDaysAging.ToString(),
          Lab_status = LabStatus,
          //Historical = project.Historical,
          Aging = project.Aging,
          Remarks = project.Remarks,
          Fk_receiving_id = project.Fk_receiving_id,
          Is_active = project.Is_active,
          Added_by = project.Added_by,
          Date_added = project.Date_added.ToString("MM/dd/yyyy"),
          Qa_approval_by = project.Qa_approval_by,
          Qa_approval_status = project.Qa_approval_status,
          Qa_approval_date = project.DryWareHouseReceiving.Qa_approval_date.ToString(),
          Lab_result_released_by = project.Lab_result_released_by,
          Lab_result_released_date = project.Lab_result_released_date,
          Lab_result_remarks = project.Lab_result_remarks,
          Lab_sub_remarks = project.Lab_sub_remarks,
          Lab_exp_date_extension = project.DryWareHouseReceiving.Lab_exp_date_extension.ToString("MM/dd/yyyy"),
          Laboratory_procedure = project.Laboratory_procedure,
          Lab_request_date = project.DryWareHouseReceiving.Lab_request_date,
          Lab_result_received_by = project.Lab_result_received_by,
          Lab_result_received_date = project.Lab_result_received_date,
          Lab_request_by = project.Lab_request_by,
          Is_received_status = project.Is_received_status,
          Po_date = project.Po_date,
          Po_number = project.Po_number,
          Pr_date = project.Pr_date,
          Pr_no = project.Pr_number,
          Lab_access_code = project.Lab_access_code,
          Bbd = project.Bbd.ToString("MM/dd/yyyy"),
          Lab_approval_aging_days = LaboratoryAging,
          Client_requestor = project.DryWareHouseReceiving.Client_requestor,
          Supplier = project.DryWareHouseReceiving.Supplier,
          Qa_supervisor_is_approve_status = project.Qa_supervisor_is_approve_status,
          Qa_supervisor_is_approve_by = project.Qa_supervisor_is_approve_by,
          Qa_supervisor_is_approve_date = project.Qa_supervisor_is_approve_date,
          Qa_supervisor_is_cancelled_status = project.Qa_supervisor_is_cancelled_status,
          Qa_supervisor_is_cancelled_by = project.Qa_supervisor_is_cancelled_by,
          Qa_supervisor_is_cancelled_date = project.Qa_supervisor_is_cancelled_date,
          Qa_supervisor_cancelled_remarks = project.Qa_supervisor_cancelled_remarks,
          Tsqa_Approval_By = project.Tsqa_Approval_By,
          Tsqa_Approval_Date = project.Tsqa_Approval_Date.ToString(),
          Tsqa_Approval_Status = project.Tsqa_Approval_Status,
          Sample_Qty = project.DryWareHouseReceiving.Sample_Qty,
          Add_access_code_by = project.Add_access_code_by

        });
      }
      return Ok(WarehouseReceivingContructor);
    }





    [HttpGet]
    [Route("api/DryWareHouseReceivingForLabTest/LabResultApproval")]
    public async Task<IActionResult> GetLabResultForApproval()
    {
        string LaboratoryResult = "LAB RESULT";    
        List<DryWhLabTestReqLogs> projects = null;
        projects = await db.Dry_wh_lab_test_req_logs.Include("DryWareHouseReceiving")
        .Where(temp => temp.Is_active.Equals(true)

        && temp.DryWareHouseReceiving.Lab_status.Contains(LaboratoryResult)
        && (temp.Lab_sub_remarks == null
        || temp.Tsqa_Approval_Status.Equals(false))
        ).ToListAsync();

   
      List<DryWhLabTestReqLogsViewModel> WarehouseReceivingContructor = new List<DryWhLabTestReqLogsViewModel>();
      foreach (var project in projects)
      {
        int LaboratoryAging = ((TimeSpan)(project.DryWareHouseReceiving.Qa_approval_date - project.Lab_request_date)).Days;
        int dayDiffExpiryDaysAging = (project.DryWareHouseReceiving.Lab_exp_date_extension - project.Bbd).Days;
        string LabStatus = "";
        if (project.DryWareHouseReceiving.Lab_status == null)
        {
          LabStatus = "LAB RECEIVED";
        }
        else
        {
          LabStatus = project.DryWareHouseReceiving.Lab_status;
        }

        WarehouseReceivingContructor.Add(new DryWhLabTestReqLogsViewModel()
        {

          Lab_req_id = project.Lab_req_id,
          Item_code = project.Item_code,
          Item_desc = project.Item_desc,
          Category = project.Category,
          Qty_received = project.Qty_received,
          Remaining_qty = project.Remaining_qty,
          Days_to_expired = dayDiffExpiryDaysAging.ToString(),
          Lab_status = LabStatus,
          Historical = project.Historical,
          //Aging = project.Aging,
          Aging = LaboratoryAging.ToString(),
          Remarks = project.Remarks,
          Fk_receiving_id = project.Fk_receiving_id,
          Is_active = project.Is_active,
          Added_by = project.Added_by,
          Date_added = project.Date_added.ToString("MM/dd/yyyy"),
          Qa_approval_by = project.Qa_approval_by,
          Qa_approval_status = project.Qa_approval_status,
          Qa_approval_date = project.DryWareHouseReceiving.Qa_approval_date.ToString(),
          Lab_result_released_by = project.DryWareHouseReceiving.Lab_result_released_by,
          Lab_result_released_date = project.DryWareHouseReceiving.Lab_result_released_date,
          Lab_result_remarks = project.Lab_result_remarks,
          Lab_sub_remarks = project.Lab_sub_remarks,
          Lab_exp_date_extension = project.DryWareHouseReceiving.Lab_exp_date_extension.ToString(),
          Laboratory_procedure = project.DryWareHouseReceiving.Laboratory_procedure,
          Uom = project.DryWareHouseReceiving.Uom,
          Lab_request_date = project.DryWareHouseReceiving.Lab_request_date,
          Lab_result_received_by = project.Lab_result_received_by,
          Lab_result_received_date = project.Lab_result_received_date,
          Lab_request_by = project.DryWareHouseReceiving.Lab_request_by,
          Is_received_status = project.Is_received_status,
          Po_date = project.DryWareHouseReceiving.Po_date,
          Po_number = project.DryWareHouseReceiving.Po_number,
          Pr_date = project.DryWareHouseReceiving.pr_date,
          Pr_no = project.DryWareHouseReceiving.pr_no,
          Lab_access_code = project.Lab_access_code,
          Bbd = project.Bbd.ToString("MM/dd/yyyy"),
          Lab_approval_aging_days = LaboratoryAging,
          Client_requestor = project.DryWareHouseReceiving.Client_requestor,
          Supplier = project.DryWareHouseReceiving.Supplier,
          Qa_supervisor_is_approve_status = project.Qa_supervisor_is_approve_status,
          Qa_supervisor_is_approve_by = project.Qa_supervisor_is_approve_by,
          Qa_supervisor_is_approve_date = project.Qa_supervisor_is_approve_date,
          Qa_supervisor_is_cancelled_status = project.Qa_supervisor_is_cancelled_status,
          Qa_supervisor_is_cancelled_by = project.Qa_supervisor_is_cancelled_by,
          Qa_supervisor_is_cancelled_date = project.Qa_supervisor_is_cancelled_date,
          Qa_supervisor_cancelled_remarks = project.Qa_supervisor_cancelled_remarks,
          Samples = project.Samples ="RAW MATS",
          Filename = project.DryWareHouseReceiving.FileName,
          Filepath = project.DryWareHouseReceiving.FilePath
        });
      }
      return Ok(WarehouseReceivingContructor);
    }


    [HttpGet]
    [Route("api/DryWareHouseReceivingForLabTest/searchreceivedidentity/{searchtext}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Search(int searchText)
    {
      string is_activated = "1";
      List<DryWhLabTestReqLogs> projects = null;  
      int ReceivedID = searchText;
      //if (searchBy == "store_name")       

      projects = await db.Dry_wh_lab_test_req_logs
        .Where(temp => temp.Is_received_status
        .Contains(is_activated) && temp.Fk_receiving_id == ReceivedID).ToListAsync();
      List<DryWhLabTestReqLogsViewModel> WarehouseStoreOrderContructor = new List<DryWhLabTestReqLogsViewModel>();
      foreach (var project in projects)
      {
        WarehouseStoreOrderContructor.Add(new DryWhLabTestReqLogsViewModel()
        {

          Lab_req_id = project.Lab_req_id,
          Item_code = project.Item_code,
          Item_desc = project.Item_desc,
          Category = project.Category,
          Qty_received = project.Qty_received,
          Remaining_qty = project.Remaining_qty,
          Days_to_expired = project.Days_to_expired.ToString(),
          Lab_status = project.Lab_status,
          Historical = project.Historical,
          Aging = project.Aging,
          Remarks = project.Remarks,
          Fk_receiving_id = project.Fk_receiving_id,
          Is_active = project.Is_active,
          Added_by = project.Added_by,
          Date_added = project.Date_added.ToString("MM/dd/yyyy"),
          Qa_approval_by = project.Qa_approval_by,
          Qa_approval_status = project.Qa_approval_status,
          Qa_approval_date = project.Qa_approval_date,
          Lab_result_released_by = project.Lab_result_released_by,
          Lab_result_released_date = project.Lab_result_released_date,
          Lab_result_remarks = project.Lab_result_remarks,
          Lab_sub_remarks = project.Lab_sub_remarks,
          Lab_exp_date_extension = project.Lab_exp_date_extension,
          Laboratory_procedure = project.Laboratory_procedure,
          Lab_request_date = project.Lab_request_date.ToString(),
          Lab_result_received_by = project.Lab_result_received_by,
          Lab_result_received_date = project.Lab_result_received_date,
          Lab_request_by = project.Lab_request_by,
          Is_received_status = project.Is_received_status,
          Bbd = project.Bbd.ToString("MM/dd/yyyy"),
          Sample_Qty = project.Sample_Qty
        }); ;
      }

      return Ok(WarehouseStoreOrderContructor);
    }



    [HttpPut]
    [Route("api/DryWareHouseReceivingForLabTest/QAApproval")]
    public async Task<DryWareHouseReceiving> Put([FromBody] DryWareHouseReceiving labTestQAStaffApprovalParams)
    {
      DryWareHouseReceiving existingDataStatus = await db.TblDryWHReceiving.Where(temp => temp.id == labTestQAStaffApprovalParams.id).FirstOrDefaultAsync();
      labTestQAStaffApprovalParams.Qa_approval_status = "1";
      if (existingDataStatus != null)
      {
        existingDataStatus.Qa_approval_by = labTestQAStaffApprovalParams.Qa_approval_by;
        existingDataStatus.Qa_approval_status = labTestQAStaffApprovalParams.Qa_approval_status;
        existingDataStatus.Qa_approval_date = DateTime.Now;
        existingDataStatus.Lab_status = labTestQAStaffApprovalParams.Lab_status;
        await db.SaveChangesAsync();

        DryWhLabTestReqLogs existingDataStatus2 = await db.Dry_wh_lab_test_req_logs.Where(temp => temp.Fk_receiving_id == labTestQAStaffApprovalParams.id).FirstOrDefaultAsync();
        labTestQAStaffApprovalParams.Qa_approval_status = "1";
        if (existingDataStatus2 != null)
        {
          existingDataStatus2.Qa_approval_by = labTestQAStaffApprovalParams.Qa_approval_by;
          existingDataStatus2.Qa_approval_status = labTestQAStaffApprovalParams.Qa_approval_status;
          existingDataStatus2.Qa_approval_date = DateTime.Now.ToString();
          existingDataStatus2.Lab_status = labTestQAStaffApprovalParams.Lab_status;
          await db.SaveChangesAsync();
        }

        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }


    
    [HttpPut]
    [Route("api/DryWareHouseReceivingForLabTest/SettingLabAccessCode")]
    public async Task<IActionResult> PutQALabAccessCode([FromBody] DryWhLabTestReqLogs[] labTestLabAccessCodeParams)
    {
      var dateTimeNow = DateTime.Now; 
      var dateOnlyString = dateTimeNow.ToShortDateString();
      foreach (DryWhLabTestReqLogs items in labTestLabAccessCodeParams)
      {        
        var existingDataStatus = await db.Dry_wh_lab_test_req_logs
          .Where(temp => temp.Lab_req_id == items.Lab_req_id).FirstOrDefaultAsync();
        if (existingDataStatus != null)
        {
          existingDataStatus.Lab_access_code = items.Lab_access_code;
          existingDataStatus.add_access_code_date = Convert.ToDateTime(dateOnlyString);
          existingDataStatus.Add_access_code_by = items.Add_access_code_by;
          await db.SaveChangesAsync();
        }
        else
        {
          return null;
        }
      }
      return Ok(labTestLabAccessCodeParams);
    }

    [HttpPut]
    [Route("api/DryWareHouseReceivingForLabTest/QAReleasingResult")]
    public async Task<DryWareHouseReceiving> PutQAResults([FromBody] DryWareHouseReceiving LabTestQAStaffApprovalParams)
    {
      try
      {
        var file = Request.Form.Files[0];
        var folderName = Path.Combine("wwwroot", "assets");
        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);


        if (file.Length > 0)
        {
          var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
          var fullPath = Path.Combine(pathToSave, fileName);
          var dbPath = Path.Combine(folderName, fileName);

          using (var stream = new FileStream(fullPath, FileMode.Create))
          {
            file.CopyTo(stream);
          }
          //return Ok(new { dbPath });
        }
        else
        {
          //return BadRequest();
        }
      }
      catch (Exception ex)
      {
        //return StatusCode(500, $"Internal server error: {ex}");
      }


      LabTestQAStaffApprovalParams.Lab_status = "LAB RESULT";

      DryWareHouseReceiving existingDataStatus = await db.TblDryWHReceiving
      .Where(temp => temp.id == LabTestQAStaffApprovalParams.id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.Lab_result_released_by = LabTestQAStaffApprovalParams.Lab_result_released_by;
        existingDataStatus.Lab_result_released_date = DateTime.Now.ToString();
        existingDataStatus.Lab_status = LabTestQAStaffApprovalParams.Lab_status;
        existingDataStatus.Lab_exp_date_extension = LabTestQAStaffApprovalParams.Lab_exp_date_extension;
        existingDataStatus.Laboratory_procedure = LabTestQAStaffApprovalParams.Laboratory_procedure;
        existingDataStatus.FileName = LabTestQAStaffApprovalParams.FileName;
        existingDataStatus.FilePath = LabTestQAStaffApprovalParams.FilePath;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }

    //QA Staff Cancelled the Lab Requestor
    [HttpPut]
    [Route("api/DryWareHouseReceivingForLabTest/CancelledQAReleasingResult")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<DryWareHouseReceiving> PutCancelQAResults([FromBody] DryWareHouseReceiving labTestCancelParams)
    {
      DryWareHouseReceiving existingDataStatus = await db.TblDryWHReceiving.Where(temp => temp.id == labTestCancelParams.id).FirstOrDefaultAsync();
      labTestCancelParams.Qa_approval_status = "3";
      if (existingDataStatus != null)
      {
        existingDataStatus.Lab_cancel_by = labTestCancelParams.Lab_cancel_by;
        existingDataStatus.Lab_cancel_date = DateTime.Now.ToString();
        existingDataStatus.Lab_cancel_remarks = labTestCancelParams.Lab_cancel_remarks;
        existingDataStatus.Qa_approval_status = labTestCancelParams.Qa_approval_status;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }




    [HttpPut]
    [Route("api/DryWareHouseReceivingForLabTest/QASupervisorApproval")]
    public async Task<DryWhLabTestReqLogs> PutLabTestResultApproval([FromBody] DryWhLabTestReqLogs labTestQASuperVisorApprovalParams)
    {
      labTestQASuperVisorApprovalParams.Qa_supervisor_is_approve_status = true;
      DryWhLabTestReqLogs existingDataStatus = await db.Dry_wh_lab_test_req_logs.Where(temp => temp.Lab_req_id == labTestQASuperVisorApprovalParams.Lab_req_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.Qa_supervisor_is_approve_status = labTestQASuperVisorApprovalParams.Qa_supervisor_is_approve_status;
        existingDataStatus.Qa_supervisor_is_approve_by = labTestQASuperVisorApprovalParams.Qa_supervisor_is_approve_by;
        existingDataStatus.Qa_supervisor_is_approve_date = DateTime.Now.ToString();
        existingDataStatus.Lab_result_remarks = labTestQASuperVisorApprovalParams.Lab_result_remarks;
        existingDataStatus.Lab_sub_remarks = labTestQASuperVisorApprovalParams.Lab_sub_remarks;
        existingDataStatus.Laboratory_procedure = labTestQASuperVisorApprovalParams.Laboratory_procedure;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }

    }

    [HttpPut]
    [Route("api/DryWareHouseReceivingForLabTest/TSQASupervisorApproval")]
    public async Task<DryWhLabTestReqLogs> PutLabTestResultTSQAApproval([FromBody] DryWhLabTestReqLogs labTestQASuperVisorApprovalParams)
    {
      labTestQASuperVisorApprovalParams.Tsqa_Approval_Status = true;
      DryWhLabTestReqLogs existingDataStatus = await db.Dry_wh_lab_test_req_logs.Where(temp => temp.Lab_req_id == labTestQASuperVisorApprovalParams.Lab_req_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.Tsqa_Approval_Status = labTestQASuperVisorApprovalParams.Tsqa_Approval_Status;
        existingDataStatus.Tsqa_Approval_By = labTestQASuperVisorApprovalParams.Tsqa_Approval_By;
        existingDataStatus.Tsqa_Approval_Date = DateTime.Now;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }



    [HttpPut]
    [Route("api/DryWareHouseReceivingForLabTest/QASupervisorCancelLabResult")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<DryWhLabTestReqLogs> PutLabTestResultCancel([FromBody] DryWhLabTestReqLogs labTestQASuperVisorApprovalParams)
    {
      DryWhLabTestReqLogs existingDataStatus = await db.Dry_wh_lab_test_req_logs
        .Where(temp => temp.Lab_req_id == labTestQASuperVisorApprovalParams.Lab_req_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.Qa_supervisor_is_cancelled_status = labTestQASuperVisorApprovalParams.Qa_supervisor_is_cancelled_status;
        existingDataStatus.Qa_supervisor_is_cancelled_by = labTestQASuperVisorApprovalParams.Qa_supervisor_is_cancelled_by;
        existingDataStatus.Qa_supervisor_is_cancelled_date = labTestQASuperVisorApprovalParams.Qa_supervisor_is_cancelled_date;
        existingDataStatus.Qa_supervisor_cancelled_remarks = labTestQASuperVisorApprovalParams.Qa_supervisor_cancelled_remarks;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
 
    }

    [HttpPut]
    [Route("api/DryWareHouseReceivingForLabTest/CancelledQASupervisorReleasingLabResult")]
    public async Task<DryWareHouseReceiving> PutCancelQASupervisorResults([FromBody] DryWareHouseReceiving labTestCancelParams)
    {
      string LabStatus="LAB APPROVED";
      DryWareHouseReceiving existingDataStatus = await db.TblDryWHReceiving
      .Where(temp => temp.id == labTestCancelParams.id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.Lab_status = LabStatus;
        existingDataStatus.Lab_result_remarks = null;
        existingDataStatus.Lab_result_released_by = null;
        existingDataStatus.Lab_result_released_date = null;
        existingDataStatus.Lab_sub_remarks = null;
        existingDataStatus.Laboratory_procedure = null;
        existingDataStatus.Lab_exp_date_extension = Convert.ToDateTime(existingDataStatus.Lab_exp_date_request);
        existingDataStatus.LabTest_CancelledReason = labTestCancelParams.LabTest_CancelledReason;
        existingDataStatus.FileName = null;
        existingDataStatus.FilePath = null;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }



  }

}

