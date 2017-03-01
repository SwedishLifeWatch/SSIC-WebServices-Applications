using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.IO
{
    /// <summary>
    /// The UserDataSetSerializer class contains file
    /// handling of the UserDataSet class.
    /// </summary>
    public class UserDataSetSerializer
    {
        /// <summary>
        /// Save UserDataSet to xml file.
        /// </summary>
        /// <param name='fileName'>File name.</param>
        /// <param name='userDataSet'>UserDataSet to save.</param>
        /// <param name='dictionaryURI'>Path.</param>
        /// <param name='renewCommonDefinitions'>Indicates if common definitions should be updated.</param>
        public static void Serialize(String fileName, UserDataSet userDataSet, String dictionaryURI, Boolean renewCommonDefinitions)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            Stream stream = File.OpenWrite(fileName);            
            formatter.Serialize(stream, userDataSet);
            stream.Close();

            if (renewCommonDefinitions)
            {

                String factorFileName = dictionaryURI + @"\Factors.bin";
                stream = File.OpenWrite(factorFileName);
                FactorList allFactors = FactorManager.GetFactors();
                formatter.Serialize(stream, allFactors);
                stream.Close();

                FactorTreeSearchCriteria searchCriteria = new FactorTreeSearchCriteria();
                FactorTreeNodeList factorTrees = new FactorTreeNodeList();

                foreach (Factor factor in allFactors)
                {
                    
                    List<Int32> factorIds = new List<Int32>();
                    factorIds.Add(factor.Id);
                    searchCriteria.RestrictSearchToFactorIds = factorIds;
                }
       

                String factorTreeFileName = dictionaryURI + @"\FactorTrees.bin";
                stream = File.OpenWrite(factorTreeFileName);
                formatter.Serialize(stream, factorTrees);
                stream.Close();

                String speciesFactQualityFileName = dictionaryURI + @"\SpeciesFactQualities.bin";
                stream = File.OpenWrite(speciesFactQualityFileName);
                formatter.Serialize(stream, SpeciesFactManager.GetSpeciesFactQualities());
                stream.Close();

                String periodsFileName = dictionaryURI + @"\Periods.bin";
                stream = File.OpenWrite(periodsFileName);
                formatter.Serialize(stream, PeriodManager.GetPeriods());
                stream.Close();
            }            
        }

        /// <summary>
        /// Read UserDataSet from xml file.
        /// </summary>
        /// <param name='fileName'>File name.</param>
        /// <param name='dictionaryURI'>Path.</param>
        /// <returns>The read UserDataSet.</returns>
        public static UserDataSet Deserialize(String fileName, String dictionaryURI)
        {
            Stream stream = File.OpenRead(fileName);
            BinaryFormatter formatter = new BinaryFormatter();
            UserDataSet userDataSet = (UserDataSet)formatter.Deserialize(stream);
            stream.Close();


            stream = File.OpenRead(dictionaryURI + @"\Factors.bin");
            FactorList allFactors = ((FactorList)formatter.Deserialize(stream));
            stream.Close();

            stream = File.OpenRead(dictionaryURI + @"\FactorTrees.bin");
            FactorTreeNodeList factorTrees = ((FactorTreeNodeList)formatter.Deserialize(stream));
            stream.Close();

            stream = File.OpenRead(dictionaryURI + @"\SpeciesFactQualities.bin");
            SpeciesFactQualityList speciesFactQualities = ((SpeciesFactQualityList)formatter.Deserialize(stream));
            stream.Close();

            stream = File.OpenRead(dictionaryURI + @"\Periods.bin");
            PeriodList periods = ((PeriodList)formatter.Deserialize(stream));
            stream.Close();

            PeriodManager.InitialisePeriods(periods);
            SpeciesFactManager.InitialiseSpeciesFactQualities(speciesFactQualities);
            FactorManager.InitialiseAllFactors(allFactors, factorTrees);
            SpeciesFactManager.InitAutomatedCalculations(userDataSet.SpeciesFacts);  

            return userDataSet;
        }

    }
}
