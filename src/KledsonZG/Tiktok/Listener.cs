using System.Net;

namespace KledsonZG.Tiktok
{
    internal class Listener
    {
        HttpListener listener = new HttpListener();
        internal Listener()
        {
            listener.Prefixes.Add("http://127.0.0.1:40019/chat/");
            
            Thread thread = new Thread(new ThreadStart(delegate 
                {
                    HandleRequests(listener); 
                }
            ) );

            listener.Start();
            thread.Start();
        }

        internal HttpListener httpListener { get { return listener; } }

        private void HandleRequests(HttpListener requestListener)
        {
            while(requestListener.IsListening)
            {
                var ctx = requestListener.GetContext();
                var request = ctx.Request;
                
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
        }
    }
}