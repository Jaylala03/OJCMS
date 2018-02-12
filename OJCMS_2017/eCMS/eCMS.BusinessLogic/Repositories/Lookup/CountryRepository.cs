//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;
using System.Linq;
using EasySoft.Helper;
using System;

namespace eCMS.BusinessLogic.Repositories
{
    public class CountryRepository : BaseLookupRepository<Country>, ICountryRepository
    {
        public CountryRepository(RepositoryContext context)
            :base(context)
        {
        }

        public Country FindCountryByName(string name, string code,bool isAdd=false)
        {
            Country country = context.Country.FirstOrDefault(x => x.Name == name || x.Code == code);
            if (isAdd)
            {
                if (country == null && name.IsNotNullOrEmpty())
                {
                    country = new Country();
                    country.Code = code;
                    country.Name = name;
                    country.LastUpdateDate = DateTime.Now;
                    country.CreateDate = DateTime.Now;
                    InsertOrUpdate(country);
                    Save();
                }
            }
            return country;
        }

    }

    public interface ICountryRepository:IBaseLookupRepository<Country>
    {
        Country FindCountryByName(string name, string code, bool isAdd = false);
    }
}
