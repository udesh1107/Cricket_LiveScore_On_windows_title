using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net;
using System.Diagnostics;

namespace Cricket_LiveScore
{
    public partial class Form1 : Form
    {
        XmlTextReader rssReader;
        XmlDocument rssDoc;
        XmlNode nodeRss;
        XmlNode nodeChannel;
        XmlNode nodeItem;
        ListViewItem rowNews;

        [DllImport("user32.dll", EntryPoint = "SetWindowText", CharSet = CharSet.Ansi)]
        public static extern bool SetWindowText(IntPtr hWnd, String strNewWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Ansi)]
        public static extern IntPtr FindWindow(string className, string windowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();




        public Form1()


        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.Invoke(new EventHandler(Form1_Load), new object[] { sender, e });
                return;
            }

            listView1.Items.Clear();
            this.Cursor = Cursors.WaitCursor;
            // Create a new XmlTextReader from the specified URL (RSS feed)
            rssReader = new XmlTextReader("http://static.espncricinfo.com/rss/livescores.xml");
            rssDoc = new XmlDocument();
            // Load the XML content into a XmlDocument
            rssDoc.Load(rssReader);

            // Loop for the <rss> tag
            for (int i = 0; i < rssDoc.ChildNodes.Count; i++)
            {
                // If it is the rss tag
                if (rssDoc.ChildNodes[i].Name == "rss")
                {
                    // <rss> tag found
                    nodeRss = rssDoc.ChildNodes[i];
                }
            }

            // Loop for the <channel> tag
            for (int i = 0; i < nodeRss.ChildNodes.Count; i++)
            {
                // If it is the channel tag
                if (nodeRss.ChildNodes[i].Name == "channel")
                {
                    // <channel> tag found
                    nodeChannel = nodeRss.ChildNodes[i];
                }
            }

            // Set the labels with information from inside the nodes
          //  lblTitle.Text = "Title: " + nodeChannel["title"].InnerText;
           // lblLanguage.Text = "Language: " + nodeChannel["language"].InnerText;
            //lblLink.Text = "Link: " + nodeChannel["link"].InnerText;
           // lblDescription.Text = "Description: " + nodeChannel["description"].InnerText;

            // Loop for the <title>, <link>, <description> and all the other tags
            for (int i = 0; i < nodeChannel.ChildNodes.Count; i++)
            {
                // If it is the item tag, then it has children tags which we will add as items to the ListView
                if (nodeChannel.ChildNodes[i].Name == "item")
                {
                    nodeItem = nodeChannel.ChildNodes[i];

                    // Create a new row in the ListView containing information from inside the nodes
                    rowNews = new ListViewItem();
                    rowNews.Text = nodeItem["title"].InnerText;
                   rowNews.SubItems.Add(nodeItem["link"].InnerText);
                    listView1.Items.Add(rowNews);
                }
            }

            this.Cursor = Cursors.Default;
            
