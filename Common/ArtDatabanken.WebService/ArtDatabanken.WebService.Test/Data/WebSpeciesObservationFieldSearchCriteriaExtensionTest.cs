using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Test.Data
{
    [TestClass]
    public class WebSpeciesObservationFieldSearchCriteriaExtensionTest
    {
        [TestMethod]
        public void GetWhereConditionOperatorEqualTest()
        {
            // Arrange
            List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias = new List<WebSpeciesObservationFieldSearchCriteria>();
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
            LogicalOperator fieldLogicalOperator = LogicalOperator.And;
            String actual;
            String expected = @" O.[Id] IN (SELECT DISTINCT SOF.[ObservationId] FROM [SwedishSpeciesObservation].[dbo].[SpeciesObservationField] AS SOF WHERE ([SOF].[Class] = 'Location' AND [SOF].[Property] = 'CoordinateUncertaintyInMeters' AND [SOF].[value_Int] = 1)) ";

            fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Value = "1";
            fieldSearchCriteria.Type = WebDataType.Int32;
            fieldSearchCriteria.Class = new WebSpeciesObservationClass() { Id = SpeciesObservationClassId.Location };
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty() { Id = SpeciesObservationPropertyId.CoordinateUncertaintyInMeters };

            fieldSearchCriterias.Add(fieldSearchCriteria);

            // Act
            actual = fieldSearchCriterias.GetWhereCondition(fieldLogicalOperator);

            // Assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void GetWhereConditionOperatorLessTest()
        {
            // Arrange
            List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias = new List<WebSpeciesObservationFieldSearchCriteria>();
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
            LogicalOperator fieldLogicalOperator = LogicalOperator.And;
            String actual;
            String expected = @" O.[Id] IN (SELECT DISTINCT SOF.[ObservationId] FROM [SwedishSpeciesObservation].[dbo].[SpeciesObservationField] AS SOF WHERE ([SOF].[Class] = 'Location' AND [SOF].[Property] = 'CoordinateUncertaintyInMeters' AND [SOF].[value_Int] < 1)) ";

            fieldSearchCriteria.Operator = CompareOperator.Less;
            fieldSearchCriteria.Value = "1";
            fieldSearchCriteria.Type = WebDataType.Int32;
            fieldSearchCriteria.Class = new WebSpeciesObservationClass() { Id = SpeciesObservationClassId.Location };
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty() { Id = SpeciesObservationPropertyId.CoordinateUncertaintyInMeters };

            fieldSearchCriterias.Add(fieldSearchCriteria);

            // Act
            actual = fieldSearchCriterias.GetWhereCondition(fieldLogicalOperator);

            // Assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void GetWhereConditionOperatorGreaterOrEqualTest()
        {
            // Arrange
            List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias = new List<WebSpeciesObservationFieldSearchCriteria>();
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
            LogicalOperator fieldLogicalOperator = LogicalOperator.And;
            String actual;
            String expected = @" O.[Id] IN (SELECT DISTINCT SOF.[ObservationId] FROM [SwedishSpeciesObservation].[dbo].[SpeciesObservationField] AS SOF WHERE ([SOF].[Class] = 'Location' AND [SOF].[Property] = 'CoordinateUncertaintyInMeters' AND [SOF].[value_Int] >= 1)) ";

            fieldSearchCriteria.Operator = CompareOperator.GreaterOrEqual;
            fieldSearchCriteria.Value = "1";
            fieldSearchCriteria.Type = WebDataType.Int32;
            fieldSearchCriteria.Class = new WebSpeciesObservationClass() { Id = SpeciesObservationClassId.Location };
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty() { Id = SpeciesObservationPropertyId.CoordinateUncertaintyInMeters };

            fieldSearchCriterias.Add(fieldSearchCriteria);

            // Act
            actual = fieldSearchCriterias.GetWhereCondition(fieldLogicalOperator);

            // Assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void GetWhereConditionOperatorBeginsWithTest()
        {
            // Arrange
            List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias = new List<WebSpeciesObservationFieldSearchCriteria>();
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
            LogicalOperator fieldLogicalOperator = LogicalOperator.And;
            String actual;
            String expected = @" O.[Id] IN (SELECT DISTINCT SOF.[ObservationId] FROM [SwedishSpeciesObservation].[dbo].[SpeciesObservationField] AS SOF WHERE ([SOF].[Class] = 'DarwinCore' AND [SOF].[Property] = 'Habitat' AND [SOF].[value_String] LIKE 'skog%')) ";

            fieldSearchCriteria.Operator = CompareOperator.BeginsWith;
            fieldSearchCriteria.Value = "skog";
            fieldSearchCriteria.Type = WebDataType.String;
            fieldSearchCriteria.Class = new WebSpeciesObservationClass() { Id = SpeciesObservationClassId.DarwinCore };
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty() { Id = SpeciesObservationPropertyId.Habitat };

            fieldSearchCriterias.Add(fieldSearchCriteria);

            // Act
            actual = fieldSearchCriterias.GetWhereCondition(fieldLogicalOperator);

            // Assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void GetWhereConditionOperatorNotEqualStringTest()
        {
            // Arrange
            List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias = new List<WebSpeciesObservationFieldSearchCriteria>();
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
            LogicalOperator fieldLogicalOperator = LogicalOperator.And;
            String actual;
            String expected = @" O.[Id] IN (SELECT DISTINCT SOF.[ObservationId] FROM [SwedishSpeciesObservation].[dbo].[SpeciesObservationField] AS SOF WHERE ([SOF].[Class] = 'DarwinCore' AND [SOF].[Property] = 'Habitat' AND [SOF].[value_String] <> 'skog')) ";

            fieldSearchCriteria.Operator = CompareOperator.NotEqual;
            fieldSearchCriteria.Value = "skog";
            fieldSearchCriteria.Type = WebDataType.String;
            fieldSearchCriteria.Class = new WebSpeciesObservationClass() { Id = SpeciesObservationClassId.DarwinCore };
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty() { Id = SpeciesObservationPropertyId.Habitat };

            fieldSearchCriterias.Add(fieldSearchCriteria);

            // Act
            actual = fieldSearchCriterias.GetWhereCondition(fieldLogicalOperator);

            // Assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void GetWhereConditionOperatorNotEqualInt32Test()
        {
            // Arrange
            List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias = new List<WebSpeciesObservationFieldSearchCriteria>();
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
            LogicalOperator fieldLogicalOperator = LogicalOperator.And;
            String actual;
            String expected = @" O.[Id] IN (SELECT DISTINCT SOF.[ObservationId] FROM [SwedishSpeciesObservation].[dbo].[SpeciesObservationField] AS SOF WHERE ([SOF].[Class] = 'Location' AND [SOF].[Property] = 'CoordinateUncertaintyInMeters' AND [SOF].[value_Int] <> 1)) ";

            fieldSearchCriteria.Operator = CompareOperator.NotEqual;
            fieldSearchCriteria.Value = "1";
            fieldSearchCriteria.Type = WebDataType.Int32;
            fieldSearchCriteria.Class = new WebSpeciesObservationClass() { Id = SpeciesObservationClassId.Location };
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty() { Id = SpeciesObservationPropertyId.CoordinateUncertaintyInMeters };

            fieldSearchCriterias.Add(fieldSearchCriteria);

            // Act
            actual = fieldSearchCriterias.GetWhereCondition(fieldLogicalOperator);

            // Assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void GetWhereConditionOperatorLikeStringTwoFieldSearchCriteriasTest()
        {
            // Arrange
            List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias = new List<WebSpeciesObservationFieldSearchCriteria>();
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria2 = new WebSpeciesObservationFieldSearchCriteria();
            LogicalOperator fieldLogicalOperator = LogicalOperator.Or;
            String actual;
            String expected = @" O.[Id] IN (SELECT DISTINCT SOF.[ObservationId] FROM [SwedishSpeciesObservation].[dbo].[SpeciesObservationField] AS SOF WHERE ([SOF].[Class] = 'Event' AND [SOF].[Property] = 'Habitat' AND [SOF].[value_String] LIKE 'bokskog%') OR ([SOF].[Class] = 'Occurrence' AND [SOF].[Property] = 'Substrate' AND [SOF].[value_String] LIKE 'bokskog%')) ";

            fieldSearchCriteria.Operator = CompareOperator.Like;
            fieldSearchCriteria.Value = "bokskog%";
            fieldSearchCriteria.Type = WebDataType.String;
            fieldSearchCriteria.Class = new WebSpeciesObservationClass() { Id = SpeciesObservationClassId.Event };
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty() { Id = SpeciesObservationPropertyId.Habitat };

            fieldSearchCriterias.Add(fieldSearchCriteria);

            fieldSearchCriteria2.Operator = CompareOperator.Like;
            fieldSearchCriteria2.Value = "bokskog%";
            fieldSearchCriteria2.Type = WebDataType.String;
            fieldSearchCriteria2.Class = new WebSpeciesObservationClass() { Id = SpeciesObservationClassId.Occurrence };
            fieldSearchCriteria2.Property = new WebSpeciesObservationProperty() { Id = SpeciesObservationPropertyId.Substrate };

            fieldSearchCriterias.Add(fieldSearchCriteria2);

            // Act
            actual = fieldSearchCriterias.GetWhereCondition(fieldLogicalOperator);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
