namespace WinClient
{
    public partial class Form1 : Form
    {
        private const string _FirstHeader = "material";
        private const string _SecondHeader = "customertype";

        private int messageCount;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            var headers = new Dictionary<string, string>();
            var messageSender = new RabbitSender();

            headers.Add(_FirstHeader, GetComboItem(comboBoxMaterial));
            headers.Add(_SecondHeader, GetComboItem(comboBoxCostumerType));

            var message = string.Format("Message: {0}", messageCount);

            messageSender.Send(message, headers);

            MessageBox.Show(string.Format("Sending Message - {0}", message), "Message sent");

            messageCount++;
        }

        private static string GetComboItem(ComboBox comboBox)
        {
            if (string.IsNullOrEmpty(comboBox.Text))
                return string.Empty;
            return comboBox.Text;
        }
    }
}
