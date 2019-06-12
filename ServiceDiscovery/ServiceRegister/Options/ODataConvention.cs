using ServiceRegister.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceRegister.Options
{
    /// <summary>
    /// OData
    /// </summary>
    public class ODataConvention : ODataOptions
    {
        private ODataRoute _oDataRoute;

        /// <summary>
        /// 实体
        /// ex: /odata/Payroll
        /// </summary>
        /// <typeparam name="TEntityType"></typeparam>
        /// <param name="name">名称</param>
        /// <param name="httpMethod">请求方式</param>
        /// <param name="permission">权限</param>
        /// <returns></returns>
        public ODataConvention EntitySet<TEntityType>(string name, HttpMethod httpMethod, string permission = null) where TEntityType : class
        {
            _oDataRoute = new ODataRoute() { Route = name, HttpMethod = httpMethod, Permission = permission };
            ODataRoutes.Add(_oDataRoute);

            return this;
        }

        /// <summary>
        /// 计数规则
        /// ex: /odata/Payroll/$Count
        /// </summary>
        /// <returns></returns>
        public ODataConvention Count()
        {
            ODataRoutes.Add(new ODataRoute() { Route = $"{_oDataRoute.Route}/$Count", Permission = _oDataRoute.Permission });

            return this;
        }

        /// <summary>
        /// 过滤规则
        /// ex: /odata/Payroll{everything}
        ///     /odata/Payroll/$Count{everything}
        /// </summary>
        /// <returns></returns>
        public ODataConvention Filter()
        {
            ODataRoutes.Add(new ODataRoute() { Route = $"{_oDataRoute.Route}/$Count{{everything}}", Permission = _oDataRoute.Permission });
            ODataRoutes.Add(new ODataRoute() { Route = $"{_oDataRoute.Route}{{everything}}", Permission = _oDataRoute.Permission });

            return this;
        }
    }
}
