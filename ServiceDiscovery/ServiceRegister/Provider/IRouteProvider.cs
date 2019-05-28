using Ocelot.Configuration.File;
using ServiceRegister.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceRegister.Provider
{
    public interface IRouteProvider<T>
    {
        /// <summary>
        /// 生成路由
        /// </summary>
        /// <param name="apis"></param>
        /// <returns></returns>
        T GenerateRoutes(List<ApiDescriptionOptions> apis);
    }
}
