using System;
using System.Xml.Serialization;
using static System.Xml.Schema.XmlSchemaForm;

namespace Migrator
{
    [Serializable]
    [XmlRoot(ElementName = "vacancy", Namespace = "", IsNullable = false)]
    public class Vacancy
    {
        public (string Folder, string SubFolder) Folder { get; set; }

        [XmlElement("id", Form = Unqualified)]
        public string Id { get; set; }

        [XmlElement("name", Form = Unqualified)]
        public string Name { get; set; }

        [XmlElement("code", Form = Unqualified)]
        public string Code { get; set; }

        [XmlElement("start_date", Form = Unqualified)]
        public string StartDate { get; set; }

        [XmlElement("req_close_date", Form = Unqualified)]
        public string ReqCloseDate { get; set; }

        [XmlElement("state_id", Form = Unqualified)]
        public string StateId { get; set; }

        [XmlElement("state_date", Form = Unqualified)]
        public string StateDate { get; set; }

        [XmlElement("final_candidate_id", Form = Unqualified)]
        public string FinalCandidateId { get; set; }

        [XmlElement("final_candidate_state_id", Form = Unqualified)]
        public string FinalCandidateStateId { get; set; }

        [XmlElement("close_date", Form = Unqualified)]
        public string CloseDate { get; set; }

        [XmlElement("work_end_date", Form = Unqualified)]
        public string WorkEndDate { get; set; }

        [XmlElement("work_days_num", Form = Unqualified)]
        public string WorkDaysNum { get; set; }

        [XmlElement("work_wdays_num", Form = Unqualified)]
        public string WorkWdaysNum { get; set; }

        [XmlElement("req_quantity", Form = Unqualified)]
        public string ReqQuantity { get; set; }

        [XmlElement("processed_quantity", Form = Unqualified)]
        public string ProcessedQuantity { get; set; }

        [XmlElement("reason_id", Form = Unqualified)]
        public string ReasonId { get; set; }

        [XmlElement("difficulty_level_id", Form = Unqualified)]
        public string DifficultyLevelId { get; set; }

        [XmlElement("recruit_type_id", Form = Unqualified)]
        public string RecruitTypeId { get; set; }

        [XmlElement("priority_id", Form = Unqualified)]
        public string PriorityId { get; set; }

        [XmlElement("salary_currency_id", Form = Unqualified)]
        public string SalaryCurrencyId { get; set; }

        [XmlElement("comment", Form = Unqualified)]
        public string Comment { get; set; }

        [XmlElement("candidates_num", Form = Unqualified)]
        public string CandidatesNum { get; set; }

        [XmlElement("publish_on_portal", Form = Unqualified)]
        public string PublishOnPortal { get; set; }

        [XmlElement("user_id", Form = Unqualified)]
        public string UserId { get; set; }

        [XmlElement("creation_date", Form = Unqualified)]
        public string CreationDate { get; set; }

        [XmlElement("last_mod_date", Form = Unqualified)]
        public string LastModDate { get; set; }

        [XmlElement("max_work_term", Form = Unqualified)]
        public VacancyMaxWorkTerm[] MaxWorkTerm { get; set; }

        [XmlArray(Form = Unqualified)]
        [XmlArrayItem("record", typeof(vacancyRecordsRecord), Form = Unqualified, IsNullable = false)]
        public vacancyRecordsRecord[] Records { get; set; }

        [XmlArray(Form = Unqualified)]
        [XmlArrayItem("hot_event", typeof(VacancyHotEventsHotEvent), Form = Unqualified, IsNullable = false)]
        public VacancyHotEventsHotEvent[] HotEvents { get; set; }

        [XmlElement("inet_data", Form = Unqualified)]
        public VacancyInetData[] InetData { get; set; }

        [XmlArray("attachments",Form = Unqualified)]
        [XmlArrayItem("attachment", typeof(VacancyAttachmentsAttachment), Form = Unqualified, IsNullable = false)]
        public VacancyAttachmentsAttachment[] Attachments { get; set; }

