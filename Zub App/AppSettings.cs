using System;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Collections.Generic;

namespace Zub_App
{
    public class AppSettings
    {
        // Our isolated storage settings
        IsolatedStorageSettings isolatedStore;

        // The isolated storage key names of our settings
        const string checkUploadtoWebKeyName = "checkUploadtoWeb";
        const string ListBoxSettingKeyName = "ListBoxSetting";
        const string orderByDateKeyName = "orderByDate";
        const string orderByNameKeyName = "orderByName";
        const string RadioButton2SettingKeyName = "RadioButton2Setting";
      
        const string UsernameSettingKeyName = "UsernameSetting";
        const string PasswordSettingKeyName = "PasswordSetting";

        // The default value of our settings
        const bool checkUploadtoWebDefault = true;
        const int ListBoxSettingDefault = 0;
        const bool orderByDateSettingDefault = true;
        const bool orderByNameSettingDefault = false;
        
        const string UsernameSettingDefault = "";
        const string PasswordSettingDefault = "";

        /// <summary>
        /// Constructor that gets the application settings.
        /// </summary>
        public AppSettings()
        {
            try
            {
                // Get the settings for this application.
                isolatedStore = IsolatedStorageSettings.ApplicationSettings;

            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception while using IsolatedStorageSettings: " + e.ToString());
            }
        }

        /// <summary>
        /// Update a setting value for our application. If the setting does not
        /// exist, then add the setting.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (isolatedStore.Contains(Key))
            {
                // If the value has changed
                if (isolatedStore[Key] != value)
                {
                    // Store the new value
                    isolatedStore[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                isolatedStore.Add(Key, value);
                valueChanged = true;
            }

            return valueChanged;
        }

        /// <summary>
        /// Get the current value of the setting, or if it is not found, set the 
        /// setting to the default setting.
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="Key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public valueType GetValueOrDefault<valueType>(string Key, valueType defaultValue)
        {
            valueType value;

            // If the key exists, retrieve the value.
            if (isolatedStore.Contains(Key))
            {
                value = (valueType)isolatedStore[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }

            return value;
        }

        /// <summary>
        /// Save the settings.
        /// </summary>
        public void Save()
        {
            isolatedStore.Save();
        }


        /// <summary>
        /// Property to get and set a CheckBox Setting Key.
        /// </summary>
        public bool checkUploadtoWeb
        {
            get
            {
                return GetValueOrDefault<bool>(checkUploadtoWebKeyName, checkUploadtoWebDefault);
            }
            set
            {
                AddOrUpdateValue(checkUploadtoWebKeyName, value);
                Save();
            }
        }


        /// <summary>
        /// Property to get and set a ListBox Setting Key.
        /// </summary>
        public int ListBoxSetting
        {
            get
            {
                return GetValueOrDefault<int>(ListBoxSettingKeyName, ListBoxSettingDefault);
            }
            set
            {
                AddOrUpdateValue(ListBoxSettingKeyName, value);
                Save();
            }
        }


        /// <summary>
        /// Property to get and set a RadioButton Setting Key.
        /// </summary>
        public bool orderByDate
        {
            get
            {
                return GetValueOrDefault<bool>(orderByDateKeyName, orderByDateSettingDefault);
            }
            set
            {
                AddOrUpdateValue(orderByDateKeyName, value);
                Save();
            }
        }


        /// <summary>
        /// Property to get and set a RadioButton Setting Key.
        /// </summary>
        public bool orderByName
        {
            get
            {
                return GetValueOrDefault<bool>(orderByNameKeyName, orderByNameSettingDefault);
            }
            set
            {
                AddOrUpdateValue(orderByNameKeyName, value);
                Save();
            }
        }

        /// <summary>
        /// Property to get and set a RadioButton Setting Key.
        /// </summary>
        

        /// <summary>
        /// Property to get and set a Username Setting Key.
        /// </summary>
        public string UsernameSetting
        {
            get
            {
                return GetValueOrDefault<string>(UsernameSettingKeyName, UsernameSettingDefault);
            }
            set
            {
                AddOrUpdateValue(UsernameSettingKeyName, value);
                Save();
            }
        }

        /// <summary>
        /// Property to get and set a Password Setting Key.
        /// </summary>
        public string PasswordSetting
        {
            get
            {
                return GetValueOrDefault<string>(PasswordSettingKeyName, PasswordSettingDefault);
            }
            set
            {
                AddOrUpdateValue(PasswordSettingKeyName, value);
                Save();
            }
        }
    }
}
