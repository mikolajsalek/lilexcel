using System.Xml;

namespace lilexcel

{
    public partial class Form1 : Form
    {

        public Form1()
        {

            InitializeComponent();

        }

        public void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void AddDataToGridView(string data1, string data2, string data3, string data4, string data5)
        {
            dataGridView1.Rows.Add(data1, data2, data3, data4, data5);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(this);
            form2.Show();

        }

        private void LoadCsvData(string filePath)
        {

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();


            using (StreamReader streamReader = new StreamReader(filePath))
            {

                string[] headers = streamReader.ReadLine().Split(',');


                foreach (string header in headers)
                {
                    dataGridView1.Columns.Add(header, header);
                }


                while (!streamReader.EndOfStream)
                {
                    string[] rowValues = streamReader.ReadLine().Split(',');


                    int rowIndex = dataGridView1.Rows.Add();
                    for (int i = 0; i < rowValues.Length; i++)
                    {
                        dataGridView1.Rows[rowIndex].Cells[i].Value = rowValues[i];
                    }
                }
            }

            MessageBox.Show("File loaded successfully.", "Load File", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {

                using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName))
                {

                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        streamWriter.Write(column.HeaderText + ",");
                    }
                    streamWriter.WriteLine();


                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {

                            string cellValue = cell.Value?.ToString().Replace(",", " ") ?? "";
                            streamWriter.Write(cellValue + ",");
                        }
                        streamWriter.WriteLine();
                    }
                }

                MessageBox.Show("File saved successfully.", "Save File", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                LoadCsvData(filePath);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView1.SelectedRows)
            {
                if (item.Selected)
                {
                    dataGridView1.Rows.RemoveAt(item.Index);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                using (XmlWriter writer = XmlWriter.Create(saveFileDialog.FileName, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Data");

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        writer.WriteStartElement("Row");

                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            writer.WriteElementString(dataGridView1.Columns[cell.ColumnIndex].HeaderText, cell.Value?.ToString());
                        }

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }

                MessageBox.Show("File saved successfully.", "Save File", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Clear();

                    using (XmlReader reader = XmlReader.Create(openFileDialog.FileName))
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "Row")
                            {
                                List<string> rowData = new List<string>();

                                while (reader.Read())
                                {
                                    if (reader.NodeType == XmlNodeType.Element && dataGridView1.Columns.Contains(reader.Name))
                                    {
                                        string data = reader.ReadElementContentAsString();
                                        rowData.Add(data);
                                    }

                                    if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Row")
                                    {
                                        int numCols = dataGridView1.Columns.Count;
                                        while (rowData.Count < numCols)
                                        {
                                            rowData.Add("");
                                        }
                                        if (rowData.Count > numCols)
                                        {
                                            rowData.RemoveRange(numCols, rowData.Count - numCols);
                                        }

                                        AddDataToGridView(rowData[0], rowData[1], rowData[2], rowData[3], rowData[4]);
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    MessageBox.Show("File loaded successfully.", "Load File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading file: " + ex.Message, "Load File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }





    }
}