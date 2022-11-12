using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MvcTaskManager.Models;

namespace MvcTaskManager.Identity
{
  public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<ClientLocation> ClientLocations { get; set; }


    public DbSet<Project> Projects { get; set; }
    public DbSet<RMProjectsPartialPo> ProjectsPartialPo { get; set; }

    public DbSet<ApplicationRole> ApplicationRoles { get; set; }

    public DbSet<Skill> Skills { get; set; }

    public DbSet<TblRejectedStats> TblRejectedStatus { get; set; }
    public DbSet<tblDryPartialReceivingRejectionModel> TblDryPartialReceivingRejection { get; set; }
    public DbSet<AllowablePercentageQAModel> TblAllowablePercentageQA { get; set; }

    public DbSet<RMPoSummaryCancelledStats> CancelledPOTransactionStatus { get; set; }
    public DbSet<RMReturnPOTransactionStatus> ReturnPOTransactionStatus { get; set; }
 
    public DbSet<SystemCapabilityStatus> System_capability_status { get; set; }
    public DbSet<tblNearlyExpiryMgmtModel> TblNearlyExpiryMgmt { get; set; }

    public DbSet<DryWhOrder> Dry_wh_orders { get; set; }

    public DbSet<LaboratoryProcedure> Laboratory_procedure { get; set; }
    public DbSet<LabTestRemarks> Laboratory_test_remarks { get; set; }

    public DbSet<LaboratorySubRemark> Laboratory_sub_remarks {get; set; }

    public DbSet<DryWhLabTestReqLogs> Dry_wh_lab_test_req_logs { get; set; }
    public DbSet<DryWareHouseReceiving> TblDryWHReceiving { get; set; }

    public DbSet<Department> Department { get; set; }
    public DbSet<tblLocation> Location { get; set; }
    public DbSet<RawMaterialsDry> Raw_Materials_Dry { get; set; }
    public DbSet<MaterialRequestLogs> Material_request_logs { get; set; }
    public DbSet<TypeofApprover> TypeofApprover { get; set; }
    public DbSet<Position> Position { get; set; }
    public DbSet<DepartmentUnit> DepartmentUnit { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<PrimaryUnit> Primary_Unit { get; set; }
    public DbSet<MaterialRequestMaster> Material_request_master { get; set; }

    public DbSet<ParentCheckList> Parent_checklist { get; set; }


    public DbSet<ChildCheckList> Child_checklist { get; set; }

    public DbSet<GrandChildCheckList> Grandchild_checklist { get; set; }
    public DbSet<CheckListParameters> Checklist_paramaters { get; set; }
    public DbSet<DynamicChecklistLogger> Dynamic_checklist_logger { get; set; }

    public DbSet<InternalOrderActivationRemarks> Internal_order_activation_remarks { get; set; }
    public DbSet<Allocation_Logs> Allocation_Logs { get; set; }
    public DbSet<DryWhOrderParent> Dry_Wh_Order_Parent { get; set; }
    public DbSet<Internal_Preparation_Logs> Internal_Preparation_Logs { get; set; }
    public DbSet<Store_Preparation_Logs> Store_Preparation_Logs { get; set; }
    public DbSet<CancelledLabTestTransactionStatus> CancelledLabTestTransactionStatus { get; set; }
    public DbSet<RoleModules> RoleModules { get; set; }
    public DbSet<Modules> Modules { get; set; }
    public DbSet<MainMenus> MainMenus { get; set; }
    //public DbSet<CheckListParameters> AspNetRoles { get; set; }


  }
}


