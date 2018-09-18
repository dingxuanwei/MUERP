using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MU.Extensions
{
    /// <summary>
    /// 读写 app.config(*.exe.config) 文件中的 appSettings 段
    /// </summary>
    public static class AppConfig
    {
        /// <summary>
        /// 获取Key指定的Value，如果不存在Key，返回String.Empty
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string key)
        {
            try
            {
                ConfigurationManager.RefreshSection("appSettings");
                string value = ConfigurationManager.AppSettings[key].ToString();
                return value == null ? string.Empty : value;
            }
            catch 
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 设置 Key, Value，如果Key存在则覆盖。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (!string.IsNullOrEmpty(Get(key)))
            {
                config.AppSettings.Settings.Remove(key);
            }
            config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 追加Key所指定的Value值。如果Key不存在则添加。追加的格式为 OldValue += "," + NewValue;
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Append(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
