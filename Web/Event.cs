using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using ASPXUtils;
using WebSite.Core;

namespace WebSite.Core
{
    public class Event : ICloneable, ILinkable
    {


        #region Properties
        private int _id;
        public int ID
        {
            get { return   _id; }
            set { _id = value; }
        }

        private string _loc;
        public string LOC
        {
            get { return _loc == null ? "" : _loc; }
            set { _loc = value; }
        }
        private string _shortName;
        public string ShortName
        {
            get
            {
                _shortName = this.Name;
                if (_shortName.Length > 40)
                    _shortName = _shortName.Substring(1, 40) + "...";

                return _shortName;
            }

        }
        private string _ShortDesc;
        public string ShortDesc
        {
            get { return _ShortDesc == null ? "" : _ShortDesc; }
            set { _ShortDesc = value; }
        }
        private string _name;
        public string Name
        {
            get { return _name == null ? "" : _name; }
            set { _name = value; }
        }

        private string _description;
        public string Description
        {
            get { return _description == null ? "" : _description; }
            set { _description = value; }
        }

        private string _imageurl;
        public string ImageURL
        {
            get { return _imageurl == null ? "" : _imageurl; }
            set { _imageurl = value; }
        }

        private DateTime _eventstartdate;
        public DateTime EventStartDate
        {
            get { return _eventstartdate; }
            set { _eventstartdate = value; }
        }

        private DateTime _eventenddate;
        public DateTime EventEndDate
        {
            get { return _eventenddate; }
            set { _eventenddate = value; }
        }

        private int _recurdays;
        public int RecurDays
        {
            get { return   _recurdays; }
            set { _recurdays = value; }
        }

        private int _recurdow;
        public int RecurDOW
        {
            get { return  _recurdow; }
            set { _recurdow = value; }
        }

        private string _dateDetail;
        public string DateDetail
        {
            get
            {

                if (this.RecurDOW >= 0)
                    _dateDetail = "Every " + DateUtils.GetDOW(this.RecurDOW);
                else
                    _dateDetail = this.EventStartDate.ToLongDateString();
                return _dateDetail;
            }
        }

        public string StartTime
        {
            get { return DateUtils.ParseTime(Convert.ToString(this.EventStartDate)); }
        }
        public string EndTime
        {
            get { return DateUtils.ParseTime(Convert.ToString(this.EventEndDate)); }
        }

        private string _active;
        public string Active
        {
            get { return _active == null ? "1" : _active; }
            set { _active = value; }
        }

        private string _featured;
        public string Featured
        {
            get { return _featured == null ? "1" : _featured; }
            set { _featured = value; }
        }

        private int _sortorder;
        public int SortOrder
        {
            get { return _sortorder; }
            set { _sortorder = value; }
        }

        private List<Event> _EventCollection;
        public List<Event> EventCollection
        {
            get { return _EventCollection; }
            set { _EventCollection = value; }
        } 
        #endregion
        #region Methods
        public int Insert()
        {
            return Insert(this);
        }
        public int Insert(Event objEvent)
        {
            return EventDAL.Add(objEvent);
        }

        public void Update()
        {
            Update(this);
        }
        public void Update(Event objEvent)
        {
            EventDAL.Update(objEvent);
        }

        public void Delete()
        {
            Delete(this);
        }
        public void Delete(Event objEvent)
        {
            EventDAL.Delete(objEvent.ID);
        }

        public Event Select(int ID)
        {
            var oResult = ((from o in SelectAll().AsQueryable() where o.ID == ID select o).Take(1)).ToList();
            return (oResult.Count > 0) ? oResult[0] : null;
        }
        public int SelectCount()
        {
            return SelectAll().Count();
        }
        public List<Event> SelectFeatured()
        {
            var oResult = ((from o in SelectAll().AsQueryable()
                            where o.EventStartDate > DateTime.Now
                            && o.Active == "True"
                            orderby o.EventStartDate descending
                            select o).Take(5)).ToList();

            return (oResult.Count > 0) ? oResult : null;
        }
        public List<Event> SelectMonth(DateTime _FromDate)
        {
            List<Event> CurrentEvents;
            List<Event> NonRecurEvents;
            List<Event> RecurEvents;
            DateTime _StartDate;
            DateTime _EndDate;
            DateTime _CurrentDate;

            CurrentEvents = SelectAll();

            _StartDate = _FromDate.AddMonths(-1);
            _EndDate = _FromDate.AddMonths(1);

            NonRecurEvents = CurrentEvents.Where(o => o.RecurDOW < 0).ToList();
            RecurEvents = CurrentEvents.Where(o => o.RecurDOW >= 0).ToList();

            _CurrentDate = _StartDate;

            while (_CurrentDate < _EndDate)
            {
                _CurrentDate = _CurrentDate.AddDays(1);
                foreach (Event ev in RecurEvents)
                {
                    if (Convert.ToString(_CurrentDate.DayOfWeek) == DateUtils.GetDOW(ev.RecurDOW))
                    {
                        Event nEv = (Event)ev.Clone();
                        nEv.EventStartDate = Convert.ToDateTime(DateUtils.ParseDate(Convert.ToString(_CurrentDate)) + " " + DateUtils.ParseTime(Convert.ToString(nEv.EventStartDate)));
                        nEv.EventEndDate = Convert.ToDateTime(DateUtils.ParseDate(Convert.ToString(_CurrentDate)) + " " + DateUtils.ParseTime(Convert.ToString(nEv.EventEndDate)));
                        NonRecurEvents.Add(nEv);
                    }
                }
            }
            return NonRecurEvents.Count > 0 ? NonRecurEvents : new List<Event>();
        }

        public List<Event> SelectAllActive()
        {
            return (List<Event>)SelectAll().Where(o => o.Active == "True").ToList();
        }
        public List<Event> SelectAllByLoc(string LOC)
        {
            return (List<Event>)SelectAll().Where(o => o.LOC == LOC).ToList();
        }
        public List<Event> SelectAll()
        {
            if (this.EventCollection == null)
                this.EventCollection = new EventDAL().GetAll();
            return this.EventCollection;
        }

        public List<Event> GetCurrent()
        {
            List<Event> CurrentEvents = (from ev in SelectMonth(DateTime.Now)
                                         where ev.EventStartDate >= DateTime.Now
                                         orderby ev.EventStartDate ascending
                                         select ev).ToList();
            return CurrentEvents;
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

 
        #region ILinkable Members

        private string caption;
        public string Caption
        {
            get
            {
                if (this.caption == null)
                    this.caption = this.EventStartDate.ToShortDateString() + " - " + HTTPUtils.ToDirFriendly(this.Name);
                return this.caption;
            }
            set { caption = value; }
        }
        


        public string URL
        {
            get
            {
                string _URL = NavUtil.URLRoot() + "/" + WebPage.GetNameBySysControl("Event") + SiteSettings.Instance.SubDirSep + HTTPUtils.ToDirFriendly(this.Caption);

                if (SiteSettings.Instance.UseIDInUrl)
                    _URL += SiteSettings.Instance.IDSep + this.ID.ToString();
                
                if (!SiteSettings.Instance.UseExtentionLessParentPages)
                    _URL = _URL + SiteSettings.Instance.PageExtention;
                return _URL;
            }
        }

        public string SystemURL { get { return ""; } set { this.SystemURL = value; } }

  

        #endregion
    }


}