using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// Delegate for species fact update event.
    /// </summary>
    public delegate void ReferenceUpdateEventHandler(Reference reference);

    /// <summary>
    ///  This class represents a reference.
    /// </summary>
    [Serializable]
    public class Reference : DataId, IListableItem
    {   
        /// <summary>
        /// Event that fires when species facts are updated.
        /// </summary>
        [field: NonSerialized]
        public event ReferenceUpdateEventHandler UpdateEvent;

        private String _name; 
        private Int32 _year; 
        private String _text;

        /// <summary>
        /// Create a Reference instance.
        /// </summary>
        /// <param name='id'>Id for reference.</param>
        /// <param name='name'>Name for reference.</param>
        /// <param name='year'>Year for reference publishing.</param>
        /// <param name='text'>Text for the reference.</param>
        public Reference(Int32 id, String name, Int32 year, String text) : base(id)
        {
            _name = name;
            _year = year;
            _text = text;
        }
        
        /// <summary>
        /// Get name for this reference.
        /// </summary>
        public  String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Get year for this reference. 
        /// </summary>
        public Int32 Year
        {
            get { return _year; }
            set { _year = value; }
        }

        /// <summary>
        /// Get text for this reference.
        /// </summary>
        public String Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// Send information about reference update.
        /// </summary>
        public void FireUpdateEvent()
        {
            if (UpdateEvent != null)
            {
                UpdateEvent.Invoke(this);
            }
        }

        #region IListableItem Members

        /// <summary>
        /// A string usable as a display name
        /// </summary>
        public string Label
        {
            get { return _name; }
        }

        #endregion
    }
}
