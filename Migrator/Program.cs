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
using Microsoft.Xrm.Tooling.Connector;

namespace Migrator
{
    internal static class Program
    {
        private static IEnumerable<Candidate> _candidates;
        private const string ImportDataPathBase = @"d:\EStaff_Server\data_rcr\obj\";
        private static IOrganizationService _orgService;

        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // _candidates = ReadXmlFolder<Candidate>($@"{ImportDataPathBase}candidates\");
            // var candidateDyn = ConvertToCandidateDyn(_candidates);

            var isConnected = ConnectToCrm();
        }

        private static bool ConnectToCrm()
        {
            // const string connectionString = @"Url=https://teamint.crm.dynamics.com/XRMServices/2011/Organization.svc; Username=TEAM\smirnoff; Password=; authtype=IFD";
            // var conn = new CrmServiceClient(connectionString);
            // var internalUrl = "https://teamint.crm.dynamics.com/XRMServices/2011/Organization.svc";
            /*const string internalUrl = "https://teamint.crm.dynamics.com";
            const string orgName = "teamint";
            const string userName = "Sergey.Smirnoff@teaminternational.com";
            var password = ReadPassword();
            var credentials = new NetworkCredential(userName, password);
            const AuthenticationType authType = AuthenticationType.Office365;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var conn = new CrmServiceClient(credentials, authType, internalUrl, "443", orgName, true, true, null);
            return conn.IsReady;*/
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var password = ReadPassword();
            var crmServiceClient =
                new CrmServiceClient($"AuthType=Office365;Username=Sergey.Smirnoff@teaminternational.com; Password={password};Url=https://teamint.crm.dynamics.com");
            _orgService = (IOrganizationService) crmServiceClient.OrganizationWebProxyClient != null
                ? (IOrganizationService) crmServiceClient.OrganizationWebProxyClient
                : (IOrganizationService) crmServiceClient.OrganizationServiceProxy;
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

        private static string ReadPassword()
        {
            var password = new SecureString();
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

            password.MakeReadOnly();
            var credentials = new NetworkCredential("", password);
            return credentials.Password;
        }

        private static IEnumerable<CandidateDyn> ConvertToCandidateDyn(IEnumerable<Candidate> candidates)
        {
            throw new NotImplementedException();
        }

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
}