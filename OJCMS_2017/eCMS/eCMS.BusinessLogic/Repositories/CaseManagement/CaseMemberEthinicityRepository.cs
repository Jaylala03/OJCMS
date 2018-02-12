using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models;
using eCMS.DataLogic.ViewModels;
using eCMS.ExceptionLoging;
using eCMS.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCMS.BusinessLogic.Repositories
{
    public class CaseMemberEthinicityRepository : BaseRepository<CaseMemberEthinicity>, ICaseMemberEthinicity, IBaseRepository<CaseMemberEthinicity>
    {


        public CaseMemberEthinicityRepository(RepositoryContext context)
            : base(context)
        {

        }

        public void InsertOrUpdate(CaseMemberEthinicity varCase)
        {
            try
            {
                if (varCase.CaseID > 0)
                {
                    var data = context.CaseMemberEthnicity.Where(v => v.CaseID == varCase.CaseID).FirstOrDefault();
                    if (data!=null)
                    {
                        data.LastUpdateDate = DateTime.Now;
                        data.EthinicityID = varCase.EthinicityID;
                        context.Entry(data).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                    }
                    else
                    {
                        varCase.CreateDate = DateTime.Now;
                        varCase.LastUpdateDate = DateTime.Now;
                        context.CaseMemberEthnicity.Add(varCase);
                        Save();

                    }
                }

            }
            catch (CustomException ex)
            {
                varCase.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                varCase.ErrorMessage = Constants.Messages.UnhandelledError;
            }
        }

        public string FindEthnicity(int CaseMemberId)
        {
            return context.CaseMemberEthnicity.Where(v => v.CaseID == CaseMemberId).Select(m => m.EthinicityID).FirstOrDefault();
        }
        public string FindEthnicityNames(int? CaseMemberId=0)
        {
            string sqlQuery = "";
            sqlQuery += "Declare @IDs nvarchar(max),@EthnicityName nvarchar(max) ";
            sqlQuery += " Select @IDs=EthinicityID from CaseMemberEthinicity  where CaseID=" + CaseMemberId + "";
            sqlQuery += " Select @EthnicityName=COALESCE(''+@EthnicityName + ',', '')+Name from Ethnicity where ID in(Select * from dbo.CSVToTable(@IDs))";
            sqlQuery += " Select @EthnicityName EthnicityName";
            var result = context.Database.SqlQuery<EthnicityViewModel>(sqlQuery.ToString()).FirstOrDefault();
            return result.EthnicityName;
        }


    }

    public interface ICaseMemberEthinicity : IBaseRepository<CaseMemberEthinicity>
    {
        void InsertOrUpdate(CaseMemberEthinicity caseEthinicity);
        string FindEthnicity(int CaseMemberId);
        string FindEthnicityNames(int? CaseMemberId=0);
    }
}
