using DataSorcererNet.Operations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataSorcererNet
{
    public interface IDataSourceProvider
    {
        object Execute(QueryOperation query);
    }
}
