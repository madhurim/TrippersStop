using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public interface IBusinessLayer<TInput,TOutput>
    {
        TOutput Process(TInput jsonIn);
    }
}
