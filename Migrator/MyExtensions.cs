using System;
using System.Linq;
using Microsoft.Xrm.Sdk;

namespace Migrator
{
    public static class MyExtensions
    {
        private static void SetField(AttributeCollection attr, string name, object value)
        {
            if ((value as string)?.IsNotNullOrEmpty() == false) return;
            attr[name] = value;
        }

        private static Entity SetupEntity(Entity e, Candidate can)
        {
            e.LogicalName = "contact";
            var a = e.Attributes;
            SetField(a, "address2_composite", can.Address);
            SetField(a, "emailaddress1", can.Email2);
            SetField(a, "emailaddress2", can.Email);
            SetField(a, "firstname", can.FirstName);
            SetField(a, "fullname", $"{can.FirstName} {can.LastName}");
            SetField(a, "yomifullname", $"{can.FirstName} {can.LastName}");
            SetField(a, "lastname", can.LastName);
            SetField(a, "mobilephone", can.MobilePhone.LimitLength(100));
            SetField(a, "new_phone1", can.HomePhone.LimitLength(100));
            SetField(a, "new_phone2", can.WorkPhone.LimitLength(100));

            // a["dcrs_candidatesource"] = new OptionSetValue((int) 100000002);
            a["dcrs_candidatesource"] = null;
            a["new_country"] = new OptionSetValue((int) 100000002);
            // a["dcrs_primaryskill"] = new OptionSetValue((int) 100000008);
            a["dcrs_primaryskill"] = null;

            var t = new OptionSetValueCollection();
            t.Add(new OptionSetValue((int) 603160001));
            a["dcrs_mstype"] = t;

            if (can.BirthDate.IsNotNullOrEmpty())
            {
                SetField(a, "birthdate", DateTime.Parse(can.BirthDate));
            }

            /*if (can.Attachments != null && can.Attachments.Any())
            {
                can.Attachments
                    .Where(ax => ax.TypeId != null && ax.TypeId == "resume")
                    .ToList()
                    .ForEach(att =>
                    {
                        if (att.Text != null)
                            SetField(a, "dcrs_resume", string.Join(Environment.NewLine, att.Text.Value));
                        if (att.Date.IsNotNullOrEmpty())
                            SetField(a, "dcrs_resumedate", DateTime.Parse(att.Date));
                    });
            }*/

            return e;
        }

        public static Entity ToEntity(this Candidate can)
        {
            return SetupEntity(new Entity(), can);
        }

        public static Entity ToEntity(this Vacancy vac)
        {
            var e = new Entity();
            e.LogicalName = "dcrs_job";
            var a = e.Attributes;
            SetField(a, "dcrs_jobtitle", vac.Name);
            SetField(a, "dcrs_location", "Kharkiv, Ukraine");
            var attachement = "";
            vac.Attachments.Where(att=>att.Text!=null).ToList().ForEach(att=>attachement+=att.Text);
            SetField(a, "dcrs_jobdescription", attachement.LimitLength(999999));
            if (vac.StartDate.IsNotNullOrEmpty())
                SetField(a, "new_jobactive", DateTime.Parse(vac.StartDate));

            if (vac.ReasonId.IsNotNullOrEmpty())
            {
                switch (vac.ReasonId)
                {
                    // New Position
                    case "new":
                        a["new_reason"] = new OptionSetValue((int) 100000000);
                        break;
                    // Replacement
                    case "replacement":
                        a["new_reason"] = new OptionSetValue((int) 100000001);
                        break;
                    // Contract / Freelance
                    case "contract":
                        a["new_reason"] = new OptionSetValue((int) 100000002);
                        break;
                    default:
                        a["new_reason"] = new OptionSetValue((int) 100000000);
                        break;
                }
            }

            if (vac.StateId.IsNotNullOrEmpty())
            {
                var baseId = 100000000;
                var statusId = 0;
                switch (vac.StateId)
                {
                        case "vacancy_opened":
                            statusId = 0;
                            break;
                        case "vacancy_closed":
                        case "vacancy_suspended":
                        case "vacancy_cancelled":
                            statusId = 2;
                            break;
                        default:
                            statusId = 0;
                            break;
                }
                a["dcrs_status"] = new OptionSetValue((int) baseId + statusId);
            }

            if (vac.PriorityId.IsNotNullOrEmpty())
            {
                var priorityId = int.Parse(vac.PriorityId);
                var baseId = 100000000;
                a["new_priority"] = new OptionSetValue((int) baseId + priorityId);
            }

            return e;
        }

        public static void UpdateEntity(this Entity e, Candidate can)
        {
            SetupEntity(e, can);
        }

        private static bool IsNotNullOrEmpty(this string src)
        {
            return !string.IsNullOrEmpty(src);
        }

        public static string LimitLength(this string source, int maxLength)
        {
            return source?.Length <= maxLength ? source : source?.Substring(0, maxLength);
        }
    }
}