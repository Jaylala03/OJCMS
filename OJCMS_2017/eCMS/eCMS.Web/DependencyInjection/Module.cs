using Autofac;
using eCMS.BusinessLogic.Helpers;
using eCMS.BusinessLogic.Repositories;
using eCMS.BusinessLogic.Repositories.Context;
using System.Web.Routing;

namespace eCMS.Web.DependencyInjection
{
    public class Module : ModuleBase
    {
        public override void RegisterRoutes(RouteCollection routeCollection)
        {
        }

        public override void RegisterComponents(ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(
                container => new RepositoryContext(container.Resolve<IConnectionString>()));

            containerBuilder.Register(
                container => new RepositoryContextInitializer(container.Resolve<RepositoryContext>())).As
                <IRepositoryContextInitializer>();

            #region Lookup

            #endregion Lookup

            #region User

            containerBuilder.Register(container => new WorkerRepository(container.Resolve<RepositoryContext>(),
                container.Resolve<IWorkerRoleActionPermissionNewRepository>())).As<IWorkerRepository>();
            containerBuilder.Register(container => new EmailTemplateCategoryRepository(container.Resolve<RepositoryContext>())).As<IEmailTemplateCategoryRepository>();
            containerBuilder.Register(container => new EmailTemplateRepository(container.Resolve<RepositoryContext>())).As<IEmailTemplateRepository>();
            containerBuilder.Register(container => new WorkerAuthenticationManager(container.Resolve<IWorkerRepository>(), container.Resolve<IWorkerRolePermissionRepository>(),
                 container.Resolve<IWorkerRolePermissionNewRepository>(), container.Resolve<IWorkerInRoleRepository>(), container.Resolve<IWorkerInRoleNewRepository>())).As<WorkerAuthenticationManager>();

            #endregion

            containerBuilder.Register(container => new WorkerRoleActionPermissionRepository(container.Resolve<RepositoryContext>())).As<IWorkerRoleActionPermissionRepository>();
            containerBuilder.Register(container => new WorkerRoleActionPermissionNewRepository(container.Resolve<RepositoryContext>())).As<IWorkerRoleActionPermissionNewRepository>();
            containerBuilder.Register(container => new ActivityTypeRepository(container.Resolve<RepositoryContext>())).As<IActivityTypeRepository>();
            containerBuilder.Register(container => new StateRepository(container.Resolve<RepositoryContext>())).As<IStateRepository>();
            containerBuilder.Register(container => new CaseMemberContactRepository(container.Resolve<RepositoryContext>())).As<ICaseMemberContactRepository>();
            containerBuilder.Register(container => new CaseMemberRepository(
                container.Resolve<RepositoryContext>(),
                container.Resolve<ICaseWorkerMemberAssignmentRepository>(),
                container.Resolve<ICaseWorkerRepository>()
                )).As<ICaseMemberRepository>();
            containerBuilder.Register(container => new CaseActionRepository(container.Resolve<RepositoryContext>(),
                container.Resolve<IWorkerRoleActionPermissionRepository>(),
                container.Resolve<IWorkerRoleActionPermissionNewRepository>(),
                container.Resolve<ICaseWorkerRepository>(),
                container.Resolve<IWorkerNotificationRepository>(),
                container.Resolve<ICaseSmartGoalServiceProviderRepository>(),
                container.Resolve<IWorkerToDoRepository>())).As<ICaseActionRepository>();
            containerBuilder.Register(container => new CaseProgressNoteRepository(container.Resolve<RepositoryContext>(),
                container.Resolve<IWorkerRoleActionPermissionRepository>(),
                 container.Resolve<IWorkerRoleActionPermissionNewRepository>(),
                container.Resolve<ICaseWorkerRepository>(),
                container.Resolve<IWorkerNotificationRepository>())).As<ICaseProgressNoteRepository>();
            containerBuilder.Register(container => new CaseRepository(
                container.Resolve<RepositoryContext>(),
                container.Resolve<IRegionRepository>(),
                container.Resolve<ICaseMemberRepository>(),
                container.Resolve<ICaseMemberContactRepository>(),
                container.Resolve<IWorkerRoleActionPermissionRepository>(),
                 container.Resolve<IWorkerRoleActionPermissionNewRepository>(),
                container.Resolve<ICaseProgressNoteRepository>(), container.Resolve<IWorkerRoleRepository>())).As<ICaseRepository>();
            containerBuilder.Register(container => new CaseStatusRepository(container.Resolve<RepositoryContext>())).As<ICaseStatusRepository>();
            containerBuilder.Register(container => new CaseSupportCircleRepository(container.Resolve<RepositoryContext>())).As<ICaseSupportCircleRepository>();
            containerBuilder.Register(container => new CaseWorkerMemberAssignmentRepository(container.Resolve<RepositoryContext>())).As<ICaseWorkerMemberAssignmentRepository>();
            containerBuilder.Register(container => new WorkerNotificationRepository(container.Resolve<RepositoryContext>())).As<IWorkerNotificationRepository>();
            containerBuilder.Register(container => new WorkerToDoRepository(container.Resolve<RepositoryContext>())).As<IWorkerToDoRepository>();
            containerBuilder.Register(container => new CaseWorkerRepository(container.Resolve<RepositoryContext>(), container.Resolve<IWorkerNotificationRepository>())).As<ICaseWorkerRepository>();
            containerBuilder.Register(container => new ContactMediaRepository(container.Resolve<RepositoryContext>())).As<IContactMediaRepository>();
            containerBuilder.Register(container => new ContactMethodRepository(container.Resolve<RepositoryContext>())).As<IContactMethodRepository>();
            containerBuilder.Register(container => new CountryRepository(container.Resolve<RepositoryContext>())).As<ICountryRepository>();
            containerBuilder.Register(container => new CurrencyRepository(container.Resolve<RepositoryContext>())).As<ICurrencyRepository>();
            containerBuilder.Register(container => new EthnicityRepository(container.Resolve<RepositoryContext>())).As<IEthnicityRepository>();
            containerBuilder.Register(container => new CaseMemberEthinicityRepository(container.Resolve<RepositoryContext>())).As<ICaseMemberEthinicity>();
            containerBuilder.Register(container => new GenderRepository(container.Resolve<RepositoryContext>())).As<IGenderRepository>();
            containerBuilder.Register(container => new HearingSourceRepository(container.Resolve<RepositoryContext>())).As<IHearingSourceRepository>();
            containerBuilder.Register(container => new IntakeMethodRepository(container.Resolve<RepositoryContext>())).As<IIntakeMethodRepository>();
            containerBuilder.Register(container => new JamatkhanaRepository(container.Resolve<RepositoryContext>())).As<IJamatkhanaRepository>();
            containerBuilder.Register(container => new LanguageRepository(container.Resolve<RepositoryContext>())).As<ILanguageRepository>();
            containerBuilder.Register(container => new MaritalStatusRepository(container.Resolve<RepositoryContext>())).As<IMaritalStatusRepository>();
            containerBuilder.Register(container => new MemberStatusRepository(container.Resolve<RepositoryContext>())).As<IMemberStatusRepository>();
            containerBuilder.Register(container => new SubProgramRepository(container.Resolve<RepositoryContext>())).As<ISubProgramRepository>();
            containerBuilder.Register(container => new ProgramRepository(container.Resolve<RepositoryContext>())).As<IProgramRepository>();
            containerBuilder.Register(container => new ReferralSourceRepository(container.Resolve<RepositoryContext>())).As<IReferralSourceRepository>();
            containerBuilder.Register(container => new RegionRepository(container.Resolve<RepositoryContext>())).As<IRegionRepository>();
            containerBuilder.Register(container => new RelationshipStatusRepository(container.Resolve<RepositoryContext>())).As<IRelationshipStatusRepository>();
            containerBuilder.Register(container => new TimeSpentRepository(container.Resolve<RepositoryContext>())).As<ITimeSpentRepository>();
            containerBuilder.Register(container => new WorkerInRoleRepository(container.Resolve<RepositoryContext>(), container.Resolve<IWorkerSubProgramRepository>())).As<IWorkerInRoleRepository>();
            containerBuilder.Register(container => new WorkerRepository(container.Resolve<RepositoryContext>()
                , container.Resolve<IWorkerRoleActionPermissionNewRepository>())).As<IWorkerRepository>();
            containerBuilder.Register(container => new WorkerRoleRepository(container.Resolve<RepositoryContext>())).As<IWorkerRoleRepository>();
            containerBuilder.Register(container => new WorkerRolePermissionRepository(container.Resolve<RepositoryContext>())).As<IWorkerRolePermissionRepository>();
            containerBuilder.Register(container => new WorkerSubProgramRepository(container.Resolve<RepositoryContext>())).As<IWorkerSubProgramRepository>();
            containerBuilder.Register(container => new CaseAuditLogRepository(container.Resolve<RepositoryContext>())).As<ICaseAuditLogRepository>();
            containerBuilder.Register(container => new CaseTrainingRepository(container.Resolve<RepositoryContext>())).As<ICaseTrainingRepository>();
            containerBuilder.Register(container => new HighestLevelOfEducationRepository(container.Resolve<RepositoryContext>())).As<IHighestLevelOfEducationRepository>();
            containerBuilder.Register(container => new GPARepository(container.Resolve<RepositoryContext>())).As<IGPARepository>();
            containerBuilder.Register(container => new AnnualIncomeRepository(container.Resolve<RepositoryContext>())).As<IAnnualIncomeRepository>();
            containerBuilder.Register(container => new SavingsRepository(container.Resolve<RepositoryContext>())).As<ISavingsRepository>();
            containerBuilder.Register(container => new HousingQualityRepository(container.Resolve<RepositoryContext>())).As<IHousingQualityRepository>();
            containerBuilder.Register(container => new ServiceLevelOutcomeRepository(container.Resolve<RepositoryContext>())).As<IServiceLevelOutcomeRepository>();
            containerBuilder.Register(container => new ImmigrationCitizenshipStatusRepository(container.Resolve<RepositoryContext>())).As<IImmigrationCitizenshipStatusRepository>();
            containerBuilder.Register(container => new IndividualStatusRepository(container.Resolve<RepositoryContext>())).As<IIndividualStatusRepository>();
            containerBuilder.Register(container => new AssessmentTypeRepository(container.Resolve<RepositoryContext>())).As<IAssessmentTypeRepository>();
            containerBuilder.Register(container => new ProfileTypeRepository(container.Resolve<RepositoryContext>())).As<IProfileTypeRepository>();
            containerBuilder.Register(container => new RiskTypeRepository(container.Resolve<RepositoryContext>())).As<IRiskTypeRepository>();
            containerBuilder.Register(container => new FinancialAssistanceCategoryRepository(container.Resolve<RepositoryContext>())).As<IFinancialAssistanceCategoryRepository>();
            containerBuilder.Register(container => new FinancialAssistanceSubCategoryRepository(container.Resolve<RepositoryContext>())).As<IFinancialAssistanceSubCategoryRepository>();
            containerBuilder.Register(container => new ReasonsForDischargeRepository(container.Resolve<RepositoryContext>())).As<IReasonsForDischargeRepository>();

            containerBuilder.Register(container => new QualityOfLifeCategoryRepository(container.Resolve<RepositoryContext>())).As<IQualityOfLifeCategoryRepository>();
            containerBuilder.Register(container => new QualityOfLifeSubCategoryRepository(container.Resolve<RepositoryContext>())).As<IQualityOfLifeSubCategoryRepository>();
            containerBuilder.Register(container => new QualityOfLifeRepository(container.Resolve<RepositoryContext>())).As<IQualityOfLifeRepository>();
            containerBuilder.Register(container => new RegionRoleRepository(container.Resolve<RepositoryContext>())).As<IRegionRoleRepository>();
            containerBuilder.Register(container => new RegionSubProgramRepository(container.Resolve<RepositoryContext>())).As<IRegionSubProgramRepository>();
            containerBuilder.Register(container => new CaseMemberProfileRepository(container.Resolve<RepositoryContext>(),
                container.Resolve<IWorkerRoleActionPermissionRepository>()
                , container.Resolve<IWorkerRoleActionPermissionNewRepository>())).As<ICaseMemberProfileRepository>();
            containerBuilder.Register(container => new CaseAssessmentLivingConditionRepository(container.Resolve<RepositoryContext>())).As<ICaseAssessmentLivingConditionRepository>();
            containerBuilder.Register(container => new CaseAssessmentRepository(container.Resolve<RepositoryContext>(), container.Resolve<ICaseAssessmentLivingConditionRepository>(),
                container.Resolve<IWorkerRoleActionPermissionRepository>(),
                container.Resolve<IWorkerRoleActionPermissionNewRepository>(),
                container.Resolve<ICaseWorkerRepository>(),
                container.Resolve<IWorkerNotificationRepository>())).As<ICaseAssessmentRepository>();
            containerBuilder.Register(container => new CaseGoalLivingConditionRepository(container.Resolve<RepositoryContext>())).As<ICaseGoalLivingConditionRepository>();
            containerBuilder.Register(container => new CaseGoalRepository(container.Resolve<RepositoryContext>(),
                container.Resolve<ICaseGoalLivingConditionRepository>(),
                container.Resolve<IWorkerRoleActionPermissionRepository>(),
                container.Resolve<IWorkerRoleActionPermissionNewRepository>())).As<ICaseGoalRepository>();

            containerBuilder.Register(container => new CaseSmartGoalRepository(container.Resolve<RepositoryContext>(),
                container.Resolve<ICaseSmartGoalServiceProviderRepository>(),
                container.Resolve<ICaseActionRepository>(),
                container.Resolve<IWorkerRoleActionPermissionRepository>(),
                container.Resolve<IWorkerRoleActionPermissionNewRepository>())).As<ICaseSmartGoalRepository>();
            containerBuilder.Register(container => new ServiceRepository(container.Resolve<RepositoryContext>())).As<IServiceRepository>();
            containerBuilder.Register(container => new ServiceProviderRepository(container.Resolve<RepositoryContext>())).As<IServiceProviderRepository>();
            containerBuilder.Register(container => new CaseSmartGoalServiceProviderRepository(container.Resolve<RepositoryContext>(),
                container.Resolve<ICaseWorkerRepository>(),
                container.Resolve<IWorkerNotificationRepository>())).As<ICaseSmartGoalServiceProviderRepository>();
            containerBuilder.Register(container => new SmartGoalRepository(container.Resolve<RepositoryContext>())).As<ISmartGoalRepository>();
            containerBuilder.Register(container => new CaseSmartGoalProgressRepository(container.Resolve<RepositoryContext>())).As<ICaseSmartGoalProgressRepository>();
            containerBuilder.Register(container => new CaseSmartGoalServiceLevelOutcomeRepository(container.Resolve<RepositoryContext>())).As<ICaseSmartGoalServiceLevelOutcomeRepository>();
            containerBuilder.Register(container => new CaseProgressNoteMembersRepository(container.Resolve<RepositoryContext>(),
                container.Resolve<IWorkerRoleActionPermissionRepository>(),
                container.Resolve<IWorkerRoleActionPermissionNewRepository>())).As<ICaseProgressNoteMembersRepository>();

            containerBuilder.Register(container => new WorkerInRoleNewRepository(container.Resolve<RepositoryContext>())).As<IWorkerInRoleNewRepository>();
            containerBuilder.Register(container => new PermissionRepository(container.Resolve<RepositoryContext>(), container.Resolve<IPermissionActionRepository>())).As<IPermissionRepository>();
            containerBuilder.Register(container => new PermissionSubProgramRepository(container.Resolve<RepositoryContext>())).As<IPermissionSubProgramRepository>();
            containerBuilder.Register(container => new PermissionRegionRepository(container.Resolve<RepositoryContext>(), container.Resolve<IPermissionSubProgramRepository>()
                , container.Resolve<IPermissionJamatkhanaRepository>(), container.Resolve<IRegionRepository>()
                , container.Resolve<IJamatkhanaRepository>())).As<IPermissionRegionRepository>();
            containerBuilder.Register(container => new PermissionJamatkhanaRepository(container.Resolve<RepositoryContext>())).As<IPermissionJamatkhanaRepository>();
            containerBuilder.Register(container => new PermissionActionRepository(container.Resolve<RepositoryContext>())).As<IPermissionActionRepository>();
            containerBuilder.Register(container => new ActionMethodRepository(container.Resolve<RepositoryContext>())).As<IActionMethodRepository>();
            containerBuilder.Register(container => new WorkerRolePermissionNewRepository(container.Resolve<RepositoryContext>())).As<IWorkerRolePermissionNewRepository>();
            containerBuilder.Register(container => new ReportRepository()).As<IReportRepository>();
        }
    }
}

