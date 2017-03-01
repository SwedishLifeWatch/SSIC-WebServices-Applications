using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.WebApplication.AnalysisPortal.Result;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Improved
{
    public class ImprovedMySettingsSummaryManager
    {
        public MySettingsSummaryViewModel Create(ResultType resultType)
        {
            return null;
        }

        public MySettingsSummaryItemBase CreateItem(MySettingsSummaryItemIdentifier identifier, ResultType resultType)
        {
            Dictionary<ResultType, List<MySettingsSummaryItemIdentifier>> dicSettingsUsed = new Dictionary<ResultType, List<MySettingsSummaryItemIdentifier>>();
            dicSettingsUsed.Add(
                ResultType.SpeciesObservationGridMap,
                MySettingsSummaryItemIdentifierManager.GetDefaultSummaryItemIdentifiersExcept(
                    MySettingsSummaryItemIdentifier.CalculationGridStatisticsSubEnvironment,
                    MySettingsSummaryItemIdentifier.CalculationTimeSeries));

            ////switch (identifier)
            ////{
            ////    case ResultType.SpeciesObservationGridMap:
            ////}

            return null;
        }

        ////private List<MySettingsSummaryItemIdentifier> GetAllSummaryItemIdentifiers()
        ////{            
        ////    return Enum.GetValues(typeof(MySettingsSummaryItemIdentifier)).Cast<MySettingsSummaryItemIdentifier>().ToList();
        ////}

        ////private List<MySettingsSummaryItemIdentifier> GetDefaultSummaryItemIdentifiers()
        ////{
        ////    return GetAllSummaryItemIdentifiersExcept(
        ////        MySettingsSummaryItemIdentifier.DataEnvironmentalData,
        ////        MySettingsSummaryItemIdentifier.DataMapLayers,
        ////        MySettingsSummaryItemIdentifier.PresentationMap,
        ////        MySettingsSummaryItemIdentifier.PresentationReport,
        ////        MySettingsSummaryItemIdentifier.PresentationTable);
        ////}

        ////private List<MySettingsSummaryItemIdentifier> GetDefaultSummaryItemIdentifiersExcept(params MySettingsSummaryItemIdentifier[] list)
        ////{
        ////    List<MySettingsSummaryItemIdentifier> defaultSummaryItemIdentifiers = GetDefaultSummaryItemIdentifiers();
        ////    return defaultSummaryItemIdentifiers.Except(list).ToList();
        ////}

        ////private List<MySettingsSummaryItemIdentifier> GetAllSummaryItemIdentifiersExcept(params MySettingsSummaryItemIdentifier[] list)
        ////{
        ////    List<MySettingsSummaryItemIdentifier> summaryIdentifiers = GetAllSummaryItemIdentifiers();
        ////    return summaryIdentifiers.Except(list).ToList();
        ////}
    }

    public static class MySettingsSummaryResultTypeManager
    {
        private static List<ResultType> _allResultTypes;
        public static List<ResultType> AllResultTypes
        {
            get
            {
                if (_allResultTypes == null)
                {
                    _allResultTypes = GetAllResultTypes();
                }

                return _allResultTypes;
            }
        }

        public static List<ResultType> GetAllResultTypesExcept(params ResultType[] list)
        {
            return AllResultTypes.Except(list).ToList();
        }

        ////public static bool CanSettingAffectResult(ResultType resultType, MySettingsSummaryItemIdentifier identifier)
        ////{
        ////    List<MySettingsSummaryItemIdentifier> settingsList = GetSettingsList(resultType);
        ////    return settingsList.Contains(identifier);
        ////}

        private static List<ResultType> GetAllResultTypes()
        {
            return Enum.GetValues(typeof(ResultType)).Cast<ResultType>().ToList();
        }
    }

    public static class MySettingsSummaryItemIdentifierManager
    {
        private static List<MySettingsSummaryItemIdentifier> _allSummaryItemIdentifiers;
        public static List<MySettingsSummaryItemIdentifier> AllSummaryItemIdentifiers
        {
            get
            {
                if (_allSummaryItemIdentifiers == null)
                {
                    _allSummaryItemIdentifiers = GetAllSummaryItemIdentifiers();
                }

                return _allSummaryItemIdentifiers;
            }
        }

        private static List<MySettingsSummaryItemIdentifier> _defaultSummaryItemIdentifiers;
        public static List<MySettingsSummaryItemIdentifier> DefaultSummaryItemIdentifiers
        {
            get
            {
                if (_defaultSummaryItemIdentifiers == null)
                {
                    _defaultSummaryItemIdentifiers = GetDefaultSummaryItemIdentifiers();
                }

                return _defaultSummaryItemIdentifiers;
            }
        }

        public static List<MySettingsSummaryItemIdentifier> GetDefaultSummaryItemIdentifiersExcept(params MySettingsSummaryItemIdentifier[] list)
        {
            ////List<MySettingsSummaryItemIdentifier> defaultSummaryItemIdentifiers = GetDefaultSummaryItemIdentifiers();
            return DefaultSummaryItemIdentifiers.Except(list).ToList();
        }

        public static List<MySettingsSummaryItemIdentifier> GetAllSummaryItemIdentifiersExcept(params MySettingsSummaryItemIdentifier[] list)
        {
            return AllSummaryItemIdentifiers.Except(list).ToList();
        }

        private static List<MySettingsSummaryItemIdentifier> GetAllSummaryItemIdentifiers()
        {
            return Enum.GetValues(typeof(MySettingsSummaryItemIdentifier)).Cast<MySettingsSummaryItemIdentifier>().ToList();
        }

        private static List<MySettingsSummaryItemIdentifier> GetDefaultSummaryItemIdentifiers()
        {
            return GetAllSummaryItemIdentifiersExcept(
                MySettingsSummaryItemIdentifier.DataEnvironmentalData,
                MySettingsSummaryItemIdentifier.DataMapLayers,
                MySettingsSummaryItemIdentifier.PresentationMap,
                MySettingsSummaryItemIdentifier.PresentationReport,
                MySettingsSummaryItemIdentifier.PresentationTable);
        }

        private static Dictionary<ResultType, List<MySettingsSummaryItemIdentifier>> _resultTypeSettings;
        public static Dictionary<ResultType, List<MySettingsSummaryItemIdentifier>> ResultTypeSettings
        {
            get
            {
                if (_resultTypeSettings == null)
                {
                    _resultTypeSettings = CreateResultTypeSettingsDictionary();
                }
                return _resultTypeSettings;
            }
        }

        private static Dictionary<ResultType, List<MySettingsSummaryItemIdentifier>> CreateResultTypeSettingsDictionary()
        {
            Dictionary<ResultType, List<MySettingsSummaryItemIdentifier>> resultTypeSettingsDictionary = new Dictionary<ResultType, List<MySettingsSummaryItemIdentifier>>();
            resultTypeSettingsDictionary.Add(
                ResultType.SpeciesObservationGridMap,
                GetDefaultSummaryItemIdentifiersExcept(
                    MySettingsSummaryItemIdentifier.CalculationGridStatisticsSubEnvironment,
                    MySettingsSummaryItemIdentifier.CalculationTimeSeries));

            resultTypeSettingsDictionary.Add(
                ResultType.SettingsSummary,
                GetDefaultSummaryItemIdentifiersExcept(
                    MySettingsSummaryItemIdentifier.CalculationGridStatisticsSubEnvironment,
                    MySettingsSummaryItemIdentifier.CalculationTimeSeries,
                    MySettingsSummaryItemIdentifier.DataProviders));

            return resultTypeSettingsDictionary;
        }

        public static List<MySettingsSummaryItemIdentifier> GetSettingsList(ResultType resultType)
        {
            return ResultTypeSettings[resultType];
        }

        public static bool CanSettingAffectResult(ResultType resultType, MySettingsSummaryItemIdentifier identifier)
        {
            List<MySettingsSummaryItemIdentifier> settingsList = GetSettingsList(resultType);
            return settingsList.Contains(identifier);
        }

        public static bool CanSubsettingAffectResult(ResultType resultType, MySettingsSummaryItemSubIdentifier subIdentifier)
        {            
            throw new NotImplementedException();
        }

        public static List<MySettingsSummaryItemSubIdentifier> GetSubsettingsThatCanAffectResult(ResultType resultType, List<MySettingsSummaryItemSubIdentifier> subIdentifiers)
        {
            throw new NotImplementedException();
        }

        public static bool CanSubsettingAffectResult2(ResultType resultType, MySettingsSummaryItemIdentifier subIdentifier)
        {
            List<MySettingsSummaryItemIdentifier> settingsList = GetSettingsList(resultType);
            return settingsList.Contains(subIdentifier);
        }

        public static List<MySettingsSummaryItemIdentifier> GetSubsettingsThatCanAffectResult2(ResultType resultType, List<MySettingsSummaryItemIdentifier> subIdentifiers)
        {
            List<MySettingsSummaryItemIdentifier> settingsList = GetSettingsList(resultType);
            List<MySettingsSummaryItemIdentifier> result = new List<MySettingsSummaryItemIdentifier>();
            foreach (MySettingsSummaryItemIdentifier identifier in subIdentifiers)
            {
                if (settingsList.Contains(identifier))
                {
                    result.Add(identifier);
                }
            }

            return result;
        }
    }

    public class MySettingsSummaryItemIdentifierModel
    {
        public MySettingsSummaryItemIdentifier Identifier { get; set; }
        public List<MySettingsSummaryItemSubIdentifier> SubIdentifiers { get; set; }

        public MySettingsSummaryItemIdentifierModel(MySettingsSummaryItemIdentifier identifier)
        {
            Identifier = identifier;
        }

        public MySettingsSummaryItemIdentifierModel(MySettingsSummaryItemIdentifier identifier, List<MySettingsSummaryItemSubIdentifier> subIdentifiers)
        {
            Identifier = identifier;
            SubIdentifiers = subIdentifiers;
        }
    }
}
