//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

//using eCMS.BusinessLogic.Migrations;
using eCMS.DataLogic.Models;
using eCMS.DataLogic.Models.Lookup;
using eCMS.DataLogic.Seed;
using eCMS.ExceptionLoging;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using EasySoft.Helper;
using System.Collections.Generic;
using System.Linq;
using eCMS.BusinessLogic.Migrations;
//using eCMS.BusinessLogic.Migrations;
namespace eCMS.BusinessLogic.Repositories.Context
{          
    //public class RepositoryContextInitializer : DropCreateDatabaseIfModelChanges<RepositoryContext>, IRepositoryContextInitializer
    public class RepositoryContextInitializer : IRepositoryContextInitializer
    {
        private readonly RepositoryContext _dbContext;

        public RepositoryContextInitializer(RepositoryContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public virtual void InitializeDatabase()
        {
            try
            {
                //Database.SetInitializer(new MigrateDatabaseToLatestVersion<RepositoryContext, Configuration>()); 
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<RepositoryContext, Configuration>()); 
                //InitializeDatabase(_dbContext);
            }
            catch (DbEntityValidationException e)
            {
                string message = string.Empty;
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        message = message+Environment.NewLine+ve.ErrorMessage;
                        break;
                    }
                }
                ExceptionManager.Manage(new CustomException(CustomExceptionType.CommonUnhandled, message));
            }
        }

        //protected override void Seed(RepositoryContext context)
        protected void Seed(RepositoryContext context)
        {
            foreach (WorkerRole entity in SeedData.WorkerRoles)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.WorkerRole.Add(entity);
            }
            Save(context, "WorkerRole");

            foreach (WorkerRolePermission entity in SeedData.WorkerRolePermission)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                context.WorkerRolePermission.Add(entity);
            }
            Save(context, "WorkerRolePermission");

            foreach (Worker user in SeedData.Workers)
            {
                user.ConfirmPassword = user.Password;
                user.CreateDate = DateTime.Now;
                user.LastUpdateDate = DateTime.Now;
                context.Worker.Add(user);
            }
            Save(context, "Worker");

            foreach (Program entity in SeedData.Program)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.Program.Add(entity);
            }
            Save(context, "Program");

            foreach (SubProgram entity in SeedData.SubProgram)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.SubProgram.Add(entity);
            }
            Save(context, "SubProgram");

            foreach (Country entity in SeedData.Countries)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.Country.Add(entity);
            }
            Save(context, "Country");

            foreach (ActionStatus entity in SeedData.ActionStatus)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.ActionStatus.Add(entity);
            }
            Save(context, "ActionStatus");

            foreach (ActivityType entity in SeedData.ActivityType)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.ActivityType.Add(entity);
            }
            Save(context, "ActivityType");

            foreach (CaseStatus entity in SeedData.CaseStatus)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.CaseStatus.Add(entity);
            }
            Save(context, "CaseStatus");

            foreach (ContactMedia entity in SeedData.ContactMedia)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.ContactMedia.Add(entity);
            }
            Save(context, "ContactMedia");

            foreach (ContactMethod entity in SeedData.ContactMethod)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.ContactMethod.Add(entity);
            }
            Save(context, "ContactMethod");

            foreach (Ethnicity entity in SeedData.Ethnicity)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.Ethnicity.Add(entity);
            }
            Save(context, "Ethnicity");

        

            foreach (Gender entity in SeedData.Genders)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.Gender.Add(entity);
            }
            Save(context, "Gender");

            foreach (HearingSource entity in SeedData.HearingSource)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.HearingSource.Add(entity);
            }
            Save(context, "HearingSource");

            foreach (IntakeMethod entity in SeedData.IntakeMethod)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.IntakeMethod.Add(entity);
            }
            Save(context, "IntakeMethod");

            foreach (Language entity in SeedData.Language)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.Language.Add(entity);
            }
            Save(context, "Language");

            foreach (MaritalStatus entity in SeedData.MaritalStatus)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.MaritalStatus.Add(entity);
            }
            Save(context, "MaritalStatus");

            foreach (MemberStatus entity in SeedData.MemberStatus)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.MemberStatus.Add(entity);
            }
            Save(context, "MemberStatus");

            foreach (Region entity in SeedData.Region)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.Region.Add(entity);
            }
            Save(context, "Region");

            foreach (State entity in SeedData.States)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.State.Add(entity);
            }
            Save(context, "State");

            foreach (EmailTemplateCategory entity in SeedData.EmailTemplateCategory)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.EmailTemplateCategory.Add(entity);
            }
            Save(context, "EmailTemplateCategory");

            foreach (EmailTemplate entity in SeedData.EmailTemplate)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                context.EmailTemplate.Add(entity);
            }
            Save(context, "EmailTemplate");

            foreach (HighestLevelOfEducation entity in SeedData.HighestLevelOfEducation)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.HighestLevelOfEducation.Add(entity);
            }
            Save(context, "HighestLevelOfEducation");

            foreach (GPA entity in SeedData.GPA)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.GPA.Add(entity);
            }
            Save(context, "GPA");

            foreach (AnnualIncome entity in SeedData.AnnualIncome)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.AnnualIncome.Add(entity);
            }
            Save(context, "AnnualIncome");

            foreach (Savings entity in SeedData.Savings)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.Savings.Add(entity);
            }
            Save(context, "Savings");

            foreach (ServiceProvider entity in SeedData.ExternalServiceProvider)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.ServiceProvider.Add(entity);
                Save(context, "ExternalServiceProvider");
                foreach (Service service in SeedData.ServiceProviderServices)
                {
                    service.ServiceProviderID = entity.ID;
                    service.CreateDate = DateTime.Now;
                    service.LastUpdateDate = DateTime.Now;
                    service.IsActive = true;
                    if (service.Description.IsNullOrEmpty())
                    {
                        service.Description = entity.Name;
                    }
                    context.Service.Add(service);
                }
                Save(context, "Service");
            }

            foreach (ServiceProvider entity in SeedData.InternalServiceProvider)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.ServiceProvider.Add(entity);
                Save(context, "InternalServiceProvider");

                foreach (Service service in SeedData.ServiceProviderServices)
                {
                    service.ServiceProviderID = entity.ID;
                    service.CreateDate = DateTime.Now;
                    service.LastUpdateDate = DateTime.Now;
                    service.IsActive = true;
                    if (service.Description.IsNullOrEmpty())
                    {
                        service.Description = entity.Name;
                    }
                    context.Service.Add(service);
                }
                Save(context, "Service");
            }

            //foreach (Service entity in SeedData.ServiceProviderServices)
            //{
            //    entity.CreateDate = DateTime.Now;
            //    entity.LastUpdateDate = DateTime.Now;
            //    entity.IsActive = true;
            //    if (entity.Description.IsNullOrEmpty())
            //    {
            //        entity.Description = entity.Name;
            //    }
            //    context.Service.Add(entity);
            //}
            //Save(context, "Service");

            foreach (HousingQuality entity in SeedData.HousingQuality)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.HousingQuality.Add(entity);
            }
            Save(context, "HousingQuality");

            foreach (ServiceLevelOutcome entity in SeedData.ServiceLevelOutcome)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.ServiceLevelOutcome.Add(entity);
            }
            Save(context, "ServiceLevelOutcome");

            foreach (ImmigrationCitizenshipStatus entity in SeedData.ImmigrationCitizenshipStatus)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.ImmigrationCitizenshipStatus.Add(entity);
            }
            Save(context, "ImmigrationCitizenshipStatus");

            foreach (IndividualStatus entity in SeedData.IndividualStatus)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.IndividualStatus.Add(entity);
            }
            Save(context, "IndividualStatus");

            foreach (AssessmentType entity in SeedData.AssessmentType)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.AssessmentType.Add(entity);
            }
            Save(context, "AssessmentType");

            foreach (ProfileType entity in SeedData.ProfileType)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.ProfileType.Add(entity);
            }
            Save(context, "ProfileType");

            foreach (RiskType entity in SeedData.RiskType)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.RiskType.Add(entity);
            }
            Save(context, "RiskType");

            foreach (FinancialAssistanceCategory entity in SeedData.FinancialAssistanceCategory)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.FinancialAssistanceCategory.Add(entity);
            }
            Save(context, "FinancialAssistanceCategory");

            foreach (FinancialAssistanceSubCategory entity in SeedData.FinancialAssistanceSubCategory)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.FinancialAssistanceSubCategory.Add(entity);
            }
            Save(context, "FinancialAssistanceSubCategory");

            foreach (ReasonsForDischarge entity in SeedData.ReasonsForDischarge)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.ReasonsForDischarge.Add(entity);
            }
            Save(context, "ReasonsForDischarge");

            foreach (QualityOfLifeCategory entity in SeedData.QualityOfLifeCategory)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.QualityOfLifeCategory.Add(entity);
            }
            Save(context, "QualityOfLifeCategory");

            foreach (QualityOfLifeSubCategory entity in SeedData.QualityOfLifeSubCategory)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.QualityOfLifeSubCategory.Add(entity);
            }
            Save(context, "QualityOfLifeSubCategory");

            foreach (QualityOfLife entity in SeedData.QualityOfLife)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.QualityOfLife.Add(entity);
            }
            Save(context, "QualityOfLife");

            foreach (RelationshipStatus entity in SeedData.RelationshipStatus)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.RelationshipStatus.Add(entity);
            }
            Save(context, "RelationshipStatus");

            foreach (ReferralSource entity in SeedData.ReferralSource)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.ReferralSource.Add(entity);
            }
            Save(context, "ReferralSource");

            foreach (TimeSpent entity in SeedData.TimeSpent)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.TimeSpent.Add(entity);
            }
            Save(context, "TimeSpent");

            foreach (SmartGoal entity in SeedData.SmartGoal)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.SmartGoal.Add(entity);
            }
            Save(context, "SmartGoal");

            foreach (Jamatkhana entity in SeedData.Jamatkhana)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                entity.IsActive = true;
                if (entity.Description.IsNullOrEmpty())
                {
                    entity.Description = entity.Name;
                }
                context.Jamatkhana.Add(entity);
            }
            Save(context, "Jamatkhana");

            List<Region> regionList = context.Region.ToList();
            List<SubProgram> subProgramList = context.SubProgram.ToList();
            foreach (Region region in regionList)
            {
                WorkerInRole workerInRole = new WorkerInRole()
                {
                    CreateDate = DateTime.Now,
                    CreatedByWorkerID = 1,
                    EffectiveFrom = DateTime.Now,
                    EffectiveTo = DateTime.Now.AddYears(50),
                    IsArchived = false,
                    LastUpdateDate = DateTime.Now,
                    LastUpdatedByWorkerID = 1,
                    ProgramID = 1,
                    RegionID = region.ID,
                    WorkerID = 1,
                    WorkerRoleID = 2
                };
                context.WorkerInRole.Add(workerInRole);
                context.SaveChanges();
                foreach (SubProgram subProgram in subProgramList)
                {
                    WorkerSubProgram workerSubProgram = new WorkerSubProgram()
                    {
                        CreateDate = DateTime.Now,
                        CreatedByWorkerID = 1,
                        IsArchived = false,
                        LastUpdateDate = DateTime.Now,
                        LastUpdatedByWorkerID = 1,
                        WorkerInRoleID = workerInRole.ID,
                        SubProgramID = subProgram.ID
                    };
                    context.WorkerSubProgram.Add(workerSubProgram);
                    context.SaveChanges();
                }
            }
        }

        private void Save(RepositoryContext context,string seedDataCategory)
        {
            try
            {
                context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException validationException)
            {
                string errorMessage = "Validation error while saving [" + seedDataCategory + "] seed data";
                foreach (var error in validationException.EntityValidationErrors)
                {
                    var entry = error.Entry;
                    foreach (var err in error.ValidationErrors)
                    {
                        errorMessage = Environment.NewLine + err.PropertyName + ": Error Message: " + err.ErrorMessage;
                    }
                }
                CustomException customException = new CustomException(CustomExceptionType.CommonInvalidData, errorMessage, validationException);
                ExceptionManager.Manage(customException);
            }
            catch (Exception ex)
            {
                string errorMessage = "Unknown error while saving [" + seedDataCategory + "] seed data";
                CustomException customException = new CustomException(CustomExceptionType.CommonInvalidData, errorMessage, ex);
                ExceptionManager.Manage(customException);
            }
        }   
    }  
}