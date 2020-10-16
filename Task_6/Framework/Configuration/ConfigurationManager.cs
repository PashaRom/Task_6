using System;
using Test.Framework.Logging;
namespace Test.Framework.Configuration
{
    public class ConfigurationManager
    {
        public static ConfigurationGetter Configuration;
        public static ConfigurationGetter TestData;
        static ConfigurationManager() 
        {
            try { 
                Configuration = new ConfigurationGetter(ConfigurationData.TestCofigurationFileName);            
                TestData = new ConfigurationGetter(ConfigurationData.TestDataFileName);
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Unexpected error occurred during creating ConfigurationManager.");
            }
        }        
    }   
}
