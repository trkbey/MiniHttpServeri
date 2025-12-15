using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

class MiniHttpServer
{
    static List<User> users = new List<User>()
    {
        new User { Id = 1, Name = "Hasan" },
        new User { Id = 2, Name = "Serdar" }
    };

    static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 8080);
        server.Start();
        Console.WriteLine("Server 8080 de aktif.");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[4096];
            int readBytes = stream.Read(buffer, 0, buffer.Length);
            string request = Encoding.UTF8.GetString(buffer, 0, readBytes);

            Console.WriteLine("---- RAW REQUEST ----");
            Console.WriteLine(request);

            string response = HandleRequest(request);

            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
            stream.Write(responseBytes, 0, responseBytes.Length);

            client.Close();
        }
    }

    static string HandleRequest(string request)
    {
        string[] lines = request.Split('\n');
        string[] requestLine = lines[0].Trim().Split(' ');

        string method = requestLine[0];
        string path = requestLine[1];
        string body = GetRequestBody(request);

        if (method == "GET" && path == "/users")
            return HttpResponse(UsersToJson(), "application/json");

        if (method == "GET" && path.StartsWith("/users/"))
        {
            int id = int.Parse(path.Split('/')[2]);
            User user = users.Find(u => u.Id == id);

            if (user == null)
                return NotFound();

            return HttpResponse(UserToJson(user), "application/json");
        }

        if (method == "POST" && path == "/users")
        {
            User user = ParseUser(body);
            users.Add(user);
            return HttpResponse("{\"status\":\"User created\"}", "application/json");
        }

        
        if (method == "PUT" && path.StartsWith("/users/"))
        {
            int id = int.Parse(path.Split('/')[2]);
            User updated = ParseUser(body);
            User user = users.Find(u => u.Id == id);

            if (user == null)
                return NotFound();

            user.Name = updated.Name;
            return HttpResponse("{\"status\":\"User updated\"}", "application/json");
        }

        if (method == "DELETE" && path.StartsWith("/users/"))
        {
            int id = int.Parse(path.Split('/')[2]);
            User user = users.Find(u => u.Id == id);

            if (user == null)
                return NotFound();

            users.Remove(user);
            return HttpResponse("{\"status\":\"User deleted\"}", "application/json");
        }

        return NotFound();
    }

    static string GetRequestBody(string request)
    {
        string[] parts = request.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.None);
        return parts.Length > 1 ? parts[1] : "";
    }

    static User ParseUser(string json)
    {
        int id = int.Parse(GetValue(json, "id"));
        string name = GetValue(json, "name");
        return new User { Id = id, Name = name };
    }

    static string GetValue(string json, string key)
    {
        int index = json.IndexOf($"\"{key}\"");
        int colon = json.IndexOf(":", index);
        int comma = json.IndexOf(",", colon);
        if (comma == -1)
            comma = json.IndexOf("}", colon);

        return json.Substring(colon + 1, comma - colon - 1)
                   .Replace("\"", "")
                   .Trim();
    }

    static string UsersToJson()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("[");

        for (int i = 0; i < users.Count; i++)
        {
            sb.Append(UserToJson(users[i]));
            if (i < users.Count - 1)
                sb.Append(",");
        }

        sb.Append("]");
        return sb.ToString();
    }

    static string UserToJson(User user)
    {
        return $"{{\"id\":{user.Id},\"name\":\"{user.Name}\"}}";
    }

    static string HttpResponse(string content, string contentType)
    {
        return
            "HTTP/1.1 200 OK\r\n" +
            $"Content-Type: {contentType}\r\n" +
            $"Content-Length: {Encoding.UTF8.GetByteCount(content)}\r\n\r\n" +
            content;
    }

    static string NotFound()
    {
        return "HTTP/1.1 404 Not Found\r\n\r\n";
    }
}

class User
{
    public int Id;
    public string Name;
}
