using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Contains condition on data that is returned.
    /// This class is used in WebDataQuery handling.
    /// Only one of the XXXCondition properties should be used.
    /// </summary>
    [DataContract]
    public class WebDataCondition : WebData
    {
        /// <summary>
        /// Create a WebDataCondition instance.
        /// </summary>
        public WebDataCondition()
        {
            DataLogicCondition = null;
            SpeciesFactCondition = null;
        }

        /// <summary>Logic condition on WebDataQuery instances.</summary>
        [DataMember]
        public WebDataLogicCondition DataLogicCondition
        { get; set; }

        /// <summary>Condition on SpeciesFact data.</summary>
        [DataMember]
        public WebSpeciesFactCondition SpeciesFactCondition
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        public override void CheckData()
        {
            base.CheckData();
            // Check that exactly one condition is set.
            if (DataLogicCondition.IsNull())
            {
                SpeciesFactCondition.CheckNotNull("SpeciesFactCondition");
            }
            else
            {
                SpeciesFactCondition.CheckNull("SpeciesFactCondition");
            }

            // Check data.
            if (DataLogicCondition.IsNotNull())
            {
                DataLogicCondition.CheckData();
            }
            if (SpeciesFactCondition.IsNotNull())
            {
                SpeciesFactCondition.CheckData();
            }
        }
    }
}
