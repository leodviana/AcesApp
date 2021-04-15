using System;
using System.Globalization;
using Xamarin.Forms;

namespace AcesApp.Converters
{
    public class CategoriaConverter : IValueConverter
    {
        

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string param = $"{value}";
            if (param.Equals("1"))
            {
                return "Ranking Masculino";
            }
            else if(param.Equals("2"))
            {
                return "Ranking Feminino";
            }
            else if(param.Equals("3"))
            {
                return "Ranking Dupla";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
