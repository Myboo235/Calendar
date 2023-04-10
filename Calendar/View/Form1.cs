using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Calendar.BLL;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Calendar
{
    public partial class Form1 : Form
    {
        
        private User u;
        public Form1(string userName)
        {
            InitializeComponent();
            monthCalendar1.MaxSelectionCount = 1;
            dateTimePicker1.Format = DateTimePickerFormat.Time;
            dateTimePicker1.ShowUpDown = true;
            dateTimePicker2.Format = DateTimePickerFormat.Time;
            dateTimePicker2.ShowUpDown = true;
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {

                foreach (User i in db.Users)
                {
                    if (i.Name == userName)
                    {
                        u = i;
                    }
                }


            }
        }

        private Boolean Check()
        {
            DateTime ds = dateTimePicker3.Value;//date Start
            DateTime df = dateTimePicker4.Value;//date Finish
            DateTime ts = dateTimePicker1.Value;//time Start
            DateTime tf = dateTimePicker2.Value;//time Finish

            if (radioButton1.Checked == false && radioButton2.Checked == false)
            {
                MessageBox.Show("Please choose type!");
                return false;
            }
            if (ds > df)
            {
                MessageBox.Show("Date Start must less than Date Finish");
                return false;
            }
            if (ts >= tf)
            {
                MessageBox.Show("Time Start must less than Time Finish");
                return false;
            }
            if (textBox1.Text == "" || label6.Text != "Write your information")
            {
                MessageBox.Show("Missing Data!");
                return false;
            }


            return true;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            richTextBox1.Text = string.Empty;
            label6.Text = "Write your information";
            label1.Text = monthCalendar1.SelectionRange.Start.ToShortDateString();
            LoadingData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Check())
            {
                string d1 = dateTimePicker3.Value.ToShortDateString();
                string d2 = dateTimePicker4.Value.ToShortDateString();

                string t1 = dateTimePicker1.Value.ToShortTimeString();
                string t2 = dateTimePicker2.Value.ToShortTimeString();
                MessageBox.Show(d1 + "\n" + d2);
                MessageBox.Show(t1 + "\n" + t2);

                //Events
                if (radioButton1.Checked)
                {
                    Event d = new Event
                    {
                        dateStart = d1,
                        dateFinish = d2,
                        timeStart = t1,
                        timeFinish = t2,
                        name = textBox1.Text,
                        description = richTextBox1.Text,
                        hostID = u.id,
                    };

                    Bll b = new Bll();
                    if (b.AddEvent(d)){
                        MessageBox.Show("Save succesfully");
                    }
                    else
                    {
                        const string message = "There is a evant here . Do you add";
                        const string caption = "Joining group";
                        var result = MessageBox.Show(message, caption,
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Question);

                        // If the no button was pressed ...
                        if (result == DialogResult.Yes)
                        {
                            DataClasses1DataContext db = new DataClasses1DataContext();
                            db.Events.InsertOnSubmit(d);
                            db.SubmitChanges();

                            //MessageBox.Show("Join meeting");
                        }
                    }
                }
                //Meeting
                else
                {
                    Meeting d = new Meeting
                    {
                        dateStart = d1,
                        dateFinish = d2,
                        timeStart = t1,
                        timeFinish = t2,
                        name = textBox1.Text,
                        description = richTextBox1.Text,
                        hostID = u.id,
                    };

                    Bll b = new Bll();


                    if (b.checkJoinMeeting(d,u.id.ToString()))
                    {
                        const string message ="There is a meeting here . Do you wanna join";
                        const string caption = "Joining group";
                        var result = MessageBox.Show(message, caption,
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Question);

                        // If the no button was pressed ...
                        if (result == DialogResult.Yes)
                        {
                             b.joinMeeting(u.id);

                            //MessageBox.Show("Join meeting");
                        }
                        else
                        {
                            if (b.AddMeeting(d))
                            {
                                MessageBox.Show("Save succesfully");
                            }
                            else
                            {
                                MessageBox.Show("There is another Event !!!");
                            }
                        }
                    }
                    else if (b.AddMeeting(d))
                    {
                        MessageBox.Show("Save succesfully");
                    }
                    else
                    {
                        MessageBox.Show("There is another Event !!!");
                    }
                }
            }
            LoadingData();

        }

        private void LoadingData()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                dateTimePicker4.Value = monthCalendar1.SelectionRange.Start;
                dateTimePicker3.Value = monthCalendar1.SelectionRange.Start;
                listBox1.Items.Clear();

                if (radioButton1.Checked) 
                {
                    foreach (Event i in db.Events)
                    {
                        if (i.dateStart == label1.Text && i.hostID == u.id)
                        {
                            listBox1.Items.Add(i.name + " from |" + i.timeStart + "| to |" + i.timeFinish + "|" + i.id);
                            richTextBox2.Text = i.description;
                        }
                    }
                }


                if (radioButton2.Checked)
                {
                    foreach (Meeting i in db.Meetings)
                    {
                        if (i.dateStart == label1.Text && i.hostID == u.id)
                        {
                            listBox1.Items.Add(i.name + " from |" + i.timeStart + "| to |" + i.timeFinish + "|" + i.id);
                            richTextBox2.Text = i.description;
                        }
                    }
                }


            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    string[] id = listBox1.SelectedItem.ToString().Split('|');
                    if (radioButton1.Checked)
                    {
                        foreach (Event i in db.Events)
                        {
                            if (i.id.ToString() == id[id.Length-1])
                            {
                                textBox1.Text = i.name;
                                richTextBox1.Text = i.description;
                                richTextBox2.Text = i.description;
                                richTextBox2.Enabled = false;
                                dateTimePicker1.Value = DateTime.Parse(i.timeStart);
                                dateTimePicker2.Value = DateTime.Parse(i.timeFinish);

                                dateTimePicker3.Value = DateTime.Parse(i.dateStart);
                                dateTimePicker4.Value = DateTime.Parse(i.dateFinish);
                            }
                            LoadingData();
                        }
                    }
                }
            }

        }

        //btn modify
        private void button3_Click(object sender, EventArgs e)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                if (listBox1.SelectedItem.ToString() != "")
                {
                    string[] s = listBox1.SelectedItem.ToString().Split('|');
                    string id = s[s.Length - 1];
                    if (radioButton1.Checked)
                    {
                        Bll b = new Bll();
                        Event ev = new Event
                        {
                            id = Int32.Parse(id),
                            name = textBox1.Text,
                            description = richTextBox1.Text,
                            timeStart = dateTimePicker1.Value.ToShortTimeString(),
                            timeFinish = dateTimePicker2.Value.ToShortTimeString(),

                            dateStart = dateTimePicker3.Value.ToShortDateString(),
                            dateFinish = dateTimePicker4.Value.ToShortDateString(),

                            hostID = u.id
                        };
                        b.ModifyEvent(ev);
                    }
                    if (radioButton2.Checked)
                    {
                        Bll b = new Bll();
                        Meeting m = new Meeting
                        {
                            id = Int32.Parse(id),
                            name = textBox1.Text,
                            description = richTextBox1.Text,
                            timeStart = dateTimePicker1.Value.ToShortTimeString(),
                            timeFinish = dateTimePicker2.Value.ToShortTimeString(),

                            dateStart = dateTimePicker3.Value.ToShortDateString(),
                            dateFinish = dateTimePicker4.Value.ToShortDateString(),

                            hostID = u.id
                        };
                        b.ModifyMeeting(m);
                    }

                    LoadingData();
                }

            }
        }

        //btn delete
        private void button4_Click(object sender, EventArgs e)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                if(listBox1.SelectedItem.ToString() != "")
                {
                    string[] s = listBox1.SelectedItem.ToString().Split('|');
                    string id = s[s.Length - 1];
                    if (radioButton1.Checked)
                    {
                        Bll b = new Bll();
                        b.DeleteEvent(id);
                    }
                    if (radioButton2.Checked)
                    {
                        Bll b = new Bll();
                        b.DeleteMeeting(id);
                    }

                    LoadingData();
                }
                
            }
        }
    }
}
