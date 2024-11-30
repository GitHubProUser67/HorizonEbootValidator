using System.Security.Cryptography;
using System.Xml;

namespace ValidateHorizonEboot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult result = ofd.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string filePath = ofd.FileName;

                if (File.Exists(filePath))
                {
                    if (filePath.EndsWith(".bin", StringComparison.InvariantCultureIgnoreCase) || filePath.EndsWith(".elf", StringComparison.InvariantCultureIgnoreCase) || filePath.EndsWith(".self", StringComparison.InvariantCultureIgnoreCase))
                        textBox1.Text = filePath;
                    else
                        MessageBox.Show("The requested file is not of eboot type!", "ERROR");
                }
                else
                    MessageBox.Show("The requested file doesn't exists!", "ERROR");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (File.ReadAllText(textBox1.Text).StartsWith("SCE"))
            {
                MessageBox.Show("The requested eboot is encrypted!", "ERROR");
                return;
            }

            try
            {
                using (FileStream fs = new FileStream(textBox1.Text, FileMode.Open, FileAccess.Read))
                {
                    // Read the first 500KB of the file
                    const int bytesToRead = 500 * 1024; // 500KB
                    byte[] buffer = new byte[bytesToRead];
                    int bytesRead = fs.Read(buffer, 0, bytesToRead);

                    // Compute the SHA1 hash of the first 500KB
                    using (SHA1 sha1 = SHA1.Create())
                        textBox2.Text = BitConverter.ToString(sha1.ComputeHash(buffer, 0, bytesRead)).Replace("-", string.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error parsing EBOOT.ELF: " + ex.Message, "ERROR");
            }
        }
    }
}
