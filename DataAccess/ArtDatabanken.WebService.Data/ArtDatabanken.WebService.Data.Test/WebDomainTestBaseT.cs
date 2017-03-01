// -----------------------------------------------------------------------
// <copyright file="WebLumpSplitEventTest.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WebDomainTestBase<T> where T : class, new() 
    {
       private T t;

       public WebDomainTestBase()
        {
            t = null;
        }

       public T GetObject()
        {
            return GetObject(false);
        }

       public T GetObject(Boolean refresh)
        {
            if (t.IsNull() || refresh)
            {
                t = new T();
            }
            return t;
        }
    }
}
