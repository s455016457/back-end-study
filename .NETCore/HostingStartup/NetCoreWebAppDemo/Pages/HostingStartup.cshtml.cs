using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace NetCoreWebAppDemo.Pages{
    public class HostingStartupModel:PageModel{
        public HostingStartupModel(IConfiguration config){
            ServiceKey_Development_Library=config["DevAccount_FromLibrary"];
            ServiceKey_Production_Library=config["ProdAcction_FromLibrary"];
            ServiceKey_ConfigurationKey1=config["ConfigurationKey1"];
            ServiceKey_ConfigurationKey2=config["ConfigurationKey2"];

            ServiceKey_Development_Package=config["DevAccount_FromPackage"];
            ServiceKey_Production_Package=config["ProdAcction_FromPackage"];
            ServiceKey_ConfigurationPackageKey1=config["ConfigurationPackageKey1"];
            ServiceKey_ConfigurationPackageKey2=config["ConfigurationPackageKey2"];
        }

        public string ServiceKey_Development_Library { get; private set; }
        public string ServiceKey_Production_Library { get; private set; }
        public string ServiceKey_ConfigurationKey1 { get; private set; }
        public string ServiceKey_ConfigurationKey2 { get; private set; }
        public string ServiceKey_Development_Package { get; private set; }
        public string ServiceKey_Production_Package { get; private set; }
        public string ServiceKey_ConfigurationPackageKey1 { get; private set; }
        public string ServiceKey_ConfigurationPackageKey2 { get; private set; }

        public void OnGet(){

        }
    }
}