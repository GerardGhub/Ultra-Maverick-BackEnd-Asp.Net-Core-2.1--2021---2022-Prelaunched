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

namespace MvcTaskManager.Controllers
{
  public class tblDryWHReceivingController : Controller
  {
    private ApplicationDbContext db;
    private static IWebHostEnvironment _webHostEnvironment;

    public tblDryWHReceivingController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
    {
      this.db = db;
      _webHostEnvironment = webHostEnvironment;
    }

    //[HttpPost]
    //[Route("api/DryWareHouseReceivingForLabTest/AttachResult")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //public async Task<string> Upload([FromForm] UploadFile obj)
    //{
    //  if (obj.files.Length > 0)
    //  {

    //    try
    //    {
    //      if (!Directory.Exists(_webHostEnvironment.WebRootPath + "\\Images\\"))
    //      {
    //        Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "\\Images\\");
    //      }

    //      using (FileStream filestream =System.IO.File.Create(_webHostEnvironment.WebRootPath+ "\\Images\\"+obj.files.FileName))
    //      {
    //        obj.files.CopyTo(filestream);
    //        filestream.Flush();
    //        return "\\Images\\" + obj.files.FileName;
    //      }
    //    }
    //    catch (Exception ex)
    //    {

    //      return ex.ToString();
    //    }


    //  }
    //  else
    //  {
    //    return "Upload Failed";
    //  }
    //}


    [HttpGet]
    [Route("api/DryWareHouseReceivingForLabTest")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetForLabTest()
    {

      string StringSequence = "3";

      var results = await (from a in db.TblDryWHReceiving
                           join b in db.Store_Preparation_Logs on a.id equals b.Prepa_Source_Key into pb
                           from b in pb.DefaultIfEmpty()
                           join c in db.Dry_wh_lab_test_req_logs on a.id equals c.Fk_receiving_id into pc
                           from c in pc.DefaultIfEmpty()


                           where

                           (b.Is_Active.Equals(true)
                            || c.Tsqa_Approval_Status.Equals(false)) &&

                           a.Is_active == 1 &&
                           a.Lab_request_by != null &&
                           a.Lab_result_released_by == null !=
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
                             total.Key.Lab_approval_aging_days,
                             total.Key.Laboratory_procedure,
                             total.Key.Lab_cancel_by,
                             total.Key.Lab_cancel_date,
                             total.Key.Lab_cancel_remarks,
                             sample_qty = total.Key.Sample_Qty,
                             qty_received = Convert.ToInt32(total.Key.Qty_received) - total.Sum(x => Convert.ToInt32(total.Key.Prepa_Allocated_Qty)),
                             expiry_days_aging = (total.Key.Lab_exp_date_extension - DateTime.Now).Days


                           }
                     ).ToListAsync();


      return Ok(results);






    }


    //[HttpPost]
    //[Route("api/DryWareHouseReceivingForLabTest/AttachResult")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //public async Task<IActionResult> Post([FromBody] ParentCheckList parentRequestParam)
    //{

    //  //if (parentRequestParam.parent_chck_details == null || parentRequestParam.parent_chck_details == ""
    //  //  || parentRequestParam.parent_chck_added_by == null || parentRequestParam.parent_chck_added_by == "")
    //  //{
    //  //  return BadRequest(new { message = "Fill up the required fields" });
    //  //}

    //  //var ParentCheckListDataInfo = await db.Parent_checklist.Where(temp => temp.parent_chck_details == parentRequestParam.parent_chck_details
    //  //).ToListAsync();

    //  //if (ParentCheckListDataInfo.Count > 0)
    //  //{
    //  //  return BadRequest(new { message = "You already have a duplicate request check the data to proceed" });
    //  //}


    //  //db.Parent_checklist.Add(parentRequestParam);
    //  //await db.SaveChangesAsync();

    //  ParentCheckList existingProject = await db.Parent_checklist.Where(temp => temp.parent_chck_details == parentRequestParam.parent_chck_details).FirstOrDefaultAsync();

    //  ParentCheckListViewModel ParentViewModel = new ParentCheckListViewModel()
    //  {

    //    Parent_chck_id = existingProject.parent_chck_id,
    //    Parent_chck_details = existingProject.parent_chck_details,
    //    Parent_chck_added_by = existingProject.parent_chck_added_by,
    //    Parent_chck_date_added = existingProject.parent_chck_date_added,
    //    Is_active = existingProject.is_active
    //  };

    //  return Ok(ParentViewModel);

    //}





