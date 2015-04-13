using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ideas.BusinessLogic.Configurations
{
    public class IdeasGlobalSettings : ConfigurationSection
    {
        [ConfigurationProperty("moderatorCollection", Options = ConfigurationPropertyOptions.IsRequired)]
        public ModeratorCollection ModeratorCollection
        {
            get
            {
                return (ModeratorCollection)this["moderatorCollection"];
            }
        }

        [ConfigurationProperty("domain", DefaultValue = "WEBMEDIA", IsRequired = true)]
        public string Domain
        {
            get { return (string)this["domain"]; }
            set { this["domain"] = value; }
        }
    }

    [ConfigurationCollection(typeof(ModeratorElement), AddItemName = "moderator", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ModeratorCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ModeratorElement();
        }

        protected override object GetElementKey(ConfigurationElement moderator)
        {
            if (moderator == null)
                throw new ArgumentNullException("moderator");

            return ((ModeratorElement)moderator).Username;
        }
    }

    public class ModeratorElement : ConfigurationElement
    {
        [ConfigurationProperty("username", IsRequired = true, IsKey = true)]
        public string Username
        {
            get
            {
                return (string)base["username"];
            }
        }

        [ConfigurationProperty("su", DefaultValue = false, IsRequired = true)]
        public bool IsSuperUser
        {
            get
            {
                return (bool)base["su"];
            }
        }
    }
}
