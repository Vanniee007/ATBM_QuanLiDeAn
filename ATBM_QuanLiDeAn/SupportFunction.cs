using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ATBM_QuanLiDeAn
{
    internal class SupportFunction
    {
        public static void ShowError(System.Windows.Controls.Label label, string content)
        {
            label.Content = content;
            label.Foreground = Brushes.IndianRed;
            //await Task.Delay(TimeSpan.FromSeconds(3));
            //label.Content = "";
            Task.Delay(TimeSpan.FromSeconds(3)).ContinueWith(t =>
            {
                label.Content = string.Empty;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        public static void ShowSuccess(System.Windows.Controls.Label label, string content)
        {
            label.Content = content;
            label.Foreground = Brushes.Green;
            Task.Delay(TimeSpan.FromSeconds(3)).ContinueWith(t =>
            {
                label.Content = string.Empty;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        public static async Task HideLabelAfterDelay(System.Windows.Controls.Label label, int second)
        {
            await Task.Delay(TimeSpan.FromSeconds(second));
            label.Content = "";
        }
        public static string FormatShortDate(string dateTimeString)
        {
            DateTime dateTime;

            // Using DateTime.Parse()
            dateTime = DateTime.Parse(dateTimeString);

            // Using DateTime.TryParse()
            if (DateTime.TryParse(dateTimeString, out dateTime))
            {
                // Conversion successful
                // Format the DateTime object to date string
                string dateString = dateTime.ToString("dd/MM/yyyy");
                return dateString;
            }
            return dateTimeString;
            
        }
    }
}
