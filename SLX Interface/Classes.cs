using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;

namespace SLX_Interface
{
    public class LoginCredentials
    {
        //Caching login credentials
        public static NetworkCredential userCred { get; set; }
    }

    public class SearchableField
    {
        public string FriendlyName { get; set; }
        public string TrueName { get; set; }
        public bool Display { get; set; }
        public bool Search { get; set; }
    }

    public class CachedSearchFields
    {
        public static ObservableCollection<SearchableField> accounts { get; set; }
        public static ObservableCollection<SearchableField> tickets { get; set; }
        public static ObservableCollection<SearchableField> contacts { get; set; }
        public static ObservableCollection<SearchableField> opportunities { get; set; }
    }

    public class Account : IEquatable<Account>
    {
        public string SDataKey { get; set; }
        public string AccountName { get; set; }
        public string AccountNameUpper { get; set; }
        public string Aka { get; set; }
        public string AlternateKeyPrefix { get; set; }
        public string AlternateKeySuffix { get; set; }
        public string AlternatePhone { get; set; }
        public string BusinessDescription { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string CreditRating { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }
        public string Division { get; set; }
        public string DoNotSolicit { get; set; }
        public string Email { get; set; }
        public string EmailType { get; set; }
        public string Employees { get; set; }
        public string ExternalAccountNumber { get; set; }
        public string Fax { get; set; }
        public string ImportSource { get; set; }
        public string Industry { get; set; }
        public string InternalAccountNumber { get; set; }
        public string LastHistoryBy { get; set; }
        public string LastHistoryDate { get; set; }
        public string MainPhone { get; set; }
        public string ModifyDate { get; set; }
        public string ModifyUser { get; set; }
        public string NationalAccount { get; set; }
        public string Notes { get; set; }
        public string NotifyDefects { get; set; }
        public string NotifyOnClose { get; set; }
        public string NotifyOnStatus { get; set; }
        public string OtherPhone1 { get; set; }
        public string OtherPhone2 { get; set; }
        public string OtherPhone3 { get; set; }
        public string ParentAccountNumber { get; set; }
        public string ParentId { get; set; }
        public string Region { get; set; }
        public string Relationship { get; set; }
        public string Revenue { get; set; }
        public string Score { get; set; }
        public string ShortNotes { get; set; }
        public string SicCode { get; set; }
        public string Status { get; set; }
        public string SubType { get; set; }
        public string TargetAccount { get; set; }
        public string Ticker { get; set; }
        public string TollFree { get; set; }
        public string TollFree2 { get; set; }
        public string Type { get; set; }
        public string WebAddress { get; set; }
        public string WebAddress2 { get; set; }
        public string WebAddress3 { get; set; }
        public string WebAddress4 { get; set; }
        public string AppId { get; set; }
        public string LastErpSyncUpdate { get; set; }
        public string PromotedToAccounting { get; set; }
        public string CreateSource { get; set; }
        public string PlanEndDate { get; set; }
        public string PlanCode { get; set; }
        public string BusinessPartner { get; set; }
        public string VantiveID { get; set; }
        public string QualifiedBy { get; set; }
        public string QualifiedDate { get; set; }
        public string Act_rep_referral { get; set; }
        public string Num_of_Users { get; set; }
        public string LeadNotes { get; set; }
        public string TimeFrame { get; set; }
        public string Budget { get; set; }
        public string Competitor { get; set; }
        public string TrialStartDate { get; set; }
        public string LeadType { get; set; }
        public string AtlasId { get; set; }
        public string SlxUsers { get; set; }
        public string UserRange { get; set; }
        public string PlanstartDate { get; set; }
        public string IsKeyAccount { get; set; }
        public string Platform { get; set; }
        public string EloquaId { get; set; }
        public string CustomerId { get; set; }
        public string ScopusId { get; set; }
        public string NsinternalId { get; set; }
        public string NSLastSyncDate { get; set; }
        public string NSLastSyncMessage { get; set; }
        public string ActVersion { get; set; }
        public string SlxVersion { get; set; }
        public string IntlLegacyId1 { get; set; }
        public string IntlLegacyId1Source { get; set; }
        public string IntlLegacyId2 { get; set; }
        public string IntlLegacyId2Source { get; set; }
        public string LegacySlxLicenseGenKey { get; set; }
        public string PreferredLanguage { get; set; }
        public string SlxconcurrentUsers { get; set; }
        public string SlxnamedUsers { get; set; }
        public string Reason { get; set; }
        public string PartnerSubType { get; set; }
        public string ZuoraID { get; set; }
        public string EmailDomain { get; set; }
        public string IsDesiredEMktg { get; set; }
        public string LastTouchedInDays { get; set; }
        public string NSUrl { get; set; }
        public string ZuoraUrl { get; set; }
        public string Owner { get; set; }
        public string AccountManager { get; set; }
        public string AccountProducts { get; set; }
        public string Address { get; set; }
        public string Addresses { get; set; }
        public string Billings { get; set; }
        public string Contacts { get; set; }
        public string Contracts { get; set; }
        public string CustomerTickets { get; set; }
        public string DefaultTicketOwner { get; set; }
        public string DivisionalManager { get; set; }
        public string ExecutiveSponser { get; set; }
        public string LeadSource { get; set; }
        public string OperatingCompany { get; set; }
        public string Opportunities { get; set; }
        public string Projects { get; set; }
        public string RegionalManager { get; set; }
        public string Returns { get; set; }
        public string SalesOrders { get; set; }
        public string Sf_Accountactivities { get; set; }
        public string ShippingAddress { get; set; }
        public string SLXLicenses { get; set; }
        public string Subsidiary { get; set; }
        public string Territory { get; set; }
        public string Tickets { get; set; }
        public string AccountSummary { get; set; }
        public string ERPTradingAccount { get; set; }

        public override string ToString()
        {
            return SDataKey;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Account objAsPart = obj as Account;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public bool Equals(Account other)
        {
            if (other == null) return false;
            return (SDataKey.Equals(other.SDataKey));
        }
    }

    public class CachedData
    {
        public static ObservableCollection<Account> accounts { get; set; } = new ObservableCollection<Account>();
    }
}