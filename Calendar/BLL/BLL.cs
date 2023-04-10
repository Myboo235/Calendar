using Calendar.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calendar.BLL
{
    public class Bll
    {

        public bool AddEvent(Event d)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                int lastID = 0;
                foreach (Event i in db.Events)
                {
                    lastID = i.id;
                }

                d.id = lastID + 1;

                foreach (Event i in db.Events)
                {
                    if (DateTime.Parse(d.dateStart) == DateTime.Parse(i.dateStart) && DateTime.Parse(d.dateFinish) == DateTime.Parse(i.dateFinish))
                    {
                        if (DateTime.Parse(d.timeStart) < DateTime.Parse(i.timeFinish) || DateTime.Parse(d.timeFinish) > DateTime.Parse(i.timeStart))
                        {
                            return false;
                        }
                    }
                    if (DateTime.Parse(d.dateFinish) > DateTime.Parse(i.dateStart) || DateTime.Parse(d.dateStart) < DateTime.Parse(i.dateFinish))
                    {
                        return false;
                    }
                    if (DateTime.Parse(d.dateFinish) == DateTime.Parse(i.dateStart))
                    {
                        if(DateTime.Parse(d.timeFinish) > DateTime.Parse(i.timeStart))
                        {
                            return false;
                        }
                    }
                    if (DateTime.Parse(d.dateStart) == DateTime.Parse(i.dateFinish))
                    {
                        if (DateTime.Parse(i.timeFinish) < DateTime.Parse(i.timeFinish))
                        {
                            return false;
                        }
                    }
                }


                db.Events.InsertOnSubmit(d);
                db.SubmitChanges(); return true;
            }
            
        }

        public bool AddMeeting(Meeting d)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                int lastID = 0;
                foreach (Meeting i in db.Meetings)
                {
                    lastID = i.id;
                }

                d.id = lastID + 1;

                foreach (Meeting i in db.Meetings)
                {
                    if (DateTime.Parse(d.dateStart) == DateTime.Parse(i.dateStart) && DateTime.Parse(d.dateFinish) == DateTime.Parse(i.dateFinish))
                    {
                        if (DateTime.Parse(d.timeStart) < DateTime.Parse(i.timeFinish) || DateTime.Parse(d.timeFinish) > DateTime.Parse(i.timeStart))
                        {
                            return false;
                        }
                    }
                    if (DateTime.Parse(d.dateFinish) > DateTime.Parse(i.dateStart) || DateTime.Parse(d.dateStart) < DateTime.Parse(i.dateFinish))
                    {
                        return false;
                    }
                    if (DateTime.Parse(d.dateFinish) == DateTime.Parse(i.dateStart))
                    {
                        if (DateTime.Parse(d.timeFinish) > DateTime.Parse(i.timeStart))
                        {
                            return false;
                        }
                    }
                    if (DateTime.Parse(d.dateStart) == DateTime.Parse(i.dateFinish))
                    {
                        if (DateTime.Parse(i.timeFinish) < DateTime.Parse(i.timeFinish))
                        {
                            return false;
                        }
                    }
                }

                db.Meetings.InsertOnSubmit(d);
                db.SubmitChanges(); return true;

            }
        }


        public void DeleteEvent(string id)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                foreach (Event i in db.Events)
                {
                    if (i.id.ToString() == id)
                    {

                        db.Events.DeleteOnSubmit(i);
                        db.SubmitChanges();  
                        break;

                    }
                }
            }
        }

        public void DeleteMeeting(string id)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                foreach (Meeting i in db.Meetings)
                {
                    if (i.id.ToString() == id)
                    {

                        db.Meetings.DeleteOnSubmit(i);
                        db.SubmitChanges();
                        break;

                    }
                }
            }
        }


        public void ModifyEvent(Event ev)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                foreach (Event i in db.Events)
                {
                    if (i.id == ev.id)
                    {

                        db.Events.DeleteOnSubmit(i);
                        db.Events.InsertOnSubmit(ev);
                        db.SubmitChanges();
                        break;

                    }
                }
            }
        }

        public void ModifyMeeting(Meeting m)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                foreach (Meeting i in db.Meetings)
                {
                    if (i.id == m.id)
                    {

                        db.Meetings.DeleteOnSubmit(i);
                        db.Meetings.InsertOnSubmit(m);
                        db.SubmitChanges();
                        break;

                    }
                }
            }
        }


        private int idMeetingjoin;
        public bool checkJoinMeeting(Meeting d,string userID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                foreach (Meeting i in db.Meetings)
                {
                    if (DateTime.Parse(d.dateStart) == DateTime.Parse(i.dateStart) && DateTime.Parse(d.dateFinish) == DateTime.Parse(i.dateFinish))
                    {
                        if (DateTime.Parse(d.timeStart) == DateTime.Parse(i.timeStart) && DateTime.Parse(d.timeFinish) == DateTime.Parse(i.timeFinish) && userID != i.hostID.ToString())
                        {
                            idMeetingjoin = i.id;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool joinMeeting(int userID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                int lastID = 0;
                foreach (MeetingMember i in db.MeetingMembers)
                {
                    lastID = i.id;
                }

                

                MeetingMember mb = new MeetingMember
                {
                    id = lastID+1,
                    meetingID = idMeetingjoin,
                    hostID  = userID, 
                    
                };
                db.MeetingMembers.InsertOnSubmit(mb);
                db.SubmitChanges();
            };
            return false;
        }
    }
}
