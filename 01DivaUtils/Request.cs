﻿/**
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
using System.Collections;
using System.Globalization;
using System.Net;
using System.Web;
using OpenSim.Framework.Servers.HttpServer;

using Diva.Interfaces;

namespace Diva.Utils
{
    public struct Request : IRequest
    {
        public string Resource;
        public IPEndPoint IPEndPoint;
        public Hashtable Query;
        public CultureInfo[] LanguageInfo;
    }

    public class RequestFactory
    {
        public static Request CreateRequest(string resource, IOSHttpRequest httpRequest)
        {
            return CreateRequest(resource, httpRequest, null);
        }

        public static Request CreateRequest(string resource, IOSHttpRequest httpRequest, CultureInfo[] cinfo)
        {
            Request request = new Request();
            request.Resource = resource;
            request.IPEndPoint = httpRequest.RemoteIPEndPoint;
            request.Query = httpRequest.Query;
            request.LanguageInfo = cinfo;

            return request;
        }

    }
}
