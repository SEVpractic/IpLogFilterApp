using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IpLogFilterApp
{
    internal class LogFilter
    {
        /// <summary>
        /// Обработка полученного файла логов
        /// </summary>
        /// <returns></returns>
        public async Task<bool> OperateWithFile()
        {
            var ips = await ReadLogAsync();
            await WriteResultFileAsync(ips);

            return true;
        }


        /// <summary>
        /// Чтение файла логов, парсинг информации
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        private async Task<Dictionary<IPAddress, long>> ReadLogAsync()
        {
            Dictionary<IPAddress, long> ips = new Dictionary<IPAddress, long>();

            if (!File.Exists(SD.LogPath)) throw new FileNotFoundException($"Файл с логами не обнаружен по указанному пути: {SD.LogPath}");

            using (var stream = File.OpenRead(SD.LogPath))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    while (!streamReader.EndOfStream)
                    {
                        await ReadLineAsync(ips, streamReader);
                    }
                }
            }

            return ips;
        }


        /// <summary>
        /// Чтение линии файла логов, фильтрация
        /// </summary>
        /// <param name="ips"></param>
        /// <param name="streamReader"></param>
        /// <returns></returns>
        private async Task<bool> ReadLineAsync(Dictionary<IPAddress, long> ips, StreamReader streamReader)
        {
            var line = await streamReader.ReadLineAsync();
            if (string.IsNullOrEmpty(line)) return false;

            if (IPAddress.TryParse(line.Split(':')[0].Trim(), out IPAddress iPAddress)
                && DateTime.TryParse(line.Substring(line.IndexOf(':') + 1).Trim(), out DateTime time))
            {
                if (SD.TimeStart >= time || SD.TimeEnd <= time) return false; //проверка временного промежутка
                //TODO добавь фильт по IP

                if (!ips.ContainsKey(iPAddress))
                {
                    ips.Add(iPAddress, 1);
                }
                else
                {
                    ips[iPAddress]++;
                }
            }

            return true;
        }


        /// <summary>
        /// Запись файла - отчета
        /// </summary>
        /// <param name="ips"></param>
        private async Task<bool> WriteResultFileAsync(Dictionary<IPAddress, long> ips)
        {
            string filePath = SD.ResultPath ?? throw new ArgumentNullException("Не передана переменная пути для файла - отчета");

            using (StreamWriter sw = File.CreateText(filePath))
            {
                foreach (var ip in ips)
                {
                    await sw.WriteLineAsync($"{ip.Key} - {ip.Value}");
                }
            }

            return true;
        }
    }
}
