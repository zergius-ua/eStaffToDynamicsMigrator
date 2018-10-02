using System;
using Microsoft.Xrm.Sdk;

namespace Migrator
{
    public class CandidateDyn
    {
        public CandidateDyn(Candidate candidate)
        {
            Address1 = new AddressDyn(candidate);
            BirthDate = DateTime.Parse(candidate.BirthDate);
            Business2 = candidate.WorkPhone;
            MobilePhone = candidate.MobilePhone;
            Home2 = candidate.HomePhone;
            LastName = candidate.LastName;
            FirstName = candidate.FirstName;
            MiddleName = candidate.MiddleName;
            EMailAddress1 = candidate.Email;
            EMailAddress2 = candidate.Email2;
            //dcrs_MSEducation = ParseEducation(candidate.Education);
        }

        public OptionSetValue AccountRoleCode { get; set; }
        public AddressDyn Address1 { get; set; }
        public AddressDyn Address2 { get; set; }
        public AddressDyn Address3 { get; set; }
        public decimal Aging30 { get; set; }
        public decimal Aging30_Base { get; set; }
        public decimal Aging60 { get; set; }
        public decimal Aging60_Base { get; set; }
        public decimal Aging90 { get; set; }
        public decimal Aging90_Base { get; set; }
        public DateTime Anniversary { get; set; }
        public decimal AnnualIncome { get; set; }
        public decimal AnnualIncome_Base { get; set; }
        public string AssistantName { get; set; }
        public string AssistantPhone { get; set; }
        public DateTime BirthDate { get; set; }
        public string Business2 { get; set; }
        public string Callback { get; set; }
        public string ChildrensNames { get; set; }
        public string Company { get; set; }
        public string ContactId { get; set; }
        public Lookup CreatedBy { get; set; }
        public Lookup CreatedByExternalParty { get; set; }
        public DateTime CreatedOn { get; set; }
        public Lookup CreatedOnBehalfBy { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal CreditLimit_Base { get; set; }
        public TwoOptions CreditOnHold { get; set; }
        public OptionSetValue CustomerSizeCode { get; set; }
        public OptionSetValue CustomerTypeCode { get; set; }
        public string dcrs_authtoken { get; set; }
        public DateTime dcrs_authtokenexp { get; set; }
        public string dcrs_AutoNumber { get; set; }
        public OptionSetValue dcrs_CandidateIndustry { get; set; }
        public OptionSetValue dcrs_CandidateSource { get; set; }
        public string dcrs_CareerBuilderMD5Hash { get; set; }
        public TwoOptions dcrs_CreateFromFile { get; set; }
        public Lookup dcrs_CurrentWorkCompany { get; set; }
        public OptionSetValue dcrs_Education { get; set; }
        public string dcrs_FacebookLink { get; set; }
        public string dcrs_GooglePlusLink { get; set; }
        public TwoOptions dcrs_IsDragDropFile { get; set; }
        public string dcrs_LinkedIn { get; set; }
        public MultiSelectOptionValue dcrs_MSCategory { get; set; }
        public MultiSelectOptionValue dcrs_MSEducation { get; set; }
        public MultiSelectOptionValue dcrs_MSIndustry { get; set; }
        public MultiSelectOptionValue dcrs_MSType { get; set; }
        public OptionSetValue dcrs_PrimarySkill { get; set; }
        public string dcrs_ProfileSummary { get; set; }
        public string dcrs_ReservedDDFile { get; set; }
        public string dcrs_Resume { get; set; }
        public DateTime dcrs_ResumeDate { get; set; }
        public string dcrs_TwitterLink { get; set; }
        public OptionSetValue dcrs_Type { get; set; }
        public Lookup dcrs_ZipCodeRelationId { get; set; }
        public Lookup DefaultPriceLevelId { get; set; }
        public string Department { get; set; }
        public string Description { get; set; }
        public TwoOptions DoNotBulkEMail { get; set; }
        public TwoOptions DoNotBulkPostalMail { get; set; }
        public TwoOptions DoNotEMail { get; set; }
        public TwoOptions DoNotFax { get; set; }
        public TwoOptions DoNotPhone { get; set; }
        public TwoOptions DoNotPostalMail { get; set; }
        public TwoOptions DoNotSendMM { get; set; }
        public OptionSetValue EducationCode { get; set; }
        public string EMailAddress1 { get; set; }
        public string EMailAddress2 { get; set; }
        public string EMailAddress3 { get; set; }
        public string EmployeeId { get; set; }
        public Image EntityImage { get; set; }
        public decimal ExchangeRate { get; set; }
        public string ExternalUserIdentifier { get; set; }
        public OptionSetValue FamilyStatusCode { get; set; }
        public string Fax { get; set; }
        public string FirstName { get; set; }
        public TwoOptions FollowEmail { get; set; }
        public string FtpSiteUrl { get; set; }
        public string FullName { get; set; }
        public OptionSetValue GenderCode { get; set; }
        public string GovernmentId { get; set; }
        public OptionSetValue HasChildrenCode { get; set; }
        public string Home2 { get; set; }
        public decimal ImportSequenceNumber { get; set; } // WholeNumber
        public TwoOptions IsBackofficeCustomer { get; set; }
        public string JobTitle { get; set; }
        public string LastName { get; set; }
        public DateTime LastOnHoldTime { get; set; }
        public DateTime LastUsedInCampaign { get; set; }
        public OptionSetValue LeadSourceCode { get; set; }
        public decimal li_CompanyId { get; set; }
        public string li_MemberToken { get; set; }
        public string ManagerName { get; set; }
        public string ManagerPhone { get; set; }
        public TwoOptions MarketingOnly { get; set; }
        public Lookup MasterId { get; set; }
        public TwoOptions Merged { get; set; }
        public string MiddleName { get; set; }
        public string MobilePhone { get; set; }
        public Lookup ModifiedBy { get; set; }
        public Lookup ModifiedByExternalParty { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Lookup ModifiedOnBehalfBy { get; set; }
        public TwoOptions msdyn_gdproptout { get; set; }
        public Lookup new_ae { get; set; }
        public Lookup new_bdr { get; set; }
        public string new_City { get; set; }
        public OptionSetValue new_Country { get; set; }
        public string new_Phone1 { get; set; }
        public string new_Phone2 { get; set; }
        public string new_SecondarySkill { get; set; }
        public OptionSetValue new_SecondarySkill1 { get; set; }
        public string new_Skype { get; set; }
        public string NickName { get; set; }
        public decimal NumberOfChildren { get; set; }
        public decimal OnHoldTime { get; set; }
        public Lookup OriginatingLeadId { get; set; }
        public DateTime OverriddenCreatedOn { get; set; }
        public Owner OwnerId { get; set; }
        public Lookup OwningBusinessUnit { get; set; }
        public Lookup OwningTeam { get; set; }
        public Lookup OwningUser { get; set; }
        public string Pager { get; set; }
        public Customer ParentCustomerId { get; set; }
        public TwoOptions ParticipatesInWorkflow { get; set; }
        public OptionSetValue PaymentTermsCode { get; set; }
        public OptionSetValue PreferredAppointmentDayCode { get; set; }
        public OptionSetValue PreferredAppointmentTimeCode { get; set; }
        public OptionSetValue PreferredContactMethodCode { get; set; }
        public Lookup PreferredEquipmentId { get; set; }
        public Lookup PreferredServiceId { get; set; }
        public Lookup PreferredSystemUserId { get; set; }
        public Guid ProcessId { get; set; }
        public string Salutation { get; set; }
        public OptionSetValue ShippingMethodCode { get; set; }
        public Lookup SLAId { get; set; }
        public Lookup SLAInvokedId { get; set; }
        public string SpousesName { get; set; }
        public Guid StageId { get; set; }
        public Status StateCode { get; set; }
        public StatusReason StatusCode { get; set; }
        public string Suffix { get; set; }
        public decimal TeamsFollowed { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string Telephone3 { get; set; }
        public OptionSetValue TerritoryCode { get; set; }
        public string TimeSpentByMeOnEm { get; set; }
        public decimal TimeZoneRuleVersionNumber { get; set; }
        public Lookup TransactionCurrencyId { get; set; }
        public string TraversedPath { get; set; }
        public decimal UTCConversionTimeZoneCode { get; set; }
        public TimeStamp VersionNumber { get; set; }
        public string WebSiteUrl { get; set; }
        public string YomiFirstName { get; set; }
    }

    public class TimeStamp
    {
    }

    public class StatusReason
    {
    }

    public class Status
    {
    }

    public class Customer
    {
    }

    public class Owner
    {
    }

    public class MultiSelectOptionValue
    {
    }

    public class TwoOptions
    {
    }

    public class Lookup
    {
    }
    
    public class Image
    {
    }
}