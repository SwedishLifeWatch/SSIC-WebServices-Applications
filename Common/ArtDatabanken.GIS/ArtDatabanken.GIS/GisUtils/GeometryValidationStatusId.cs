namespace ArtDatabanken.GIS.GisUtils
{
    /// <summary>
    /// Possible Geometry Validation status codes.
    /// </summary>
    public enum GeometryValidationStatusId
    {
        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Valid.
        /// </summary>
        Valid = 24400,
        /// <summary>
        /// Not valid, reason unknown.
        /// </summary>
        NotValidReasonUnknown = 24401,
        /// <summary>
        /// Not valid because point {0} is an isolated point, which is not valid in this type of object.
        /// </summary>
        NotValidBecauseIsolatedPoint = 24402,
        /// <summary>
        /// Not valid because some pair of polygon edges overlap.
        /// </summary>
        NotValidBecausePolygonEdgesOverlap = 24403,
        /// <summary>
        /// Not valid because polygon ring {0} intersects itself or some other ring.
        /// </summary>
        NotValidBecausePolygonRingIntersectsItself = 24404,
        /// <summary>
        /// Not valid because some polygon ring intersects itself or some other ring.
        /// </summary>
        NotValidBecauseSomePolygonRingIntersectsItself = 24405,
        /// <summary>
        /// Not valid because curve {0} degenerates to a point.
        /// </summary>
        NotValidBecauseCurveDegeneratesToAPoint = 24406,
        /// <summary>
        /// Not valid because polygon ring {0} collapses to a line at point {1}.
        /// </summary>
        NotValidBecausePolygonRingCollapsesToALine = 24407,
        /// <summary>
        /// Not valid because polygon ring {0} is not closed.
        /// </summary>
        NotValidBecausePolygonRingIsNotClosed = 24408,
        /// <summary>
        /// Not valid because some portion of polygon ring {0} lies in the interior of a polygon.
        /// </summary>
        NotValidBecausePolygonRingLiesInTheInteriorOfAPolygon = 24409,
        /// <summary>
        /// Not valid because ring {0} is the first ring in a polygon of which it is not the exterior ring.
        /// </summary>
        NotValidBecauseRingIsFirstRingButNotExteriorRing = 24410,
        /// <summary>
        /// Not valid because ring {0} lies outside the exterior ring {1} of its polygon.
        /// </summary>
        NotValidBecauseRingLiesOutsideExteriorRing = 24411,
        /// <summary>
        /// Not valid because the interior of a polygon with rings {0} and {1} is not connected.
        /// </summary>
        NotValidBecauseInteriorOfAPolygonIsNotConnected = 24412,
        /// <summary>
        /// Not valid because of two overlapping edges in curve {0}.
        /// </summary>
        NotValidBecauseTwoOverlappingEdgesInCurve = 24413,
        /// <summary>
        /// Not valid because an edge of curve {0} overlaps an edge of curve {1}.
        /// </summary>
        NotValidBecauseAnEdgeOfCurveOverlapseAnEdgeOfAnotherCurve = 24414,
        /// <summary>
        /// Not valid some polygon has an invalid ring structure.
        /// </summary>
        NotValidBecauseSomePolygonHasInvalidRingStructure = 24415,
        /// <summary>
        /// Not valid because in curve {0} the edge that starts at point {1} is either a line or a degenerate arc with antipodal endpoints.
        /// </summary>
        NotValidBecauseAntipodalEndpoints = 24416        
    }
}