using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;

namespace SteganoMagic
{
    public partial class Form1 : Form
    {
        string loadedFilePath = "";
        long fileSize, secretmsgsize;
        int fileNameSize;
        FileInfo finfo;
        string msg = "";
        string i2ic, i2is, i2ie;
        string ext;
        int i2isize;

        public Form1()
        {
            InitializeComponent();
        }

        /////////////////////////////////////////////// Browse File Method ///////////////////////////////////////////////
        public bool browsefile(string type, TextBox t)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                loadedFilePath = openFileDialog1.FileName;
                fileNameSize = justFName(loadedFilePath).Length;
                type = type.ToLower();
                ext = (justFName(loadedFilePath).Substring(fileNameSize - (type.Length), type.Length));
                ext = ext.ToLower();
                if (!ext.Equals(type))
                {
                    MessageBox.Show("  Select only" + type + " files  ");
                    loadedFilePath = "";
                    fileNameSize = 0;
                    return false;
                }
                else
                {
                    t.Text = loadedFilePath;
                    finfo = new FileInfo(loadedFilePath);
                    fileSize = finfo.Length;
                    return true;
                }
            }
            return false;
        }

        private string justFName(string path)
        {
            string output;
            int i;
            if (path.Length == 3)   // i.e: "C:\\"
                return path.Substring(0, 1);
            for (i = path.Length - 1; i > 0; i--)
                if (path[i] == '\\')
                    break;
            output = path.Substring(i + 1);
            return output;
        }

        ///////////////////////////////////////////////Text in Doc : EMBEDD : BROWSE ///////////////////////////////////////////////
        private void button1_Click(object sender, EventArgs e)
        {
            if (browsefile(".txt", textBox1))
            {
                label7.Text = fileSize.ToString() + " Bytes";

                string text = File.ReadAllText(loadedFilePath, Encoding.UTF8);

                var foundIndexes = new List<int>();
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] == ' ')
                        foundIndexes.Add(i);
                }
                secretmsgsize = ((foundIndexes.Count) / 8) - 1;
                if (secretmsgsize < 0)
                    secretmsgsize = 0;
                label8.Text = secretmsgsize.ToString() + " Characters";
            }
            else
            {
                label7.Text = "0";
                label8.Text = "0";
                textBox1.Text = "";
            }
        }

        ////////////////////////////////////////////TEXT IN DOC : EXTRACT : BROWSE //////////////////////////////////////////////
        private void button3_Click(object sender, EventArgs e)
        {
            browsefile(".txt", textBox3);

            textBox11.Text = "";
        }

        ////////////////////////////////////////////IMAGE IN IMAGE : EMBEDD : BROWSE CARRIER FILE///////////////////////////////////////////////
        private void button5_Click(object sender, EventArgs e)
        {
            browsefile(".jpg", textBox4);
            i2ic = loadedFilePath;
            Image img = Image.FromFile(loadedFilePath);
            Bitmap bmp = new Bitmap(img);
            var fileLength = new FileInfo(loadedFilePath).Length;
            secretmsgsize = ((bmp.Height * bmp.Width - 10) / 11);
            label27.Text = (fileLength / 1024).ToString() + " KB";
            label26.Text = secretmsgsize.ToString() + " Pixels";
        }

        ////////////////////////////////////////////IMAGE IN IMAGE : EMBEDD : BROWSE SECRET FILE///////////////////////////////////////////////
        private void button6_Click(object sender, EventArgs e)
        {
            browsefile(".jpg", textBox5);
            i2is = loadedFilePath;

            Image imgs = Image.FromFile(i2is);
            Bitmap bmps = new Bitmap(imgs);
            i2isize = bmps.Width * bmps.Height;
        }

        ////////////////////////////////////////////IMAGE IN IMAGE : EXTRACT : BROWSE ///////////////////////////////////////////////
        private void button12_Click(object sender, EventArgs e)
        {
            browsefile(".jpg", textBox9);
            i2ie = loadedFilePath;
        }

        ///////////////////////////////////////////////Text in Image : EMBEDD : BROWSE ///////////////////////////////////////////////
        private void button8_Click(object sender, EventArgs e)
        {
            browsefile(".jpg", textBox6);
            Image img = Image.FromFile(loadedFilePath);
            Bitmap bmp = new Bitmap(img);
            var fileLength = new FileInfo(loadedFilePath).Length;
            secretmsgsize = (((bmp.Height * bmp.Width * 3) - 8) / 8);
            label22.Text = (fileLength / 1024).ToString() + " KB";
            label21.Text = secretmsgsize.ToString() + " Characters";
        }

        ////////////////////////////////////////////TEXT IN IMAGE : EXTRACT : BROWSE ///////////////////////////////////////////////
        private void button10_Click(object sender, EventArgs e)
        {
            browsefile(".jpg", textBox8);
            textBox10.Text = "";
        }

        ///////////////////////////////////////////////TEXT IN DOC : EMBEDD : LOGIC ///////////////////////////////////////////////
        private void button2_Click(object sender, EventArgs e)
        {
            //Checking for validation.
            if (loadedFilePath == "")
            {
                MessageBox.Show("File Path cannot be blank...!");
                textBox1.Text = "";
                textBox2.Text = "";
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("Your Msg Should not be blank.");
            }
            else if (textBox2.Text.Length > secretmsgsize)
            {
                MessageBox.Show("You can Embedd Maximum " + secretmsgsize + " characters.");
                textBox2.Text = "";
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;

                SimpleObjectFactory sof = new SimpleObjectFactory();
                ITextFile textFile = sof.getTextFile();
                string text = textFile.getTextFromFile(loadedFilePath);

                string msg = textBox2.Text;

                ITextInDoc textInDoc = sof.getTextInDoc();
                text = textInDoc.Embedd(text,msg);

                string path = loadedFilePath.Remove(loadedFilePath.Length - fileNameSize) + justFName(loadedFilePath).Remove(fileNameSize - 4) + "1.txt";

                if (!File.Exists(path))
                {
                    File.Create(path).Dispose();
                    using (TextWriter tw = new StreamWriter(path))
                    {
                        tw.Write("" + text + "");
                        tw.Close();
                    }
                    label33.Text = "Output File : " + path;
                }
                else if (File.Exists(path))
                {
                    using (TextWriter tw = new StreamWriter(path))
                    {
                        MessageBox.Show(justFName(loadedFilePath).Remove(fileNameSize - 4) + "1.txt File already exist...!");
                        label33.Text = "";
                    }
                }

                Cursor.Current = Cursors.Arrow;
                textBox1.Text = "";
                textBox2.Text = "";
                msg = "";
                loadedFilePath = "";
            }
        }

        ////////////////////////////////////////////TEXT IN DOC : EXTRACT : LOGIC ///////////////////////////////////////////////
        private void button4_Click(object sender, EventArgs e)
        {
            if (loadedFilePath == "")
            {
                MessageBox.Show("File Path cannot be blank...!");
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                SimpleObjectFactory sof = new SimpleObjectFactory();
                ITextFile textFile = sof.getTextFile();
                string text = textFile.getTextFromFile(loadedFilePath);

                ITextInDoc textInDoc = sof.getTextInDoc();
                text = textInDoc.Extract(text);
                
                textBox11.Text = text;
                loadedFilePath = "";
                Cursor.Current = Cursors.Arrow;
            }
        }

        ////////////////////////////////////////////TEXT IN IMAGE : EMBEDD : LOGIC ///////////////////////////////////////////////

        private void button9_Click(object sender, EventArgs e)
        {
            
            if (loadedFilePath == "")
            {
                MessageBox.Show("File Path cannot be blank...!");
                textBox6.Text = "";
                textBox7.Text = "";
            }
            else if (textBox7.Text == "")
            {
                MessageBox.Show("Your Msg Should not be blank.");
            }
            else if (textBox7.Text.Length > secretmsgsize)
            {
                MessageBox.Show("You can Embedd Maximum " + secretmsgsize + " characters.");
                textBox7.Text = "";
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;

                SimpleObjectFactory sof = new SimpleObjectFactory();
                //Text Processing
                string msg = textBox7.Text;

                ITextInImage textInImage = sof.getTextInImage();
                Bitmap eMap = textInImage.Embedd(loadedFilePath, msg);

                string path = loadedFilePath.Remove(loadedFilePath.Length - fileNameSize) + justFName(loadedFilePath).Remove(fileNameSize - 4) + "1.jpg";
                if (!File.Exists(path))
                {
                    eMap.Save(path);
                    label31.Text = "Output File : " + path;
                }
                else if (File.Exists(path))
                {
                    MessageBox.Show(justFName(loadedFilePath).Remove(fileNameSize - 4) + "1.jpg File already exist...!");
                    label31.Text = "";
                }

                loadedFilePath = "";
                Cursor.Current = Cursors.Arrow;

            }
        }


        ////////////////////////////////////////////TEXT IN IMAGE : EXTRACT : LOGIC ///////////////////////////////////////////////

        private void button11_Click(object sender, EventArgs e)
        {
            if (loadedFilePath == "")
            {
                MessageBox.Show("File Path cannot be blank...!");
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;

                SimpleObjectFactory sof = new SimpleObjectFactory();
                ITextInImage textInImage = sof.getTextInImage();
                msg = textInImage.Extract(loadedFilePath);
                textBox10.Text = msg;
                loadedFilePath = "";
                Cursor.Current = Cursors.Arrow;
            
            }
        }

        ////////////////////////////////////////////IMAGE IN IMAGE : EXTRACT : LOGIC ///////////////////////////////////////////////

        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
}