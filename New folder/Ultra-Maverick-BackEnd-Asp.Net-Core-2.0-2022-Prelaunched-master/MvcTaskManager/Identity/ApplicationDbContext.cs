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
    //public DbSet<Store_Preparation_LogsModel> Store_Preparation_Logs { get; set; }

    public DbSet<Project> Projects { get; set; }
    public DbSet<RMProjectsPartialPo> ProjectsPartialPo { get; set; }

    public DbSet<ApplicationRole> ApplicationRoles { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<TaskPriority> TaskPriorities { get; set; }
    public DbSet<TaskStatus> TaskStatuses { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<TaskStatusDetail> TaskStatusDetails { get; set; }
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


    //public DbSet<CheckListParameters> AspNetRoles { get; set; }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        base.OnModelCreating(modelBuilder);

    //        modelBuilder.Entity<ClientLocation>().HasData(
    //            new ClientLocation() { ClientLocationID = 1, ClientLocationName = "Boston" },
    //            new ClientLocation() { ClientLocationID = 2, ClientLocationName = "New Delhi" },
    //            new ClientLocation() { ClientLocationID = 3, ClientLocationName = "New Jersy" },
    //            new ClientLocation() { ClientLocationID = 4, ClientLocationName = "New York" },
    //            new ClientLocation() { ClientLocationID = 5, ClientLocationName = "London" },
    //            new ClientLocation() { ClientLocationID = 6, ClientLocationName = "Tokyo" }
    //        );

    //        modelBuilder.Entity<Project>().HasData(
    //            new Project() { ProjectID = 1, ProjectName = "Hospital Management System", DateOfStart = Convert.ToDateTime("2017-8-1"), Active = true, ClientLocationID = 2, Status = "In Force", TeamSize = 14 },
    //            new Project() { ProjectID = 2, ProjectName = "Reporting Tool", DateOfStart = Convert.ToDateTime("2018-3-16"), Active = true, ClientLocationID = 1, Status = "Support", TeamSize = 81 }
    //        );

    //        modelBuilder.Entity<Country>().HasData(
    //            new Country() { CountryID = 1, CountryName = "China" },
    //            new Country() { CountryID = 2, CountryName = "United States" },
    //            new Country() { CountryID = 3, CountryName = "Indonesia" },
    //            new Country() { CountryID = 4, CountryName = "Brazil" }

    //        );

    //        modelBuilder.Entity<TaskPriority>().HasData(
    //            new TaskPriority() { TaskPriorityID = 1, TaskPriorityName = "Urgent" },
    //            new TaskPriority() { TaskPriorityID = 2, TaskPriorityName = "Normal" },
    //            new TaskPriority() { TaskPriorityID = 3, TaskPriorityName = "Below Normal" },
    //            new TaskPriority() { TaskPriorityID = 4, TaskPriorityName = "Low" }
    //         );

    //        modelBuilder.Entity<TaskStatus>().HasData(
    //            new TaskStatus() { TaskStatusID = 1, TaskStatusName = "Holding" }, //Tasks that need to be documented still
    //            new TaskStatus() { TaskStatusID = 2, TaskStatusName = "Prioritized" }, //Tasks that are placed in priority order; so need to start ASAP
    //            new TaskStatus() { TaskStatusID = 3, TaskStatusName = "Started" }, //Tasks that are currently working
    //            new TaskStatus() { TaskStatusID = 4, TaskStatusName = "Finished" }, //Tasks that are finished workng
    //            new TaskStatus() { TaskStatusID = 5, TaskStatusName = "Reverted" } //Tasks that are reverted back, with comments or issues
    //         );

    //    }
  }
}


