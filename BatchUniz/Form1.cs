using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;




namespace BatchUniz
{
    public partial class Form1 : Form
    {

        string rootPath = "";
        string outputPath = "";

        public Form1()
        {
            InitializeComponent();
            this.textBox1.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            this.textBox2.Text = this.textBox1.Text;
            rootPath = this.textBox1.Text;
            outputPath = this.textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
          

            DialogResult result = this.folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.rootPath = this.folderBrowserDialog1.SelectedPath;
                this.textBox1.Text = this.rootPath;

            }

           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialog2.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.outputPath = this.folderBrowserDialog2.SelectedPath;
                this.textBox2.Text = this.outputPath;
            }
        }

        /// <summary>
        /// click to start running the unzipping progress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (this.backgroundWorker1.IsBusy != true)
            {
                this.backgroundWorker1.RunWorkerAsync();
            }
            this.progressBar1.Value = 2;
        }

        

        /// <summary>
        /// Worker that actuall does the unzipping. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {
            string[] genericFoldersEntries;
            genericFoldersEntries = Directory.GetFiles(rootPath, "*.zip");  // If zipped

            int numFoldersToUnzip = genericFoldersEntries.Count();
            int count = 0;

            // for each zipped folder. 
            
            foreach (string genericFolder in genericFoldersEntries)
            {
                try
                {
                    ZipFile.ExtractToDirectory(genericFolder, outputPath); // unzip
                }
                catch (Exception e2)
                {
                    Console.WriteLine(e2.Message);
                }

                int percent = (int)((count / (numFoldersToUnzip + 1.0)) * 100);
                this.backgroundWorker1.ReportProgress(percent); // reports % complete
                count++;
            }
        }


        /// <summary>
        /// Allows for easy changing of the progress bar by the worker thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.progressBar1.Value = 100;
        }
    }
}
