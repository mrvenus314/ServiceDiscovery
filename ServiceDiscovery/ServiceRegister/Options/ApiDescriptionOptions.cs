using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceRegister.Options
{
    public class ApiDescriptionOptions
    {
        /// <summary>
        /// Gets or sets Microsoft.AspNetCore.Mvc.ApiExplorer.ApiDescription.ActionDescriptor
        ///     for this api.
        /// </summary>
        public ActionDescriptor ActionDescriptor { get; set; }

        /// <summary>
        /// Gets or sets group name for this api.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the supported HTTP method for this api, or null if all HTTP methods
        ///     are supported.
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// Gets a list of Microsoft.AspNetCore.Mvc.ApiExplorer.ApiParameterDescription for
        ///     this api.
        /// </summary>
        public IList<ApiParameterDescription> ParameterDescriptions { get; }

        /// <summary>
        /// Gets arbitrary metadata properties associated with the Microsoft.AspNetCore.Mvc.ApiExplorer.ApiDescription.
        /// </summary>
        public IDictionary<object, object> Properties { get; }

        /// <summary>
        /// Gets or sets relative url path template (relative to application root) for this  api.
        /// </summary>
        public string RelativePath { get; set; }

        /// <summary>
        /// Gets the list of possible formats for a request.
        /// Will be empty if the action does not accept a parameter decorated with the [FromBody] attribute.
        /// </summary>
        public IList<ApiRequestFormat> SupportedRequestFormats { get; }

        /// <summary>
        /// Gets the list of possible formats for a response.        
        /// Will be empty if the action returns no response, or if the response type is unclear.
        /// Use ProducesAttribute on an action method to specify a response type.
        /// </summary>
        public IList<ApiResponseType> SupportedResponseTypes { get; }

        public string ClaimType { get; set; } = "role";

        /// <summary>
        /// 
        /// </summary>
        public string Permission { get; set; }
    }
}