    [HttpGet]
    [Route("api/DryWareHouseReceivingForLabTest/NearlyExpiry")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetItemNearlyExpiry()
    {


      List<DryWareHouseReceiving> projects = await db.TblDryWHReceiving.Include("tblNearlyExpiryMgmtModel").Where(temp => temp.Is_active == 1
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetLabResult()
    {

      List<DryWhLabTestReqLogs> projects = null;
      projects = await db.Dry_wh_lab_test_req_logs.Include("DryWareHouseReceiving")
        .Where(temp => temp.Is_active.Equals(true)

        //&& temp.DryWareHouseReceiving.lab_status.Contains(LaboratoryReceived)


          && temp.Qa_supervisor_is_approve_status.Equals(true)
          && temp.Tsqa_Approval_Status.Equals(true)
          //&& temp.lab_result_received_by != null
          && temp.lab_result_received_by == null
      &&   temp.lab_access_code == String.Empty

      ).ToListAsync();


      List<DryWhLabTestReqLogsViewModel> WarehouseReceivingContructor = new List<DryWhLabTestReqLogsViewModel>();
      foreach (var project in projects)
      {
        int LaboratoryAging = ((TimeSpan)(project.DryWareHouseReceiving.Qa_approval_date - project.lab_request_date)).Days;
        int dayDiffExpiryDaysAging = (project.DryWareHouseReceiving.Lab_exp_date_extension - project.bbd).Days;
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
          Qa_approval_date = project.DryWareHouseReceiving.Qa_approval_date.ToString("MM/dd/yyyy"),

          Lab_result_released_by = project.Lab_result_released_by,
          Lab_result_released_date = project.Lab_result_released_date,
          Lab_result_remarks = project.Lab_result_remarks,
          Lab_sub_remarks = project.lab_sub_remarks,
          Lab_exp_date_extension = project.DryWareHouseReceiving.Lab_exp_date_extension.ToString("MM/dd/yyyy"),
          Laboratory_procedure = project.laboratory_procedure,
          Lab_request_date = project.DryWareHouseReceiving.Lab_request_date,
          Lab_result_received_by = project.lab_result_received_by,
          Lab_result_received_date = project.lab_result_received_date,
          Lab_request_by = project.lab_request_by,
          Is_received_status = project.is_received_status,

          Po_date = project.po_date,
          Po_number = project.po_number,
          Pr_date = project.pr_date,
          Pr_number = project.pr_number,

          Lab_access_code = project.lab_access_code,
          Bbd = project.bbd.ToString("MM/dd/yyyy"),
          Lab_approval_aging_days = LaboratoryAging,
          Client_requestor = project.DryWareHouseReceiving.Client_requestor,
          Supplier = project.DryWareHouseReceiving.Supplier,

          Qa_supervisor_is_approve_status = project.Qa_supervisor_is_approve_status,
          Qa_supervisor_is_approve_by = project.Qa_supervisor_is_approve_by,
          Qa_supervisor_is_approve_date = project.Qa_supervisor_is_approve_date,


          Qa_supervisor_is_cancelled_status = project.qa_supervisor_is_cancelled_status,
          Qa_supervisor_is_cancelled_by = project.qa_supervisor_is_cancelled_by,
          Qa_supervisor_is_cancelled_date = project.qa_supervisor_is_cancelled_date,
          Qa_supervisor_cancelled_remarks = project.qa_supervisor_cancelled_remarks,

          Tsqa_Approval_By = project.Tsqa_Approval_By,
          Tsqa_Approval_Date = project.Tsqa_Approval_Date.ToString(),
          Tsqa_Approval_Status = project.Tsqa_Approval_Status,
          Sample_Qty = project.Sample_Qty

        });
      }

      return Ok(WarehouseReceivingContructor);



    }



    [HttpGet]
    [Route("api/DryWareHouseReceivingForLabTest/LabResultWithAccessCode")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetLabResultWithAccessCode()
    {

    

      var GetResultWithAccessCode = db.Dry_wh_lab_test_req_logs
        .Include(x => x.DryWareHouseReceiving).ToList()
        .GroupBy(d => new { d.lab_access_code, d.lab_request_by, d.lab_request_date,
        d.Is_active, d.Qa_supervisor_is_approve_status, d.Tsqa_Approval_Status})
        .Where(temp => temp.Key.Is_active.Equals(true)
        && temp.Key.Qa_supervisor_is_approve_status.Equals(true)
        && temp.Key.Tsqa_Approval_Status.Equals(true))


             .Select(g => new
             {
               Lab_access_code = g.Key.lab_access_code,
               Lab_request_by = g.Key.lab_request_by,
               Lab_request_date = g.Key.lab_request_date,
               Is_Active = g.Key.Is_active,
               Qa_supervisor_is_approve_status = g.Key.Qa_supervisor_is_approve_status,
               Tsqa_Approval_Status = g.Key.Tsqa_Approval_Status,
               TotalItems = g.Sum(a => Convert.ToInt32(a.Is_active.Equals(true)))
             
             }).ToList();


      return Ok(GetResultWithAccessCode);



    }



    [HttpGet]
    [Route("api/DryWareHouseReceivingForLabTest/SearchLabResultWithAccessCode/{LabAccessCode}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetSearchLabResultWithAccessCode(int LabAccessCode)
    {
      string LabCode = LabAccessCode.ToString();
      List<DryWhLabTestReqLogs> projects = null;
      projects = await db.Dry_wh_lab_test_req_logs.Include("DryWareHouseReceiving")
        .Where(temp => temp.Is_active.Equals(true)

        //&& temp.DryWareHouseReceiving.lab_status.Contains(LaboratoryReceived)
    

          && temp.Qa_supervisor_is_approve_status.Equals(true)
          && temp.Tsqa_Approval_Status.Equals(true)
          //&& temp.lab_result_received_by != null
          && temp.lab_access_code != null
          && temp.lab_access_code.Contains(LabCode)

      ).ToListAsync();


      List<DryWhLabTestReqLogsViewModel> WarehouseReceivingContructor = new List<DryWhLabTestReqLogsViewModel>();
      foreach (var project in projects)
      {
        int LaboratoryAging = ((TimeSpan)(project.DryWareHouseReceiving.Qa_approval_date - project.lab_request_date)).Days;
        int dayDiffExpiryDaysAging = (project.DryWareHouseReceiving.Lab_exp_date_extension - project.bbd).Days;
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
          Qa_approval_date = project.DryWareHouseReceiving.Qa_approval_date.ToString("MM/dd/yyyy"),

          Lab_result_released_by = project.Lab_result_released_by,
          Lab_result_released_date = project.Lab_result_released_date,
          Lab_result_remarks = project.Lab_result_remarks,
          Lab_sub_remarks = project.lab_sub_remarks,
          Lab_exp_date_extension = project.DryWareHouseReceiving.Lab_exp_date_extension.ToString("MM/dd/yyyy"),
          Laboratory_procedure = project.laboratory_procedure,
          Lab_request_date = project.DryWareHouseReceiving.Lab_request_date,
          Lab_result_received_by = project.lab_result_received_by,
          Lab_result_received_date = project.lab_result_received_date,
          Lab_request_by = project.lab_request_by,
          Is_received_status = project.is_received_status,

          Po_date = project.po_date,
          Po_number = project.po_number,
          Pr_date = project.pr_date,
          Pr_number = project.pr_number,

          Lab_access_code = project.lab_access_code,
          Bbd = project.bbd.ToString("MM/dd/yyyy"),
          Lab_approval_aging_days = LaboratoryAging,
          Client_requestor = project.DryWareHouseReceiving.Client_requestor,
          Supplier = project.DryWareHouseReceiving.Supplier,

          Qa_supervisor_is_approve_status = project.Qa_supervisor_is_approve_status,
          Qa_supervisor_is_approve_by = project.Qa_supervisor_is_approve_by,
          Qa_supervisor_is_approve_date = project.Qa_supervisor_is_approve_date,


          Qa_supervisor_is_cancelled_status = project.qa_supervisor_is_cancelled_status,
          Qa_supervisor_is_cancelled_by = project.qa_supervisor_is_cancelled_by,
          Qa_supervisor_is_cancelled_date = project.qa_supervisor_is_cancelled_date,
          Qa_supervisor_cancelled_remarks = project.qa_supervisor_cancelled_remarks,

          Tsqa_Approval_By = project.Tsqa_Approval_By,
          Tsqa_Approval_Date = project.Tsqa_Approval_Date.ToString(),
          Tsqa_Approval_Status = project.Tsqa_Approval_Status,
          Sample_Qty = project.Sample_Qty

        });
      }

      return Ok(WarehouseReceivingContructor);



    }


    [HttpGet]
    [Route("api/DryWareHouseReceivingForLabTest/LabResultApproval")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetLabResultForApproval()
    {


      string LaboratoryResult = "LAB RESULT";



      List<DryWhLabTestReqLogs> projects = null;
      projects = await db.Dry_wh_lab_test_req_logs.Include("DryWareHouseReceiving")
        .Where(temp => temp.Is_active.Equals(true)

        && temp.DryWareHouseReceiving.Lab_status.Contains(LaboratoryResult)

        && temp.lab_sub_remarks == null
        && temp.Tsqa_Approval_Status.Equals(false)

      ).ToListAsync();


 


      List<DryWhLabTestReqLogsViewModel> WarehouseReceivingContructor = new List<DryWhLabTestReqLogsViewModel>();
      foreach (var project in projects)
      {
        int LaboratoryAging = ((TimeSpan)(project.DryWareHouseReceiving.Qa_approval_date - project.lab_request_date)).Days;
        int dayDiffExpiryDaysAging = (project.DryWareHouseReceiving.Lab_exp_date_extension - project.bbd).Days;
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
          Fk_receiving_id = project.Fk_receiving_id,
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
 
          //Is_active = project.is_active,
          Added_by = project.Added_by,
          Date_added = project.Date_added.ToString("MM/dd/yyyy"),
          Qa_approval_by = project.Qa_approval_by,
          Qa_approval_status = project.Qa_approval_status,
          Qa_approval_date = project.DryWareHouseReceiving.Qa_approval_date.ToString(),
          Lab_result_released_by = project.Lab_result_released_by,
          Lab_result_released_date = project.Lab_result_released_date,
          Lab_result_remarks = project.Lab_result_remarks,
          Lab_sub_remarks = project.lab_sub_remarks,
          Lab_exp_date_extension = project.DryWareHouseReceiving.Lab_exp_date_extension.ToString(),
          Laboratory_procedure = project.laboratory_procedure,
          Lab_request_date = project.DryWareHouseReceiving.Lab_request_date,
          Lab_result_received_by = project.lab_result_received_by,
          Lab_result_received_date = project.lab_result_received_date,
          Lab_request_by = project.lab_request_by,
          Is_received_status = project.is_received_status,

          Po_date = project.po_date,
          Po_number = project.po_number,
          Pr_date = project.pr_date,
          Pr_number = project.pr_number,

          Lab_access_code = project.lab_access_code,
          Bbd = project.bbd.ToString("MM/dd/yyyy"),
          Lab_approval_aging_days = LaboratoryAging,
          Client_requestor = project.DryWareHouseReceiving.Client_requestor,
          Supplier = project.DryWareHouseReceiving.Supplier,

          Qa_supervisor_is_approve_status = project.Qa_supervisor_is_approve_status,
          Qa_supervisor_is_approve_by = project.Qa_supervisor_is_approve_by,
          Qa_supervisor_is_approve_date = project.Qa_supervisor_is_approve_date,


          Qa_supervisor_is_cancelled_status = project.qa_supervisor_is_cancelled_status,
          Qa_supervisor_is_cancelled_by = project.qa_supervisor_is_cancelled_by,
          Qa_supervisor_is_cancelled_date = project.qa_supervisor_is_cancelled_date,
          Qa_supervisor_cancelled_remarks = project.qa_supervisor_cancelled_remarks

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

      projects = await db.Dry_wh_lab_test_req_logs.Where(temp => temp.is_received_status.Contains(is_activated) && temp.Fk_receiving_id == ReceivedID).ToListAsync();


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
          Lab_sub_remarks = project.lab_sub_remarks,
          Lab_exp_date_extension = project.lab_exp_date_extension,
          Laboratory_procedure = project.laboratory_procedure,
          Lab_request_date = project.lab_request_date.ToString(),
          Lab_result_received_by = project.lab_result_received_by,
          Lab_result_received_date = project.lab_result_received_date,
          Lab_request_by = project.lab_request_by,
          Is_received_status = project.is_received_status





        }); ;
      }

      return Ok(WarehouseStoreOrderContructor);
    }












    [HttpPut]
    [Route("api/DryWareHouseReceivingForLabTest/QAApproval")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PutQALabAccessCode([FromBody] DryWhLabTestReqLogs[] labTestLabAccessCodeParams)
    {

      
      foreach (DryWhLabTestReqLogs items in labTestLabAccessCodeParams)
      {

        
        var existingDataStatus = await db.Dry_wh_lab_test_req_logs.Where(temp => temp.Lab_req_id == items.Lab_req_id).FirstOrDefaultAsync();
        if (existingDataStatus != null)
        {
          existingDataStatus.lab_access_code = items.lab_access_code;
          await db.SaveChangesAsync();
          return Ok(existingDataStatus);
        }
        else
        {
          return null;
        }


 





      }
      return Ok(labTestLabAccessCodeParams);


    }


    //[HttpPut]
    //[Route("api/DryWareHouseReceivingForLabTest/QAReleasingResult")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //public async Task<DryWareHouseReceiving> PutQAResults([FromBody] DryWareHouseReceiving labTestQAStaffApprovalParams)
    //{


    //  labTestQAStaffApprovalParams.Lab_status = "LAB RESULT";
    //  DryWareHouseReceiving existingDataStatus = await db.TblDryWHReceiving.Where(temp => temp.id == labTestQAStaffApprovalParams.id).FirstOrDefaultAsync();
    //  if (existingDataStatus != null)
    //  {
    //    existingDataStatus.Lab_result_released_by = labTestQAStaffApprovalParams.Lab_result_released_by;
    //    existingDataStatus.Lab_result_released_date = DateTime.Now.ToString();
    //    existingDataStatus.Lab_status = labTestQAStaffApprovalParams.Lab_status;
    //    existingDataStatus.Lab_exp_date_extension = labTestQAStaffApprovalParams.Lab_exp_date_extension;
    //    existingDataStatus.Laboratory_procedure = labTestQAStaffApprovalParams.Laboratory_procedure;

    //    await db.SaveChangesAsync();
    //    return existingDataStatus;
    //  }
    //  else
    //  {
    //    return null;
    //  }
    //}


    [HttpPut]
    [Route("api/DryWareHouseReceivingForLabTest/QAReleasingResult")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<DryWareHouseReceiving> PutQAResults([FromForm] DryWareHouseReceiving labTestQAStaffApprovalParams)
    {
      
      if (labTestQAStaffApprovalParams.files.Length > 0)
      {


        if (!Directory.Exists(_webHostEnvironment.WebRootPath + "\\Images\\"))
        {
          Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "\\Images\\");
        }

        using (FileStream filestream = System.IO.File.Create(_webHostEnvironment.WebRootPath + "\\Images\\" + labTestQAStaffApprovalParams.files.FileName))
        {
          labTestQAStaffApprovalParams.files.CopyTo(filestream);
          filestream.Flush();
          //return "\\Images\\" + labTestQAStaffApprovalParams.files.FileName;
        }



      }



      labTestQAStaffApprovalParams.Lab_status = "LAB RESULT";
      DryWareHouseReceiving existingDataStatus = await db.TblDryWHReceiving.Where(temp => temp.id == labTestQAStaffApprovalParams.id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.Lab_result_released_by = labTestQAStaffApprovalParams.Lab_result_released_by;
        existingDataStatus.Lab_result_released_date = DateTime.Now.ToString();
        existingDataStatus.Lab_status = labTestQAStaffApprovalParams.Lab_status;
        existingDataStatus.Lab_exp_date_extension = labTestQAStaffApprovalParams.Lab_exp_date_extension;
        existingDataStatus.Laboratory_procedure = labTestQAStaffApprovalParams.Laboratory_procedure;
        existingDataStatus.FileName = labTestQAStaffApprovalParams.files.FileName;
        existingDataStatus.FilePath = _webHostEnvironment.WebRootPath + "\\Images\\" + labTestQAStaffApprovalParams.files.FileName;
        await db.SaveChangesAsync();
        return existingDataStatus;



      }
      else
      {
        return null;
      }

    }




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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        existingDataStatus.lab_sub_remarks = labTestQASuperVisorApprovalParams.lab_sub_remarks;
        existingDataStatus.laboratory_procedure = labTestQASuperVisorApprovalParams.laboratory_procedure;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
      DryWhLabTestReqLogs existingDataStatus = await db.Dry_wh_lab_test_req_logs.Where(temp => temp.Lab_req_id == labTestQASuperVisorApprovalParams.Lab_req_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.qa_supervisor_is_cancelled_status = labTestQASuperVisorApprovalParams.qa_supervisor_is_cancelled_status;
        existingDataStatus.qa_supervisor_is_cancelled_by = labTestQASuperVisorApprovalParams.qa_supervisor_is_cancelled_by;
        existingDataStatus.qa_supervisor_is_cancelled_date = labTestQASuperVisorApprovalParams.qa_supervisor_is_cancelled_date;
        existingDataStatus.qa_supervisor_cancelled_remarks = labTestQASuperVisorApprovalParams.qa_supervisor_cancelled_remarks;

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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<DryWareHouseReceiving> PutCancelQASupervisorResults([FromBody] DryWareHouseReceiving labTestCancelParams)
    {
      string LabStatus="LAB APPROVED";
      DryWareHouseReceiving existingDataStatus = await db.TblDryWHReceiving.Where(temp => temp.id == labTestCancelParams.id).FirstOrDefaultAsync();
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


        //existingDataStatus.qa_approval_status = null;
        //existingDataStatus.qa_approval_by = null;
        //existingDataStatus.qa_approval_date = null;
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

