namespace WinClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private int messageCount = 0;

        private void buttonSend_Click(object sender, EventArgs e)
        {   
            var topics = new List<string>();
            var messageSender = new RabbitSender();

            topics.Add(GetComboItem(this.comboBoxCostumerType));
            topics.Add(GetComboItem(this.comboBoxOrderSize));
            topics.Add(GetComboItem(this.comboBoxProduct));

            var message = string.Format("Message: {0}", messageCount);

            var routingkey = messageSender.Send(message, topics);

            MessageBox.Show(string.Format("Sending Message - {0}, Routing Key - {1}", message, routingkey), "Message sent");

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
