using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace MediaVF.CodeSync
{
    public class StatusImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            SyncStatus status = (SyncStatus)value;

            string imagePath = string.Empty;
            switch (status)
            {
                case SyncStatus.Stopped:
                    imagePath = "Icons/deny.png";
                    break;
                case SyncStatus.InProgress:
                    imagePath = "Icons/sync.png";
                    break;
                case SyncStatus.New:
                case SyncStatus.Completed:
                    imagePath = "Icons/accept.png";
                    break;
                case SyncStatus.Failed:
                    imagePath = "Icons/alert.png";
                    break;
            }

            return imagePath;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
