# Ultra-Maverick-BackEnd-Asp.Net-Core-2.1--2021---2022-Prelaunched



ANGULAR SPA NEW GERARD

node js 
vs code
angular cli


md c:\angular
cd angular

ng new TaskManagerQA --style=scss --routing

ng serve

ng serve --open
ng serve --open --port=5200


npm install jquery --save
npm install popper.js --save
npm install bootstrap --save
npm install font-awesome --save


ng g component Dashboard

ng g component About

Dta bind 4 
Interpolation {{}
}
Property []
Event ()
Two Way Binding [()]


ng g component MyProfile


ng g module Admin

ng g service Dashboard

Microsoft.EntityFrameworkCore.Sql Server 5.0.5
Microsoft.AspNetCore.Mvc 2.2.0

ng g service Projects

www.learnrxjs.io


//Fore JSon server api
mpm install json-server -g
cd c:data
//end


Microsoft.AspNetCore.Identity.EntityFrameworkCore



ng g class Project
ng g component Projects

ng g component Login
ng g service Login

ng g class LoginViewModel
Microsoft.EntityFrameworkCore.Tools
Add-Migration Initial
Update-Database

Microsoft.AspNetCore.Razor.Design 

Microsoft.VisualStudio.Web.CodeGeneration.Design b


ng g service JwtInterceptor
ng g service JwtUnAuthorizedInterceptor


npm install @auth0/angular-jwt --save

ng g service CanActivateGuard

ng g class ClientLocation
ng g service ClientLocations

Admin123# 
000000
2021-09-20
    $("#AddRejectBtn").hide();
http://localhost:54573/api/projects

        [HttpGet]
        [Route("api/projects/{ProjectName}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get(string ProjectName)
        {
      //System.Threading.Thread.Sleep(1000);
      //List<Project> projects = db.Projects.Include("ClientLocation").ToList();
      //List<Project> projects = db.Projects.Include("ClientLocation").Where(temp => temp.ProjectName == ProjectName).FirstOrDefault();

      List<Project> projects = db.Projects.Include("ClientLocation").Where(temp => temp.ProjectName == ProjectName).ToList();
      List<ProjectViewModel> projectsViewModel = new List<ProjectViewModel>();
            foreach (var project in projects)
            {
                projectsViewModel.Add(new ProjectViewModel() { ProjectID = project.ProjectID, ProjectName = project.ProjectName, TeamSize = project.TeamSize, DateOfStart = project.DateOfStart.ToString("dd/MM/yyyy"), Active = project.Active , ClientLocation = project.ClientLocation, ClientLocationID = project.ClientLocationID, Status = project.Status });
            }
            return Ok(projectsViewModel);
        }

ng g directive TeamSizeValidator

ng g directive ClientLocationStatusValidator

ng g directive ProjectIDUniqueValidator


ng g component SignUp

ng g service countries
ng g class country
ng g service custom-validators  // Customvalidators

ng g class SignUpViewModel

ng g component Tasks

add-migration SignUpForm
Update-Database


ng g component project

ng g component CheckBoxPrinter


npm install gulp-cli -g

npm install gulp --save-dev
ng build --watch
gulp


ng g pipe NumberToWords
ng g pipe Paging


ng g pipe Filter


ng g directive Alert

ng g directive Repeater
ng g component ProjectDetails

ng g module AdminRouting

ng g module Employee

ng g module EmployeeRouting

ng g service RouterLogger


ng g service CanDeactivateGuard
ng g module shared
ng g component Countries

ng g component ClientLocations

ng g component TaskPriorities

ng g component TaskStatus

ng g component Masters

ng g directive ComponentLoader

ng g pipe Sort

Add-Migration TaskPriority

Update-Database

ng g class TaskPriority

ng g service TaskPriorities

Add-Migration TaskStatuses

ng g class TaskStatus

ng g service TaskStatuses

ng g component CreateTask

ng g component EditTask

ng g component UpdateTaskStatus

Add-Migration Tasks''
Update-Database

ng g class Task
ng g service Tasks

ng g class GroupedTask
ng g class TaskStatusDetail


ng add @angular/material
indigo pink Theme
yes for styling global typograp[hty 
yes browser animations

ng serve --open


ng g module Material

go to materials icon

implement sweet alert and ngx toaster

npm install moment --save


npm i --save angular-highcharts highcharts


ng g component RejectedStatus

ng g class RejectedStatus

ng g service RejectedStatus


post

ng g component system-capability-status
ng g class system-capability-status
ng g service system-capability-status


ng g component UserAccount

ng g class UserAccount

ng g service UserAccount



Update datqabase change databse taskmanager to ultraMaverickDB


ng g component AllowablePercentage

ng g class AllowablePercentage

ng g service AllowablePercentage


ng g component CancelledPOTransactionStatus

ng g class CancelledPOTransactionStatus

ng g service CancelledPOTransactionStatus


ng g component ProjectsCancelledPo


ng g component ReturnedPOTransactionStatus

ng g class ReturnedPOTransactionStatus

ng g service ReturnedPOTransactionStatus


ng g component ProjectsPartialPo

ng g class ProjectsPartialPo  elliminatere

ng g service ProjectsPartialPo


npmjs/com/package/ngx-spinner

ng add ngx-spinner
npm install @angular/cdk ===
ng g s busy --skip-tests
ng g interceptor loading --skip-tests




ng g component ProjetPONearlyExpiryApproval


ng g service ProjetPONearlyExpiryApproval



ng g component tblNearlyExpiryMgmt

ng g class tblNearlyExpiryMgmt

ng g service tblNearlyExpiryMgmt

    public string Days_expiry_setup { get; set; }
    public string Is_expired { get; set; }


ng g component AspNetRoles

ng g class AspNetRoles

ng g service AspNetRoles



ng g component WhRejectionApproval

ng g class WhRejectionApproval

ng g service WhRejectionApproval





ng g service WhCheckerDashboard

ng g class DryWhStoreOrders

ng g component StoreOrder

ng g component PreparedStoreOrder


ng g component WhRejectionApproval

ng g class WhRejectionApproval

ng g service WhRejectionApproval


ng g component StoreOrderActiveCancelledTransaction

ng g component StoreOrderDispatchingRecord



ng g class StorePreparationLogs

ng g component ParentMainModules

ng g class MainMenus

ng g service MainMenus

ng g component Modules
ng g class Modules

ng g service Modules



ng g class NearlyExpiryItems

ng g component NearlyExpiryItems











