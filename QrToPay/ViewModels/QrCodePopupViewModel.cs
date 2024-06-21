using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace QrToPay.ViewModels
{
    public partial class QrCodePopupViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ImageSource qrCodeImage;

        public QrCodePopupViewModel(ImageSource qrCodeImage)
        {
            QrCodeImage = qrCodeImage;
        }
    }
}
