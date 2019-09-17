using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace goproxy_helper
{
    public partial class main : Form
    {
        private static string serviceName="ProxyFreeAdmin";
        private static string binName = "proxy-admin-free";
        private static string titleType = "社区版";
        public main()
        {
            InitializeComponent();
            if (File.Exists("proxy-admin"))
            {
                binName = "proxy-admin";
                serviceName = "ProxyAdmin";
                titleType = "商业版";
               
            }
            this.Text += " " + titleType;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Service.Exists(serviceName)) {
                MessageBox.Show("服务已经安装！");
                return;
            }
            MessageBox.Show(exec(binName,"install",true,false,true)?"安装成功":"安装失败");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private string chromePath()
        {
            object path2;
            path2 = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe", "", null);
            if (path2 != null)
            {
                return path2.ToString();
            }
            return "";
        }
        private string firefoxPath()
        {
            object  path2;
             path2 = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\firefox.exe", "", null);
            if (path2 != null) {
                return path2.ToString();
            }
            return "";
        }

        private void main_Load(object sender, EventArgs e)
        {
           
            string chromebinpath = this.chromePath();
            string firefoxbinpath = this.firefoxPath();
            if (chromebinpath == "" && firefoxbinpath == "")
            {
                label2.Text = "请先安装最新版chrome或者firefox，才能正常使用goproxy控制面板。";
            }
            refreshStatus();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!Service.Exists(serviceName))
            {
                MessageBox.Show("服务未安装！");
                return;
            }
            Service.Stop(serviceName);
            //exec(binName, "stop", true, false);
            MessageBox.Show(Service.Start(serviceName) ? "重启成功" : "重启失败");
        }

        private void refreshStatus()
        {
            Thread demoThread = new Thread(new ThreadStart(threadMethod));
            demoThread.IsBackground = true;
            demoThread.Start();//启动线程 
            
        }
          void threadMethod() {
            string statusText = "";
            while (true) {
                bool isExists = Service.Exists(serviceName);
                if (isExists)
                {
                    if (Service.IsRunning(serviceName))
                    {
                        statusText = "运行中";
                    }
                    else
                    {
                        statusText = "已停止";
                    }
                }
                else
                {
                    statusText = "未安装";
                }
                Action<String> AsyncUIDelegate = delegate (string n) { status.Text = n; };
                status.Invoke(AsyncUIDelegate, new object[] { statusText });
                Thread.Sleep(1000);
            }
        }
        private bool exec(string cmd,string args,bool CreateNoWindow, bool UseShellExecute,bool wait) {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = cmd;
            p.StartInfo.CreateNoWindow = CreateNoWindow;
            p.StartInfo.UseShellExecute = UseShellExecute;
            p.StartInfo.Arguments = args;
            try
            {
                bool ok = p.Start();
                if (wait) {
                    p.WaitForExit();
                }
                return ok;
            }
            catch (Exception e) {
                return false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!Service.Exists(serviceName))
            {
                MessageBox.Show("服务未安装！");
                return;
            }
            MessageBox.Show(exec(binName, "uninstall", true, false,true) ? "卸载成功" : "卸载失败");
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /**
            if (!Service.Exists(serviceName))
            {
                MessageBox.Show("服务未安装！");
                return;
            }**/
            exec("explorer.exe", "C:\\gpfa", true, false,false);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!Service.Exists(serviceName))
            {
                MessageBox.Show("服务未安装！");
                return;
            }
            string chromebinpath = this.chromePath();
            string firefoxbinpath = this.firefoxPath();
            string browser = "";

            if (chromebinpath != "")
            {
                browser = "chrome.exe";
            }
            else if (firefoxbinpath != "")
            {
                browser = "firefox.exe";
            }
            else {
                MessageBox.Show("请先安装最新版 Google Chrome 或者 Firefox 浏览器！");
                return;
            }
            System.Diagnostics.Process.Start(browser, "http://127.0.0.1:32080");
            //exec("cmd.exe", "start " + browser + " http://127.0.0.1:32080", true, true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("太难了，最近压力很大");
        }
    }
}
