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
  public class tblDryWHReceivingController : Controller
  {
    private ApplicationDbContext db;
    public tblDryWHReceivingController(ApplicationDbContext db)
    {
      this.db = db;
    }



    [HttpGet]
    [Route("api/DryWareHouseReceivingForLabTest")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get()
    {
      //List<DryWareHouseReceiving> projects = db.tblDryWHReceiving.Where(temp => temp.is_active.Equals(true)

      int CancelledByQAStatus = 3;
      List<DryWareHouseReceiving> projects = await db.TblDryWHReceiving.Where(temp => temp.is_active == 1

      && temp.lab_request_by != null
      && temp.lab_result_released_by == null
      != temp.qa_approval_status.Contains(CancelledByQAStatus.ToString())
      ).ToListAsync();


      List<DryWareHouseReceivingViewModel> projectsViewModel = new List<DryWareHouseReceivingViewModel>();
      foreach (var project in projects)
      {
        //int dayDiff = (project.Expiration_date_string - DateTime.Now).Days;
        int dayDiffExpiryDaysAging = (project.lab_exp_date_extension - DateTime.Now).Days;
        //if(project.qa_approval_date == null)
        //{
        //  int LaboratoryAging = ((DateTime.Now - DateTime.Now)).Days;
        //}
        //else
        //{
        //  int LaboratoryAging = ((TimeSpan)(project.qa_approval_date - DateTime.Now)).Days;
        //}
        int LaboratoryAging = ((TimeSpan)(project.qa_approval_date - DateTime.Now)).Days;
        projectsViewModel.Add(new DryWareHouseReceivingViewModel()
        {
          Id = project.id,
          Lab_access_code = project.lab_access_code,
          Index_id_partial = project.index_id_partial,
          //Item_code = project.DateOfStart.ToString("dd/MM/yyyy"),
          Item_code = project.item_code,
          Item_description = project.item_description,
          Category = project.category,
          Uom = project.uom,
          Qty_received = project.qty_received,
          Historical_lab_transact_count = project.historical_lab_transact_count,
          Lab_status = project.lab_status,
          //Expiry_days_aging = project.expiry_days_aging,
          Client_requestor = project.client_requestor,
          Lab_request_date = project.lab_request_date,
          Lab_request_by = project.lab_request_by,
          Po_number = project.po_number,
          Qa_approval_status = project.qa_approval_status,
          Qa_approval_by = project.qa_approval_by,
          Qa_approval_date = project.qa_approval_date.ToString("MM/dd/yyyy"),
          Lab_result_released_by = project.lab_result_released_by,
          Lab_result_released_date = project.lab_result_released_date,
          Lab_result_remarks = project.lab_result_remarks,
          Lab_sub_remarks = project.lab_sub_remarks,
          Is_active = project.is_active.ToString(),
          Lab_exp_date_extension = project.lab_exp_date_extension.ToString("MM/dd/yyyy"),

          //Sample
          Expiry_days_aging = dayDiffExpiryDaysAging,
          Lab_approval_aging_days = LaboratoryAging,
          Supplier = project.supplier,
          Po_date = project.po_date,
          Pr_no = project.pr_no,
          Pr_date = project.pr_date
          //DaysBeforeExpired = dayDiff

        });
      }
      return Ok(projectsViewModel);




    }




    [HttpGet]
    [Route("api/DryWareHouseReceivingForLabTest/NearlyExpiry")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetItemNearlyExpiry()
    {


      List<DryWareHouseReceiving> projects = await db.TblDryWHReceiving.Include("tblNearlyExpiryMgmtModel").Where(temp => temp.is_active == 1
      && temp.FK_Sub_Category_IsExpirable == 1

      && (temp.lab_exp_date_extension - DateTime.Now).Days < temp.tblNearlyExpiryMgmtModel.p_nearly_expiry_desc
      //&& temp.lab_request_by != null temp.is_active.Equals(true)
      //&& temp.lab_result_released_by == null
      //!= temp.qa_approval_status.Contains(CancelledByQAStatus.ToString())
      ).ToListAsync();


      List<DryWareHouseReceivingViewModelNearlyExpiry> projectsViewModel = new List<DryWareHouseReceivingViewModelNearlyExpiry>();
      foreach (var project in projects)
      {
        //int dayDiff = (project.Expiration_date_string - DateTime.Now).Days;
        int dayDiffExpiryDaysAging = (project.lab_exp_date_extension - DateTime.Now).Days;

        int LaboratoryAging = ((TimeSpan)(project.qa_approval_date - DateTime.Now)).Days;
        projectsViewModel.Add(new DryWareHouseReceivingViewModelNearlyExpiry()
        {
          Id = project.id,
          Item_code = project.item_code,
          Item_description = project.item_description,
          Qty_received = project.qty_received,
          Lab_exp_date_extension = project.lab_exp_date_extension.ToString("MM/dd/yyyy"),
          Expiry_days_aging = dayDiffExpiryDaysAging,
          Standard_Expiry_Days = project.tblNearlyExpiryMgmtModel.p_nearly_expiry_desc.ToString(),
          RemainingQty = project.qty_received,



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

      //string data_is_pending = "1";
      string is_activated = "1";
      //string LaboratoryResult = "LAB RESULT";
      //string LaboratoryReceived = "LAB RECEIVED";


      //projects = db.dry_wh_lab_test_req_logs.Where(temp => temp.is_received_status.Contains(is_activated)).ToList();

      //db.Projects.Include("ClientLocation").Where
      List<DryWhLabTestReqLogs> projects = null;
      projects = await db.Dry_wh_lab_test_req_logs.Include("DryWareHouseReceiving")
        .Where(temp => temp.is_active.Contains(is_activated)

        //&& temp.DryWareHouseReceiving.lab_status.Contains(LaboratoryReceived)


          && temp.qa_supervisor_is_approve_status.Equals(true)
          && temp.lab_result_received_by != null

      ).ToListAsync();

      //&& temp.is_approved != null
      List<DryWhLabTestReqLogsViewModel> WarehouseReceivingContructor = new List<DryWhLabTestReqLogsViewModel>();
      foreach (var project in projects)
      {
        int LaboratoryAging = ((TimeSpan)(project.DryWareHouseReceiving.qa_approval_date - project.lab_request_date)).Days;
        int dayDiffExpiryDaysAging = (project.DryWareHouseReceiving.lab_exp_date_extension - project.bbd).Days;
        string LabStatus = "";
            if (project.DryWareHouseReceiving.lab_status == null)
            {

            LabStatus = "LAB RECEIVED";
            //project.DryWareHouseReceiving.lab_status
            }
            else
            {
            LabStatus = project.DryWareHouseReceiving.lab_status;
            }

        WarehouseReceivingContructor.Add(new DryWhLabTestReqLogsViewModel()
        {

          Lab_req_id = project.lab_req_id,
          Item_code = project.item_code,
          Item_desc = project.item_desc,
          Category = project.category,
          Qty_received = project.qty_received,
          Remaining_qty = project.remaining_qty,
          Days_to_expired = dayDiffExpiryDaysAging.ToString(),
          Lab_status = LabStatus,
          Historical = project.historical,
          Aging = project.aging,
          Remarks = project.remarks,
          Fk_receiving_id = project.fk_receiving_id,
          //Is_active = project.is_active,
          Added_by = project.added_by,
          Date_added = project.date_added.ToString("MM/dd/yyyy"),
          Qa_approval_by = project.qa_approval_by,
          Qa_approval_status = project.qa_approval_status,
          Qa_approval_date = project.DryWareHouseReceiving.qa_approval_date.ToString("MM/dd/yyyy"),

          Lab_result_released_by = project.lab_result_released_by,
          Lab_result_released_date = project.lab_result_released_date,
          Lab_result_remarks = project.lab_result_remarks,
          Lab_sub_remarks = project.lab_sub_remarks,
          Lab_exp_date_extension = project.DryWareHouseReceiving.lab_exp_date_extension.ToString("MM/dd/yyyy"),
          Laboratory_procedure = project.laboratory_procedure,
          Lab_request_date = project.DryWareHouseReceiving.lab_request_date,
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
          Client_requestor = project.DryWareHouseReceiving.client_requestor,
          Supplier = project.DryWareHouseReceiving.supplier,

          Qa_supervisor_is_approve_status = project.qa_supervisor_is_approve_status,
          Qa_supervisor_is_approve_by = project.qa_supervisor_is_approve_by,
          Qa_supervisor_is_approve_date = project.qa_supervisor_is_approve_date,


          Qa_supervisor_is_cancelled_status = project.qa_supervisor_is_cancelled_status,
          Qa_supervisor_is_cancelled_by = project.qa_supervisor_is_cancelled_by,
          Qa_supervisor_is_cancelled_date = project.qa_supervisor_is_cancelled_date,
          Qa_supervisor_cancelled_remarks = project.qa_supervisor_cancelled_remarks

        }) ; 
      }

      return Ok(WarehouseReceivingContructor);



    }


    [HttpGet]
    [Route("api/DryWareHouseReceivingForLabTest/LabResultApproval")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetLabResultForApproval()
    {

      //string data_is_pending = "1";
      string is_activated = "1";
      string LaboratoryResult = "LAB RESULT";


      //projects = db.dry_wh_lab_test_req_logs.Where(temp => temp.is_received_status.Contains(is_activated)).ToList();

      //db.Projects.Include("ClientLocation").Where
      List<DryWhLabTestReqLogs> projects = null;
      projects = await db.Dry_wh_lab_test_req_logs.Include("DryWareHouseReceiving")
        .Where(temp => temp.is_active.Contains(is_activated)

        && temp.DryWareHouseReceiving.lab_status.Contains(LaboratoryResult)
          //&& temp.qa_supervisor_is_approve_status.Equals(false)
                    && temp.lab_result_received_by == null

      ).ToListAsync();

      //&& temp.is_approved != null
      List<DryWhLabTestReqLogsViewModel> WarehouseReceivingContructor = new List<DryWhLabTestReqLogsViewModel>();
      foreach (var project in projects)
      {
        int LaboratoryAging = ((TimeSpan)(project.DryWareHouseReceiving.qa_approval_date - project.lab_request_date)).Days;
        int dayDiffExpiryDaysAging = (project.DryWareHouseReceiving.lab_exp_date_extension - project.bbd).Days;
        string LabStatus = "";
        if (project.DryWareHouseReceiving.lab_status == null)
        {

          LabStatus = "LAB RECEIVED";
          //project.DryWareHouseReceiving.lab_status
        }
        else
        {
          LabStatus = project.DryWareHouseReceiving.lab_status;
        }

        WarehouseReceivingContructor.Add(new DryWhLabTestReqLogsViewModel()
        {

          Lab_req_id = project.lab_req_id,
          Item_code = project.item_code,
          Item_desc = project.item_desc,
          Category = project.category,
          Qty_received = project.qty_received,
          Remaining_qty = project.remaining_qty,
          Days_to_expired = dayDiffExpiryDaysAging.ToString(),
          Lab_status = LabStatus,
          Historical = project.historical,
          Aging = project.aging,
          Remarks = project.remarks,
          Fk_receiving_id = project.fk_receiving_id,
          //Is_active = project.is_active,
          Added_by = project.added_by,
          Date_added = project.date_added.ToString("MM/dd/yyyy"),
          Qa_approval_by = project.qa_approval_by,
          Qa_approval_status = project.qa_approval_status,
          Qa_approval_date = project.DryWareHouseReceiving.qa_approval_date.ToString(),
          Lab_result_released_by = project.lab_result_released_by,
          Lab_result_released_date = project.lab_result_released_date,
          Lab_result_remarks = project.lab_result_remarks,
          Lab_sub_remarks = project.lab_sub_remarks,
          Lab_exp_date_extension = project.DryWareHouseReceiving.lab_exp_date_extension.ToString(),
          Laboratory_procedure = project.laboratory_procedure,
          Lab_request_date = project.DryWareHouseReceiving.lab_request_date,
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
          Client_requestor = project.DryWareHouseReceiving.client_requestor,
          Supplier = project.DryWareHouseReceiving.supplier,

          Qa_supervisor_is_approve_status = project.qa_supervisor_is_approve_status,
          Qa_supervisor_is_approve_by = project.qa_supervisor_is_approve_by,
          Qa_supervisor_is_approve_date = project.qa_supervisor_is_approve_date,


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

      //string data_is_pending = "1";
      string is_activated = "1";
      List<DryWhLabTestReqLogs> projects = null;

      //string ReceivedID = searchText;
      int ReceivedID = searchText;
      //if (searchBy == "store_name")       

      projects = await db.Dry_wh_lab_test_req_logs.Where(temp => temp.is_received_status.Contains(is_activated) && temp.fk_receiving_id == ReceivedID).ToListAsync();


      List<DryWhLabTestReqLogsViewModel> WarehouseStoreOrderContructor = new List<DryWhLabTestReqLogsViewModel>();
      foreach (var project in projects)
      {
        WarehouseStoreOrderContructor.Add(new DryWhLabTestReqLogsViewModel()
        {

          Lab_req_id = project.lab_req_id,
          Item_code = project.item_code,
          Item_desc = project.item_desc,
          Category = project.category,
          Qty_received = project.qty_received,
          Remaining_qty = project.remaining_qty,
          Days_to_expired = project.days_to_expired.ToString(),
          Lab_status = project.lab_status,
          Historical = project.historical,
          Aging = project.aging,
          Remarks = project.remarks,
          Fk_receiving_id = project.fk_receiving_id,
          Is_active = project.is_active,
          Added_by = project.added_by,
          Date_added = project.date_added.ToString("MM/dd/yyyy"),
          Qa_approval_by = project.qa_approval_by,
          Qa_approval_status = project.qa_approval_status,
          Qa_approval_date = project.qa_approval_date,
          Lab_result_released_by = project.lab_result_released_by,
          Lab_result_released_date = project.lab_result_released_date,
          Lab_result_remarks = project.lab_result_remarks,
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
      if (existingDataStatus != null)
      {
        existingDataStatus.qa_approval_by = labTestQAStaffApprovalParams.qa_approval_by;
        existingDataStatus.qa_approval_status = labTestQAStaffApprovalParams.qa_approval_status;
        existingDataStatus.qa_approval_date = labTestQAStaffApprovalParams.qa_approval_date;
        existingDataStatus.lab_status = labTestQAStaffApprovalParams.lab_status;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }


    [HttpPut]
    [Route("api/DryWareHouseReceivingForLabTest/QAReleasingResult")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<DryWareHouseReceiving> PutQAResults([FromBody] DryWareHouseReceiving labTestQAStaffApprovalParams)
    {
      string LaboratoryStatus = "LAB RESULT";
      DryWareHouseReceiving existingDataStatus = await db.TblDryWHReceiving.Where(temp => temp.id == labTestQAStaffApprovalParams.id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.lab_result_released_by = labTestQAStaffApprovalParams.lab_result_released_by;
        existingDataStatus.lab_result_released_date = labTestQAStaffApprovalParams.lab_result_released_date;
        existingDataStatus.lab_status = LaboratoryStatus;
        //labTestQAStaffApprovalParams.lab_status;
        existingDataStatus.lab_result_remarks = labTestQAStaffApprovalParams.lab_result_remarks;
        existingDataStatus.lab_sub_remarks = labTestQAStaffApprovalParams.lab_sub_remarks;
        existingDataStatus.lab_exp_date_extension = labTestQAStaffApprovalParams.lab_exp_date_extension;
        existingDataStatus.laboratory_procedure = labTestQAStaffApprovalParams.laboratory_procedure;
      
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
      if (existingDataStatus != null)
      {
        existingDataStatus.lab_cancel_by = labTestCancelParams.lab_cancel_by;
        existingDataStatus.lab_cancel_date = labTestCancelParams.lab_cancel_date;
        existingDataStatus.lab_cancel_remarks = labTestCancelParams.lab_cancel_remarks;
        existingDataStatus.qa_approval_status = labTestCancelParams.qa_approval_status;
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
      DryWhLabTestReqLogs existingDataStatus = await db.Dry_wh_lab_test_req_logs.Where(temp => temp.lab_req_id == labTestQASuperVisorApprovalParams.lab_req_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.qa_supervisor_is_approve_status = labTestQASuperVisorApprovalParams.qa_supervisor_is_approve_status;
        existingDataStatus.qa_supervisor_is_approve_by = labTestQASuperVisorApprovalParams.qa_supervisor_is_approve_by;
        existingDataStatus.qa_supervisor_is_approve_date = labTestQASuperVisorApprovalParams.qa_supervisor_is_approve_date;
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
      DryWhLabTestReqLogs existingDataStatus = await db.Dry_wh_lab_test_req_logs.Where(temp => temp.lab_req_id == labTestQASuperVisorApprovalParams.lab_req_id).FirstOrDefaultAsync();
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
        existingDataStatus.lab_status = LabStatus;
        existingDataStatus.lab_result_remarks = null;

        existingDataStatus.lab_result_released_by = null;
        existingDataStatus.lab_result_released_date = null;
        existingDataStatus.lab_sub_remarks = null;
        existingDataStatus.laboratory_procedure = null;
        existingDataStatus.lab_exp_date_extension = Convert.ToDateTime(existingDataStatus.lab_exp_date_request);



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

