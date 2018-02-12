using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eCMS.BusinessLogic.Repositories
{
    public class WorkerToDoRepository : BaseRepository<WorkerToDo>, IWorkerToDoRepository
    {
        public WorkerToDoRepository(RepositoryContext context)
            : base(context)
        {
        }

        public void InsertOrUpdate(WorkerToDo workertodo)
        {
            workertodo.LastUpdateDate = DateTime.Now;
            if (workertodo.ID == default(int))
            {
                workertodo.CreateDate = workertodo.LastUpdateDate;
                workertodo.CreatedByWorkerID = workertodo.LastUpdatedByWorkerID;
                // New entity
                context.WorkerToDo.Add(workertodo);
            }
            else
            {
                // Existing entity
                context.Entry(workertodo).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public List<WorkerToDo> FindAllByWorkerID(int workerId)
        {
            return context.WorkerToDo.Where(item => item.CreatedByWorkerID == workerId).ToList();
        }
    }

    public interface IWorkerToDoRepository : IBaseRepository<WorkerToDo>
    {
        List<WorkerToDo> FindAllByWorkerID(int workerId);
        void InsertOrUpdate(WorkerToDo workertodo);
    }
}
