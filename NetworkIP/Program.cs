using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;

namespace NetworkIP
{
    class Program
    {
        static void Main(string[] args)
        {
            string hostName = Dns.GetHostName();
            System.Net.IPAddress[] addressList = Dns.GetHostAddresses(hostName);



            Console.WriteLine("判断是否为Windows Linux OSX");
            Console.WriteLine($"Linux:{RuntimeInformation.IsOSPlatform(OSPlatform.Linux)}");
            Console.WriteLine($"OSX:{RuntimeInformation.IsOSPlatform(OSPlatform.OSX)}");
            Console.WriteLine($"Windows:{RuntimeInformation.IsOSPlatform(OSPlatform.Windows)}");
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();//获取本地计算机上网络接口的对象
            Console.WriteLine("适配器个数：" + adapters.Length);
            Console.WriteLine();
            foreach (NetworkInterface adapter in adapters)
            {
                if (adapter.Name == "以太网")
                {
                    Console.WriteLine("描述：" + adapter.Description);
                    Console.WriteLine("标识符：" + adapter.Id);
                    Console.WriteLine("名称：" + adapter.Name);
                    Console.WriteLine("类型：" + adapter.NetworkInterfaceType);
                    Console.WriteLine("速度：" + adapter.Speed * 0.001 * 0.001 + "M");
                    Console.WriteLine("操作状态：" + adapter.OperationalStatus);
                    Console.WriteLine("MAC 地址：" + adapter.GetPhysicalAddress());

                    // 格式化显示MAC地址                
                    PhysicalAddress pa = adapter.GetPhysicalAddress();//获取适配器的媒体访问（MAC）地址
                    byte[] bytes = pa.GetAddressBytes();//返回当前实例的地址
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        sb.Append(bytes[i].ToString("X2"));//以十六进制格式化
                        if (i != bytes.Length - 1)
                        {
                            sb.Append("-");
                        }
                    }
                    Console.WriteLine("MAC 地址：" + sb);

                    IPInterfaceProperties ip = adapter.GetIPProperties();     //IP配置信息
                    if (ip.UnicastAddresses.Count > 0)
                    {
                        Console.WriteLine("IP地址:" + ip.UnicastAddresses[0].Address.ToString());
                        Console.WriteLine("子网掩码:" + ip.UnicastAddresses[0].IPv4Mask.ToString());
                    }
                    if (ip.GatewayAddresses.Count > 0)
                    {
                        Console.WriteLine("默认网关:" + ip.GatewayAddresses[0].Address.ToString());   //默认网关
                    }
                    int DnsCount = ip.DnsAddresses.Count;
                    Console.WriteLine("DNS服务器地址：");   //默认网关
                    if (DnsCount > 0)
                    {
                        //其中第一个为首选DNS，第二个为备用的，余下的为所有DNS为DNS备用，按使用顺序排列
                        for (int i = 0; i < DnsCount; i++)
                        {
                            Console.WriteLine("              " + ip.DnsAddresses[i].ToString());
                        }
                    }
                    Console.WriteLine();

                }
            }
            Console.ReadKey();
        }
    }
}
