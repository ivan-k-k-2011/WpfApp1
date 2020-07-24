using System;
using System.Windows;
using System.IO;
using PcapDotNet.Core;
using PcapDotNet.Packets;


namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int j = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create the offline device
                OfflinePacketDevice selectedDevice = new OfflinePacketDevice(System.IO.Path.Combine(inpath.Text, "SV.pcap"));

                // Open the capture file
                using (PacketCommunicator communicator =
                        selectedDevice.Open(65536,                                  // portion of the packet to capture
                                                                                    // 65536 guarantees that the whole packet will be captured on all the link layers
                                            PacketDeviceOpenAttributes.Promiscuous, // promiscuous mode
                                            1000))                                  // read timeout
                {
                    communicator.SetFilter("ether proto 0x88ba");
                    communicator.ReceivePackets(0, DispatcherHandler);
                }
            }
            catch
            {
                return;
            }
        }
        private void DispatcherHandler(Packet packet)
        {
            using (StreamWriter sw = new StreamWriter(System.IO.Path.Combine(outpath.Text, "outSV.csv"), true))
            {
                if (j == 0)
                {
                    sw.WriteLine("№,ток1,ток2,ток3,ток4,напряжение1,напряжение2,напряжение3,напряжение4");
                }
                string pak = "";
                int k = 0, caseSwitch = 0;
                const int LineLength = 4;
                for (int i = 60; i != packet.Length; ++i)
                {
                    if (k < 4)
                    {
                        pak += (packet[i]).ToString("X2");
                        k++;
                    }
                    if ((i + 1) % LineLength == 0 && pak != "")
                    {
                        switch (caseSwitch)
                        {
                            case 0:
                                sw.Write(j+"," + Convert.ToInt32(pak,16) + ",");
                                j++;
                                pak = "";
                                caseSwitch++;
                                break;
                            case 1:
                                sw.Write(Convert.ToInt32(pak, 16) + ",");
                                pak = "";
                                caseSwitch++;
                                break;
                            case 2:
                                sw.Write(Convert.ToInt32(pak, 16) + ",");
                                pak = "";
                                caseSwitch++;
                                break;
                            case 3:
                                sw.Write(Convert.ToInt32(pak, 16) + ",");
                                pak = "";
                                caseSwitch++;
                                break;
                            case 4:
                                sw.Write(Convert.ToInt32(pak, 16) + ",");
                                pak = "";
                                caseSwitch++;
                                break;
                            case 5:
                                sw.Write(Convert.ToInt32(pak, 16) + ",");
                                pak = "";
                                caseSwitch++;
                                break;
                            case 6:
                                sw.Write(Convert.ToInt32(pak, 16) + ",");
                                pak = "";
                                caseSwitch++;
                                break;
                            case 7:
                                sw.WriteLine(Convert.ToInt32(pak, 16));
                                pak = "";
                                caseSwitch = 0;
                                break;
                        }
                    }
                    if (k >= 4 && k < 9)
                        k++;
                    if (k == 9)
                        k = 0;
                }
            }
        }
    }
}