            //Use_Notify();  
        }
        private void Use_Notify(object sender, EventArgs e)
        {
            //String url = listView1.SelectedItems[0].SubItems[1].Text;
            //reader readera=new reader();
            Thread threada = new Thread(this.getScore);
           // Thread threadb = new Thread(this.exitToolStripMenuItem_Click);
            threada.Start();
            //Thread.Sleep(100);

           // textBox1.Text = "";
           // threada.Suspend();
            //  notifyIcon1.ContextMenuStrip = menuStrip1;
            // lstNews.Items.Clear();
          
        }
        private void getScore()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            //this.Invoke((MethodInvoker)delegate
            //    {
            int firstvalue = 0;
            while (true)
            {
               // InitializeComponent();

                String url = nodeItem["link"].InnerText;
              
           //String url = richTextBox1.Text;
                     //textBox1.Text = "";
               
            string result = null;

            try
            {
               WebClient client = new WebClient();
                result = client.DownloadString(url);
              //  result = "Mountaineers 56/4 (19.0 ov, T Maruma 0*, H Masakadza 18*, KO Meth 4/14) - Stumps |  4";
                int start=result.IndexOf("<title>");
                int end = result.IndexOf("</title>");

                string write = result.Substring(start + 7, end - start-37);

                //HWND WINAPI GetDesktopWindow(void);
                SetWindowText(GetDesktopWindow(),write);
                SetWindowText(GetForegroundWindow(), write);
                label2.Text = write;
                char[] array = write.ToCharArray();
                int firstSlash = 0;
                for (int i = 0; i < array.Length; i++)
                {
                    if(array[i]=='/'){
                        firstSlash = i;
                        break;

                    }
                }
                int secondValue = 0;
                String wickets = write.Substring(firstSlash + 1, 2);
                secondValue = Convert.ToInt16(wickets);

                if (secondValue != firstvalue&&wickets!=null)
                {
                    notifyIcon1.BalloonTipText = "Match Results" + "-" + write;
                    notifyIcon1.BalloonTipTitle = "One wicket down";
                    notifyIcon1.ShowBalloonTip(5);
                }


                firstvalue = Convert.ToInt16(wickets);
                //int firstSlash = result.IndexOf("/");
                //System.IO.File.WriteAllText(@"D:\sample.txt", write+" "+wickets);
              //  SetWindowText(FindWindow("wndclass_desked_gsk", null), write);
               // SetWindowText(FindWindow(@"HwndWrapper([A-Za-z0-9_[]-:]*)", @"([A-Za-z0-9_]*)(\\s-\\sMicrosoft\\sVisual\\sStudio)"), write);
                

                //System.IO.File.WriteAllText(@"D:\sample.txt", write);
               // String[] result1 = GetStringInBetween("<title>", "</title>", result.Trim().ToLower());
    //            MatchCollection matches = Regex.Matches(result.Trim().ToLower(), @"<title>[A-Za-z0-9/.,*\s]*</title>");

    //            foreach (Match match in matches)
    //{
    //    foreach (Capture capture in match.Captures)
    //    {
    //       // Console.WriteLine(capture.Value);
    //     System.IO.File.WriteAllText(@"D:\sample.txt", capture.Value);
    //    }
    //}

               
            }
            catch (Exception ex)
            {
                // handle error
               // MessageBox.Show("Please Add a valid URL and Press Enter"+"  "+ex);
                Thread.Sleep(5000);
            }
            Thread.Sleep(1000); 
           // SetWindowText(FindWindow("wndclass_desked_gsk", null), "----------------------------------------Cricket_Live_Score 234/5*--------------------------------------  ");
        }
                //});

             ////  // this.Cursor = Cursors.WaitCursor;
             ////   // Create a new XmlTextReader from the specified URL (RSS feed)
             //   rssReader = new XmlTextReader("http://static.espncricinfo.com/rss/livescores.xml");
             //   rssDoc = new XmlDocument();
             //  // // Load the XML content into a XmlDocument
             //   rssDoc.Load(rssReader);

             // //  // Loop for the <rss> tag
             //   for (int i = 0; i < rssDoc.ChildNodes.Count; i++)
             //   {
             //      // // If it is the rss tag
             //       if (rssDoc.ChildNodes[i].Name == "rss")
             //       {
             //         //  // <rss> tag found
             //           nodeRss = rssDoc.ChildNodes[i];
             //       }
             //   }

             // //  // Loop for the <channel> tag
             //   for (int i = 0; i < nodeRss.ChildNodes.Count; i++)
             //   {
             //    //   // If it is the channel tag
             //       if (nodeRss.ChildNodes[i].Name == "channel")
             //       {
             //        //   // <channel> tag found
             //           nodeChannel = nodeRss.ChildNodes[i];
             //       }
             //   }

             //  // // Set the labels with information from inside the nodes
             // //  //lblTitle.Text = "Title: " + nodeChannel["title"].InnerText;
             ////   //lblLanguage.Text = "Language: " + nodeChannel["language"].InnerText;
             // //  //lblLink.Text = "Link: " + nodeChannel["link"].InnerText;
             //  // //lblDescription.Text = "Description: " + nodeChannel["description"].InnerText;

             //  // // Loop for the <title>, <link>, <description> and all the other tags
             //   for (int i = 0; i < nodeChannel.ChildNodes.Count; i++)
             //   {
             //      // // If it is the item tag, then it has children tags which we will add as items to the ListView
             //       if (nodeChannel.ChildNodes[i].Name == "item")
             //       {


             //           nodeItem = nodeChannel.ChildNodes[i];


             //           notifyIcon1.BalloonTipText = "Match Results" + "-" + nodeItem["title"].InnerText;
             //         //  // notifyIcon1.BalloonTipText = "hiiiii";// Create a new row in the ListView containing information from inside the nodes
             //          // //rowNews = new ListViewItem();
             //        //   //rowNews.Text = nodeItem["title"].InnerText;
             //         //  //rowNews.SubItems.Add(nodeItem["link"].InnerText);
             //         //  //lstNews.Items.Add(rowNews);
             //         //  // notifyIcon1.Container
                        

             //          // //  this.Visible = false;
                      

             //       }
             //   }
             //   notifyIcon1.BalloonTipTitle = "CricInfo Live Score";
             //   notifyIcon1.ShowBalloonTip(5);

              //  //this.Cursor = Cursors.Default;






            }
        


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string processName = "Cricket_LiveScore";

            Process[] processes = Process.GetProcessesByName(processName);



            foreach (Process process in processes)
            {

                process.Kill();

            }

            string processName1 = "Cricket_LiveScore.vshost";

            Process[] processes1 = Process.GetProcessesByName(processName1);



            foreach (Process process in processes1)
            {

                process.Kill();

            }
            this.Close();
            this.Dispose();
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // When an items is selected
            if (listView1.SelectedItems.Count == 1)
            {
                // Loop through all the nodes under <channel>
                for (int i = 0; i < nodeChannel.ChildNodes.Count; i++)
                {
                    // Until you find the <item> node
                    if (nodeChannel.ChildNodes[i].Name == "item")
                    {
                        // Store the item as a node
                        nodeItem = nodeChannel.ChildNodes[i];
                        // If the <title> tag matches the current selected item
                        if (nodeItem["title"].InnerText == listView1.SelectedItems[0].Text)
                        {
                            // It's the item we were looking for, get the description
                            richTextBox1.Text = nodeItem["title"].InnerText+"  "+nodeItem["link"].InnerText;
                            Thread threada = new Thread(this.getScore);
                            // Thread threadb = new Thread(this.exitToolStripMenuItem_Click);
                            threada.Start();
                            // We don't need to loop anymore
                            break;
                        }
                    }
                }
            }

        }

        private void listView1_DoubleClick(object sender, EventArgs e){

            System.Diagnostics.Process.Start(listView1.SelectedItems[0].SubItems[1].Text);  
    }

       
       

       
       
    }
}
