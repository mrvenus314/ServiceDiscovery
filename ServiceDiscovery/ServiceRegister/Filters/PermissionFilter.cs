using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceRegister.Filters
{
    /// <summary>
    /// 接口权限过滤器
    /// </summary>
    public class PermissionFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 权限
        /// </summary>
        public string Permission { get; set; }
    }
}
