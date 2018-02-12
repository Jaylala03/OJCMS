//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

namespace eCMS.BusinessLogic.Repositories.Context
{
    public class ConnectionString : IConnectionString
    {
        public string DefaultConnectionString { get; private set; }

        public ConnectionString(string connectionString)
        {
            DefaultConnectionString = connectionString;
        }
    }
}