using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;
using eCMS.DataLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace eCMS.BusinessLogic.Repositories
{
    public class JamatkhanaRepository : BaseLookupRepository<Jamatkhana>, IJamatkhanaRepository
    {
        public JamatkhanaRepository(RepositoryContext context)
            : base(context)
        {
        }
        public IEnumerable<SelectListItem> FindAllByRegionID(int regionID)
        {
            return context.Jamatkhana.Where(item => item.RegionID == regionID).OrderBy(item => item.Name).AsEnumerable().Select(item => new SelectListItem { Text = item.Name, Value = item.ID.ToString() }).ToList(); ;
        }

        public List<Jamatkhana> FindAllJamatkhanaByRegionID(int regionID)
        {
            if (regionID > 0)
            {
                return context.Jamatkhana.Where(item => item.IsActive == true && item.RegionID == regionID).OrderBy(item => item.Name).ToList();
            }
            else
            {
                return context.Jamatkhana.Where(item => item.IsActive == true).OrderBy(item => item.Name).ToList();
            }
        }
        public List<int> FindAllJamatkhanaIDsByRegionID(int regionID)
        {
            if (regionID > 0)
            {
                return context.Jamatkhana.Where(item => item.IsActive == true && item.RegionID == regionID).OrderBy(item => item.Name).Select(item => item.ID).ToList();
            }
            else
            {
                return context.Jamatkhana.Where(item => item.IsActive == true).OrderBy(item => item.Name).Select(item => item.ID).ToList();
            }
        }
        public List<SelectListItem> FindAllByWorkerRegionID(int regionID)
        {
            List<DropDownViewModel> subprogram = null;
            string loggedinworkers = String.Join(",", CurrentLoggedInWorkerRoleIDs);
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("SELECT JK.ID,JK.Name ");
            sqlQuery.Append("FROM WorkerInRoleNew AS WIR ");
            sqlQuery.Append("INNER JOIN WorkerRolePermissionNew AS WRP ON WIR.WorkerRoleID = WRP.WorkerRoleID ");
            sqlQuery.Append("INNER JOIN Permission AS P ON WRP.PermissionID = P.ID ");
            sqlQuery.Append("INNER JOIN PermissionRegion AS PR ON P.ID = PR.PermissionID ");
            sqlQuery.Append("INNER JOIN Jamatkhana AS JK ON PR.RegionID = JK.RegionID ");
            sqlQuery.Append("WHERE WIR.WorkerID = " + CurrentLoggedInWorker.ID + " AND WIR.WorkerRoleID IN (" + loggedinworkers + ") ");
            sqlQuery.Append("AND JK.IsActive = 1 ");
            if (regionID > 0)
            {
                sqlQuery.Append("AND PR.RegionID = " + regionID + " ");
            }
            sqlQuery.Append("GROUP BY JK.ID,JK.Name ");
            sqlQuery.Append("ORDER BY JK.Name ");

            subprogram = context.Database.SqlQuery<DropDownViewModel>(sqlQuery.ToString()).ToList();

            return subprogram.AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
        }

        public List<SelectListItem> FindAllByWorkerRegionIDs(string programIDs, string regionIDs)
        {
            List<DropDownViewModel> subprogram = null;
            string loggedinworkers = String.Join(",", CurrentLoggedInWorkerRoleIDs);
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("SELECT JK.ID,JK.Name ");
            sqlQuery.Append("FROM WorkerInRoleNew AS WIR ");
            sqlQuery.Append("INNER JOIN WorkerRolePermissionNew AS WRP ON WIR.WorkerRoleID = WRP.WorkerRoleID ");
            sqlQuery.Append("INNER JOIN Permission AS P ON WRP.PermissionID = P.ID ");
            sqlQuery.Append("INNER JOIN PermissionRegion AS PR ON P.ID = PR.PermissionID ");
            sqlQuery.Append("INNER JOIN Jamatkhana AS JK ON PR.RegionID = JK.RegionID ");
            sqlQuery.Append("WHERE JK.IsActive = 1 ");
            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && CurrentLoggedInWorker.ID > 0)
            {
                sqlQuery.Append("AND WIR.WorkerID = " + CurrentLoggedInWorker.ID + " AND WIR.WorkerRoleID IN (" + loggedinworkers + ") ");
            }
            if (!string.IsNullOrEmpty(programIDs))
            {
                sqlQuery.Append("AND PR.ProgramID IN (" + programIDs + ") ");
            }
            if (!string.IsNullOrEmpty(regionIDs))
            {
                sqlQuery.Append("AND PR.RegionID IN (" + regionIDs + ") ");
            }
            sqlQuery.Append("GROUP BY JK.ID,JK.Name ");
            sqlQuery.Append("ORDER BY JK.Name ");

            subprogram = context.Database.SqlQuery<DropDownViewModel>(sqlQuery.ToString()).ToList();

            return subprogram.AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
        }

    }

    public interface IJamatkhanaRepository : IBaseLookupRepository<Jamatkhana>
    {
        IEnumerable<SelectListItem> FindAllByRegionID(int regionID);

        List<SelectListItem> FindAllByWorkerRegionID(int regionID);

        List<Jamatkhana> FindAllJamatkhanaByRegionID(int regionID);
        List<int> FindAllJamatkhanaIDsByRegionID(int regionID);

        List<SelectListItem> FindAllByWorkerRegionIDs(string programIDs,string regionIDs);
    }
}
