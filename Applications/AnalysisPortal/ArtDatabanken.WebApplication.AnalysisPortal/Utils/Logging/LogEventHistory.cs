using System;
using System.Collections.Generic;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Logging
{
    public class LogEventHistoryItem
    {
        public int? TaxonId { get; set; }
        public int? RevisionId { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string HttpAction { get; set; }
        public string Url { get; set; }
        public string Form { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public DateTime Date { get; set; }
        public string Referrer { get; set; }
    }

    public class LogEventHistory
    {
        private readonly List<LogEventHistoryItem> _historyItems;

        public List<LogEventHistoryItem> HistoryItems
        {
            get { return _historyItems; }
        }

        public LogEventHistory()
        {
            _historyItems = new List<LogEventHistoryItem>();
        }
    }
}
