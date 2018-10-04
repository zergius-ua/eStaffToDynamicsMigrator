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
        private const string ImportDataPathBase = @"d:\EStaff_Server\data_rcr\obj\";
        private static IOrganizationService _orgService;
        private const int FlushCount = 1;
        private static bool _isConnected = false;
        private int idx = 0;
        
        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _isConnected = ConnectToCrm();

            var fileNames = Directory.GetFiles($@"{ImportDataPathBase}candidates\", "*.xml", SearchOption.AllDirectories).ToList(); //.Skip(150+377).ToList();
            Console.WriteLine($"Started at: {DateTime.Now}");
            while(fileNames.Any())
            {
                var tempFolderList = fileNames.Count > FlushCount ? fileNames.GetRange(0, FlushCount) : fileNames;
                foreach (var fileName in tempFolderList)
                {
                    Candidates.AddRange(ReadXmlSubFolder<Candidate>(fileName));
                }

                UploadCrmData();
                idx++;
                fileNames.RemoveRange(0, tempFolderList.Count);
            }

            Console.WriteLine($"Finished at: {DateTime.Now}");
            Console.WriteLine("Exiting application.");
        }

        private static IList<T> ReadXmlSubFolder<T>(string fileName) where T : class
        {
            var collection = new List<T>();
            var ser = new XmlSerializer(typeof(T));
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

            //else if(obj?.GetType() == )
            // if (obj != null)
            // collection.Add(obj);
            return collection;
        }

        private static void UploadCrmData()
        {
            if (_isConnected)
            {
                // Upload data to Dynamics 365
                var entityCreateList = new List<Entity>();
                var entityUpdateList = new List<Entity>();
                Candidates.ToList().ForEach(c =>
                {
                    var e = LookupExisting($"{c.FirstName} {c.LastName}");
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
                });
                Candidates.Clear();
            }
        }

        private static bool CreateEntities(List<Entity> entityCreateList)
        {
            if (!entityCreateList.Any()) return false;
            Console.Write($"{idx} : Creating {entityCreateList[0].Attributes["fullname"]} : ");
            var multipleRequest = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = false,
                    ReturnResponses = true
                },
                Requests = new OrganizationRequestCollection()
            };
            entityCreateList.ForEach(entity =>
            {
                var createRequest = new CreateRequest {Target = entity};
                multipleRequest.Requests.Add(createRequest);
            });
            var multipleResponse = (ExecuteMultipleResponse) _orgService.Execute(multipleRequest);
            var success = !multipleResponse.Results.Where(r => r.Key == "IsFaulted" && r.Value.ToString() == "True").ToList().Any();
            Console.WriteLine(!success ? "Create failed!" : "OK!");
            return success;
        }

        private static bool UpdateEntities(List<Entity> entityUpdateList)
        {
            if (!entityUpdateList.Any()) return false;
            Console.Write($"{idx} : Updating {entityUpdateList[0].Attributes["fullname"]} : ");
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
            Console.WriteLine(!success ? "Update failed!" : "OK!");
            return success;
        }

        private static Entity LookupExisting(string fullName)
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
                if (a.TypeId == "resume" && a.Text != null)
                {
                    a.Text.Value = File.ReadAllLines($@"{srcFolderName}\{a.Text.ExtObjectId.Substring(2)}");
                }
            });

            return candidate;
        }
    }
}