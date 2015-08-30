﻿using System;
using System.Linq;
using System.Web.Http.Controllers;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;

namespace Trippism.APIExtention.Filters
{
    public class ServiceStackFormatterConfigAttribute : Attribute, IControllerConfiguration
    {
        public void Initialize(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor)
        {
            var formatter = controllerSettings.Formatters.OfType<JsonMediaTypeFormatter>().Single();
            controllerSettings.Formatters.Remove(formatter);

            formatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = { ContractResolver = new CamelCasePropertyNamesContractResolver() }
            };

            controllerSettings.Formatters.Add(formatter);

        }
    }
}