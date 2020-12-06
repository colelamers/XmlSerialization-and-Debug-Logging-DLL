using System.IO;
using System.Xml.Serialization;

namespace ProjectLogging
{
    public class SetupConfigFile
    {
        /*
         * Created by Cole Lamers 
         * Date: 2020-12-06
         * 
         * == Purpose ==
         * This code sets up the XML serialization so that any configuration class created
         * can be easily passed through and referenced directly.    
         * 
         * Changes: (date, comment)
         * 
         */

        /* 
         * == Global Task List ==
         * TODO: --2-- Need to work on a function that sets the default path/location when saving/generating the file
         * TODO: --4-- rework file naming for GetProjectName()? maybe not go off of project? not a priority or major.
         */

        public static void LoadSaveFile<ConfigClass>(ref ConfigClass config)
        {
            //TODO: --1-- think of a better way to make this
            SaveToFile(ref config);
            LoadFromFile(ref config);
        }//Combines both actions of save and load so both do not need to be called when program is initially run

        public static void SaveToFile<ConfigClass>(ref ConfigClass config)
        {
            if (!File.Exists(Path.GetFullPath(GetProjectName())))
            {
                using (StreamWriter sw = File.CreateText(GetProjectName())) { }
            }//Creates the config file if it doesn't exist
            using (var stream = new FileStream(GetProjectName(), FileMode.Create))
            {
                XmlSerializer XML = new XmlSerializer(typeof(ConfigClass));
                XML.Serialize(stream, config);
            }//Serializes the object type passed through
        }//Passes through a refernce to a class, generates the serializer that way

        public static void LoadFromFile<ConfigClass>(ref ConfigClass config)
        {
            using (var stream = new FileStream(GetProjectName(), FileMode.Open))
            {
                var XML = new XmlSerializer(typeof(ConfigClass));
                config = (ConfigClass)XML.Deserialize(stream);
            }
        }//Loads in the existing referenced config file

        public static string GetProjectName()
        {
            return System.Reflection.Assembly.GetCallingAssembly().GetName().Name + "_Config.xml";
        }//Creates a config file based on the project assembly name
    }
}
