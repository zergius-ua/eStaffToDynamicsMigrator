using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;

namespace Migrator
{
    internal static class Program
    {
        private static IEnumerable<Candidate> _candidates;
        private static IEnumerable<CandidateDyn> _candidatesDyn;
        private const string ImportDataPathBase = @"d:\EStaff_Server\data_rcr\obj\";
        private static IOrganizationService _orgService;
        private static IList<Entity> _entities = new List<Entity>();

        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
            // Read eStaff data (all records)
            Console.WriteLine("Reading eStaff data...");
            using (var spinner = new Spinner(Console.CursorLeft, Console.CursorTop))
            {
                spinner.Start();
                _candidates = ReadXmlFolder<Candidate>($@"{ImportDataPathBase}candidates\");
                spinner.Stop();
                /*
                Console.WriteLine("Converting to Dynamics 365...");
                spinner.Start();
                _candidatesDyn = ConvertToCandidateDyn(_candidates);
                spinner.Stop();
                */
            }

            // Connect to Dynamics 365
            var isConnected = ConnectToCrm();
            if (isConnected)
            {
                Console.WriteLine("Connected");
                
                /*
                Console.WriteLine("Retrieving data...");

                // Read CRM data
                using (var spinner = new Spinner(Console.CursorLeft, Console.CursorTop))
                {
                    spinner.Start();
                    var qe = new QueryExpression("contact")
                    {
                        ColumnSet = new ColumnSet(true),
                        Criteria = new FilterExpression()
                    };
                    qe.Criteria.AddCondition(new ConditionExpression("yomifullname", ConditionOperator.Equal, "Andriy Syrovenko"));
                    // qe.Criteria.AddCondition(new ConditionExpression("yomifullname", ConditionOperator.Equal, "Andrew Shtompel"));
                    // this will retrieve all fields, you should only retrieve attribute you need ;)
                    var collection = _orgService.RetrieveMultiple(qe);
                    spinner.Stop();
                    
                    // /*
//                     _entities = collection.Entities.ToList();
//                    collection.Entities.ToList().ForEach(entity =>
//                    {
//                        if(entity.GetAttributeValue<string>("yomifullname") == "Andriy Syrovenko")
//                        {
//                            _entities.Add(entity);
//                        }
//                    });
//                    var ent = _entities[0];
//                    ent["mobilephone"] = ent["mobilephone"] + " (---)";
//                    // ent.Attributes[""] = "";
//                    // ent.GetAttributeValue<DateTime>("") = DateTime.Now;
//                    _orgService.Update(ent);
                    // #1#
                    
                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
                }
            // */
                
                // Form records to upload
                {
                    _candidates.ToList().ForEach(c =>
                    {
                        if (LookupExisting(c.FullName))
                        {
                            // Update
                        }
                        else
                        {
                            // Create
                        }
                    });
                }
                // Upload data to Dynamics 365
                using (var spinner = new Spinner(Console.CursorLeft, Console.CursorTop))
                {
                    spinner.Start();
                    spinner.Stop();
                }
            }

            Console.WriteLine("Exiting application.");
        }

        private static bool LookupExisting(string fullName)
        {
            using (var spinner = new Spinner(Console.CursorLeft, Console.CursorTop))
            {
                spinner.Start();
                var qe = new QueryExpression("contact")
                {
                    ColumnSet = new ColumnSet(true),
                    Criteria = new FilterExpression()
                };
                qe.Criteria.AddCondition(new ConditionExpression("yomifullname", ConditionOperator.Equal, fullName));
                var collection = _orgService.RetrieveMultiple(qe);
                spinner.Stop();
                return collection.Entities.Any();
            }
        }

        private static bool ConnectToCrm()
        {
            const string userName = "Sergey.Smirnoff@teaminternational.com";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var password = ReadPassword();
            Console.WriteLine("Connecting...");
            using (var spinner = new Spinner(Console.CursorLeft, Console.CursorTop))
            {
                spinner.Start();
                var crmServiceClient = new CrmServiceClient(userName, password, "NorthAmerica", "teamint", false, true, null, true);
                _orgService = crmServiceClient.OrganizationWebProxyClient ?? (IOrganizationService) crmServiceClient.OrganizationServiceProxy;
                spinner.Stop();
            }

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

        /*
        private static IEnumerable<CandidateDyn> ConvertToCandidateDyn(IEnumerable<Candidate> candidates)
        {
            var list = new List<CandidateDyn>();
            candidates.Where(i=>i.FullName == "Syrovenko Andriy").ToList().ForEach(candidate =>
            {
                var candidateDyn = new CandidateDyn(candidate);
                list.Add(candidateDyn);
            });
            return list;
        }
        */
        
        private static IEnumerable<T> ReadXmlFolder<T>(string path) where T : class
        {
            var collection = new List<T>();
            var fileNames = Directory.GetFiles(path, "*.xml", SearchOption.AllDirectories).ToList();
            var ser = new XmlSerializer(typeof(T));
            fileNames.ForEach(name =>
            {
                var obj = ser.Deserialize(new FileStream(name, FileMode.Open)) as T;
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
            });
            return collection;
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
            var fileNames = Directory.GetFiles(srcFolderName, "*", SearchOption.AllDirectories).ToList();
            fileNames.ForEach(name =>
            {
                var atts = candidate.Attachments.Where(a => a.Text != null).ToList();
                atts.ForEach(att => { att.Text.Value = File.ReadAllLines(name); });
                atts = candidate.Attachments.Where(a => a.Data != null).ToList();
                atts.ForEach(att => { att.Data.Value = File.ReadAllBytes(name); });
            });
            return candidate;
        }
    }

    public class Spinner : IDisposable
    {
        private const string Sequence = @"/-\|";
        private int counter = 0;
        private readonly int left;
        private readonly int top;
        private readonly int delay;
        private bool active;
        private readonly Thread thread;

        public Spinner(int left, int top, int delay = 100)
        {
            this.left = left;
            this.top = top;
            this.delay = delay;
            thread = new Thread(Spin);
        }

        public void Start()
        {
            active = true;
            if (!thread.IsAlive)
                thread.Start();
        }

        public void Stop()
        {
            active = false;
            Console.Write("\b");
        }

        private void Spin()
        {
            while (active)
            {
                Turn();
                Thread.Sleep(delay);
            }
        }

        private void Draw(char c)
        {
            Console.SetCursorPosition(left, top);
            Console.Write(c);
        }

        private void Turn()
        {
            Draw(Sequence[++counter % Sequence.Length]);
        }

        public void Dispose()
        {
            Stop();
        }
    }
}