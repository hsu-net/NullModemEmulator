using Microsoft.Win32;

namespace Hsu.NullModemEmulator;

internal class RegistryHelper
{
    private const string NullModemEmulatorName = "com0com";
    private const string REGISTRY_KEY = $@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{NullModemEmulatorName}";
    private const string REGISTRY_KEY_32 = $@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{NullModemEmulatorName}";

    private static string GetInstallPath(string key)
    {
        using RegistryKey com = Registry.LocalMachine.OpenSubKey(key);
        if (com == null) return null;
        return com.GetValue("InstallLocation")?.ToString();
    }

    public static string GetInstallPath()
    {
        return GetInstallPath(REGISTRY_KEY) ?? GetInstallPath(REGISTRY_KEY_32);
    }
}