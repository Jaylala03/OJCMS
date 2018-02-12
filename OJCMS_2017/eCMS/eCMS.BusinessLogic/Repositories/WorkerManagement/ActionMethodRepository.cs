//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models;
using eCMS.DataLogic.Models.Lookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class ActionMethodRepository : BaseRepository<ActionMethod>, IActionMethodRepository
    {
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public ActionMethodRepository(RepositoryContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Add or Update program to database
        /// </summary>
        /// <param name="program">data to save</param>
        public void InsertOrUpdate(ActionMethod actionmethod)
        {
            actionmethod.LastUpdateDate = DateTime.Now;
            if (actionmethod.ID == default(int))
            {
                //set the date when this record was created
                actionmethod.CreateDate = actionmethod.LastUpdateDate;
                //add a new record to database
                context.ActionMethod.Add(actionmethod);
            }
            else
            {
                //update an existing record to database
                context.Entry(actionmethod).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public List<PermissionAction> FindAllByPermissionID(int permisionID)
        {
            if (permisionID > 0)
            {
                var data = context.PermissionAction.Join(context.ActionMethod, left => left.ActionMethodID, right => right.ID, (left, right) => new { left, right })
                    .Where(item => item.left.PermissionID == permisionID)
                    .OrderBy(item => item.right.ControllerName).Select(item => item.left).ToList();
                return data;
            }
           
            return null;
        }
    }

    /// <summary>
    /// interface of ActionMethod containing necessary database operations
    /// </summary>
    public interface IActionMethodRepository : IBaseRepository<ActionMethod>
    {
        void InsertOrUpdate(ActionMethod actionmethod);
        List<PermissionAction> FindAllByPermissionID(int permisionID);
    }
}