        [XmlElement("doc_info", Form = Unqualified)]
        public VacancyDocInfo[] DocInfo { get; set; }

        [XmlAttribute("SPXML-FORM")]
        public string SpXmlForm { get; set; }
    }

    [Serializable]
    [XmlType(TypeName = "vacancyDoc_info", AnonymousType = true)]
    public class VacancyDocInfo
    {
        [XmlElement("creation", Form = Unqualified)]
        public VacancyDocInfoCreation[] Creation { get; set; }

        [XmlElement("modification", Form = Unqualified)]
        public VacancyDocInfoModification[] Modification { get; set; }
    }

    [Serializable]
    [XmlType(TypeName = "vacancyDoc_infoModification", AnonymousType = true)]
    public class VacancyDocInfoModification
    {
        [XmlElement("user_login", Form = Unqualified)]
        public string UserLogin { get; set; }
    }

    [Serializable]
    [XmlType(TypeName = "vacancyDoc_infoCreation", AnonymousType = true)]
    public class VacancyDocInfoCreation
    {
        [XmlElement("user_login", Form = Unqualified)]
        public string UserLogin { get; set; }
    }

    [Serializable]
    [XmlType(TypeName = "vacancyAttachmentsAttachment", AnonymousType = true)]
    public class VacancyAttachmentsAttachment
    {
        [XmlElement("id", Form = Unqualified)]
        public string Id { get; set; }

        [XmlElement("type_id", Form = Unqualified)]
        public string TypeId { get; set; }

        [XmlElement("date", Form = Unqualified)]
        public string Date { get; set; }

        [XmlElement("content_type", Form = Unqualified)]
        public string ContentType { get; set; }

        [XmlElement("text", Form = Unqualified, IsNullable = true)]
        public VacancyAttachmentsAttachmentText Text { get; set; }
    }

    [Serializable]
    [XmlType("vacancyAttachmentsAttachmentText", AnonymousType = true)]
    public class VacancyAttachmentsAttachmentText
    {
        [XmlAttribute("INLINE-EXT-OBJECT-ID")]
        public string InlineExtObjectId { get; set; }

        [XmlAttribute("EXT-OBJECT-ID")]
        public string ExtObjectId { get; set; }

        [XmlAttribute("SIZE")]
        public uint Size { get; set; }
        
        [XmlText]
        public string Value { get; set; }
    }

    [Serializable]
    [XmlType(TypeName = "vacancyInet_data", AnonymousType = true)]
    public class VacancyInetData
    {
        [XmlElement("work_type_id", Form = Unqualified)]
        public string WorkTypeId { get; set; }

        [XmlElement("use_comment", Form = Unqualified)]
        public string UseComment { get; set; }

        [XmlElement("use_ext_comment", Form = Unqualified)]
        public string UseExtComment { get; set; }
    }

    [Serializable]
    [XmlType(TypeName = "vacancyHot_eventsHot_event", AnonymousType = true)]
    public class VacancyHotEventsHotEvent
    {
        [XmlElement("type_id", Form = Unqualified)]
        public string TypeId { get; set; }

        [XmlElement("events_num", Form = Unqualified)]
        public string EventsNum { get; set; }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    public class vacancyRecordsRecord
    {
        [XmlElement("date", Form = Unqualified)]
        public string Date { get; set; }

        [XmlElement("type_id", Form = Unqualified)]
        public string TypeId { get; set; }

        [XmlElement("state_id", Form = Unqualified)]
        public string StateId { get; set; }
    }

    [Serializable]
    [XmlType(TypeName = "vacancyMax_work_term", AnonymousType = true)]
    public class VacancyMaxWorkTerm
    {
        [XmlElement("length", Form = Unqualified)]
        public string Length { get; set; }

        [XmlElement("unit_id", Form = Unqualified)]
        public string UnitId { get; set; }
    }
}