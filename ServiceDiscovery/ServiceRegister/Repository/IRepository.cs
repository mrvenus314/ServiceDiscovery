using Ocelot.Configuration.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceRegister.Repository
{
    public interface IRepository<TResult>
    {
        void Add(string route);

        TResult Get(string key);
    }
}
