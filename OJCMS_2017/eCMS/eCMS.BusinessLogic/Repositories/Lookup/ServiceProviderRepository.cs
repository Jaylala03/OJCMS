using EasySoft.Helper;
using eCMS.BusinessLogic.Helpers;
using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models;
using eCMS.DataLogic.Models.Lookup;
using eCMS.DataLogic.ViewModels;
using eCMS.ExceptionLoging;
using eCMS.Shared;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace eCMS.BusinessLogic.Repositories
{
    public class ServiceProviderRepository : BaseLookupRepository<ServiceProvider>, IServiceProviderRepository
    {
        public ServiceProviderRepository(RepositoryContext context)
            : base(context)
        {
        }

        public List<SelectListItem> FindAllForDropDownList(int serviceTypeID)
        {
            if (serviceTypeID == 1)
            {
                return context.ServiceProvider.Where(item => item.IsExternal == false).OrderBy(item => item.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
            }
            else if (serviceTypeID == 2)
            {
                return context.ServiceProvider.Where(item => item.IsExternal == true).OrderBy(item => item.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
            }
            return new List<SelectListItem>();
        }

        public List<SelectListItem> FindAllActiveForDropDownList(int serviceTypeID, int? RegionId = 0)
        {
            if (serviceTypeID == 1)
            {
                return context.ServiceProvider.Where(item => item.IsExternal == false && item.RegionID==RegionId && item.IsActive == true ).OrderBy(item => item.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
            }
            else if (serviceTypeID == 2)
            {
                var serviceprovider = context.ServiceProvider.Where(item => (item.IsExternal == true && item.RegionID == RegionId && item.IsActive == true) || item.Name == "Other").OrderBy(item => item.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
               
                return serviceprovider;
            }
            return new List<SelectListItem>();
        }

        public DataSourceResult FindAllByRegion(ServiceProvider searchProvider, DataSourceRequest paramDSRequest)
        {
            StringBuilder sqlQuery = new StringBuilder(@"SElect s.ID,
s.CreateDate,
s.LastUpdateDate,
s.Name,
s.Description,
s.IsActive,
s.IsExternal,
r.Name as RegionName from ServiceProvider s
left join Region r on r.ID =s.RegionID where s.ID>0");


            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1)
            {
                sqlQuery.Append(@" AND RegionID in(
SElect RegionID from WorkerInRole
Where WorkerID=" + CurrentLoggedInWorker.ID + "");
                sqlQuery.Append(")");
            }


            if (searchProvider.RegionID > 0)
            {
                sqlQuery.Append(" And s.RegionID=" + searchProvider.RegionID + "");
            }
            if (!string.IsNullOrEmpty(searchProvider.Name))
            {
                sqlQuery.Append(" And s.Name='" + searchProvider.Name + "'");
            }
            if (!string.IsNullOrEmpty(searchProvider.Description))
            {
                sqlQuery.Append(" And s.Description='" + searchProvider.Description + "'");
            }
            //if (searchProvider.IsActive !=null)
            //{
            //    sqlQuery.Append(" And s.IsActive=" + (searchProvider.IsActive==true?1:0) + "");
            //}
            //if (searchProvider.IsExternal != null)
            //{
            //    sqlQuery.Append(" And s.IsActive=" + (searchProvider.IsExternal == true ? 1 : 0) + "");
            //}
           

            DataSourceResult dataSourceResult = context.Database.SqlQuery<ServiceProviderListViewModel>(sqlQuery.ToString()).AsEnumerable().GroupBy(m=>m.ID).Select(m=>m.First()).ToDataSourceResult(paramDSRequest);
            return dataSourceResult;
        }
        
    }

    public interface IServiceProviderRepository : IBaseLookupRepository<ServiceProvider>
    {
        List<SelectListItem> FindAllForDropDownList(int serviceTypeID);
        List<SelectListItem> FindAllActiveForDropDownList(int serviceTypeID,int? RegionId);
        DataSourceResult FindAllByRegion(ServiceProvider searchProvider,DataSourceRequest paramDSRequest);
    }
}
