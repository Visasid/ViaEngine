using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using ViaEngine.Graphics;

namespace ViaEngine.Localization;

public class Language
{
    private Dictionary<string, string> translations;

    /// <summary>
    /// Creates a new translation.
    /// </summary>
    public void AddTranslation(string name, string translation)
    {
        translations.Add(name, translation);
    }

    public string GetLocalizedString(string name)
    {
        if (translations[name] != null) return translations[name];
        return name;
    }

    /// <summary>
    /// Creates a new language dictionary.
    /// </summary>
    public Language()
    {
        translations = new Dictionary<string, string>();
    }

    /// <summary>
    /// Creates a new language based on a xml configuration file.
    /// </summary>
    /// <param name="fileName">The path to the xml file, relative to the content root directory.</param>
    /// <returns>Translations list created by this method.</returns>
    public static Language FromFile(ContentManager content, string fileName)
    {
        Language lang = new Language();

        string filePath = Path.Combine(content.RootDirectory, fileName);

        using (Stream stream = TitleContainer.OpenStream(filePath))
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {
                XDocument doc = XDocument.Load(reader);
                XElement root = doc.Root;

                // The <Translations> element contains individual <Translation> elements
                //
                // Example:
                // <Translations>
                //      <Translation name="menu.start" trans="Start" />
                //      <Translation name="menu.options" trans="Options" />
                // </Translations>
                //
                // So we retrieve all of the <Translation> elements then loop through each one.
                var regions = root.Element("Translations")?.Elements("Translation");

                if (regions != null)
                {
                    foreach (var region in regions)
                    {
                        string name = region.Attribute("name")?.Value;
                        string trans = region.Attribute("trans")?.Value;

                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(trans)) lang.AddTranslation(name, trans);
                    }
                }

                return lang;
            }
        }
    }
}
