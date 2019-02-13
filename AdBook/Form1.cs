using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AdBook
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        const string path = @"C:\Users\....";
        public void ClearAll()  // Tabort alla texter från textbox
        {
            txtBoxName.Clear();
            txtBoxPostName.Clear();
            txtBoxPostCode.Clear();
            txtBoxStreetName.Clear();
            txtBoxTelephoneNo.Clear();
            txtBoxEmail.Clear();
            txtBoxName.Focus();
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            string name = txtBoxName.Text;
            string streetName = txtBoxStreetName.Text;
            string postCode = txtBoxPostCode.Text;
            string postName = txtBoxPostName.Text;
            string telephoneNo = txtBoxTelephoneNo.Text;
            string email = txtBoxEmail.Text;
            Save(name, streetName, postCode, postName, telephoneNo, email); // Spara ny kontakter
            ClearAll();
        }

        public void Save(string name, string streetName, string postCode, string postName, string telephoneNo, string email) // Kontorellera för att spara till .txt fil
        {
            StreamWriter sw = new StreamWriter(path, true);

            string missing = "";
            if (txtBoxName.Text == "")
            {
                missing = "name";
            }
            if (txtBoxStreetName.Text == "")
            {
                missing = missing + ",street name";
            }
            if (txtBoxPostCode.Text == "")
            {
                missing += ",post code";
            }
            if (txtBoxPostName.Text == "")
            {
                missing += ",post address";
            }
            if (txtBoxTelephoneNo.Text == "")
            {
                missing += ",telephone no";
            }
            if (txtBoxEmail.Text == "")
            {
                missing += ",email";
            }
            if (missing == "")
            {
                sw.WriteLine(name + "," + streetName + "," + postCode + "," + postName + "," + telephoneNo + "," + email);
                MessageBox.Show($" Tack {txtBoxName.Text}! All information sparas framgångsrikt .. ");
            }
            else
            {
                if (missing.IndexOf(",") == 0)
                {
                    missing = missing.Substring(1, missing.Length - 1);
                }
                MessageBox.Show($"Filla upp {missing} och forsök igen");
            }
            sw.Close();
        }

        public new List<string> Load() 
        {
            List<string> addressBook = new List<string>();
            StreamReader sr = new StreamReader(path, true); 
            string row = "";
            while ((row = sr.ReadLine()) != null)
            {
                addressBook.Add(row);
            }
            sr.Close();
            return addressBook;
        }

        private void btnShowAll_Click(object sender, System.EventArgs e)
        {
            listView1.Items.Clear();
            List<string> infoList = Load();
            foreach (var item in infoList)
            {
                string[] info = item.Split(',');
                ListViewItem listItems = new ListViewItem(info);
                listView1.Items.Add(listItems);
            }

        }

        public new void Update()
        {
            try
            {
                if (txtBoxName.Text == "" ||
                    txtBoxStreetName.Text == "" ||
                    txtBoxPostCode.Text == "" ||
                    txtBoxPostName.Text == "" ||
                    txtBoxTelephoneNo.Text == "" ||
                    txtBoxEmail.Text == "")
                {
                    MessageBox.Show("Filla up alla information for att uppdetera.");
                }
                else
                {
                    string name = txtBoxName.Text;
                    string streetName = txtBoxStreetName.Text;
                    string postCode = txtBoxPostCode.Text;
                    string postName = txtBoxPostName.Text;
                    string telephoneNo = txtBoxTelephoneNo.Text;
                    string email = txtBoxEmail.Text;

                    listView1.SelectedItems[0].SubItems[0].Text = name;
                    listView1.SelectedItems[0].SubItems[1].Text = streetName;
                    listView1.SelectedItems[0].SubItems[2].Text = postCode;
                    listView1.SelectedItems[0].SubItems[3].Text = postName;
                    listView1.SelectedItems[0].SubItems[4].Text = telephoneNo;
                    listView1.SelectedItems[0].SubItems[5].Text = email;
                    Save(name, streetName, postCode, postName, telephoneNo, email);

                    List<string> infoList = Load();
                    foreach (var item in infoList)
                    {
                        int itemIndex = 0;
                        if (listView1.Items[itemIndex].SubItems[0].Text == listView1.Items[itemIndex].SubItems[0].Text)
                        {
                            File.WriteAllLines(path,
                                listView1.Items.Cast<ListViewItem>().
                                Select(lvItems => lvItems.Text + "," + lvItems.SubItems[1].Text
                            + "," + lvItems.SubItems[2].Text + "," + lvItems.SubItems[3].Text + "," + lvItems.SubItems[4].Text + "," + lvItems.SubItems[5].Text));
                        }
                    }
                    ClearAll();
                }
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        public void Remove()
        {
            try
            {

                if (listView1.SelectedItems.Count > 0)
                {
                    DialogResult answer = MessageBox.Show("Är du säker att remove?", "Remove", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    if (answer == DialogResult.OK)
                    {
                        foreach (ListViewItem lvItems in listView1.SelectedItems.Cast<ListViewItem>())
                        {
                            listView1.Items.Remove(lvItems);

                        }
                        File.WriteAllLines(path,
                            listView1.Items.Cast<ListViewItem>().
                            Select(lvItems => lvItems.Text + "," + lvItems.SubItems[1].Text
                        + "," + lvItems.SubItems[2].Text + "," + lvItems.SubItems[3].Text + "," + lvItems.SubItems[4].Text + "," + lvItems.SubItems[5].Text));
                    }
                    else if (answer == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Välj information som ska raderas.");
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                // when selected as search result and trying to delete .. it gives error  || you have to select first to delete..
            }
        }

        private void Search()  //Leta efter persons information och skriva ut matchande object i listview
        {
            if (txtBoxSearch.Text == "")
            {
                MessageBox.Show("Filla up information för att söka.");

            }
            else
            {
                listView1.Items.Clear();
                List<string> infoList = Load();
                foreach (var item in infoList)
                {
                    if (item.Contains(txtBoxSearch.Text))
                    {
                        string[] info = item.Split(',');
                        ListViewItem listItem = new ListViewItem(info);
                        listView1.Items.Add(listItem);
                    }
                }
                txtBoxSearch.Clear();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void btnUpdate_Click(object sender, System.EventArgs e)
        {
            Update();
            ClearAll();
        }

        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            Remove();
            ClearAll();
        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            Search();

        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)     //När du väljer en rad i listview visas den i textboxes
        {
            try
            {
                string name = listView1.SelectedItems[0].SubItems[0].Text;
                string streetName = listView1.SelectedItems[0].SubItems[1].Text;
                string postCode = listView1.SelectedItems[0].SubItems[2].Text;
                string postName = listView1.SelectedItems[0].SubItems[3].Text;
                string telephoneNo = listView1.SelectedItems[0].SubItems[4].Text;
                string email = listView1.SelectedItems[0].SubItems[5].Text;

                txtBoxName.Text = name;
                txtBoxStreetName.Text = streetName;
                txtBoxPostCode.Text = postCode;
                txtBoxPostName.Text = postName;
                txtBoxTelephoneNo.Text = telephoneNo;
                txtBoxEmail.Text = email;
            }
            catch (ArgumentOutOfRangeException)
            {
                // När du väljer en tom rad .. kan det visa ett fel.
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
