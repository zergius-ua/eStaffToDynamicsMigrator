using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;

namespace Migrator
{
    internal static class Program
    {
        private static readonly List<Candidate> Candidates = new List<Candidate>();
        private static readonly List<Vacancy> Vacancies = new List<Vacancy>();
        private const string ImportDataPathBase = @"d:\EStaff_Server\data_rcr\obj\";
        private static IOrganizationService _orgService;
        private const int FlushCount = 20;
        private static bool _isConnected = false;
        private static int _idx = 0;

        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var fileNames = Directory.GetFiles($@"{ImportDataPathBase}vacancies\", "*.xml", SearchOption.AllDirectories).ToList().Skip(_idx).ToList();
            while (fileNames.Any())
            {
                var tempFolderList = fileNames.Count > FlushCount ? fileNames.GetRange(0, FlushCount) : fileNames;
                foreach (var fileName in tempFolderList)
                {
                    Vacancies.AddRange(ReadXmlSubFolder<Vacancy>(fileName));
                }

                fileNames.RemoveRange(0, tempFolderList.Count);
            }

            // var vacEntity = Vacancies[0].ToEntity();
            // Environment.Exit(1);

            _isConnected = ConnectToCrm();
            if (!_isConnected)
            {
                Console.WriteLine("Failed to connect to Dynamics 365. Exiting");
                Environment.Exit(1);
            }

            // var jobs = LookupAllJobEntities();
            var existingJobs = LookupAllJobEntities().ToList();
            var newJobs = new List<Entity>();
            Vacancies.ForEach(v =>
            {
                var ent = v.ToEntity();
                var ex = existingJobs.Any(j => j.Attributes["dcrs_jobtitle"].ToString() == v.Name && j.Attributes["dcrs_location"].ToString() == "Kharkiv, Ukraine");
                if (!ex)
                    newJobs.Add(ent);
            });

            var updJobs = existingJobs.Except(newJobs).ToList();

            if (updJobs.Except(existingJobs).Any())
            {
                UpdateEntities(updJobs);
            }

            CreateEntities(newJobs);

            Environment.Exit(1);

            /*if (_isConnected)
            {
                var can = LookupExisting("Tomas Jaramillo");
                Console.WriteLine($"Type: {can.Attributes["dcrs_mstype"]}");
                can = LookupExisting("Daniel Jedruszak");
                Console.WriteLine($"Type: {can.Attributes["dcrs_mstype"]}");
            }
            return;*/

            /*var fileNames = Directory.GetFiles($@"{ImportDataPathBase}candidates\", "*.xml", SearchOption.AllDirectories).ToList().Skip(_idx).ToList();
            Console.WriteLine($"Started at: {DateTime.Now}");
            var counter = 0;
            while (fileNames.Any())
            {
                var tempFolderList = fileNames.Count > FlushCount ? fileNames.GetRange(0, FlushCount) : fileNames;
                foreach (var fileName in tempFolderList)
                {
                    Candidates.AddRange(ReadXmlSubFolder<Candidate>(fileName));
                }

                counter += Candidates.Count;

                UploadCrmData();
                _idx += tempFolderList.Count;
                fileNames.RemoveRange(0, tempFolderList.Count);
                Console.WriteLine($"Loaded {counter} records");
            }

            Console.WriteLine($"Finished at: {DateTime.Now}");
            Console.WriteLine("Exiting application.");*/
        }

