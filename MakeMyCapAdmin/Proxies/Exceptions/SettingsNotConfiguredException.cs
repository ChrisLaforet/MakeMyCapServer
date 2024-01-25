namespace MakeMyCapAdmin.Proxies.Exceptions;

public class SettingsNotConfiguredException : Exception
{
	public SettingsNotConfiguredException() : base("Settings are not configured") { }
}