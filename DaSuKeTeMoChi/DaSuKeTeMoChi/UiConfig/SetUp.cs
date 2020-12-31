using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using UtilityClassDLL;

namespace DaSuKeTeMoChi.UiConfig
{
    public class SetUp
    {

        public void CallSettings(object _settings)
        {
            string[] SettingValues = File.ReadAllLines(@"Settings.txt");
            for (int i = 0; i < SettingValues.Length; i++)
            {
                string[] values = StrCut.ArrSplit(SettingValues[i], "=");
                Type ts = _settings.GetType().GetProperty(values[0]).PropertyType;
                _settings.GetType().GetProperty(values[0]).SetValue(_settings, Convert.ChangeType(values[1], ts), null);
            }
        }

        public Tuple<SolidColorBrush,Color,int> SetUpSettings(object _settings)
        {
            SolidColorBrush TextColor = (SolidColorBrush)new BrushConverter().ConvertFrom((string)_settings.GetType().GetProperty("TextColor").GetValue(_settings, null));
            Color FieldColor = (Color)ColorConverter.ConvertFromString((string)_settings.GetType().GetProperty("FieldColor").GetValue(_settings, null));
            int SliderValue = Convert.ToInt32(_settings.GetType().GetProperty("SliderValue").GetValue(_settings, null));

            return Tuple.Create(TextColor, FieldColor, SliderValue);
        }
    }
}
