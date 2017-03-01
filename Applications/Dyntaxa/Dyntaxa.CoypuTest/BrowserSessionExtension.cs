using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coypu;

namespace Dyntaxa.CoypuTest
{
    public static class BrowserSessionExtension
    {
        public static ElementScope SearchButtonUpper(this BrowserSession browserSession)
        {
            return browserSession.FindXPath("//*[@id='upperSearchArea']/div/table/tbody/tr/td[2]/button");
        }
        public static ElementScope SearchButtonLower(this BrowserSession browserSession)
        {
            return browserSession.FindXPath("//*[@id='fullContainer']/fieldset[2]/div/button");
        }

        public static ElementScope SearchButtonReset(this BrowserSession browserSession)
        {
            return browserSession.FindXPath("//*[@id='fullContainer']/fieldset[2]/div/a");
        }

        public static ElementScope SearchTextBox(this BrowserSession browserSession)
        {
            return browserSession.FindXPath("//*[@id='SearchString']");
        }

        public static ElementScope TabSearch(this BrowserSession browserSession)
        {
            return browserSession.FindXPath("//*[@id='menuContainer']/ul/li[1]/a");
        }

        public static ElementScope TabTaxonInfo(this BrowserSession browserSession)
        {
            return browserSession.FindXPath("//*[@id='menuContainer']/ul/li[2]/a");
        }

        public static ElementScope TabRevisions(this BrowserSession browserSession)
        {
            return browserSession.FindXPath("//*[@id='menuContainer']/ul/li[3]/a");
        }

        public static ElementScope TabMatch(this BrowserSession browserSession)
        {
            return browserSession.FindXPath("//*[@id='menuContainer']/ul/li[4]/a");
        }

        public static ElementScope TabExport(this BrowserSession browserSession)
        {
            return browserSession.FindXPath("//*[@id='menuContainer']/ul/li[5]/a");
        }

        public static ElementScope TabExportStraightList(this BrowserSession browserSession)
        {
            return browserSession.FindXPath("//*[@id='menuContainer']/ul/li[5]/ul[1]/li[1]/a");
        }

        public static ElementScope TabExportHierarchyList(this BrowserSession browserSession)
        {
            return browserSession.FindXPath("//*[@id='menuContainer']/ul/li[5]/ul[1]/li[2]/a");
        }

        public static void ExecuteSearchTaxon(this BrowserSession browserSession, string taxonString)
        {
            browserSession.TabSearch().Click();
            browserSession.SearchTextBox().FillInWith(taxonString);
            browserSession.SearchButtonUpper().Click();
            Thread.Sleep(1000);
            browserSession.TabTaxonInfo().Click();
            Thread.Sleep(1000);
            browserSession.TabRevisions().Click();
            Thread.Sleep(1000);
            browserSession.TabMatch().Click();
            Thread.Sleep(1000);
            browserSession.TabExport().Hover();
            Thread.Sleep(1000);
            //browserSession.TabExportStraightList().Click();
            //Thread.Sleep(1000);
            //browserSession.TabExport().Hover();
            //Thread.Sleep(1000);
            //browserSession.TabExportHierarchyList().Click();
            //Thread.Sleep(1000);

        }
    }
}
