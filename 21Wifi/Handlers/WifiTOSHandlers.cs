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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

using log4net;
using OpenSim.Framework.Servers.HttpServer;
using OpenSim.Server.Base;

using Diva.Utils;

namespace Diva.Wifi
{    
    public class WifiTOSGetHandler : BaseStreamHandler
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private WebApp m_WebApp;

        public WifiTOSGetHandler(WebApp webapp) :
                base("GET", "/wifi/tos")
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
            string resource = GetParam(path);
            //m_log.DebugFormat("[INVENTORY HANDLER HANDLER GET]: resource {0}", resource);
            string userId = string.Empty;
            if (httpRequest.Query.ContainsKey("uid"))
                userId = HttpUtility.UrlDecode(httpRequest.Query["uid"].ToString());

            Request request = RequestFactory.CreateRequest(string.Empty, httpRequest, Localization.GetLanguageInfo(httpRequest.Headers.Get("accept-language")));
            Environment env = new Environment(request);

            string result = m_WebApp.Services.TOSGetRequest(env, userId);

            return WebAppUtils.StringToBytes(result);

        }

    }

    public class WifiTOSPostHandler : BaseStreamHandler
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private WebApp m_WebApp;

        public WifiTOSPostHandler(WebApp webapp) :
            base("POST", "/wifi/tos")
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
            string resource = GetParam(path);
            //m_log.DebugFormat("[INVENTORY HANDLER POST]: resource {0}", resource);

            StreamReader sr = new StreamReader(requestData);
            string body = sr.ReadToEnd();
            sr.Close();
            body = body.Trim();
            Dictionary<string, object> postdata =
                    ServerUtils.ParseQueryString(body);

            Request request = RequestFactory.CreateRequest(string.Empty, httpRequest, Localization.GetLanguageInfo(httpRequest.Headers.Get("accept-language")));
            Environment env = new Environment(request);

            string action = postdata.Keys.FirstOrDefault(key => key.StartsWith("action-"));
            if (action == null)
                action = string.Empty;
            else
                action = action.Substring("action-".Length);

            string userID = string.Empty;
            string sessionID = string.Empty;
            if (postdata.ContainsKey("uid"))
                userID = postdata["uid"].ToString();
            if (postdata.ContainsKey("sid"))
                sessionID = postdata["sid"].ToString();

            return WebAppUtils.StringToBytes(m_WebApp.Services.TOSPostRequest(env, action, userID, sessionID));

        }

    }

}
