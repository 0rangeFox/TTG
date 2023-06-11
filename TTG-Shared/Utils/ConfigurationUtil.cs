using System.Xml.Serialization;

namespace TTG_Shared.Utils; 

public static class ConfigurationUtil {

    private const string ConfigurationPath = "config.xml";

    public static void Save<T>(T config, string configurationPath = ConfigurationPath) {
        using var writer = new StreamWriter(configurationPath);
        new XmlSerializer(typeof(T)).Serialize(writer, config);
    }

    public static T Load<T>(T defaultConfig, string configurationPath = ConfigurationPath) {
        if (File.Exists(configurationPath)) {
            using var reader = new StreamReader(configurationPath);
            return (T) new XmlSerializer(typeof(T)).Deserialize(reader);
        }

        Save(defaultConfig);
        return defaultConfig;
    }

}