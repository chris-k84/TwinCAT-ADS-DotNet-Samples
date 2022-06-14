using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TwinCAT;
using TwinCAT.Ads;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;

namespace WpfAdsSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public myDataCapture myAdsLogger { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            myAdsLogger = new myDataCapture();
            DataContext = myAdsLogger;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            myAdsLogger.Initialise();
        }
    }

    public class myDataCapture : IDisposable
    {
        AdsSession session;
        AmsAddress address = new AmsAddress("10.0.2.15.1.1", 851);
        SessionSettings settings = SessionSettings.Default;
        StateInfo Info;
        DeviceInfo deviceInfo;
        AdsConnection connection;
        public List<string> Symbols = new List<string>();
        public int eventCall = 0;
        ISymbolLoader loader;
        public List<int> data = new List<int>();
        public myDataCapture()
        {
            session = new AdsSession(address, settings);
        }
        public void Initialise()
        {
            OpenConnection();
            GetSymbols();
        }
        void OpenConnection()
        {
            Info = new StateInfo();

            connection = (AdsConnection)session.Connect();

            Info = connection.ReadState();
            deviceInfo = connection.ReadDeviceInfo();
        }
        void GetSymbols()
        {
            SymbolLoaderSettings loadersettings = new SymbolLoaderSettings(SymbolsLoadMode.Flat);
            loader = SymbolLoaderFactory.Create(connection, loadersettings);

            foreach (Symbol s in loader.Symbols)
            {
                Symbols.Add(s.InstancePath);
            }
        }
        public void AddEvent(string VarPath)
        {
            Symbol symbol = (Symbol)loader.Symbols[VarPath];
            symbol.NotificationSettings = new NotificationSettings(AdsTransMode.OnChange, 1, 0);
            symbol.ValueChanged += On_SymbolChange;
        }
        public void RemoveEvent(string VarPath)
        {
            Symbol symbol = (Symbol)loader.Symbols[VarPath];

            symbol.ValueChanged -= On_SymbolChange;
        }
        void On_SymbolChange(object sender, ValueChangedEventArgs e)
        {
            //Console.WriteLine("The Var " + e.Symbol + " has value " + e.Value);
            //Console.WriteLine(sender.ToString());
            data.Add(Convert.ToInt16(e.Value));
            eventCall++;
        }

        public void Dispose()
        {
            session.Dispose();
        }
    }
}
