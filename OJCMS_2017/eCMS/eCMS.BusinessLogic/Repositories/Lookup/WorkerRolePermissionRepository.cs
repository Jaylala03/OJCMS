using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;
using eCMS.Shared;
using System.Collections.Generic;
using System.Linq;
namespace eCMS.BusinessLogic.Repositories
{
    public class WorkerRolePermissionRepository : BaseRepository<WorkerRolePermission>, IWorkerRolePermissionRepository
    {
        public WorkerRolePermissionRepository(RepositoryContext context)
            : base(context)
        {
            
        }
        
        public void FindVisiblity(int workerID,
            ref VisibilityStatus regionVisiblity,
            ref VisibilityStatus programVisiblity,
            ref VisibilityStatus subProgramVisiblity,
            ref VisibilityStatus caseVisiblity)
        {
            List<WorkerRolePermission> permissionList = context.WorkerInRole.Join(context.WorkerRolePermission, left => left.WorkerRoleID, right => right.WorkerRoleID, (left, right) => new { left, right }).
                Where(item=>item.left.WorkerID==workerID).Select(item=>item.right).ToList();
            //Region Visibility
            //int count = permissionList.Where(item => item.Permission == 1 || item.Permission == 2 || item.Permission == 3 || item.Permission == 4).Count();
            //if (count > 0)
            //{
            //    regionVisiblity = VisibilityStatus.All;
            //}
            int count = permissionList.Where(item => item.Permission == 1 || item.Permission == 2 || item.Permission == 3 || item.Permission == 4 || item.Permission == 5 || item.Permission == 6 || item.Permission == 7 || item.Permission == 8).Count();
            if (count > 0 && (regionVisiblity== VisibilityStatus.None || regionVisiblity== VisibilityStatus.UnDefined))
            {
                regionVisiblity = VisibilityStatus.Assigned;
            }

            //Program and SubProgram Visibility
            //count = permissionList.Where(item => item.Permission == 1 || item.Permission == 2 || item.Permission == 5 || item.Permission == 6).Count();
            //if (count > 0)
            //{
            //    programVisiblity = VisibilityStatus.All;
            //    subProgramVisiblity = VisibilityStatus.All;
            //}
            count = permissionList.Where(item => item.Permission == 1 || item.Permission == 2 || item.Permission == 5 || item.Permission == 6 || item.Permission == 3 || item.Permission == 4 || item.Permission == 7 || item.Permission == 8).Count();
            if (count > 0 && (programVisiblity == VisibilityStatus.None || programVisiblity == VisibilityStatus.UnDefined))
            {
                programVisiblity = VisibilityStatus.Assigned;
                subProgramVisiblity = VisibilityStatus.Assigned;
            }

            //Case Visibility
            count = permissionList.Where(item => item.Permission == 1 || item.Permission == 3 || item.Permission == 5 || item.Permission == 7).Count();
            if (count > 0)
            {
                caseVisiblity = VisibilityStatus.All;
            }
            count = permissionList.Where(item => item.Permission == 2 || item.Permission == 4 || item.Permission == 6 || item.Permission == 8).Count();
            if (count > 0 && (caseVisiblity == VisibilityStatus.None || caseVisiblity == VisibilityStatus.UnDefined))
            {
                caseVisiblity = VisibilityStatus.Assigned;
            }
        }
    }

    public interface IWorkerRolePermissionRepository : IBaseRepository<WorkerRolePermission>
    {
        void FindVisiblity(int workerID,
            ref VisibilityStatus regionVisiblity,
            ref VisibilityStatus programVisiblity,
            ref VisibilityStatus subProgramVisiblity,
            ref VisibilityStatus caseVisiblity);
    }
}
