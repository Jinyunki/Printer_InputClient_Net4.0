using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Controls;

namespace Printer_InputClient_Net4._0.View
{
    /// <summary>
    /// DataListView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DataListView : UserControl
    {
        public DataListView()
        {
            InitializeComponent();
            Messenger.Default.Register<FocusMessage>(this, (message) =>
            {
                // 포커스를 변경할 로직을 여기에 작성합니다.
                PrintCountView.Focus();
            });
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }
    }

    public class FocusMessage
    {
        
    }
}
