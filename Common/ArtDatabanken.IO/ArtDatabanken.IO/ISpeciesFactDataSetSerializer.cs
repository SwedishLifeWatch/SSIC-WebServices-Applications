using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ArtDatabanken.Data;

namespace ArtDatabanken.IO
{
    /// <summary>
    /// The ISpeciesFactDataSetSerializer class contains file
    /// handling of the ISpeciesFactDataSet interface.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class ISpeciesFactDataSetSerializer
    {
        /// <summary>
        /// Read species fact data set from xml file.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name='fileName'>File name.</param>
        /// <param name='dictionaryUri'>Path to files.</param>
        /// <returns>The read species fact data set.</returns>
        public static ISpeciesFactDataSet Deserialize(IUserContext userContext,
                                                      String fileName,
                                                      String dictionaryUri)
        {
            BinaryFormatter formatter;
            ISpeciesFactDataSet speciesFactDataSet;
            Stream stream;

            stream = File.OpenRead(fileName);
            formatter = new BinaryFormatter();
            speciesFactDataSet = (SpeciesFactDataSet)formatter.Deserialize(stream);
            stream.Close();

            //stream = File.OpenRead(dictionaryUri + @"\Factors.bin");
            //FactorList allFactors = ((FactorList)formatter.Deserialize(stream));
            //stream.Close();

            //stream = File.OpenRead(dictionaryUri + @"\FactorTrees.bin");
            //FactorTreeNodeList factorTrees = ((FactorTreeNodeList)formatter.Deserialize(stream));
            //stream.Close();

            //stream = File.OpenRead(dictionaryUri + @"\SpeciesFactQualities.bin");
            //SpeciesFactQualityList speciesFactQualities = ((SpeciesFactQualityList)formatter.Deserialize(stream));
            //stream.Close();

            //stream = File.OpenRead(dictionaryUri + @"\Periods.bin");
            //PeriodList periods = ((PeriodList)formatter.Deserialize(stream));
            //stream.Close();

            // TODO: Do we need this funtionality?
            // TODO: How should it work in the onion design pattern?
            //PeriodManager.InitialisePeriods(periods);
            //SpeciesFactManager.InitialiseSpeciesFactQualities(speciesFactQualities);
            //FactorManager.InitialiseAllFactors(allFactors, factorTrees);
            speciesFactDataSet.InitAutomatedCalculations(userContext);

            return speciesFactDataSet;
        }

        /// <summary>
        /// Save ISpeciesFactDataSet to xml file.
        /// </summary>
        /// <param name='fileName'>File name.</param>
        /// <param name='speciesFactDataSet'>Species fact data set to save.</param>
        /// <param name='dictionaryUri'>Path.</param>
        /// <param name='renewCommonDefinitions'>Indicates if common definitions should be updated.</param>
        public static void Serialize(String fileName,
                                     ISpeciesFactDataSet speciesFactDataSet,
                                     String dictionaryUri,
                                     Boolean renewCommonDefinitions)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            Stream stream = File.OpenWrite(fileName);
            formatter.Serialize(stream, speciesFactDataSet);
            stream.Close();

            // TODO: Do we need this funtionality?
            // TODO: How should it work in the onion design pattern?
            //if (renewCommonDefinitions)
            //{

            //    String factorFileName = dictionaryUri + @"\Factors.bin";
            //    stream = File.OpenWrite(factorFileName);
            //    FactorList allFactors = FactorManager.GetFactors();
            //    formatter.Serialize(stream, allFactors);
            //    stream.Close();

            //    FactorTreeSearchCriteria searchCriteria = new FactorTreeSearchCriteria();
            //    FactorTreeNodeList factorTrees = new FactorTreeNodeList();

            //    foreach (Factor factor in allFactors)
            //    {

            //        List<Int32> factorIds = new List<Int32>();
            //        factorIds.Add(factor.Id);
            //        searchCriteria.RestrictSearchToFactorIds = factorIds;
            //    }


            //    String factorTreeFileName = dictionaryUri + @"\FactorTrees.bin";
            //    stream = File.OpenWrite(factorTreeFileName);
            //    formatter.Serialize(stream, factorTrees);
            //    stream.Close();

            //    String speciesFactQualityFileName = dictionaryUri + @"\SpeciesFactQualities.bin";
            //    stream = File.OpenWrite(speciesFactQualityFileName);
            //    formatter.Serialize(stream, SpeciesFactManager.GetSpeciesFactQualities());
            //    stream.Close();

            //    String periodsFileName = dictionaryUri + @"\Periods.bin";
            //    stream = File.OpenWrite(periodsFileName);
            //    formatter.Serialize(stream, PeriodManager.GetPeriods());
            //    stream.Close();
            //}
        }
    }
}
