using System;
using System.DirectoryServices;

namespace IdeasAPI.Helpers
{
    public static class SearchResultExtensions
    {
        public static string GetString(this SearchResult userObject, string propertyName)
        {
            return userObject.GetProperty(propertyName)?.ToString();
        }

        public static byte[] GetByteArray(this SearchResult userObject, string propertyName)
        {
            return userObject.GetProperty(propertyName) as byte[];
        }


        public static DateTime? GetDateTime(this SearchResult userObject, string propertyName)
        {
            var dateString = userObject.GetString(propertyName);
            if (dateString == null)
            {
                return null;
            }
            DateTime dateTime;
            if(!DateTime.TryParse(dateString, out dateTime))
            {
                return null;
            }
            return dateTime;
        }

        public static object GetProperty(this SearchResult userObject, string propertyName)
        {
            if (!userObject.Properties.Contains(propertyName))
            {
                return null;
            }
            var collection = userObject.Properties[propertyName];
            if (collection == null || collection.Count == 0)
            {
                return null;
            }
            return collection[0];
        }
    }
}