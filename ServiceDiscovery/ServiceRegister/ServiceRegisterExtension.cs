using AutoMapper;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Ocelot.Configuration.File;
using ServiceRegister.Consts;
using ServiceRegister.Filters;
using ServiceRegister.Options;
using ServiceRegister.Provider;
using ServiceRegister.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceRegister
{
    /// <summary>
    /// 注册服务
    /// </summary>
    public static class ServiceRegisterExtension
    {
        /// <summary>
        /// api列表
        /// </summary>
        public static List<ApiDescriptionOptions> ApiList;        

        /// <summary>
        /// 注册api
        /// </summary>
        /// <param name="app"></param>
        /// <param name="apiDescriptionGroupCollectionProvider"></param>
        public static IApplicationBuilder UseApiRegister(this IApplicationBuilder app, IApiDescriptionGroupCollectionProvider apiDescriptionGroupCollectionProvider)
        {
            var apis = apiDescriptionGroupCollectionProvider;

            ApiList = new List<ApiDescriptionOptions>();

            foreach (var api in apis.ApiDescriptionGroups.Items)
            {
                foreach (var action in api.Items)
                {
                    var desc = Mapper.Instance.Map<ApiDescription, ApiDescriptionOptions>(action, opt => opt.ConfigureMap().ForMember(dest => dest.Permission, o => o.Ignore()));

                    var filter = action.ActionDescriptor.FilterDescriptors.FirstOrDefault(l => l.Filter.GetType().Name == ActionFilterConst.PERMISSIONFILTER);
                    if (filter != null)
                    {
                        var f = (PermissionFilter)filter.Filter;
                        desc.Permission = f.Permission;
                    }

                    ApiList.Add(desc);
                }
            }

            return app;
        }        
    }
}