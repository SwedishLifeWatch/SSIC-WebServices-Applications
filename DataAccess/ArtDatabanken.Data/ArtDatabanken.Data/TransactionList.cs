using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the ITransaction interface.
    /// </summary>
    [Serializable]
    public class TransactionList : DataList<ITransaction>
    {
        /// <summary>
        /// Get/set ITransaction by list index.
        /// </summary>
        public Boolean IsDataSourceInTransaction(IDataSourceInformation dataSourceInformation)
        {
            Boolean isDataSourceInTransaction;

            isDataSourceInTransaction = false;
            if (this.IsNotEmpty())
            {
                foreach (ITransaction transaction in this)
                {
                    if ((transaction.DataSourceInformation.Address == dataSourceInformation.Address) &&
                        (transaction.DataSourceInformation.Name == dataSourceInformation.Name) &&
                        (transaction.DataSourceInformation.Type == dataSourceInformation.Type))
                    {
                        isDataSourceInTransaction = true;
                        break;
                    }
                }
            }

            return isDataSourceInTransaction;
        }
    }
}
