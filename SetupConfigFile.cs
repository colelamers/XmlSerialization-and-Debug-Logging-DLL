using System.IO;
using System.Xml.Serialization;
using System.Reflection;

namespace ProjectLogging
{
    /*
     * Created by Cole Lamers 
     * Date: 2020-12-06
     * 
     * == Purpose ==
     * This code sets up the XML serialization so that any configuration class 
     * created can be easily passed through and referenced directly.    
     * 
     * Changes: (date,  comment)
     * 2020-12-15,  Modified the name of the ConfigClass type to ConfigObject 
     *              for easier comprehension.
     *              Revised some aspects of GetProjectName. Added a TODO because
     *              I will have to further test it.
     *              Added XML comments to functions.
     * 2020-12-21,  Revised GetProjectName function to pass through a generic
     *              type ConfigObject so that it can get the assembly reference
     *              of what is passed through and not of the ProjectLogging 
     *              reference.
     */

    /* 
     * == Global Task List ==
     * TODO: --2-- Need to work on a function that sets the default path/location when saving/generating the file
     * TODO: --4-- rework file naming for GetProjectName()? maybe not go off of project?
     * TODO: --1-- create a function that does not require a reference object and that it can make the xml config file and update to it as items are added to it; likely a modification to the save? might need a self creating class at that point...
     */
    public class SetupConfigFile
    {
        /// <summary>
        /// Saves the file to create it. Then it loads it in to the program.
        /// </summary>
        /// <typeparam name="ConfigObject">This should be the object of data that will be loaded into the config file.</typeparam>
        /// <param name="config">The object type that the configuration will be based on.</param>
        public static void LoadAndSaveFile<ConfigObject>(
            ref ConfigObject config)
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
            if (!File.Exists(Path.GetFullPath(GetProjectName<ConfigObject>())))
            {
                using (StreamWriter sw = File.CreateText(
                    GetProjectName<ConfigObject>())) { }
            }//Creates the config file if it doesn't exist
            using (var stream = new FileStream(GetProjectName<ConfigObject>(), 
                FileMode.Create))
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
            using (var stream = new FileStream(GetProjectName<ConfigObject>(),
                FileMode.Open))
            {
                var XML = new XmlSerializer(typeof(ConfigObject));
                config = (ConfigObject)XML.Deserialize(stream);
            }
        }
        /// <summary>
        /// Returns the full XML file name.
        /// </summary>
        /// <returns></returns>
        public static string GetProjectName<ConfigObject>()
        {//Creates a config file based on the project assembly name
            System.Type configtype = typeof(ConfigObject);
            Assembly assem = Assembly.GetAssembly(configtype);
            return assem.GetName().Name + "_Config.xml";
        }
    }
}
