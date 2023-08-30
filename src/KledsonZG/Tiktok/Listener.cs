using System.Net;
using KledsonZG.Util;

namespace KledsonZG.Tiktok
{
    internal class Listener
    {
        private HttpListener listener = new HttpListener();
        internal Listener()
        {
            listener.Prefixes.Add("http://127.0.0.1:40019/chat/");
            listener.Prefixes.Add("http://127.0.0.1:40019/setstream/");
            
            Thread thread = new Thread(new ThreadStart(delegate 
                {
                    HandleRequests(listener); 
                }
            ) );

            listener.Start();
            thread.Start();
        }
        internal void Stop()
        {
            if(listener.IsListening == false)
                return;
            
            listener.Stop();
        }
        internal HttpListener httpListener { get { return listener; } }

        private void HandleRequests(HttpListener requestListener)
        {
            while(requestListener.IsListening)
            {
                try
                {
                    var ctx = requestListener.GetContext();
                    var request = ctx.Request;

                    if(request.RawUrl == "/setstream")
                    {
                        PrintLiveInfo(ctx);
                        continue;
                    }
                    
                    var stream = request.InputStream;
                    int c = 0;
                    string body = "";

                    StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                    while( (c = reader.Read() ) != -1)
                    {
                        body += (char) c;
                    }
                    
                    reader.Close();
                    stream.Flush();
                    
                    Console.WriteLine(body);
                    
                    var response = ctx.Response;
                    var msg = "Received successful!";
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(msg);
                    
                    response.ContentLength64 = bytes.Length;
                    response.OutputStream.Write(bytes, 0, bytes.Length);

                    response.OutputStream.Close();
                }
                catch(Exception e)
                {
                    if(e.Message.Contains("E/S") == false)
                        Console.WriteLine("Houve um erro na instância listener: " + e.GetBaseException() + " " + e.Message);
                }         
            }
        }

        private void PrintLiveInfo(HttpListenerContext ctx)
        {
            try
            {
                var request = ctx.Request;
                var streamUrl = request.Headers.Get("stream");
                var response = ctx.Response;
                var output = response.OutputStream;

                response.ContentLength64 = 0;
                
                if(streamUrl == null)
                {                       
                    response.StatusCode = (int) HttpStatusCode.BadGateway;

                    output.Close();
                    return;
                }

                var streamInfo = Format.GetStreamNameByStreamURL(streamUrl);
                var msg = "----------------------------------------------------------------------------------------------------\n" +
                    "Live de: " + streamInfo + "\n" +
                    "----------------------------------------------------------------------------------------------------";
                Console.WriteLine(msg);

                output.Close();
                return;
            }
            catch
            {
                return;
            }
        }
    }
}