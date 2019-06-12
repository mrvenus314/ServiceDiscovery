using ServiceRegister.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceRegister.Options
{
    /// <summary>
    /// OData选项
    /// </summary>
    public class ODataOptions
    {
        public List<ODataRoute> _oDataRoutes;
        public List<ODataRoute> ODataRoutes
        {
            get
            {
                if (_oDataRoutes == null)
                    _oDataRoutes = new List<ODataRoute>();

                return _oDataRoutes;
            }
        }
    }

    /// <summary>
    /// OData接口路由规则
    /// </summary>
    public class ODataRoute
    {
        /// <summary>
        /// 路由
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public HttpMethod HttpMethod { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public string Permission { get; set; }
    }

}