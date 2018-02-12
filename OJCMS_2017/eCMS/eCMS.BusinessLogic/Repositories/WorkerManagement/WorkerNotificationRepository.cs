using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eCMS.BusinessLogic.Repositories
{
    public class WorkerNotificationRepository : BaseRepository<WorkerNotification>, IWorkerNotificationRepository
    {
        public WorkerNotificationRepository(RepositoryContext context)
            : base(context)
        {
        }

        public void InsertOrUpdate(WorkerNotification workernotification)
        {
            workernotification.LastUpdateDate = DateTime.Now;
            if (workernotification.ID == default(int))
            {
                workernotification.CreateDate = workernotification.LastUpdateDate;
                workernotification.CreatedByWorkerID = workernotification.LastUpdatedByWorkerID;
                // New entity
                context.WorkerNotification.Add(workernotification);
            }
            else
            {
                // Existing entity
                context.Entry(workernotification).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public List<WorkerNotification> FindAllByWorkerID(int workerId)
        {
            return context.WorkerNotification.Where(item => item.CreatedByWorkerID == workerId).ToList();
        }
    }

    public interface IWorkerNotificationRepository : IBaseRepository<WorkerNotification>
    {
        List<WorkerNotification> FindAllByWorkerID(int workerId);
        void InsertOrUpdate(WorkerNotification workernotification);
    }
}
