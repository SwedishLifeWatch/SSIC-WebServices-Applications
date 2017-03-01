using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using AnalysisPortal.Helpers;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Json;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace AnalysisPortal.Controllers
{
#if DEBUG
    public class DoStuffViewModel
    {
        public string Text { get; set; }
    }

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AsyncServiceController : AsyncController
    {
        public Task<ViewResult> Stuff()
        {
            return Task.Factory.StartNew(() => DoStuff("Some other stuff")).ContinueWith(t =>
                {
                    return View(new DoStuffViewModel()
                    {
                        Text = t.Result
                    });
                });
        }

        private string DoStuff(string input)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("DoStuff started: {0}", DateTime.Now.ToLongTimeString()));
            Thread.Sleep(5000);
            return input;
        }

        public JsonNetResult CancelRequest(int? id)
        {
            JsonModel jsonModel;
            
            try
            {
                if (!id.HasValue)
                {
                    if (CancellationDictionary.Keys.Count > 0)
                    {
                        id = CancellationDictionary.Keys.First();
                    }
                    else
                    {
                        jsonModel = JsonModel.CreateFailure("There are no requests to cancel");
                        return new JsonNetResult(jsonModel);
                    }
                }

                if (CancellationDictionary.ContainsKey(id.Value))
                {
                    CancellationDictionary[id.Value].Cancel();
                    System.Diagnostics.Debug.WriteLine(string.Format("GetGridResult cancelled. Id: {0}, Time {1}", id.Value, DateTime.Now.ToLongTimeString()));
                    jsonModel = JsonModel.CreateSuccess(string.Format("Request with Id {0} was cancelled", id.Value));
                }
                else
                {
                    jsonModel = JsonModel.CreateFailure(string.Format("Request with Id {0} was not found", id.Value));    
                }
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }            
            return new JsonNetResult(jsonModel);
        }

        private static int _counter = 0;

        private JsonModel GetGridResult(CancellationToken ct)
        {            
            JsonModel jsonModel;
            try
            {
                if (ct.IsCancellationRequested)
                {
                    return null;
                }
                MySettings mySettings = Session["mySettings"] as MySettings;
//                mySettings.Filter.Taxa.TaxonIds = new ObservableCollection<int>() { 5, 7, 8, 11, 14, 16 };
                //mySettings.Filter.Taxa.TaxonIds = new ObservableCollection<int>() { 5, 7, 8, 11, 14, 16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40 };

                mySettings.Filter.Taxa.IsActive = true;
                IUserContext userContext = CoreData.UserManager.GetCurrentUser();
                IList<IGridCellSpeciesObservationCount> res = GetSpeciesObservationGridCellResultFromWebService(userContext, mySettings);
                SpeciesObservationGridResult model = SpeciesObservationGridResult.Create(res);
                //SpeciesObservationGridResult model = resultsManager.GetSpeciesObservationGridCellResult();
                jsonModel = JsonModel.CreateFromObject(model);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }            
            return jsonModel;
        }

        public IList<IGridCellSpeciesObservationCount> GetSpeciesObservationGridCellResultFromWebService(IUserContext userContext, MySettings mySettings)
        {
            var speciesObservationSearchCriteriaManager = new SpeciesObservationSearchCriteriaManager(userContext);
            SpeciesObservationSearchCriteria searchCriteria = speciesObservationSearchCriteriaManager.CreateSearchCriteria(mySettings);
            ICoordinateSystem coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            GridStatisticsSetting gridStatisticsSetting = mySettings.Calculation.GridStatistics;
            IGridSpecification gridSpecification = new GridSpecification();

            if (gridStatisticsSetting.CoordinateSystemId.HasValue)
            {
                gridSpecification.GridCoordinateSystem = (GridCoordinateSystem)gridStatisticsSetting.CoordinateSystemId;
            }
            else
            {
                gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            }

            if (gridStatisticsSetting.GridSize.HasValue)
            {
                gridSpecification.GridCellSize = gridStatisticsSetting.GridSize.Value;
                gridSpecification.IsGridCellSizeSpecified = true;
            }            
            IList<IGridCellSpeciesObservationCount> gridCellObservations = CoreData.AnalysisManager.GetGridSpeciesObservationCounts(userContext, searchCriteria, gridSpecification, coordinateSystem);

            // Add result to cache                        
            return gridCellObservations;
        }

        public static Dictionary<int, CancellationTokenSource> CancellationDictionary = new Dictionary<int, CancellationTokenSource>();
        
        public Task<JsonNetResult> MyLongRunningProcess()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            _counter++;
            int id = _counter;
            CancellationDictionary.Add(id, tokenSource);
            //tokenSource.Cancel();
            //var t1 = Task<int>.Factory.StartNew(()

            //                => GenerateNumbers(cToken), cToken);

            //// to register a delegate for a callback when a 

            //// cancellation request is made

            //cToken.Register(() => cancelNotification());

            //var task = Task.Factory.StartNew(() => GetGridResult(token), token);            
            //task.ContinueWith(t =>
            //{
            //    return new JsonNetResult(t.Result);                
            //});

            System.Diagnostics.Debug.WriteLine(string.Format("GetGridResult started. Id: {0}, Time {1}", id, DateTime.Now.ToLongTimeString()));
            return Task.Factory.StartNew(() => GetGridResult(token), token).ContinueWith(t =>
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("GetGridResult ended. Id: {0}, Time {1}", id, DateTime.Now.ToLongTimeString()));
                    return new JsonNetResult(t.Result);              
            });
        }

        Random random = new Random();

        public JsonNetResult Query1(string id)
        {
            JsonModel jsonModel;
            try
            {
                var sleepTime = TimeSpan.FromMilliseconds(random.Next(2000, 20000));                
                Thread.Sleep(sleepTime);
                System.Diagnostics.Debug.WriteLine(string.Format("{0} finished after {1} seconds", id, sleepTime.Seconds));
                jsonModel = JsonModel.CreateFromObject(string.Format("{0} finished after {1} seconds", id, sleepTime.Seconds));
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }
            return new JsonNetResult(jsonModel);
        }

        public JsonNetResult GetAvailableWorkerThreads()
        {
            JsonModel jsonModel;
            try
            {
                int availableWorker, availableIO;
                int maxWorker, maxIO;
                ThreadPool.GetAvailableThreads(out availableWorker, out availableIO);
                ThreadPool.GetMaxThreads(out maxWorker, out maxIO);
                jsonModel = JsonModel.CreateFromObject(new { AvailableWorkers = availableWorker, MaxWorkers = maxWorker, AvailableIO = availableIO, MaxIO = maxIO });
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }
            return new JsonNetResult(jsonModel);
        }
    }

#endif
}
