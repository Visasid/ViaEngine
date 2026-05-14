using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace ViaEngine.Localization;

public class LocalizationHandler
{
    private List<Language> languages;
    public int selectedLanguage = 0;

    public LocalizationHandler() 
    {
        languages = new List<Language>();
    }

    /// <summary>
    /// Returns translated string based on language ID.
    /// </summary>
    public string GetTranslatedString(string code)
    {
        return languages[selectedLanguage].GetLocalizedString(code);
    }

    public void AddLanguage(ContentManager content, string fileName)
    {
        Language lang = Language.FromFile(content, fileName);
        if (lang != null && !languages.Contains(lang)) languages.Add(lang);
    }
}
