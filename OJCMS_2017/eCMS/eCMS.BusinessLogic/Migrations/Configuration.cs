using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Seed;
using eCMS.ExceptionLoging;
using System;
using System.Data.Entity.Migrations;

namespace eCMS.BusinessLogic.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<eCMS.BusinessLogic.Repositories.Context.RepositoryContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "eCMS.BusinessLogic.Repositories.Context.RepositoryContext";
        }

        protected override void Seed(eCMS.BusinessLogic.Repositories.Context.RepositoryContext context)
        {
           // context.HighestLevelOfEducation.AddOrUpdate(
           //   p => p.Name,
           // SeedData.HighestLevelOfEducation.ToArray()
           //);

           // context.SaveChanges();

           // context.GPA.AddOrUpdate(
           //   p => p.Name,
           // SeedData.GPA.ToArray()
           //);

           // context.SaveChanges();

           // context.AnnualIncome.AddOrUpdate(
           //   p => p.Name,
           // SeedData.AnnualIncome.ToArray()
           //);

           // context.SaveChanges();

           // context.Savings.AddOrUpdate(
           //   p => p.Name,
           // SeedData.Savings.ToArray()
           //);

           // context.SaveChanges();

           // context.ServiceProvider.AddOrUpdate(
           //   p => p.Name,
           // SeedData.ExternalServiceProvider.ToArray()
           //);

            //context.SaveChanges();

           // context.WorkerRole.AddOrUpdate(
           //   p => p.Name,
           // SeedData.WorkerRoles.ToArray()
           //);

           // context.SaveChanges();

           // context.ServiceProvider.AddOrUpdate(
           //   p => p.Name,
           // SeedData.InternalServiceProvider.ToArray()
           //);

           // context.SaveChanges();

           // context.Service.AddOrUpdate(
           //   p => p.Name,
           // SeedData.ServiceProviderServices.ToArray()
           //);

           // context.SaveChanges();

           // context.HousingQuality.AddOrUpdate(
           //   p => p.Name,
           // SeedData.HousingQuality.ToArray()
           //);

           // context.SaveChanges();

           // context.ServiceLevelOutcome.AddOrUpdate(
           //   p => p.Name,
           // SeedData.ServiceLevelOutcome.ToArray()
           //);

           // context.SaveChanges();

           // context.ImmigrationCitizenshipStatus.AddOrUpdate(
           //   p => p.Name,
           // SeedData.ImmigrationCitizenshipStatus.ToArray()
           //);

           // context.SaveChanges();

           // context.IndividualStatus.AddOrUpdate(
           //   p => p.Name,
           // SeedData.IndividualStatus.ToArray()
           //);

           // context.SaveChanges();

           // context.AssessmentType.AddOrUpdate(
           //   p => p.Name,
           // SeedData.AssessmentType.ToArray()
           //);

           // context.SaveChanges();

           // context.ProfileType.AddOrUpdate(
           //   p => p.Name,
           // SeedData.ProfileType.ToArray()
           //);

           // context.SaveChanges();

           // context.RiskType.AddOrUpdate(
           //   p => p.Name,
           // SeedData.RiskType.ToArray()
           //);

           // context.SaveChanges();

           // context.FinancialAssistanceCategory.AddOrUpdate(
           //   p => p.Name,
           // SeedData.FinancialAssistanceCategory.ToArray()
           //);

           // context.SaveChanges();

           // context.FinancialAssistanceSubCategory.AddOrUpdate(
           //   p => p.Name,
           // SeedData.FinancialAssistanceSubCategory.ToArray()
           //);

           // context.SaveChanges();

           // context.ReasonsForDischarge.AddOrUpdate(
           //   p => p.Name,
           // SeedData.ReasonsForDischarge.ToArray()
           //);

           // context.SaveChanges();

           // context.QualityOfLifeCategory.AddOrUpdate(
           //   p => p.Name,
           // SeedData.QualityOfLifeCategory.ToArray()
           //);

           // context.SaveChanges();

           // context.QualityOfLifeSubCategory.AddOrUpdate(
           //   p => p.Name,
           // SeedData.QualityOfLifeSubCategory.ToArray()
           //);

           // context.SaveChanges();

           // context.QualityOfLife.AddOrUpdate(
           //   p => p.Name,
           // SeedData.QualityOfLife.ToArray()
           //);

           // context.SaveChanges();

           // context.SubProgram.AddOrUpdate(
           //   p => p.Name,
           // SeedData.SubProgram.ToArray()
           //);

           // context.SaveChanges();

           // context.RelationshipStatus.AddOrUpdate(
           //   p => p.Name,
           // SeedData.RelationshipStatus.ToArray()
           //);

            context.ReferralSource.AddOrUpdate(
              p => p.Name,
            SeedData.ReferralSource.ToArray()
           );

           // context.SaveChanges();

           // context.TimeSpent.AddOrUpdate(
           //   p => p.Name,
           // SeedData.TimeSpent.ToArray()
           //);

           // context.WorkerRoleActionPermission.AddOrUpdate(
           //   p => new { p.WorkerRoleID, p.AreaName, p.ControllerName, p.ActionName },
           // SeedData.WorkerRoleActionPermission.ToArray()
           //);

           // context.WorkerInRole.AddOrUpdate(
           //   p => new { p.WorkerRoleID, p.ProgramID, p.RegionID },
           // SeedData.WorkerInRoles.ToArray()
           //);
            //try
            //{
            //    context.SmartGoal.AddOrUpdate(
            //      p => p.Name,
            //    SeedData.SmartGoal.ToArray()
            //   );

            //    context.SaveChanges();
            //}
            //catch { }

        }

        private void Save(RepositoryContext context, string seedDataCategory)
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
