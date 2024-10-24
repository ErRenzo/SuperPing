using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using System;

namespace SuperPingAguayo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // Inizziazione del MainWindow e i suoi componenti

        public MainWindow()
        {
            InitializeComponent();
            CreazioneDinamicaXaml();
        }

        private void CreazioneDinamicaXaml()
        {
            StackPanel stackPanel = new StackPanel();

            // Primo Grid

            Grid gridPing = new Grid();
            gridPing.Background = System.Windows.Media.Brushes.AliceBlue;
            gridPing.Height = 400;

            for (int i = 0; i < 10; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(2, GridUnitType.Star);
                gridPing.RowDefinitions.Add(rowDefinition);
            }
            for (int i = 0; i < 4; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                columnDefinition.Width = new GridLength(5, GridUnitType.Star);
                gridPing.ColumnDefinitions.Add(columnDefinition);
            }

            Label netTest = new Label();
            netTest.Content = "Network Test";
            netTest.FontWeight = FontWeights.Bold;
            netTest.FontSize = 20;
            Grid.SetRow(netTest, 0);
            Grid.SetColumn(netTest, 0);
            netTest.VerticalAlignment = VerticalAlignment.Center;
            gridPing.Children.Add(netTest);

            Label HostIp = new Label();
            HostIp.Content = "Host-IP";
            HostIp.FontSize = 20;
            Grid.SetRow(HostIp, 0);
            Grid.SetColumn(HostIp, 1);
            netTest.VerticalAlignment = VerticalAlignment.Center;
            gridPing.Children.Add(HostIp);

            Label Status = new Label();
            Status.Content = "Status";
            Status.FontSize = 20;
            Grid.SetRow(Status, 0);
            Grid.SetColumn(Status, 2);
            Status.HorizontalAlignment = HorizontalAlignment.Center;
            gridPing.Children.Add(Status);

            Label Latency = new Label();
            Latency.Content = "Latency";
            Latency.FontSize = 20;
            Grid.SetRow(Latency, 0);
            Grid.SetColumn(Latency, 3);
            Latency.HorizontalAlignment = HorizontalAlignment.Center;
            gridPing.Children.Add(Latency);

            int rows = 9;
            images = new Image[rows];
            indirizziBox = new TextBox[rows];
            status = new Label[rows];
            ms = new Label[rows];

            for (int i = 1; i < 10; i++)
            {
                Image image = new Image
                {
                    Name = "StatIcon" + (i - 1),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Stretch = Stretch.Fill,
                    Margin = new Thickness(40,0,40,0)
                };
                Grid.SetRow(image, i);
                Grid.SetColumn(image, 0);
                gridPing.Children.Add(image);
                images[i - 1] = image;

                TextBox txtIP = new TextBox
                {
                    Name = "txtIP" + (i - 1),
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 0, 10, 0),
                    FontSize = 24,
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };
                Grid.SetRow(txtIP, i);
                Grid.SetColumn(txtIP, 1);
                gridPing.Children.Add(txtIP);
                indirizziBox[i - 1] = txtIP;

                Label lblStat = new Label
                {
                    Name = "lblStatus0" + (i - 1),
                    HorizontalAlignment = HorizontalAlignment.Center                  
                };
                lblStat.FontSize = 25;
                Grid.SetRow(lblStat, i);
                Grid.SetColumn(lblStat, 2);
                gridPing.Children.Add(lblStat);
                status[i - 1] = lblStat;

                Label lblMs = new Label
                {
                    Name = "lblMs0" + (i - 1),
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                lblMs.FontSize = 25;
                Grid.SetRow(lblMs, i);
                Grid.SetColumn(lblMs, 3);
                gridPing.Children.Add(lblMs);
                ms[i - 1] = lblMs;
            }

            //Secondo Grid

            Grid gridBtn = new Grid();

            for (int i = 0; i < 2; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = GridLength.Auto;
                gridBtn.RowDefinitions.Add(rowDefinition);
            }
            for (int i = 0; i < 3; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                if (i == 2)
                {
                    columnDefinition.Width = new GridLength(1, GridUnitType.Star);
                    gridBtn.ColumnDefinitions.Add(columnDefinition);
                }
                else
                {
                    columnDefinition.Width = new GridLength(3, GridUnitType.Star);
                    gridBtn.ColumnDefinitions.Add(columnDefinition);
                }
            }
            Button btnPingIstantaneo = new Button
            {
                Name = "btnPingIstantaneo",
                Content = "Ping Istantaneo",
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            btnPingIstantaneo.Click += btnPingIstantaneo_Click;
            Grid.SetRow(btnPingIstantaneo, 0);
            Grid.SetColumn(btnPingIstantaneo, 0);
            gridBtn.Children.Add(btnPingIstantaneo);

            Button btnPerTempo = new Button
            {
                Name = "btnPerTempo",
                Content = "Ping a Ripetizione (60s)",
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            btnPerTempo.Click += btnPerTempo_Click;
            Grid.SetRow(btnPerTempo, 0);
            Grid.SetColumn(btnPerTempo, 1);
            gridBtn.Children.Add(btnPerTempo);

            Button btnStop = new Button
            {
                Name = "btnStop",
                Content = "STOP",
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            btnStop.Click += btnStop_Click;
            Grid.SetRow(btnStop, 0);
            Grid.SetColumn(btnStop, 2);
            gridBtn.Children.Add(btnStop);

            Button btnSalva = new Button
            {
                Name = "btnSalva",
                Content = "Salva nomi host",
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            btnSalva.Click += btnSalva_Click;
            Grid.SetRow(btnSalva, 1);
            Grid.SetColumn(btnSalva, 0);
            gridBtn.Children.Add(btnSalva);

            Button btnCompila = new Button
            {
                Name = "btnCompila",
                Content = "Compila Host",
                FontSize = 15,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            btnCompila.Click += btnCompila_Click;
            Grid.SetRow(btnCompila, 1);
            Grid.SetColumn(btnCompila, 1);
            gridBtn.Children.Add(btnCompila);

            Border border = new Border
            {
                BorderBrush = System.Windows.Media.Brushes.Black,
                BorderThickness = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(20, 10, 20, 10)
            };
            Grid.SetRow(border, 1);
            Grid.SetColumn(border, 2);

            lblSecCounter = new Label
            {
                Name = "lblSecCounter",
                Content = "0s",
                FontSize = 15,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            border.Child = lblSecCounter;

            gridBtn.Children.Add(border);

            stackPanel.Children.Add(gridPing);
            stackPanel.Children.Add(gridBtn);

            this.Content = stackPanel;
        }

        // Creazione del vettore dei blocchi, e collegamento con delle dichiarazioni di istanza utili per il mantenimento del programma
        // Così da non eseguire ogni volta la creazione dei vettori oggetti mentre il ping è eseguito,
        // e permettere a una istanza di avere le stesse caratteristiche in una sola esecuzione del programma facilitando i collegamenti.

        private Image[] images;
        private Label[] ms;
        private Label[] status;
        private TextBox[] indirizziBox;

        // Individuamento Ping tramite indirizzo IP, visibilità dei contenuti se ricercati
        void StartPing()
        {
            this.Cursor = Cursors.Wait;
            Ping pinger = new Ping();
            for (int i = 0; i < indirizziBox.Length; i++)
            {
                if (indirizziBox[i].Text != "")
                {
                    VisibilityNotNull(i);
                    try
                    {
                        PingReply reply = pinger.Send(indirizziBox[i].Text);
                        string statusOut = reply.Status.ToString();
                        if (statusOut != "TimedOut")
                        {
                            string millisec = reply.RoundtripTime.ToString();
                            ms[i].Content = millisec;
                            status[i].Content = statusOut;
                            StartImageSuccess(i, millisec);
                        }
                        else
                        {
                            status[i].Content = statusOut;
                            StartImageError(i);
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Host non trovato");
                        status[i].Content = "Error";
                        StartImageError(i);
                    }
                }
                else
                {
                    VisibilityNull(i);
                }
            }
            this.Cursor = Cursors.Arrow;
        }

        // Visibilità se inserito qualsiasi testo sui blocchi txt del XAML
        void VisibilityNotNull(int i)
        {
            status[i].Visibility = Visibility.Visible;
            ms[i].Visibility = Visibility.Visible;
            images[i].Visibility = Visibility.Visible;
        }

        // Visibilità se non è stato inserito del testo sui blocchi txt del XAML
        void VisibilityNull(int i)
        {
            status[i].Visibility = Visibility.Collapsed;
            ms[i].Visibility = Visibility.Collapsed;
            images[i].Visibility = Visibility.Collapsed;
        }

        // Gestione errore Ping, senza output di ms
        void StartImageError(int i)
        {
            images[i].Source = new BitmapImage(new Uri("Immagini/Error.jpg", UriKind.Relative));
            ms[i].Visibility = Visibility.Collapsed;
        }

        // Gestione successo Ping, con output di ms
        void StartImageSuccess(int i, string millisec)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            if (int.Parse(millisec) > 50)
            {
                bi.UriSource = new Uri("/Immagini/Warning.png", UriKind.Relative);
            }
            else
            {
                bi.UriSource = new Uri("/Immagini/Success.png", UriKind.Relative);
            }
            bi.EndInit();
            images[i].Source = bi;
        }

        // Ricerca istantanea del Ping
        private void btnPingIstantaneo_Click(object sender, RoutedEventArgs e)
        {
            StartPing();
        }

        // Ricerca a tempo del ping (pingTimer) e contatore secondi (secondTimer e counter)

        private DispatcherTimer secondTimer;
        private DispatcherTimer pingTimer;
        private int counter = 0;
        private Label lblSecCounter;

        private void btnPerTempo_Click(object sender, RoutedEventArgs e)
        {
            secondTimer = new DispatcherTimer();
            secondTimer.Tick += new EventHandler(secondTimer_Tick);
            secondTimer.Interval = TimeSpan.FromSeconds(1);
            secondTimer.Start();

            pingTimer = new DispatcherTimer();
            pingTimer.Tick += new EventHandler(pingTimer_Tick);
            pingTimer.Interval = TimeSpan.FromSeconds(60);
            pingTimer.Start();
        }

        private void secondTimer_Tick(object sender, EventArgs e)
        {
            counter++;
            lblSecCounter.Content = counter + "s";
        }

        private void pingTimer_Tick(object sender, EventArgs e)
        {
            StartPing();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            secondTimer.Stop();
            pingTimer.Stop();
            MessageBox.Show("Tempo raggiunto: " + counter + "s");
            counter = 0;
        }

        // Salvataggio dei nomi host nel file di testo
        private void btnSalva_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hai Salvato tutti i nomi Host/IP sul file di testo NomiHost.txt\n\nIl salvataggio avviene a righe, probabilità di sovrascrittura sul file di testo");
            SalvaHost();
        }

        private void SalvaHost()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("C:\\Users\\nando\\source\\repos\\SuperPingAguayo\\SuperPingAguayo\\Resources\\NomiHost.txt"))
                {
                    for (int i = 0; i < indirizziBox.Length; i++)
                    {
                        sw.WriteLine(indirizziBox[i].Text);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        // Compilazione dei nomi host dal file di testo al programma
        private void btnCompila_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Scrittura dal file di testo al programma.... \n\nAttenzione: A partire dalla nona riga, il testo non verrà scritto");
            CompilaHost();
        }

        private void CompilaHost()
        {
            try
            {
                using (StreamReader sr = new StreamReader("C:\\Users\\nando\\source\\repos\\SuperPingAguayo\\SuperPingAguayo\\Resources\\NomiHost.txt"))
                {
                    for (int i = 0; i < indirizziBox.Length; i++)
                    {
                        indirizziBox[i].Text = sr.ReadLine();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}