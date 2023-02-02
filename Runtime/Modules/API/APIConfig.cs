using System;
using System.Collections.Generic;
using UnityEngine;
namespace Intelmatix.Modules.API
{
    /// <summary>
    /// <description>
    /// This class saves the configuration of the API
    /// </description><br/>
    /// @author:  Anthony Shinomiya M.
    /// @date:  2022-01-21
    /// </summary>
    [Serializable]
    public class APIConfig
    {
        [System.Serializable]
        private enum Environment
        {
            Local,
            Development,
            Test,
            Production
        }
        [Header("API Environment")]
        [SerializeField] private Environment environment = Environment.Local;
        [SerializeField] private string localPath = "http://localhost:3000";
        [SerializeField] private string developmentPath = "https://dev-api.intelmatix.com";
        [SerializeField] private string testPath = "https://test-api.intelmatix.com";
        [SerializeField] private string productionPath = "https://api.intelmatix.com";

        private string api => environment switch
        {
            Environment.Local => localPath,
            Environment.Development => developmentPath,
            Environment.Test => testPath,
            Environment.Production => productionPath,
            _ => throw new ArgumentOutOfRangeException()
        };

        [Header("Authorization Key")]
        [SerializeField] private string authorizationKey = "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJhZG1pbiIsImlhdCI6MTY2Mjk5NDU1MS44NjM1NDg1LCJwcm9mIjoiQSJ9.Ldqbat2572LBhLcFOutqaWmDMGlUJIbEGSTsN1V0g90";

        [Header("Paths")]
        [SerializeField] private string rootPath = "/api/edix/visualization";

        public string API => api;
        public string AuthorizationKey => authorizationKey;
        public string RootPath => rootPath;

    }


}
