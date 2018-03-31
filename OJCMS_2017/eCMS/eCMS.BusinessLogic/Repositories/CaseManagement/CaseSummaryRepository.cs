using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models;
using eCMS.DataLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCMS.BusinessLogic.Repositories
{
    public class CaseSummaryRepository : BaseRepository<CaseSummary>, ICaseSummaryRepository
    {
        public CaseSummaryRepository(RepositoryContext context
           )
            : base(context)
        {

        }
        public CaseSummaryVM GetCaseDetails(int CaseID)
        {

            StringBuilder sqlQuery = new StringBuilder(@"SELECT [C].[ID] AS CaseID,[C].[ProgramID], [PR].[Name] AS Program, ");
            sqlQuery.Append(" [P].[Name] AS SubProgram, [R].Name AS Region, [JK].Name AS Jamatkhaana, [C].CaseNumber AS ReferenceCase, ");
            sqlQuery.Append(" [C].[EnrollDate] AS EnrolmentDate, [IM].[Name] AS IntakeMethod, [RE].[Name] AS ReferralSource ,");
            sqlQuery.Append(" [C].[ReferralDate] AS ReferralDate, [RT].Name AS RiskLevel, [C].[PresentingProblem] AS PresentingProblem, ");
            sqlQuery.Append(" [C].[Address] AS Address, [C].[City] AS City, [C].[PostalCode] AS PostalCode,");
            sqlQuery.Append(" ((CASE WHEN Education = 1 THEN 'Education' ELSE '' END) + ");
            sqlQuery.Append(" (CASE WHEN IncomeLivelihood = 1 THEN ',IncomeLivelihood' ELSE '' END) + ");
            sqlQuery.Append(" (CASE WHEN Assets = 1 THEN ',Assets' ELSE '' END) +  ");
             sqlQuery.Append(" (CASE WHEN Housing = 1 THEN ',Housing' ELSE '' END) + ");
            sqlQuery.Append(" (CASE WHEN SocialSupport = 1 THEN ',SocialSupport' ELSE '' END) +  ");
             sqlQuery.Append(" (CASE WHEN Dignity = 1 THEN ',Dignity' ELSE '' END) + ");
            sqlQuery.Append(" (CASE WHEN Health = 1 THEN ',Health' ELSE '' END)) AS AreaOfNeed ");
            sqlQuery.Append(" FROM [dbo].[Case] [C] ");
            sqlQuery.Append(" INNER JOIN [dbo].[Program] [PR] ON [PR].[ID]=[C].[ProgramID]");
            sqlQuery.Append(" INNER JOIN [dbo].[Region] [R] ON [R].[ID]=[C].[RegionID]");
            sqlQuery.Append(" INNER JOIN [dbo].[SubProgram] [P] ON [P].[ID]=[C].[SubProgramID]");
            //sqlQuery.Append(" INNER JOIN [dbo].[CaseStatus] [CS] ON [CS].[ID]=[C].[CaseStatusID]");
            sqlQuery.Append(" INNER JOIN [dbo].[Jamatkhana] [JK] ON [JK].[ID]=[C].[JamatkhanaID]");
            sqlQuery.Append(" INNER JOIN [dbo].[IntakeMethod] [IM] ON [IM].[ID]=[C].[IntakeMethodID]");
            sqlQuery.Append(" INNER JOIN [dbo].[ReferralSource] [RE] ON [RE].[ID]=[C].[ReferralSourceID]");
            sqlQuery.Append(" INNER JOIN [dbo].[RiskType] [RT] ON [RT].[ID]=[C].[RiskTypeID]");

            sqlQuery.Append(" WHERE [C].[IsArchived] = 0 AND C.Id = " + CaseID + " ");

            CaseSummaryVM casesummary = context.Database.SqlQuery<CaseSummaryVM>(sqlQuery.ToString()).AsEnumerable().FirstOrDefault();
            casesummary.DoesHouseHoldIncomeExists = context.CaseHouseholdIncome.Any(cus => cus.CaseID == CaseID);
            casesummary.DoesInitialAssessmentExists = context.CaseInitialAssessment.Any(cus => cus.CaseID == CaseID);
            casesummary.DoesFamilyMembersExists = context.CaseMember.Any(cus => cus.CaseID == CaseID);

            return casesummary;
        }
    }

    public interface ICaseSummaryRepository : IBaseRepository<CaseSummary>
    {
        CaseSummaryVM GetCaseDetails(int CaseID);
    }
}