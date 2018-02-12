//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.BusinessLogic.Helpers;
using eCMS.DataLogic.Models;
using eCMS.DataLogic.Models;
using eCMS.DataLogic.Models.Lookup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace eCMS.BusinessLogic.Repositories.Context
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(IConnectionString connectionString)
            : base(connectionString.DefaultConnectionString)
        {
        }

        public RepositoryContext()
            : base(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString)
        {
        }


        public override int SaveChanges()
        {
           
            foreach (var changeInfo in this.ChangeTracker.Entries())
            {
                var user = HttpContext.Current.User.Identity.Name;
                foreach (var item in GetAuditRecordsForChange(changeInfo, user))
                {
                    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString))
                    {
                        string sql = "INSERT  INTO  [dbo].[CaseAuditLog] ([LogID], [EventType],[TableName],[ActionID],[RecordID],[ColumnName],[OriginalValue],[NewValue],[Created_by],[Created_date]) values(@LogID,@EventType,@TableName,@ActionID,@RecordID,@ColumnName,@OriginalValue,@NewValue,@Created_by,@Created_date)";
                        cnn.Open();
                        using (SqlCommand cmd = new SqlCommand(sql, cnn))
                        {
                            cmd.Parameters.AddWithValue("@LogID", item.LogID == null ? Guid.NewGuid() : item.LogID);
                            cmd.Parameters.AddWithValue("@EventType", item.EventType == null ? string.Empty : item.EventType);
                            cmd.Parameters.AddWithValue("@TableName", item.TableName == null ? string.Empty : item.TableName);
                            cmd.Parameters.AddWithValue("@ActionID", item.ActionID == null ? string.Empty : item.ActionID);
                            cmd.Parameters.AddWithValue("@RecordID", item.RecordID == null ? string.Empty : item.RecordID);
                            cmd.Parameters.AddWithValue("@ColumnName", item.ColumnName == null ? string.Empty : item.ColumnName);
                            cmd.Parameters.AddWithValue("@OriginalValue", item.OriginalValue == null ? string.Empty : item.OriginalValue);
                            cmd.Parameters.AddWithValue("@NewValue", item.NewValue == null ? string.Empty : item.NewValue);
                            cmd.Parameters.AddWithValue("@Created_by", item.Created_by == null ? string.Empty : item.Created_by);
                            cmd.Parameters.AddWithValue("@Created_date", item.Created_date == null ? DateTime.UtcNow : item.Created_date);
                            cmd.ExecuteNonQuery();

                        }
                    }
                }



            }

            return base.SaveChanges();
        }

       

        private List<CaseAuditLog> GetAuditRecordsForChange(DbEntityEntry dbEntry, string userId)
        {
            List<CaseAuditLog> result = new List<CaseAuditLog>();

            DateTime changeTime = DateTime.UtcNow;

            // Get the Table() attribute, if one exists
            TableAttribute tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;

            // Get table name (if it has a Table attribute, use that, otherwise get the pluralized name)
            string tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name.Split('_')[0];
            string ID = string.Empty;
            string sql = string.Empty;
            if ((tableName == "Case") || (tableName == "CaseAction") || (tableName == "CaseAssessment") ||  (tableName == "CaseGoal") || (tableName == "CaseMember")
               || (tableName == "CaseMemberProfile") || (tableName == "CaseProgressNote") || (tableName == "CaseSmartGoal") || (tableName == "CaseSupportCircle")
               || (tableName == "CaseWorker") || (tableName == "CaseMember_Ethinicity") 
               )
            {
                // Get primary key value (If you have more than one key column, this will need to be adjusted)
                
                  string keyName = "ID";
                try
                {
                     keyName = dbEntry.Entity.GetType().GetProperties().FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0).Name;
                }
                catch(Exception ex)
                {
                
                }
                

                if (dbEntry.State == EntityState.Added)
                {
                    foreach (string propertyName in dbEntry.CurrentValues.PropertyNames)
                    {

                        if (tableName == "CaseMember" || tableName == "CaseWorker" || tableName == "CaseSupportCircle")
                            {
                                ID = dbEntry.CurrentValues.GetValue<object>("CaseID").ToString();
                            }
                            if (tableName == "Case")
                            {
                                ID = dbEntry.CurrentValues.GetValue<object>("ID").ToString();
                            }
                            if (ID == string.Empty)
                            {
                            if (tableName == "CaseProgressNote")
                                sql = "Select CaseID from CaseMember where ID=" + Convert.ToInt32(dbEntry.CurrentValues.GetValue<object>("CaseMemberID")) + "";

                            if (tableName == "CaseAction")
                                sql = "Select CaseID from CaseWorker where ID=" + Convert.ToInt32(dbEntry.CurrentValues.GetValue<object>("CaseWorkerID")) + "";

                            if (tableName == "CaseAssessment")
                                sql = "Select CaseID from CaseMember where ID=" + Convert.ToInt32(dbEntry.CurrentValues.GetValue<object>("CaseMemberID")) + "";
                           if (tableName == "CaseGoal")
                                sql = "Select CaseID from CaseMember where ID=" + Convert.ToInt32(dbEntry.CurrentValues.GetValue<object>("CaseMemberID")) + "";
                           
                          
                            if (tableName == "CaseMemberProfile")
                                sql = "Select CaseID from CaseMember where ID=" + Convert.ToInt32(dbEntry.CurrentValues.GetValue<object>("CaseMemberID")) + "";

                            if (tableName == "CaseSmartGoal")
                                sql = "Select CaseID from CaseMember where ID=(Select CaseMemberID from CaseGoal where ID=" + Convert.ToInt32(dbEntry.CurrentValues.GetValue<object>("CaseGoalID")) + ")";

                           
                            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString))
                            {
                                cnn.Open();
                                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                                {
                                    var reader = cmd.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        ID = reader[0].ToString();
                                    }

                                }
                            }
                        }
                    }
                    // For Inserts, just add the whole record
                    // If the entity implements IDescribableEntity, use the description from Describe(), otherwise use ToString()
                    result.Add(new CaseAuditLog()
                    {
                        ActionID = ID,
                        LogID = Guid.NewGuid(),
                        EventType = "A", // Added
                        TableName = tableName,
                        RecordID = dbEntry.CurrentValues.GetValue<object>(keyName).ToString(),  // Again, adjust this if you have a multi-column key
                        ColumnName = "*ALL",    // Or make it nullable, whatever you want
                        NewValue = (dbEntry.CurrentValues.ToObject() is IDescribableEntity) ? (dbEntry.CurrentValues.ToObject() as IDescribableEntity).Describe() : dbEntry.CurrentValues.ToObject().ToString(),
                        Created_by = userId,
                        Created_date = changeTime
                    }
                        );
                }
                else if (dbEntry.State == EntityState.Deleted)
                {
                    // Same with deletes, do the whole record, and use either the description from Describe() or ToString()

                    foreach (string propertyName in dbEntry.OriginalValues.PropertyNames)
                    {

                        if (tableName == "CaseMember" || tableName == "CaseWorker" || tableName == "CaseSupportCircle")
                        {
                            ID = dbEntry.OriginalValues.GetValue<object>("CaseID").ToString();
                        }
                        if (tableName == "Case")
                        {
                            ID = dbEntry.OriginalValues.GetValue<object>("ID").ToString();
                        }
                        if (ID == string.Empty)
                        {
                            if (tableName == "CaseProgressNote")
                                sql = "Select CaseID from CaseMember where ID=" + Convert.ToInt32(dbEntry.OriginalValues.GetValue<object>("CaseMemberID")) + "";

                            if (tableName == "CaseAction")
                                sql = "Select CaseID from CaseWorker where ID=" + Convert.ToInt32(dbEntry.OriginalValues.GetValue<object>("CaseWorkerID")) + "";

                            if (tableName == "CaseAssessment")
                                sql = "Select CaseID from CaseMember where ID=" + Convert.ToInt32(dbEntry.OriginalValues.GetValue<object>("CaseMemberID")) + "";
                            if (tableName == "CaseGoal")
                                sql = "Select CaseID from CaseMember where ID=" + Convert.ToInt32(dbEntry.OriginalValues.GetValue<object>("CaseMemberID")) + "";


                            if (tableName == "CaseMemberProfile")
                                sql = "Select CaseID from CaseMember where ID=" + Convert.ToInt32(dbEntry.OriginalValues.GetValue<object>("CaseMemberID")) + "";

                            if (tableName == "CaseSmartGoal")
                                sql = "Select CaseID from CaseMember where ID=(Select CaseMemberID from CaseGoal where ID=" + Convert.ToInt32(dbEntry.OriginalValues.GetValue<object>("CaseGoalID")) + ")";


                            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString))
                            {
                                cnn.Open();
                                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                                {
                                    var reader = cmd.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        ID = reader[0].ToString();
                                    }

                                }
                            }
                        }
                    }

                    result.Add(new CaseAuditLog()
                    {
                        ActionID = ID,
                        LogID = Guid.NewGuid(),
                        EventType = "D", // Deleted
                        TableName = tableName,
                        RecordID = dbEntry.OriginalValues.GetValue<object>(keyName).ToString(),
                        ColumnName = "*ALL",
                        NewValue = (dbEntry.OriginalValues.ToObject() is IDescribableEntity) ? (dbEntry.OriginalValues.ToObject() as IDescribableEntity).Describe() : dbEntry.OriginalValues.ToObject().ToString(),
                        Created_by = userId,
                        Created_date = changeTime
                    }
                        );
                }
                else if (dbEntry.State == EntityState.Modified)
                {
                    string[] columsToExclude = { "CreatedByWorkerID", "LastUpdatedByWorkerID", "CreateDate", "LastUpdateDate", "DisplayID", "ImmigrationCitizenshipStatusNote" };
                    foreach (string propertyName in dbEntry.OriginalValues.PropertyNames)
                    {
                        if (tableName == "CaseMember" || tableName == "CaseWorker" || tableName == "CaseSupportCircle")
                        {
                            ID = dbEntry.CurrentValues.GetValue<object>("CaseID").ToString();
                        }
                        if (tableName == "Case")
                        {
                            ID = dbEntry.CurrentValues.GetValue<object>("ID").ToString();
                        }
                        if (ID == string.Empty)
                        {
                            if (tableName == "CaseProgressNote")
                                sql = "Select CaseID from CaseMember where ID=" + Convert.ToInt32(dbEntry.CurrentValues.GetValue<object>("CaseMemberID")) + "";

                            if (tableName == "CaseAction")
                                sql = "Select CaseID from CaseWorker where ID=" + Convert.ToInt32(dbEntry.CurrentValues.GetValue<object>("CaseWorkerID")) + "";

                            if (tableName == "CaseAssessment")
                                sql = "Select CaseID from CaseMember where ID=" + Convert.ToInt32(dbEntry.CurrentValues.GetValue<object>("CaseMemberID")) + "";
                            if (tableName == "CaseGoal")
                                sql = "Select CaseID from CaseMember where ID=" + Convert.ToInt32(dbEntry.CurrentValues.GetValue<object>("CaseMemberID")) + "";


                            if (tableName == "CaseMemberProfile")
                                sql = "Select CaseID from CaseMember where ID=" + Convert.ToInt32(dbEntry.CurrentValues.GetValue<object>("CaseMemberID")) + "";

                            if (tableName == "CaseSmartGoal")
                                sql = "Select CaseID from CaseMember where ID=(Select CaseMemberID from CaseGoal where ID=" + Convert.ToInt32(dbEntry.CurrentValues.GetValue<object>("CaseGoalID")) + ")";


                            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString))
                            {
                                cnn.Open();
                                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                                {
                                    var reader = cmd.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        ID = reader[0].ToString();
                                    }

                                }
                            }
                        }
                        // For updates, we only want to capture the columns that actually changed
                        if (!object.Equals(dbEntry.GetDatabaseValues().GetValue<object>(propertyName), dbEntry.CurrentValues.GetValue<object>(propertyName)) && !columsToExclude.Contains(propertyName))
                        {
                            result.Add(new CaseAuditLog()
                            {
                                ActionID = ID,
                                LogID = Guid.NewGuid(),
                                EventType = "M",    // Modified
                                TableName = tableName,
                                RecordID = dbEntry.OriginalValues.GetValue<object>(keyName).ToString(),
                                ColumnName = propertyName,
                                OriginalValue = dbEntry.GetDatabaseValues().GetValue<object>(propertyName) == null ? null : dbEntry.GetDatabaseValues().GetValue<object>(propertyName).ToString(),
                                NewValue = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString(),
                                Created_by = userId,
                                Created_date = changeTime
                            }
                                );
                        }
                    }
                }
                // Otherwise, don't do anything, we don't care about Unchanged or Detached entities
            }
            return result;
        }

        public bool IsDisposed
        {
            get
            {
                try
                {
                    DbConnection dbConnection = this.Database.Connection;
                    if (dbConnection == null)
                    {
                        return true;
                    }
                    return false;
                }
                catch
                {
                    return true;
                }
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<CaseMemberProfile>().HasKey(t => t.ID);
            modelBuilder.Entity<CaseAuditLog>().HasKey(t => t.LogID);
            modelBuilder.Entity<TrainingModule>().HasKey(t => t.ID);
            //modelBuilder.Entity<WorkerInRole>().HasKey(t => new { t.WorkerID, t.WorkerRoleID, t.ProgramID, t.RegionID });
            modelBuilder.Entity<RegionRole>().HasKey(t => new { t.RegionID, t.WorkerRoleID });
            modelBuilder.Entity<RegionSubProgram>().HasKey(t => new { t.RegionID, t.SubProgramID });
            //modelBuilder.Entity<WorkerProgram>().HasKey(t => new { t.WorkerID, t.ProgramID });
            modelBuilder.Entity<WorkerSubProgram>().HasKey(t => new { t.WorkerInRoleID, t.SubProgramID });
            modelBuilder.Entity<PermissionSubProgram>().HasKey(t => new { t.PermissionRegionID, t.SubProgramID });
            //modelBuilder.Entity<WorkerSubProgram>().HasRequired(t => t.WorkerInRole).WithMany().HasForeignKey(t => new { t.WorkerInRoleID });
            //modelBuilder.Entity<WorkerRegion>().HasKey(t => new { t.WorkerID, t.RegionID });
            modelBuilder.Entity<CaseWorkerMemberAssignment>().HasKey(t => new { t.CaseWorkerID, t.CaseMemberID });
            modelBuilder.Entity<CaseAssessmentLivingCondition>().HasKey(t => new { t.CaseAssessmentID, t.QualityOfLifeID });
            modelBuilder.Entity<CaseGoalLivingCondition>().HasKey(t => new { t.CaseGoalID, t.QualityOfLifeCategoryID });
            modelBuilder.Entity<CaseProgressNoteMembers>().HasKey(t => new { t.ID});
            //modelBuilder.Entity<CaseSmartGoalServiceProvider>().HasKey(t => new { t.CaseSmartGoalID, t.ServiceProviderID });
        }

        #region Lookup Data

        public DbSet<State> State { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Gender> Gender { get; set; }

        #endregion Lookup Data

        #region Worker Management

        public DbSet<Worker> Worker { get; set; }
        public DbSet<WorkerRole> WorkerRole { get; set; }
        public DbSet<WorkerRolePermissionNew> WorkerRolePermissionNew { get; set; }

        #endregion Worker Management

        public DbSet<Region> Region { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<ActionMethod> ActionMethod { get; set; }
        public DbSet<PermissionRegion> PermissionRegion { get; set; }
        public DbSet<PermissionSubProgram> PermissionSubProgram { get; set; }
        public DbSet<PermissionJamatkhana> PermissionJamatkhana { get; set; }
        public DbSet<PermissionAction> PermissionAction { get; set; }
        public DbSet<Program> Program { get; set; }
        public DbSet<SubProgram> SubProgram { get; set; }
        public DbSet<RelationshipStatus> RelationshipStatus { get; set; }
        public DbSet<TimeSpent> TimeSpent { get; set; }
        public DbSet<WorkerInRole> WorkerInRole { get; set; }
        public DbSet<WorkerInRoleNew> WorkerInRoleNew { get; set; }
        public DbSet<WorkerSubProgram> WorkerSubProgram { get; set; }
        public DbSet<WorkerRoleActionPermission> WorkerRoleActionPermission { get; set; }
        public DbSet<Case> Case { get; set; }
        public DbSet<CaseMemberContact> CaseMemberContact { get; set; }
        public DbSet<CaseMember> CaseMember { get; set; }
        public DbSet<CaseProgressNote> CaseProgressNote { get; set; }
        public DbSet<CaseAction> CaseAction { get; set; }
        public DbSet<CaseSupportCircle> CaseSupportCircle { get; set; }
        public DbSet<CaseWorker> CaseWorker { get; set; }
        public DbSet<CaseWorkerMemberAssignment> CaseWorkerMemberAssignment { get; set; }
        public DbSet<ActivityType> ActivityType { get; set; }
        public DbSet<ActionStatus> ActionStatus { get; set; }
        public DbSet<CaseStatus> CaseStatus { get; set; }
        public DbSet<ContactMedia> ContactMedia { get; set; }
        public DbSet<ContactMethod> ContactMethod { get; set; }
        public DbSet<Ethnicity> Ethnicity { get; set; }
        public DbSet<CaseMemberEthinicity> CaseMemberEthnicity { get; set; }
        public DbSet<HearingSource> HearingSource { get; set; }
        public DbSet<IntakeMethod> IntakeMethod { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<MaritalStatus> MaritalStatus { get; set; }
        public DbSet<MemberStatus> MemberStatus { get; set; }
        public DbSet<ReferralSource> ReferralSource { get; set; }
        public DbSet<WorkerRolePermission> WorkerRolePermission { get; set; }
        public DbSet<EmailTemplateCategory> EmailTemplateCategory { get; set; }
        public DbSet<EmailTemplate> EmailTemplate { get; set; }
        public DbSet<HighestLevelOfEducation> HighestLevelOfEducation { get; set; }
        public DbSet<GPA> GPA { get; set; }
        public DbSet<AnnualIncome> AnnualIncome { get; set; }
        public DbSet<Savings> Savings { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<ServiceProvider> ServiceProvider { get; set; }
        public DbSet<HousingQuality> HousingQuality { get; set; }
        public DbSet<ServiceLevelOutcome> ServiceLevelOutcome { get; set; }
        public DbSet<ImmigrationCitizenshipStatus> ImmigrationCitizenshipStatus { get; set; }

        public DbSet<IndividualStatus> IndividualStatus { get; set; }

        public DbSet<AssessmentType> AssessmentType { get; set; }

        public DbSet<ProfileType> ProfileType { get; set; }

        public DbSet<RiskType> RiskType { get; set; }

        public DbSet<FinancialAssistanceCategory> FinancialAssistanceCategory { get; set; }

        public DbSet<FinancialAssistanceSubCategory> FinancialAssistanceSubCategory { get; set; }

        public DbSet<ReasonsForDischarge> ReasonsForDischarge { get; set; }

        public DbSet<QualityOfLifeCategory> QualityOfLifeCategory { get; set; }

        public DbSet<QualityOfLifeSubCategory> QualityOfLifeSubCategory { get; set; }

        public DbSet<QualityOfLife> QualityOfLife { get; set; }

        public DbSet<CaseAssessment> CaseAssessment { get; set; }

        public DbSet<CaseAssessmentLivingCondition> CaseAssessmentLivingCondition { get; set; }

        public DbSet<CaseGoal> CaseGoal { get; set; }

        public DbSet<CaseGoalLivingCondition> CaseGoalLivingCondition { get; set; }

        public DbSet<CaseMemberProfile> CaseMemberProfile { get; set; }

        public DbSet<Jamatkhana> Jamatkhana { get; set; }

        public DbSet<CaseSmartGoal> CaseSmartGoal { get; set; }

        public DbSet<CaseSmartGoalProgress> CaseSmartGoalProgress { get; set; }

        public DbSet<RegionRole> RegionRole { get; set; }

        public DbSet<RegionSubProgram> RegionSubProgram { get; set; }

        public DbSet<SmartGoal> SmartGoal { get; set; }

        public DbSet<CaseSmartGoalServiceProvider> CaseSmartGoalServiceProvider { get; set; }

     

        public DbSet<CaseSmartGoalServiceLevelOutcome> CaseSmartGoalServiceLevelOutcome { get; set; }

        public DbSet<WorkerNotification> WorkerNotification { get; set; }

        public DbSet<WorkerToDo> WorkerToDo { get; set; }

        public DbSet<CaseSmartGoalAssignment> CaseSmartGoalAssignment { get; set; }

        public DbSet<CaseAuditLog> CaseAuditLog { get; set; }

        public DbSet<TrainingModule> TrainingModule { get; set; }
        public DbSet<CaseProgressNoteMembers> CaseProgressNoteMembers { get; set; }
    }

    public interface IDescribableEntity
    {
        string Describe();
    }
}