//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************


using eCMS.BusinessLogic.Repositories;
using eCMS.Web.Controllers;
namespace eCMS.Web.Areas.WorkerManagement.Controllers
{
    public class WorkerBaseController : BaseController
    {
        public WorkerBaseController(IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository
            , IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository)
            : base(workerroleactionpermissionRepository, workerroleactionpermissionnewRepository)
        {
        }

        public WorkerBaseController(IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository)
            : base(workerroleactionpermissionRepository)
        {
        }
    }
}
