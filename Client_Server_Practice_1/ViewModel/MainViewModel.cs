using Client_Server_Practice_1.Communication;
using Client_Server_Practice_1.TheClient;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;

namespace Client_Server_Practice_1.ViewModel
{

    public class MainViewModel : ViewModelBase
    {

        public RelayCommand ConnectServer { get; set; }
        public RelayCommand ConnectClient { get; set; }
        public RelayCommand SendBtnClicked { get; set; }
        public string Message { get; set; }
        public ObservableCollection<string> MessageList { get; set; }

        public bool isServer = false;
        public bool isClient = false;

        private int port = 8055;

        Server server;
        Client client;

        public MainViewModel()
        {
            if (IsInDesignMode)
            {

            }
            else
            {
            ConnectServer = new RelayCommand(()=> 
            {
                server = new Server(port, UpdateGuiNewMessage);
                isServer = true;
                
            });
            ConnectClient = new RelayCommand(() => 
            {
                client = new Client(port, UpdateGuiNewMessage);
                isClient = true;
            });
            SendBtnClicked = new RelayCommand(() => 
            {
                if (isClient)
                {
                    client.Send(Message);
                }
                else
                {

                server.Send(Message);
                }
            });
            MessageList = new ObservableCollection<string>();
            }
        }

        public void UpdateGuiNewMessage(string message)
        {
            App.Current.Dispatcher.Invoke(() => 
            {
                MessageList.Add(message);
            });
        }
    }
}