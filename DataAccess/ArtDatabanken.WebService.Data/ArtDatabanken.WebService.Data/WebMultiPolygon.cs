using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// A MultiPolygon is a geometric collection whose elements
    /// are polygons. The interiors of any two polygons in a
    /// MultiPolygon may not intersect. The boundaries of any two
    /// elements in a MultiPolygon may intersect at most at a
    /// finite number of points.
    /// 
    /// The assertions for MultiPolygons are:
    /// 
    /// 1. The interiors of 2 Polygons that are elements of a
    /// MultiPolygon may not intersect. ∀ M ∈ MultiPolygon, ∀ Pi,
    /// Pj ∈ M.Geometries(), i≠j, Interior(Pi) ∩ Interior(Pj) = ∅
    /// 
    /// 2. The Boundaries of any 2 Polygons that are elements of
    /// a MultiPolygon may not ‘cross’ and may touch at only a
    /// finite number of points. (Note that crossing is prevented
    /// by assertion 1 above). ∀ M ∈ MultiPolygon, ∀ Pi,
    /// Pj ∈ M.Geometries(), ∀ ci ∈ Pi.Boundaries(),
    /// cj ∈ Pj.Boundaries() ci ∩ cj = {p1, ….., pk | pi ∈ Point,
    /// 1 &lt;= i &lt;= k}
    /// 
    /// 3. A MultiPolygon is defined as topologically closed.
    /// 
    /// 4. A MultiPolygon may not have cut lines, spikes or punctures,
    /// a MultiPolygon is a Regular, Closed point set:
    /// ∀ M ∈ MultiPolygon, M = Closure(Interior(M))
    /// 
    /// 5. The interior of a MultiPolygon with more than 1 Polygon
    /// is not connected, the number of connected components of the
    /// interior of a MultiPolygon is equal to the number of Polygons
    /// in the MultiPolygon.
    /// 
    /// The boundary of a MultiPolygon is a set of closed curves
    /// (LinearRings) corresponding to the boundaries of
    /// its element Polygons. Each Curve in the boundary of the
    /// MultiPolygon is in the boundary of exactly 1 element Polygon,
    /// and every Curve in the boundary of an element Polygon is in
    /// the boundary of the MultiPolygon.
    /// </summary>
    [DataContract]
    public class WebMultiPolygon : WebData
    {
        /// <summary>
        /// Polygons that defines the multi polygon.
        /// </summary>
        [DataMember]
        public List<WebPolygon> Polygons
        { get; set; }
    }
}
