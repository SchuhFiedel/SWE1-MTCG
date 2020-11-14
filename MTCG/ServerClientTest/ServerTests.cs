using NUnit.Framework;
using System;
using System.IO;
using System.Collections.Generic;
using MTCG;
using System.Net;
using System.Net.Sockets;
using Moq;
using MTCG.Server;

namespace ServerTests
{
    public class ServerTests
    {
        public static string address = "127.0.0.1";
        public static Int32 port = 8000;
        public string uri = "http://" + address + ":" + port;
        List<string> messages = new List<string>();
        /*
        public static Mock<MTCG.Server.RequestContext> Context()
        {
            return response;
        }*/

        RequestContext req = new RequestContext();
        
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void RequestContextTest1()
        {
            string messageOne = "GET" + " " + "/messages" +" "+ "HTTP / 1.1\n" +
                                "Host: " + uri + "\n" +
                                "Connection: close \n" +
                                "Keep-Alive: timeout=50, max=0 \n" +
                                "Access-Control-Allow-Origin: *\n" +
                                "Access-Control-Allow-Credentials: true\n" +
                                "Content-Type: text/plain; charset=utf-8\n";


            string response = req.Context(messageOne, messages);
            string expected = "ALL MESSAGES:\n " + "";
                        
            Assert.AreEqual(response,expected);
            
        }

        [Test]
        public void RequestContextTest2()
        {
            string text = "Hello!";

            string messageTwo = "POST" + " " + "/messages" + " " + "HTTP/1.1\n" +
                                "Host: " + uri + "\n" +
                                "Content-Length:" + text.Length + " \n" +
                                "Content-Language: de \n" +
                                "Connection: close \n" +
                                "Keep-Alive: timeout=50, max=0 \n" +
                                "Access-Control-Allow-Origin: *\n" +
                                "Access-Control-Allow-Credentials: true\n" +
                                "Content-Type: text/plain; charset=utf-8\n" +
                                "\n" + text;

            string response = req.Context(messageTwo, messages);
            string expected = "Added: " + text + " To ID: " + "0";

            Assert.AreEqual(expected, response);
            Assert.AreEqual("Hello!", messages[0]);

        }

        [Test]
        public void RequestContextTest3()
        {
            string text = "There!";

            string messageTwo = "POST" + " " + "/messages" + " " + "HTTP/1.1\n" +
                                "Host: " + uri + "\n" +
                                "Content-Length:" + text.Length + " \n" +
                                "Content-Language: de \n" +
                                "Connection: close \n" +
                                "Keep-Alive: timeout=50, max=0 \n" +
                                "Access-Control-Allow-Origin: *\n" +
                                "Access-Control-Allow-Credentials: true\n" +
                                "Content-Type: text/plain; charset=utf-8\n" +
                                "\n" + text;

            string response = req.Context(messageTwo, messages);
            string expected = "Added: " + text + " To ID: " + "1";

            Assert.AreEqual(expected, response);

        }

        [Test]
        public void RequestContextTest4()
        {
            string messageTwo = "GET" + " " + "/messages/0" + " " + "HTTP / 1.1\n" +
                                "Host: " + uri + "\n" +
                                "Connection: close \n" +
                                "Keep-Alive: timeout=50, max=0 \n" +
                                "Access-Control-Allow-Origin: *\n" +
                                "Access-Control-Allow-Credentials: true\n" +
                                "Content-Type: text/plain; charset=utf-8\n";

            string response = req.Context(messageTwo, messages);
            string expected = "SPECIFIC MESSAGE on: " + "0" + ": \n" + "Hello!";

            Assert.AreEqual(expected, response);

        }

        [Test]
        public void RequestContextTest5()
        {
            string text = "PUT ME ON TOP!";

            string messageTwo = "PUT" + " " + "/messages/0" + " " + "HTTP/1.1\n" +
                                "Host: " + uri + "\n" +
                                "Content-Length:" + text.Length + " \n" +
                                "Content-Language: de \n" +
                                "Connection: close \n" +
                                "Keep-Alive: timeout=50, max=0 \n" +
                                "Access-Control-Allow-Origin: *\n" +
                                "Access-Control-Allow-Credentials: true\n" +
                                "Content-Type: text/plain; charset=utf-8\n" +
                                "\n" + text;

            string response = req.Context(messageTwo, messages);
            string expected = "Changed " + "0" + ": " + text;

            Assert.AreEqual(expected, response);

        }

        [Test]
        public void RequestContextTest6()
        {

            string messageTwo = "DELETE" + " " + "/messages/0" + " " + "HTTP/1.1\n" +
                                "Host: " + uri + "\n" +
                                "Content-Language: de \n" +
                                "Connection: close \n" +
                                "Keep-Alive: timeout=50, max=0 \n" +
                                "Access-Control-Allow-Origin: *\n" +
                                "Access-Control-Allow-Credentials: true\n";

            string response = req.Context(messageTwo, messages);
            string expected = "Deleted " + "0";

            Assert.AreEqual(expected, response);

        }

    }
}