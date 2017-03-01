using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.AnalysisService.Data
{
    public class WebBoundingBoxEqualityComparer : IEqualityComparer<WebBoundingBox>
    {
        public bool Equals(WebBoundingBox b1, WebBoundingBox b2)
        {
            if (b1 == null && b2 == null)
                return true;
            if (b1 == null || b2 == null)
                return false;

            const double epsilon = 0.001;
            if (Math.Abs(b1.Max.X - b2.Max.X) < epsilon && Math.Abs(b1.Max.Y - b2.Max.Y) < epsilon && Math.Abs(b1.Min.X - b2.Min.X) < epsilon && Math.Abs(b1.Min.Y - b2.Min.Y) < epsilon)
                return true;
            return false;
        }

        public int GetHashCode(WebBoundingBox obj)
        {
            if (obj == null)
                return 0;

            int hCode = obj.Max.X.GetHashCode() ^ obj.Max.Y.GetHashCode() ^ obj.Min.X.GetHashCode() ^ obj.Min.Y.GetHashCode();
            return hCode.GetHashCode();
        }

    }
}
