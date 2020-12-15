using System.IO;
using System.Xml.Serialization;

namespace ProjectLogging
{
    /*
     * Created by Cole Lamers 
     * Date: 2020-12-06
     * 
     * == Purpose ==
     * This code sets up the XML serialization so that any configuration class created
     * can be easily passed through and referenced directly.    
     * 
     * Changes: (date,  comment)
     * 2020-12-15,  Modified the name of the ConfigClass type to ConfigObject for easier comprehension.
     *              Revised some aspects of GetProjectName. Added a TODO because I will have to further test it.
     *              Added XML comments to functions.
     */

    /* 
     * == Global Task List ==
     * TODO: --2-- Need to work on a function that sets the default path/location when saving/generating the file
     * TODO: --4-- rework file naming for GetProjectName()? maybe not go off of project?
     */
    public class SetupConfigFile
    {
        /// <summary>
        /// Saves the file to create it. Then it loads it in to the program.
        /// </summary>
        /// <typeparam name="ConfigObject">This should be the object of data that will be loaded into the config file.</typeparam>
        /// <param name="config">The object type that the configuration will be based on.</param>
        public static void LoadAndSaveFile<ConfigObject>(ref ConfigObject config)
        {//Combines both actions of save and load so both do not need to be called when program is initially run
            //TODO: --1-- think of a better way to make this
            SaveToFile(ref config);
            LoadFromFile(ref config);
        }
        /// <summary>
        /// Detects if the specified file exists. It will create and/or serialize it in XML.
        /// </summary>
        /// <typeparam name="ConfigObject"></typeparam>
        /// <param name="config">The type of object passed through.</param>
        public static void SaveToFile<ConfigObject>(ref ConfigObject config)
        {//Passes through a refernce to a class, generates the serializer that way
            if (!File.Exists(Path.GetFullPath(GetProjectName())))
            {
                using (StreamWriter sw = File.CreateText(GetProjectName())) { }
            }//Creates the config file if it doesn't exist
            using (var stream = new FileStream(GetProjectName(), FileMode.Create))
            {
                XmlSerializer XML = new XmlSerializer(typeof(ConfigObject));
                XML.Serialize(stream, config);
            }//Serializes the object type passed through
        }
        /// <summary>
        /// Loads in an already existing XML file.
        /// </summary>
        /// <typeparam name="ConfigObject"></typeparam>
        /// <param name="config">The type of object passed through.</param>
        public static void LoadFromFile<ConfigObject>(ref ConfigObject config)
        {//Loads in the existing referenced config file
            using (var stream = new FileStream(GetProjectName(), FileMode.Open))
            {
                var XML = new XmlSerializer(typeof(ConfigObject));
                config = (ConfigObject)XML.Deserialize(stream);
            }
        }
        /// <summary>
        /// Returns the full XML file name.
        /// </summary>
        /// <returns></returns>
        public static string GetProjectName()
        {//Creates a config file based on the project assembly name
            string xmlFileName = System.Reflection.Assembly.GetCallingAssembly().GetName().Name;
            //TODO: --1-- This is going to require some testing. I may have screwed up the _Config.xml naming part. Looks like to me it'll constantly get tacked on. See if what I now made works or needs to be resolved. Originally it just contained the else statement.
            if (xmlFileName.Contains("_Config.xml"))
                return xmlFileName;
            else
                return System.Reflection.Assembly.GetCallingAssembly().GetName().Name + "_Config.xml";
        }
    }
}
