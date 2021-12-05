using Newtonsoft.Json;
using Noma.ApplicationL.Common.CustomEventArguments;
using Noma.ApplicationL.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Noma.Infrastructure
{
    public class LocalHttpRequestsReceiver : IRequestsReceiver
    {
        public event EventHandler<NoteCreationEventArgs> NoteCreationRequested;

        HttpListener listener;

        Thread listeningThread;

        public void StartReceiving()
        {
            if (!HttpListener.IsSupported)
            {
                Debug.WriteLine("Http listener is not supported");
                return;
            }

            if (listener == null)
            {  
                listener = new HttpListener();
            }
            else
            {
                listener.Stop();
            }

           // abort thread here

            lock (listener)
            {
                listeningThread = new Thread(() =>
                {
                    HttpListener listener = new HttpListener();
                    listener.Prefixes.Add("http://localhost:4333/Noma/");
                    listener.Start();
                    while (listener.IsListening)
                    {
                        HandleRequest(listener.GetContext());
                    }
                });

                listeningThread.Start();
            }
        }

        public void StopReceiving()
        {
            lock (listener)
            {
                listener.Stop();
            }

            // abort thread here
        }

        private async void HandleRequest(HttpListenerContext context)
        {
            if (context.Request.HttpMethod == "POST" && context.Request.RawUrl.ToLower() == "/noma/")
            {
                await PostToNoma(context);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.Close();
            }
        }

        private async Task PostToNoma(HttpListenerContext context)
        {
            using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                string body = await reader.ReadToEndAsync();
                dynamic json = null;
                try
                {
                    json = JsonConvert.DeserializeObject(body);
                }
                catch (JsonSerializationException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.Close();

                    return;
                }
               
                try
                {
                    string noteText = json.NoteText;
                    if (!string.IsNullOrEmpty(noteText))
                    {
                        NoteCreationRequested?.Invoke(
                            this,
                            new NoteCreationEventArgs()
                            {
                                Content = noteText
                            });

                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        context.Response.Close();

                        return;
                    }
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message);

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.Close();

                    return;
                }
            }
        }
    }
}
