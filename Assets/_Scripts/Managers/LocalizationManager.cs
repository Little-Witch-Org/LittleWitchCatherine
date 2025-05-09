using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace _Scripts.Managers
{
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance;

        private void Awake()
        {


            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

            }
            else
            {
                Destroy(gameObject);
            }

        }

        public Locale GetCurrentLocale()
        {
            return LocalizationSettings.SelectedLocale;
        }

        public string GetCurrentLanguageCode()
        {
            return LocalizationSettings.SelectedLocale.Identifier.Code;
        }
        
        public void SetLanguage(string languageCode)
        {
            var availableLocales = LocalizationSettings.AvailableLocales;
    
            foreach (var locale in availableLocales.Locales)
            {
                if (locale.Identifier.Code == languageCode)
                {
                    LocalizationSettings.SelectedLocale = locale;
                    PlayerPrefs.SetString("SelectedLanguage", languageCode); // Сохраняем выбор
                    PlayerPrefs.Save();
                    break;
                }
            }
        }
        
        //todo for ui
        /*public class LanguageSettings : MonoBehaviour
        {
            public Dropdown languageDropdown;

            private void Start()
            {
                // Заполняем Dropdown доступными языками
                var locales = LocalizationSettings.AvailableLocales.Locales;
                languageDropdown.ClearOptions();
        
                foreach (var locale in locales)
                {
                    languageDropdown.options.Add(new Dropdown.OptionData(locale.name));
                }

                // Восстанавливаем сохранённый язык
                string savedLanguage = PlayerPrefs.GetString("SelectedLanguage", "en");
                SetLanguage(savedLanguage);

                // Назначаем обработчик изменения выбора
                languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
            }

            private void OnLanguageChanged(int index)
            {
                var selectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
                SetLanguage(selectedLocale.Identifier.Code);
            }

            private void SetLanguage(string languageCode)
            {
                // ... (код из примера выше)
            }
        }*/
    }
}