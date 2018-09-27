using System;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Migrator
{
    [Serializable]
    [XmlRoot(ElementName = "candidate", Namespace = "", IsNullable = false)]
    public class Candidate
    {
        public (string Folder, string SubFolder) Folder { get; set; }

        [XmlElement("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }

        [XmlElement("code", Form = XmlSchemaForm.Unqualified)]
        public string Code { get; set; }

        [XmlElement("lastname", Form = XmlSchemaForm.Unqualified)]
        public string Lastname { get; set; }

        [XmlElement("firstname", Form = XmlSchemaForm.Unqualified)]
        public string Firstname { get; set; }

        [XmlElement("fullname", Form = XmlSchemaForm.Unqualified)]
        public string Fullname { get; set; }

        [XmlElement("is_candidate", Form = XmlSchemaForm.Unqualified)]
        public string IsCandidate { get; set; }

        [XmlElement("gender_id", Form = XmlSchemaForm.Unqualified)]
        public string GenderId { get; set; }

        [XmlElement("birth_date", Form = XmlSchemaForm.Unqualified)]
        public string BirthDate { get; set; }

        [XmlElement("birth_year", Form = XmlSchemaForm.Unqualified)]
        public string BirthYear { get; set; }

        [XmlElement("age", Form = XmlSchemaForm.Unqualified)]
        public string Age { get; set; }

        [XmlElement("address", Form = XmlSchemaForm.Unqualified)]
        public string Address { get; set; }

        [XmlElement("mobile_phone", Form = XmlSchemaForm.Unqualified)]
        public string MobilePhone { get; set; }

        [XmlElement("home_phone", Form = XmlSchemaForm.Unqualified)]
        public string HomePhone { get; set; }

        [XmlElement("email", Form = XmlSchemaForm.Unqualified)]
        public string Email { get; set; }

        [XmlElement("creation_date", Form = XmlSchemaForm.Unqualified)]
        public string CreationDate { get; set; }

        [XmlElement("last_mod_date", Form = XmlSchemaForm.Unqualified)]
        public string LastModDate { get; set; }

        [XmlElement("desired_position_name", Form = XmlSchemaForm.Unqualified)]
        public string DesiredPositionName { get; set; }

        [XmlElement("salary", Form = XmlSchemaForm.Unqualified)]
        public string Salary { get; set; }

        [XmlElement("salary_currency_id", Form = XmlSchemaForm.Unqualified)]
        public string SalaryCurrencyId { get; set; }

        [XmlElement("uni_salary", Form = XmlSchemaForm.Unqualified)]
        public string UniSalary { get; set; }

        [XmlElement("user_id", Form = XmlSchemaForm.Unqualified)]
        public string UserId { get; set; }

        [XmlElement("state_id", Form = XmlSchemaForm.Unqualified)]
        public string StateId { get; set; }

        [XmlElement("state_date", Form = XmlSchemaForm.Unqualified)]
        public string StateDate { get; set; }

        [XmlElement("cp_date", Form = XmlSchemaForm.Unqualified)]
        public string CpDate { get; set; }

        [XmlElement("last_comment", Form = XmlSchemaForm.Unqualified)]
        public string LastComment { get; set; }

        [XmlElement("main_vacancy_id", Form = XmlSchemaForm.Unqualified)]
        public string MainVacancyId { get; set; }

        [XmlElement("doc_info", Form = XmlSchemaForm.Unqualified)]
        public CandidateDocInfo DocInfo { get; set; }

        [XmlArray("spots", Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("spot", typeof(CandidateSpotsSpot),
            Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public CandidateSpotsSpot[] Spots { get; set; }

        [XmlArray("attachments", Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("attachment", typeof(CandidateAttachmentsAttachment),
            Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public CandidateAttachmentsAttachment[] Attachments { get; set; }

        [XmlArray("hot_event", Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("hot_event", typeof(CandidateHotEventsHotEvent),
            Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public CandidateHotEventsHotEvent[] HotEvents { get; set; }

        [XmlAttribute("SPXML-FORM")]
        public string SpXmlForm { get; set; }
    }

    [Serializable]
    [XmlType("candidateHot_eventsHot_event", AnonymousType = true)]
    public class CandidateHotEventsHotEvent
    {
        [XmlElement("type_id", Form = XmlSchemaForm.Unqualified)]
        public string TypeId { get; set; }

        [XmlElement("vacancy_id", Form = XmlSchemaForm.Unqualified)]
        public string VacancyId { get; set; }

        [XmlElement("date", Form = XmlSchemaForm.Unqualified)]
        public string Date { get; set; }
    }

    [Serializable]
    [XmlType("candidateAttachmentsAttachment", AnonymousType = true)]
    public class CandidateAttachmentsAttachment
    {
        [XmlElement("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }

        [XmlElement("type_id", Form = XmlSchemaForm.Unqualified)]
        public string TypeId { get; set; }

        [XmlElement("date", Form = XmlSchemaForm.Unqualified)]
        public string Date { get; set; }

        [XmlElement("content_type", Form = XmlSchemaForm.Unqualified)]
        public string ContentType { get; set; }

        [XmlElement("text", Form = XmlSchemaForm.Unqualified, IsNullable = true)]
        public CandidateAttachmentsAttachmentText Text { get; set; }

        [XmlElement("data", Form = XmlSchemaForm.Unqualified, IsNullable = true)]
        public CandidateAttachmentsAttachmentData Data { get; set; }

        [XmlElement("file_name", Form = XmlSchemaForm.Unqualified, IsNullable = true)]
        public string FileName { get; set; }
    }

    [Serializable]
    [XmlType("candidateAttachmentsAttachmentData", AnonymousType = true)]
    public class CandidateAttachmentsAttachmentData
    {
        [XmlAttribute("INLINE-EXT-OBJECT-ID")]
        public string InlineExtObjectId { get; set; }

        [XmlAttribute("EXT-OBJECT-ID")]
        public string ExtObjectId { get; set; }

        [XmlAttribute("SIZE")]
        public uint Size { get; set; }

        [XmlText]
        public byte[] Value { get; set; }
    }

    [Serializable]
    [XmlType("candidateAttachmentsAttachmentText", AnonymousType = true)]
    public class CandidateAttachmentsAttachmentText
    {
        [XmlAttribute("INLINE-EXT-OBJECT-ID")]
        public string InlineExtObjectId { get; set; }

        [XmlAttribute("EXT-OBJECT-ID")]
        public string ExtObjectId { get; set; }

        [XmlAttribute("SIZE")]
        public uint Size { get; set; }

        [XmlText]
        public string[] Value { get; set; }
    }

    [Serializable]
    [XmlType("candidateSpotsSpot", AnonymousType = true)]
    public class CandidateSpotsSpot
    {
        [XmlElement("vacancy_id", Form = XmlSchemaForm.Unqualified)]
        public string VacancyId { get; set; }

        [XmlElement("start_date", Form = XmlSchemaForm.Unqualified)]
        public string StartDate { get; set; }

        [XmlElement("state_id", Form = XmlSchemaForm.Unqualified)]
        public string StateId { get; set; }

        [XmlElement("state_date", Form = XmlSchemaForm.Unqualified)]
        public string StateDate { get; set; }
    }

    [Serializable]
    [XmlType("candidateDoc_info", AnonymousType = true)]
    public class CandidateDocInfo
    {
        [XmlElement("creation", Form = XmlSchemaForm.Unqualified)]
        public CandidateDocInfoCreation[] Creation { get; set; }

        [XmlElement("modification", Form = XmlSchemaForm.Unqualified)]
        public CandidateDocInfoModification[] Modification { get; set; }
    }

    [Serializable]
    [XmlType("candidateDoc_infoModification", AnonymousType = true)]
    public class CandidateDocInfoModification
    {
        [XmlElement("user_login", Form = XmlSchemaForm.Unqualified)]
        public string UserLogin { get; set; }
    }

    [Serializable]
    [XmlType("candidateDoc_infoCreation", AnonymousType = true)]
    public class CandidateDocInfoCreation
    {
        [XmlElement("user_login", Form = XmlSchemaForm.Unqualified)]
        public string UserLogin { get; set; }
    }
}