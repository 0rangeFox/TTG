using System.Xml.Serialization;

namespace TTG_Shared.Utils; 

public static class ConfigurationUtil {

    private const string ConfigurationPath = "config.xml";

    public static T Load<T>(T defaultConfig, string configurationPath = ConfigurationPath) {
        var serializer = new XmlSerializer(typeof(T));

        if (File.Exists(configurationPath)) {
            using var reader = new StreamReader(configurationPath);
            return (T) serializer.Deserialize(reader);
        }

        using var writer = new StreamWriter(configurationPath);
        serializer.Serialize(writer, defaultConfig);
        return defaultConfig;
    }

}