        private static IList<T> ReadXmlSubFolder<T>(string fileName) where T : class
        {
            var collection = new List<T>();
            var ser = new XmlSerializer(typeof(T));
            try
            {
                var obj = ser.Deserialize(new FileStream(fileName, FileMode.Open)) as T;


                if (obj?.GetType() == typeof(Candidate))
                {
                    var candidate = obj as Candidate;
                    if (candidate != null)
                    {
                        candidate = LoadAttachments(candidate);
                        collection.Add(candidate as T);
                    }
                    else
                    {
                        collection.Add(obj);
                    }
                }

                else if (obj?.GetType() == typeof(Vacancy))
                {
                    var vacancy = obj as Vacancy;
                    if (vacancy != null)
                    {
                        // TODO: read attachment
                        vacancy = LoadAttachments(vacancy);
                        collection.Add(vacancy as T);
                    }
                    else
                    {
                        collection.Add(obj);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }

            return collection;
        }

        private static void UploadCrmData()
        {
            if (_isConnected)
            {
                // Upload data to Dynamics 365
                var entityCreateList = new List<Entity>();
                var entityUpdateList = new List<Entity>();
                /*entityUpdateList.AddRange(LookupExisting());
                entityCreateList.AddRange(LookupMissed(entityUpdateList));*/
                Candidates.ToList().ForEach(c =>
                {
                    var e = LookupExistingEntity($"{c.FirstName} {c.LastName}");
                    if (e != null)
                    {
                        // Update
                        e.UpdateEntity(c);
                        entityUpdateList.Add(e);
                    }
                    else
                    {
                        // Create
                        e = c.ToEntity();
                        if (e != null)
                        {
                            entityCreateList.Add(e);
                        }
                    }
                });

                if (entityCreateList.Count >= 0)
                {
                    CreateEntities(entityCreateList);
                    entityCreateList.Clear();
                }

                if (entityUpdateList.Count >= 0)
                {
                    UpdateEntities(entityUpdateList);
                    entityUpdateList.Clear();
                }

                Candidates.Clear();
            }
        }

        /*private static IEnumerable<Entity> LookupMissed(IEnumerable<Entity> entityUpdateList)
        {
            var entities = new List<Entity>();
            Candidates.ForEach(c =>
            {
                var e = c.ToEntity();
                if (e != null)
                    entities.Add(e);
            });

            return entities.Except(entityUpdateList).ToList();
        }

        private static IEnumerable<Entity> LookupExisting()
        {
            var res = new List<Entity>();
            var qe = new QueryExpression("contact")
            {
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression()
            };
            Candidates.ForEach(c => qe.Criteria.AddCondition(new ConditionExpression("yomifullname", ConditionOperator.Equal, $"{c.FirstName} {c.LastName}")));
            var collection = _orgService.RetrieveMultiple(qe);
            res.AddRange(collection.Entities);
            return res;
        }*/

        private static bool CreateEntities(List<Entity> entityCreateList)
        {
            if (!entityCreateList.Any()) return false;
            // Console.Write($"{_idx} : Creating {entityCreateList[0].Attributes["fullname"]} : ");
            var multipleRequest = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = false,
                    ReturnResponses = true
                },
                Requests = new OrganizationRequestCollection()
            };
            /*/
            entityCreateList.ForEach(entity =>
            {
                var createRequest = new CreateRequest {Target = entity};
                multipleRequest.Requests.Add(createRequest);
            });
            /*/
            var createRequest = new CreateRequest {Target = entityCreateList[0]};
            multipleRequest.Requests.Add(createRequest);            //*/

            var multipleResponse = (ExecuteMultipleResponse) _orgService.Execute(multipleRequest);
            var success = !multipleResponse.Results.Where(r => r.Key == "IsFaulted" && r.Value.ToString() == "True").ToList().Any();
            // var color = Console.ForegroundColor;
            // Console.ForegroundColor = success ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed;
            // Console.WriteLine(!success ? "Create failed!" : "OK!");
            // Console.ForegroundColor = color;
            return success;
        }

