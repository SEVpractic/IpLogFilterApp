namespace IpLogFilterApp;

internal static class SD
{
    private static string logPath = "log_file.txt";
    private static string resultPath = "file_result.txt";
    private static string? addressStart;
    private static string? addressMask;
    private static DateTime? timeStart;
    private static DateTime? timeEnd;

    public static string? LogPath => logPath; //--file-log — путь к файлу с логами
    public static string? ResultPath => resultPath; //--file-output — путь к файлу с результатом
    public static string? AddressStart => addressStart; //--address-start —  нижняя граница диапазона адресов, необязательный параметр, по умолчанию обрабатываются все адреса
    public static string? AddressMask => addressMask; //--address-mask — маска подсети, задающая верхнюю границу диапазона десятичное число.Необязательный параметр.В случае, если он не указан, обрабатываются все адреса, начиная с нижней границы диапазона.Параметр нельзя использовать, если не задан address-start
    public static DateTime? TimeStart => timeStart; //--time-start —  нижняя граница временного интервала
    public static DateTime? TimeEnd => timeEnd; //--time-end — верхняя граница временного интервала.

    public static void ConfigureSD()
    {
        ConfigureFromEnvironmentVariable();
        ConfigureFromCommandLine();                   
    }

    /// <summary>
    /// Получение переменных среды
    /// </summary>
    private static void ConfigureFromEnvironmentVariable()
    {
        string? invarLogPath = Environment.GetEnvironmentVariable("--file-log");
        string? invarResultPath = Environment.GetEnvironmentVariable("--file-output");
        string? invarAddressStart = Environment.GetEnvironmentVariable("--address-start");
        string? invarAddressMask = Environment.GetEnvironmentVariable("--address-mask");
        bool invarTimeStartExist = DateTime.TryParse(Environment.GetEnvironmentVariable("--time-start"), out DateTime invarTimeStart);
        bool invarTimeEndExist = DateTime.TryParse(Environment.GetEnvironmentVariable("--time-end"), out DateTime invarTimeEnd);

        if (!string.IsNullOrEmpty(invarLogPath)) logPath = invarLogPath;
        if (!string.IsNullOrEmpty(invarResultPath)) resultPath = invarResultPath;
        if (!string.IsNullOrEmpty(invarAddressStart)) addressStart = invarAddressStart;
        if (!string.IsNullOrEmpty(invarAddressMask)) addressMask = invarAddressMask;
        if (invarTimeStartExist) timeStart = invarTimeStart;
        if (invarTimeEndExist) timeEnd = invarTimeEnd;
    }

    /// <summary>
    /// Получение переменных командной строки
    /// </summary>
    private static void ConfigureFromCommandLine()
    {
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i] == "--file-log") logPath = args[i + 1];
            if (args[i] == "--file-output") resultPath = args[i + 1];
            if (args[i] == "--address-start") addressStart = args[i + 1];
            if (args[i] == "--address-mask") addressMask = args[i + 1];
            if (args[i] == "--time-start")
            {
                if (DateTime.TryParse(args[i + 1], out DateTime result)) timeStart = result;
            }
            if (args[i] == "--time-end")
            {
                if (DateTime.TryParse(args[i + 1], out DateTime result)) timeEnd = result;
            }
        }
    }
}
