using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eCMS.BusinessLogic.Repositories
{
    public class CaseAuditLogRepository : BaseRepository<CaseAuditLog>, ICaseAuditLogRepository
    {
        public CaseAuditLogRepository(RepositoryContext context)
            : base(context)
        {
        }

        public override IQueryable<CaseAuditLog> All
        {
            get { return context.CaseAuditLog.OrderBy(item => item.Created_date); }
        }

        public void InsertOrUpdate(CaseAuditLog caseAuditLog)
        {

            caseAuditLog.Created_date = DateTime.Now;
            if (caseAuditLog.LogID == default(Guid))
            {
                //set the date when this record was created
                caseAuditLog.Created_date = caseAuditLog.Created_date;
                //set the id of the worker who has created this record
                caseAuditLog.Created_by = caseAuditLog.Created_by;
                //add a new record to database
                context.CaseAuditLog.Add(caseAuditLog);
            }
            else
            {
                //update an existing record to database
                context.Entry(caseAuditLog).State = System.Data.Entity.EntityState.Modified;
            }
            Save();

        }

        public DataSourceResult Search(DataSourceRequest dsRequest, string caseId, string tableName)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }

            var tables =tableName.Split(',');
            //DataSourceResult dsResult = context.CaseAuditLog
            //    .Where(item => item.ActionID == caseId && tables.Contains(item.TableName))
            //    .OrderByDescending(item => item.Created_date);
            List<CaseAuditLog> auditList = new List<CaseAuditLog>();
            CaseAuditLog auditLog = new CaseAuditLog();
            var result = (from c in context.CaseAuditLog
                                         where c.ActionID == caseId
                                         && tables.Contains(c.TableName)
                                         select c).ToList();
            //var zone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
          
        
            foreach(var item in result)
            {
                auditLog.ActionID = item.ActionID;
                auditLog.ColumnName = item.ColumnName;
                auditLog.Created_by = item.Created_by;
                auditLog.Date = item.Created_date.ToLocalTime().ToString();
                auditLog.EventType = item.EventType;
                auditLog.LogID = item.LogID;
                auditLog.NewValue = item.NewValue;
                auditLog.OriginalValue = item.OriginalValue;
                auditLog.RecordID = item.RecordID;
                auditLog.TableName = item.TableName;
                auditLog.Created_date = item.Created_date;
                auditList.Add(auditLog);
                auditLog = new CaseAuditLog();
            }

            DataSourceResult dsResult = auditList.ToDataSourceResult(dsRequest);
            return dsResult;
        }

    }

    public interface ICaseAuditLogRepository : IBaseRepository<CaseAuditLog>
    {
        void InsertOrUpdate(CaseAuditLog caseAuditLog);
        DataSourceResult Search(DataSourceRequest dsRequest, string caseId, string tableName);
    }
}
