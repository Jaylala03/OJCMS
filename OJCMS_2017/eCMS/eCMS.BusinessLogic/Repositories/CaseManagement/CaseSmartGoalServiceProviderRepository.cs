//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class CaseSmartGoalServiceProviderRepository : BaseRepository<CaseSmartGoalServiceProvider>, ICaseSmartGoalServiceProviderRepository
    {
        private readonly ICaseWorkerRepository caseworkerRepository;
        private readonly IWorkerNotificationRepository workernotificationRepository;
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public CaseSmartGoalServiceProviderRepository(RepositoryContext context,
            ICaseWorkerRepository caseworkerRepository,
            IWorkerNotificationRepository workernotificationRepository)
            : base(context)
        {
            this.caseworkerRepository = caseworkerRepository;
            this.workernotificationRepository = workernotificationRepository;
        }

        public IQueryable<CaseSmartGoalServiceProvider> AllIncluding(int caseSmartGoalId, params Expression<Func<CaseSmartGoalServiceProvider, object>>[] includeProperties)
        {
            IQueryable<CaseSmartGoalServiceProvider> query = context.CaseSmartGoalServiceProvider;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.Where(item => item.CaseSmartGoalID == caseSmartGoalId);
        }
        
        /// <summary>
        /// Add or Update CaseSmartGoalServiceProvider to database
        /// </summary>
        /// <param name="CaseSmartGoalServiceProvider">data to save</param>
        public void InsertOrUpdate(CaseSmartGoalServiceProvider casesmartgoalserviceprovider)
        {
            bool isNew = false;
            if (casesmartgoalserviceprovider.WorkerID == 0)
            {
                casesmartgoalserviceprovider.WorkerID = null;
            }
            casesmartgoalserviceprovider.LastUpdateDate = DateTime.Now;
            if (casesmartgoalserviceprovider.ID == default(int))
            {
                isNew = true;
                //set the date when this record was created
                casesmartgoalserviceprovider.CreateDate = casesmartgoalserviceprovider.LastUpdateDate;
                //set the id of the worker who has created this record
                casesmartgoalserviceprovider.CreatedByWorkerID = casesmartgoalserviceprovider.LastUpdatedByWorkerID;
                //add a new record to database
                context.CaseSmartGoalServiceProvider.Add(casesmartgoalserviceprovider);
            }
            else
            {
                //update an existing record to database
                context.Entry(casesmartgoalserviceprovider).State = System.Data.Entity.EntityState.Modified;
            }
            Save();
            if (casesmartgoalserviceprovider.ID > 0 && casesmartgoalserviceprovider.WorkerID.HasValue && casesmartgoalserviceprovider.WorkerID.Value>0)
            {
                string caseLink = "/CaseManagement/CaseSmartGoalServiceProvider/Index?casesmartgoalId=" + casesmartgoalserviceprovider.CaseSmartGoalID + "&CaseID=" + casesmartgoalserviceprovider.CaseID + "&CaseMemberID=" + casesmartgoalserviceprovider.CaseMemberID;
                WorkerNotification workerNotification = new WorkerNotification()
                {
                    IsRead = false,
                    LastUpdateDate = DateTime.Now,
                    LastUpdatedByWorkerID = casesmartgoalserviceprovider.LastUpdatedByWorkerID,
                    ReferenceLink = caseLink,
                    WorkerID = casesmartgoalserviceprovider.WorkerID.Value
                };
                if (isNew)
                {
                    workerNotification.Notification = "A new service provider has been added to a case. Please <a href='" + caseLink + "' target='_blank'>click here</a> to see the service provider detail.";
                }
                else
                {
                    workerNotification.Notification = "A service provider has been updated. Please <a href='" + caseLink + "' target='_blank'>click here</a> to see the service provider detail.";
                }
                workernotificationRepository.InsertOrUpdate(workerNotification);
                workernotificationRepository.Save();
            }
        }

        public CaseSmartGoalServiceProvider Find(int caseSmartGoalId, int serviceProviderID)
        {
            return context.CaseSmartGoalServiceProvider.SingleOrDefault(item => item.CaseSmartGoalID == caseSmartGoalId && item.ServiceProviderID == serviceProviderID);
        }

        public override CaseSmartGoalServiceProvider Find(int id)
        {
            return context.CaseSmartGoalServiceProvider.SingleOrDefault(item => item.ID == id);
        }
    }

    /// <summary>
    /// interface of CaseSmartGoalServiceProvider containing necessary database operations
    /// </summary>
    public interface ICaseSmartGoalServiceProviderRepository : IBaseRepository<CaseSmartGoalServiceProvider>
    {
        IQueryable<CaseSmartGoalServiceProvider> AllIncluding(int caseSmartGoalId, params Expression<Func<CaseSmartGoalServiceProvider, object>>[] includeProperties);
        void InsertOrUpdate(CaseSmartGoalServiceProvider CaseSmartGoalServiceProvider);
        CaseSmartGoalServiceProvider Find(int caseSmartGoalId, int serviceProviderID);
    }
}
