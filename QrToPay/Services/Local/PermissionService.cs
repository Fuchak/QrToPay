using Microsoft.Maui.ApplicationModel;
using Plugin.LocalNotification;

namespace QrToPay.Services.Local;
public class PermissionService
{
    public static async Task<bool> RequestPermissionAsync<TPermission>(string permissionName) where TPermission : Permissions.BasePermission, new()
    {
        var status = await CheckPermissions<TPermission>();

        if (IsGranted(status))
        {
            return true;
        }
        else
        {
            bool goToSettings = await Shell.Current.DisplayAlert("Błąd",
                    $"Nie przyznano uprawnień do {permissionName}. Niektóre funkcje systemu nie będą dostępne.",
                    "Ustawienia",
                    "OK");

            if (goToSettings)
            {
                AppInfo.Current.ShowSettingsUI();
            }
            return false;
        }
    }

    private static async Task<PermissionStatus> CheckPermissions<TPermission>() where TPermission : Permissions.BasePermission, new()
    {
        var status = await Permissions.CheckStatusAsync<TPermission>();

        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<TPermission>();
        }
        return status;
    }

    private static bool IsGranted(PermissionStatus status)
    {
        return status == PermissionStatus.Granted;
    }
}