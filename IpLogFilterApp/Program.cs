namespace IpLogFilterApp;
internal class Program
{
    static void Main(string[] args)
    {
        SD.ConfigureSD();
        LogFilter logFilter = new LogFilter();
        logFilter.OperateWithFile();
    }
}