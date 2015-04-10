using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using System.Security.Principal;
using System.Collections;
using System.Drawing;
using System.IO;

namespace IdeasAPI.Helpers
{
    public static class UserHelper
    {
        public static Dictionary<object, object> DisplayUser(IIdentity id)
        {
            var result = new Dictionary<object, object>();

            WindowsIdentity winId = id as WindowsIdentity;
            if (id == null)
            {
                Console.WriteLine("Identity is not a windows identity");
                return result;
            }

            string userInQuestion = winId.Name.Split('\\')[1];
            string myDomain = winId.Name.Split('\\')[0]; // this is the domain that the user is in
            // the account that this program runs in should be authenticated in there                    
            DirectoryEntry entry = new DirectoryEntry("LDAP://" + myDomain);
            DirectorySearcher adSearcher = new DirectorySearcher(entry);

            adSearcher.SearchScope = SearchScope.Subtree;
            adSearcher.Filter = "(&(objectClass=user)(samaccountname=" + userInQuestion + "))";
            SearchResult userObject = adSearcher.FindOne();
            if (userObject != null)
            {
                foreach (DictionaryEntry property in userObject.Properties)
                {
                    var valueCollenction = property.Value as ResultPropertyValueCollection;
                    if (valueCollenction != null)
                        for (int i = 0; i < valueCollenction.Count; i++)
                        {
                            result.Add(string.Format("{0}-{1}", property.Key, i), valueCollenction[i]);
                        }
                }
            }

            return result;
        }

        public static Image Base64ToImage(byte[] base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = base64String;
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }
    }
}