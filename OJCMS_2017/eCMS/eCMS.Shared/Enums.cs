//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

namespace eCMS.Shared
{
    public enum VisibilityStatus
    {
        UnDefined = 0,
        None = 1,
        All = 2,
        Assigned = 3
    }

    public enum MailType
    {
        Email = 1,
        SMS = 2,
        Message = 3
    }

    public enum PhoneType
    {
        Mobile = 1,
        Home = 2,
        Office = 3
    }

    public enum EmailType
    {
        Personal = 1,
        Official = 2
    }

    public enum QualityOfLifeType
    {
        AssetsAndProduction = 1,
        DignityAndSelfRespect = 2,
        Education = 3,
        Health = 4,
        Housing = 5,
        ImmediateNeeds = 6,
        IncomeAndLivelihood = 7,
        SocialSupport = 8
    }

    public enum ServiceLevelOutcomeType
    {
        InProcess=6
    }
    public enum WorkerNoteActivityType
    { 
        AddCase=1,
        EditCase=2,
        EditHouseholdIncome=3,
        WorkNote=4,
        CaseIndividual = 5,
        CaseWorker = 6,
        CaseInitialAssessment = 7,
    }
    public enum IncomeRanges
    { 
        Lessthan20000 = 1,
        From20000to25000 = 2,
        From25000to40000=3,
        From40000to60000=4,
        From60000to80000=5,
        Greaterthan80000=6,
    }
}
