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
        /// <param name="name"></param>
        /// <param name="httpMethod"></param>
        /// <param name="claimType"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        public ODataConvention EntitySet<TEntityType>(string name, HttpMethod httpMethod, string claimType, string permission = null) where TEntityType : class
        {
            _oDataRoute = new ODataRoute() { Route = name, HttpMethod = httpMethod, ClaimType = claimType, Permission = permission };
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
