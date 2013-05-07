using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media.Imaging;
using System.IO;

namespace ClientWPF
{
    public class ByteArrayToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            // crée un BitmapImage à partir d'un byte[]
            BitmapImage imageSource = null;
            byte[] array = (byte[])value;
            if (array != null)
            {
                imageSource = new BitmapImage();
                imageSource.BeginInit();
                imageSource.StreamSource = new MemoryStream(array);
                imageSource.EndInit();
            }
            return imageSource;
        }

        public object ConvertBack(object value, Type targetType, object
        parameter, CultureInfo culture)
        {
            throw new Exception("Non implementee");
        }
    }
}