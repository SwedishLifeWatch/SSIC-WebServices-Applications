//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Reference
//{
//    public abstract class ReferenceModel
//    {
//        private IRetrieveReferenceRelationsBehavior retrieveReferenceRelationsBehavior;

//        public void RetrieveReferenceRelations()
//        {
//            retrieveReferenceRelationsBehavior.AlgorithmInterface();
//        }

//        //private RetrieveReferenceRelationsStrategy retrieveReferenceRelationsStrategy;
//    }

//    /// <summary>
//    /// The 'Strategy' abstract class
//    /// </summary>
//    interface IRetrieveReferenceRelationsBehavior
//    {
//        void AlgorithmInterface();
//    }

//    /// <summary>
//    /// A 'ConcreteStrategy' class
//    /// </summary>
//    class RetrieveReferenceRelationsFromDyntaxaDbBehavior : IRetrieveReferenceRelationsBehavior
//    {
//        public void AlgorithmInterface()
//        {
//            Console.WriteLine(
//              "Called ConcreteStrategyA.AlgorithmInterface()");
//        }
//    }

//    class RetrieveReferenceRelationsFromReferenceServiceBehavior : IRetrieveReferenceRelationsBehavior
//    {
//        public void AlgorithmInterface()
//        {
//            Console.WriteLine(
//              "Called ConcreteStrategyB.AlgorithmInterface()");
//        }
//    }
//}
