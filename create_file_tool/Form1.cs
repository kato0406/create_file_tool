using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace create_file_tool
{
    public partial class Form1 : Form
    {
        private string jsonFileName; 

        public Form1()
        {
            InitializeComponent();

            label2.Visible = false;
            numericUpDown1.Visible = false;
            label3.Visible = true;
            textBox2.Visible = true;
            button3.Visible = true;
            label4.Visible = false;
            label5.Visible = false;
            numericUpDown2.Visible = false;
            label6.Visible = false;
            textBox3.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                JObject JObject = JObject.Parse(File.ReadAllText(jsonFileName));

                // フォルダとファイルを作成
                CreateFoldersAndFiles(textBox1.Text, JObject);
            } 
            else
            {
                if (textBox1.Text == "" || textBox3.Text == "")
                {
                    MessageBox.Show("inputに値が入力されてません。");
                    return;
                }

                DateTime dateTime = DateTime.Now;
                string formatDatetime = dateTime.ToString("yyyyMMddHHmmss");
                long desiredFileSizeInBytes = (int)numericUpDown2.Value * 1024 * 1024;

                for (int i = 1; i <= numericUpDown1.Value; i++)
                {
                    FileInfo fileInfo = new FileInfo($@"{textBox1.Text}\{formatDatetime}_{i}.{textBox3.Text}");
                    

                    using (FileStream fs = fileInfo.Create())
                    {
                        fs.SetLength(desiredFileSizeInBytes);
                    }
                }
            }

            MessageBox.Show("作成が完了しました。");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = dialog.SelectedPath;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = dialog.FileName;
                    jsonFileName = dialog.FileName; 
                }
            }
        }

        private void CreateFoldersAndFiles(string path, JObject JObject)
        {
            foreach (var value in JObject)
            {
                string fullPath = Path.Combine(path, value.Key);

                if (value.Value is JObject subStructure)
                {
                    // サブフォルダがある場合は再帰的に処理
                    Directory.CreateDirectory(fullPath);
                    CreateFoldersAndFiles(fullPath, subStructure);
                }
                else
                {
                    FileInfo fileInfo = new FileInfo($@"{fullPath}.txt");
                    using (StreamWriter writer = fileInfo.CreateText())
                    {
                        writer.Write(value.Value.ToString());
                    }
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                label2.Visible = false;
                numericUpDown1.Visible = false;
                label3.Visible = true;
                textBox2.Visible = true;
                button3.Visible = true;
                label4.Visible = false;
                label5.Visible = false;
                numericUpDown2.Visible = false;
                label6.Visible = false;
                textBox3.Visible = false;
            } 
            else
            {
                label3.Visible = false;
                textBox2.Visible = false;
                button3.Visible = false;
                label2.Visible = true;
                numericUpDown1.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                numericUpDown2.Visible = true;
                label6.Visible = true;
                textBox3.Visible = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Visible = false;
            numericUpDown1.Visible = false;
        }
    }
}
