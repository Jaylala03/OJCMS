using EasySoft.Helper;
using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace eCMS.BusinessLogic.Repositories
{
    public class StateRepository : BaseLookupRepository<State>, IStateRepository
    {
        public StateRepository(RepositoryContext context)
            : base(context)
        {
        }

        public override IQueryable<State> All
        {
            get { return context.State.OrderBy(item => item.Name); }
        }

        public override IQueryable<State> AllIncluding(params Expression<Func<State, object>>[] includeProperties)
        {
            IQueryable<State> query = context.State;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.OrderBy(item => item.Name);
        }

        public State Find(string name)
        {
            return context.State.SingleOrDefault(item => item.Name.ToLower() == name.ToLower());
        }

        public IQueryable<State> FindAllByCountryID(int countryID)
        {
            return context.State.Where(item => item.CountryID == countryID).OrderBy(item => item.Name);
        }

        public IEnumerable<SelectListItem> FindAllForDropDownListByCountryID(int countryID)
        {
            return context.State.Where(item => item.CountryID == countryID).OrderBy(item => item.Name).AsEnumerable().Select(item => new SelectListItem { Text = item.Name, Value = item.ID.ToString() }).ToList();
        }

        public State FindStateByName(int? countryID, string LongName, string ShortName)
        {
            State state = new State();
            //state.ID = 19;
            //state.Name = "FL";
            if (countryID.HasValue)
            {
                state = context.State.FirstOrDefault(x => x.CountryID == countryID && (x.Name == LongName || x.Code == ShortName));
            }
            else if (LongName.IsNotNullOrEmpty() && ShortName.IsNullOrEmpty())
            {
                state = context.State.FirstOrDefault(x => x.Name == LongName);
            }
            else
            {
                state = context.State.FirstOrDefault(x => x.Name == LongName || x.Code == ShortName);
            }
            if (state != null)
            {
                Remove(state);
            }
            else
            {
                if (!string.IsNullOrEmpty(LongName) && countryID.HasValue)
                {
                    state = new State();
                    if (countryID.HasValue)
                    {
                        state.CountryID = countryID.Value;
                    }
                    state.Name = LongName;
                    if (ShortName.IsNotNullOrEmpty() && ShortName.Length < 16)
                    {
                        state.Code = ShortName;
                    }
                    else
                    {
                        state.Code = null;
                    }
                    InsertOrUpdate(state);
                    Save();
                }
            }
            return state;

        }

        public IEnumerable<SelectListItem> FindAllForDropDownList(string keyword)
        {
            return context.State.Where(item => item.Name.StartsWith(keyword)).OrderBy(item => item.Name).Skip(0).Take(10).AsEnumerable().Select(item => new SelectListItem { Text = item.Name, Value = item.ID.ToString() });
        }

        public IEnumerable<SelectListItem> FindAllForDropDownList(string keyword, int countryID)
        {
            return context.State.Where(item => item.Name.StartsWith(keyword) && item.CountryID==countryID).OrderBy(item => item.Name).Skip(0).AsEnumerable().Select(item => new SelectListItem { Text = item.Name, Value = item.ID.ToString() });
        }
    }

    public interface IStateRepository : IBaseLookupRepository<State>
    {
        State Find(string name);
        IQueryable<State> FindAllByCountryID(int countryID);
        IEnumerable<SelectListItem> FindAllForDropDownListByCountryID(int countryID);
        State FindStateByName(int? countryID, string state_long_name, string state_short_name);
        IEnumerable<SelectListItem> FindAllForDropDownList(string keyword);
        IEnumerable<SelectListItem> FindAllForDropDownList(string keyword,int countryID);
    }
}
