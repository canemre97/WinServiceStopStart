using System;
using System.ServiceProcess;
using System.Timers;

namespace WinServiceStopStart
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string serviceName = "ServiceName";
            while (true)
            {
                DateTime now = DateTime.Now;
                DateTime targetTime = new DateTime(now.Year, now.Month, now.Day, 05, 00, 0);

                if (now > targetTime)
                {
                    targetTime = targetTime.AddDays(1);
                }

                int initialDelay = (int) (targetTime - now).TotalMilliseconds;

                // Timer'ı ayarla
                Timer timer = new Timer();
                timer.Elapsed += (sender, e) => StopAndStartWindowsService(serviceName);
                timer.AutoReset = false; // Sadece bir kez çalışmasını sağlar
                timer.Interval = initialDelay; // Başlangıçta ilk çalışma süresini ayarlar
                timer.Start();

                Console.WriteLine(
                    $"Windows hizmeti ({serviceName}) her gün saat 05:00'da durdurulup yeniden başlatılacak.");

                while (timer.Enabled)
                {
                    // Uygulamayı sonsuz döngüde tutar ve zamanı bekler
                }
            }
        }

        private static void StopAndStartWindowsService(string serviceName)
        {
            try
            {
                ServiceController serviceController = new ServiceController(serviceName);

                // Hizmeti durdur
                if (serviceController.Status == ServiceControllerStatus.Running)
                {
                    serviceController.Stop();
                    serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                    Console.WriteLine("Windows hizmeti başarıyla durduruldu.");
                }

                // Hizmeti başlat
                if (serviceController.Status == ServiceControllerStatus.Stopped)
                {
                    serviceController.Start();
                    serviceController.WaitForStatus(ServiceControllerStatus.Running);
                    Console.WriteLine("Windows hizmeti başarıyla başlatıldı.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata oluştu: " + ex.Message);
            }
        }
    }
}