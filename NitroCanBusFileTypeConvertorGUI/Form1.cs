using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.ListBox;
using DataConverter;
using DataConverter.FileTypes.Entities;
using DataConverter.FileTypes.SavvyCanConvertor;
using DataConverter.FileTypes.BusWatchAnalyzer;
using DataConverter.FileTypes.NitroAnalyzer;
using DataConverter.FileTypes.JNKAnalyzer;

namespace NitroCanBusFileTypeConvertorGUI
{
    public partial class Form1 : Form
    {
        silly mySilly;
        
        //TODO: connnect programs.cs to this, and connect library to it.
        public Form1()
        {
            
            InitializeComponent();
            mySilly = new silly();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            mySilly.typeinput = listBox1.SelectedItem.ToString();
            mySilly.input = GetTheType(listBox1.SelectedItem.ToString());
        }

        private IFileType GetTheType(string v)
        {
            switch (v.ToLower()) {
                case "savvycan":
                    return new SavvyCanConvertor();
                case "buswatch":
                    return new BusWatchAnalyzer();
                case "nitroanalyzer":
                    return new NitroAnalyzer();
                case "jnk":
                    return new JNKAnalyzer();
            }
            return null;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }



        private void openFileDialog1_FileOk_1(object sender, CancelEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //TODO: Reference code in silly that does everything
            mySilly.DoTheConversion();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           openFileDialog1.Title = "Choose Input File";
           openFileDialog1.Filter = "Bus/Nitro|*.txt|JNK|*.jnk|Savvy|*.csv"; // Maybe change this filter to be specific for chosen
           if( openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                mySilly.inputfile = openFileDialog1.FileName; 
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void OutputButton_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                mySilly.outputfile = saveFileDialog1.FileName;
                textBox2.Text = saveFileDialog1.FileName;
            }
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            mySilly.OnReady += enablebutton;
        }
        private void enablebutton(object sender, MyEventsArgs e)
        {
            ConvertButton.Enabled = e.isready;
        }
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var temp = new List<string>() ;
            var temp2 = new List<IFileType>();
            foreach (var a in ((CheckedListBox)sender).CheckedItems) {
                temp.Add(a.ToString());
                temp2.Add(GetTheType(a.ToString()));
            }
            mySilly.outputtypes = temp;
            mySilly.output = temp2;

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
