using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions
{
    public static class ListExtension
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this List<T> list)
        {
            var collection = new ObservableCollection<T>(list);
            return collection;
        }
    }
}
