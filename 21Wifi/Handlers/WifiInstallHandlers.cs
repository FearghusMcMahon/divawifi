/**
 * Copyright (c) Crista Lopes (aka Diva). All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 * 
 *     * Redistributions of source code must retain the above copyright notice, 
 *       this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright notice, 
 *       this list of conditions and the following disclaimer in the documentation 
 *       and/or other materials provided with the distribution.
 *     * Neither the name of the Organizations nor the names of Individual
 *       Contributors may be used to endorse or promote products derived from 
 *       this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL 
 * THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, 
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
 * GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED 
 * AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED 
 * OF THE POSSIBILITY OF SUCH DAMAGE.
 * 
 */

using Nini.Config;
using log4net;
using System;
using System.Reflection;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Web;

using System.Collections.Generic;
using OpenSim.Server.Base;
using OpenSim.Services.Interfaces;
using OpenSim.Framework;
using OpenSim.Framework.Servers.HttpServer;
using OpenMetaverse;

using Diva.Utils;

using Environment = Diva.Utils.Environment;


namespace Diva.Wifi
{
    public class WifiInstallGetHandler : BaseStreamHandler
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private WebApp m_WebApp;

        public WifiInstallGetHandler(WebApp webapp) :
                base("GET", "/wifi/install")
        {
            m_WebApp = webapp;
        }

        public override byte[] Handle(string path, Stream requestData,
                IOSHttpRequest httpRequest, IOSHttpResponse httpResponse)
        {
            // path = /wifi/...
            //m_log.DebugFormat("[Wifi]: path = {0}", path);
            //m_log.DebugFormat("[Wifi]: ip address = {0}", httpRequest.RemoteIPEndPoint);
            //foreach (object o in httpRequest.Query.Keys)
            //    m_log.DebugFormat("  >> {0}={1}", o, httpRequest.Query[o]);

            httpResponse.ContentType = "text/html";

            Request request = RequestFactory.CreateRequest(string.Empty, httpRequest, Localization.GetLanguageInfo(httpRequest.Headers.Get("accept-language")));
            Environment env = new Environment(request);

            string result = m_WebApp.Services.InstallGetRequest(env);

            return WebAppUtils.StringToBytes(result);

        }

    }

    public class WifiInstallPostHandler : BaseStreamHandler
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private WebApp m_WebApp;

        public WifiInstallPostHandler(WebApp webapp) :
            base("POST", "/wifi/install")
        {
            m_WebApp = webapp;
        }

        public override byte[] Handle(string path, Stream requestData,
                IOSHttpRequest httpRequest, IOSHttpResponse httpResponse)
        {
            StreamReader sr = new StreamReader(requestData);
            string body = sr.ReadToEnd();
            sr.Close();
            body = body.Trim();

            httpResponse.ContentType = "text/html";

            string resource = GetParam(path);

            try
            {
                Dictionary<string, object> request =
                        ServerUtils.ParseQueryString(body);

                string password = String.Empty;
                string password2 = String.Empty;

                if (request.ContainsKey("password"))
                    password = request["password"].ToString();
                if (request.ContainsKey("password2"))
                    password2 = request["password2"].ToString();

                Request req = RequestFactory.CreateRequest(string.Empty, httpRequest, Localization.GetLanguageInfo(httpRequest.Headers.Get("accept-language")));
                Environment env = new Environment(req);

                string result = m_WebApp.Services.InstallPostRequest(env, password, password2);

                return WebAppUtils.StringToBytes(result);

            }
            catch (Exception e)
            {
                m_log.DebugFormat("[USER ACCOUNT POST HANDLER]: Exception {0}",  e);
            }

            return WebAppUtils.FailureResult();

        }

    }

}
