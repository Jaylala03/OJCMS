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
    public class ServiceRepository : BaseLookupRepository<Service>, IServiceRepository
    {
        public ServiceRepository(RepositoryContext context)
            : base(context)
        {
        }
       
        public List<SelectListItem> FindAllForDropDownList(int serviceproviderID)
        {
            return context.Service.Where(item => item.ServiceProviderID == serviceproviderID).OrderBy(item => item.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
        }

        public List<SelectListItem> FindAllActiveForDropDownList(int serviceproviderID)
        {
            return context.Service.Where(item => item.ServiceProviderID == serviceproviderID && item.IsActive==true).OrderBy(item => item.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
        }
        public DataSourceResult FindAllByRegion( Service searchService, DataSourceRequest paramDSRequest)
        {
            StringBuilder sqlQuery = new StringBuilder(@"SElect s.ID,
s.CreateDate,
s.LastUpdateDate,
s.Name,
s.Description,
s.IsActive,
sp.Name as ServiceProviderName,
r.Name as RegionName
from Service s
left join ServiceProvider sp on sp.ID =s.ServiceProviderID
left join Region r on r.ID =sp.RegionID where s.ID>0");
            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1)
            {
                sqlQuery.Append(@" AND RegionID in(
SElect RegionID from WorkerInRole
Where WorkerID=" + CurrentLoggedInWorker.ID + "");
                sqlQuery.Append(")");
            }
            if (searchService.ServiceTypeID ==1)
            {
                sqlQuery.Append(" And sp.IsExternal=0");
            }
            else if (searchService.ServiceTypeID == 2)
            {
                sqlQuery.Append(" And sp.IsExternal=1");
            } 
            if (searchService.ServiceProviderID > 0)
            {
                sqlQuery.Append(" And s.ServiceProviderID=" + searchService.ServiceProviderID + "");
            }
            if (!string.IsNullOrEmpty(searchService.Name))
            {
                sqlQuery.Append(" And s.Name='" + searchService.Name + "'");
            }
            if (!string.IsNullOrEmpty(searchService.Description))
            {
                sqlQuery.Append(" And s.Description='" + searchService.Description + "'");
            }
            if (searchService.RegionID>0)
            {
                sqlQuery.Append(" And sp.RegionID=" + searchService.RegionID + "");
            }


            DataSourceResult dataSourceResult = context.Database.SqlQuery<ServiceListViewModel>(sqlQuery.ToString()).AsEnumerable().GroupBy(m => m.ID).Select(m => m.First()).ToDataSourceResult(paramDSRequest);
            return dataSourceResult;
        }


        public Service FindByID(int ServiceId)
        {
            Service service = new Service();
            StringBuilder sqlQuery = new StringBuilder(@"SElect s.ID,
r.ID as RegionID,
sp.ID as ServiceProviderID,
sp.IsExternal as ServiceTypeID,
s.CreateDate,
s.LastUpdateDate,
s.Name,
s.Description,
s.IsActive,
sp.Name as ServiceProviderName,
r.Name as RegionName,
s.LastUpdateDate,
s.CreateDate
from Service s
left join ServiceProvider sp on sp.ID =s.ServiceProviderID
left join Region r on r.ID =sp.RegionID where s.ID=" + ServiceId + "");

            var serviceModel = context.Database.SqlQuery<ServiceListViewModel>(sqlQuery.ToString()).FirstOrDefault();
            if (serviceModel != null)
            {
                service.ID = serviceModel.ID;
                service.RegionID = serviceModel.RegionID;
                service.ServiceProviderID = serviceModel.ServiceProviderID;
                //service.ServiceTypeID = serviceModel.ServiceTypeID;
                service.Name = serviceModel.Name;
                service.Description = serviceModel.Description;
                service.IsActive=serviceModel.IsActive;
                service.CreateDate = serviceModel.CreateDate;
                service.LastUpdateDate = serviceModel.LastUpdateDate;
                if (serviceModel.ServiceTypeID==true)
                {
                    service.ServiceTypeID = 2;
                }
                else{
                    service.ServiceTypeID = 1;
                }
            }
            return service;
           
        }
    }

    public interface IServiceRepository : IBaseLookupRepository<Service>
    {
        List<SelectListItem> FindAllForDropDownList(int serviceproviderID);
        List<SelectListItem> FindAllActiveForDropDownList(int serviceproviderID);
        DataSourceResult FindAllByRegion( Service searchService, DataSourceRequest paramDSRequest);
        Service FindByID(int ServiceId);
    }
}
