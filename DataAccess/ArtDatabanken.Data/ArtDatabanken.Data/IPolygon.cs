using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// A Polygon is a planar Surface, defined by 1 exterior boundary
    /// and 0 or more interior boundaries. Each interior boundary
    /// defines a hole in the Polygon.
    /// 
    /// The assertions for polygons
    /// (the rules that define valid polygons) are:
    /// 
    /// 1. Polygons are topologically closed.
    /// 
    /// 2. The boundary of a Polygon consists of a set of
    /// LinearRings that make up its exterior and interior boundaries.
    /// 
    /// 3. No two rings in the boundary cross, the rings in the
    /// boundary of a Polygon may intersect at a Point but
    /// only as a tangent : ∀ P ∈ Polygon, ∀ c1, c2 ∈ P.Boundary(),
    /// c1 ≠ c2, ∀ p, q ∈ Point, p, q ∈ c1, p ≠ q, [ p ∈ c2 ⇒ q ∉ c2]
    /// 
    /// 4. A Polygon may not have cut lines, spikes or punctures:
    /// ∀ P ∈ Polygon, P = Closure(Interior(P))
    /// 
    /// 5. The Interior of every Polygon is a connected point set.
    /// 
    /// 6. The Exterior of a Polygon with 1 or more holes is not
    /// connected. Each hole defines a connected component of the
    /// Exterior.
    /// 
    /// In the above assertions, Interior, Closure and Exterior have
    /// the standard topological definitions. The combination of 1
    /// and 3 make a Polygon a Regular Closed point set.
    /// Polygons are simple geometries.
    /// </summary>
    public interface IPolygon
    {
        /// <summary>
        /// Linear rings that defines the polygon.
        /// </summary>
        List<ILinearRing> LinearRings
        { get; set; }
    }
}