        private static bool UpdateEntities(List<Entity> entityUpdateList)
        {
            if (!entityUpdateList.Any()) return false;
            // Console.Write($"{_idx} : Updating {entityUpdateList[0].Attributes["fullname"]} : ");
            var multipleRequest = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = false,
                    ReturnResponses = true
                },
                Requests = new OrganizationRequestCollection()
            };
            entityUpdateList.ForEach(entity =>
            {
                var updateRequest = new UpdateRequest {Target = entity};
                multipleRequest.Requests.Add(updateRequest);
            });
            var multipleResponse = (ExecuteMultipleResponse) _orgService.Execute(multipleRequest);
            var success = !multipleResponse.Results.Where(r => r.Key == "IsFaulted" && r.Value.ToString() == "True").ToList().Any();
            // var color = Console.ForegroundColor;
            // Console.ForegroundColor = success ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed;
            // Console.WriteLine(!success ? "Update failed!" : "OK!");
            // Console.ForegroundColor = color;
            return success;
        }

        private static Entity LookupExistingEntity(string fullName)
        {
            var qe = new QueryExpression("contact")
            {
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression()
            };
            qe.Criteria.AddCondition(new ConditionExpression("yomifullname", ConditionOperator.Equal, fullName));
            var collection = _orgService.RetrieveMultiple(qe);
            return collection.Entities.FirstOrDefault();
        }

        private static Entity[] LookupAllJobEntities()
        {
            var qe = new QueryExpression("dcrs_job")
            {
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression()
            };
            // qe.Criteria.AddCondition(new ConditionExpression("yomifullname", ConditionOperator.Equal, fullName));
            var collection = _orgService.RetrieveMultiple(qe);
            return collection.Entities.ToArray();
        }

        private static bool ConnectToCrm()
        {
            const string userName = "Sergey.Smirnoff@teaminternational.com";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var password = ReadPassword();

            Console.WriteLine("Connecting...");
            var crmServiceClient = new CrmServiceClient(userName, password, "NorthAmerica", "teamint", false, true, null, true);
            _orgService = crmServiceClient.OrganizationWebProxyClient ?? (IOrganizationService) crmServiceClient.OrganizationServiceProxy;

            var userRequest = new WhoAmIRequest();
            try
            {
                var userResponse = (WhoAmIResponse) _orgService.Execute(userRequest);
                var currentUserId = userResponse.UserId;
                return currentUserId.ToString() != "00000000-0000-0000-0000-000000000000";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        private static SecureString ReadPassword()
        {
            var password = new SecureString();
            Console.WriteLine("Enter your password:");
            var nextKey = Console.ReadKey(true);
            while (nextKey.Key != ConsoleKey.Enter)
            {
                if (nextKey.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password.RemoveAt(password.Length - 1);
                        // erase the last * as well
                        Console.Write(nextKey.KeyChar);
                        Console.Write(" ");
                        Console.Write(nextKey.KeyChar);
                    }
                }
                else
                {
                    password.AppendChar(nextKey.KeyChar);
                    Console.Write("*");
                }

                nextKey = Console.ReadKey(true);
            }

            Console.WriteLine("");

            password.MakeReadOnly();
            return password;
        }

        private static (string, string) ParseId(string id)
        {
            if (string.IsNullOrEmpty(id) || id.Length < 18) return (string.Empty, string.Empty);
            return (id.Substring(2, 14), id.Substring(16));
        }

        private static Candidate LoadAttachments(Candidate candidate)
        {
            candidate.Folder = ParseId(candidate?.Id);
            if (candidate.Attachments == null) return candidate;
            var srcFolderName = $@"{ImportDataPathBase}candidates\{candidate.Folder.Folder}\{candidate.Folder.SubFolder}_files";
            if (!Directory.Exists(srcFolderName)) return candidate;
            candidate.Attachments.ToList().ForEach(a =>
            {
                if (a.TypeId == "resume" && a.Text?.ExtObjectId != null)
                {
                    a.Text.Value = File.ReadAllLines($@"{srcFolderName}\{a.Text.ExtObjectId.Substring(2)}");
                }
            });

            return candidate;
        }

        private static Vacancy LoadAttachments(Vacancy vacancy)
        {
            vacancy.Folder = ParseId(vacancy?.Id);
            if (vacancy.Attachments == null) return vacancy;
            var srcFolderName = $@"{ImportDataPathBase}vacancies\{vacancy.Folder.Folder}\{vacancy.Folder.SubFolder}_files";
            if (!Directory.Exists(srcFolderName)) return vacancy;
            vacancy.Attachments.ToList().ForEach(a =>
            {
                if (a.TypeId == "vacancy_desc" && a.Text?.ExtObjectId != null)
                {
                    a.Text.Value = File.ReadAllText($@"{srcFolderName}\{a.Text.ExtObjectId.Substring(2)}");
                }
            });

            return vacancy;
        }
    }
}