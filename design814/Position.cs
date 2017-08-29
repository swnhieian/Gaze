using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
using MsgPack;
using MsgPack.Serialization;
using NetMQ.Sockets;
using System.Threading;
using System.Windows.Shapes;
using System.Windows;
using System.IO;

namespace design814
{
    class Position
    {
        string IPHeader;
        string ServicePort;
        Dictionary<string, string> dic = new Dictionary<string, string>();
        RequestSocket _requestSocket;
        SubscriberSocket subscriberSocket;

        public Position()
        {
            IPHeader = ">tcp://127.0.0.1:"; // >tcp
            ServicePort = "50020";
            _requestSocket = new RequestSocket(IPHeader + ServicePort);
            _requestSocket.SendFrame("SUB_PORT");
            string subport = _requestSocket.ReceiveFrameString();
            subscriberSocket = new SubscriberSocket(IPHeader + subport);
            subscriberSocket.Subscribe("surface");
            // conext to surface            
        }
        public void start(MainWindow window, MyDelegate method)
        {
            while (true)
            {
                NetMQMessage recievedMsg;
                recievedMsg = subscriberSocket.ReceiveMultipartMessage();
                var m = MsgPack.Unpacking.UnpackObject(recievedMsg[1].ToByteArray());
                //Dictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject> mpoDict = MessagePackSerializer.Create<Dictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject>>().Unpack(new MemoryStream(recievedMsg[1].ToByteArray()));
                Dictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject> mpoDict = MessagePackSerializer.Get<Dictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject>>().Unpack(new MemoryStream(recievedMsg[1].ToByteArray()));
                MsgPack.MessagePackObject gos = mpoDict[new MsgPack.MessagePackObject("gaze_on_srf")]; // choose gaze on suf mode
                List<MessagePackObject> dataList = gos.AsList().ToList();
                int dataNo = 0;
                double interval1;
                double interval2 = 0;
                foreach (var data in dataList) // = gaze on suf
                {
                    dataNo += 1;
                   // Console.WriteLine(dataNo);
                    MessagePackObjectDictionary dict = data.AsDictionary();
                    MessagePackObject normPos;
                    MessagePackObject ts;

                    /*if (dict.TryGetValue(new MessagePackObject("timestamp"), out ts))
                    {
                        double tts = ts.AsDouble();
                        Console.WriteLine(tts);
                    }*/
                    //else tts = "hhhhh";
                    
                 //if (dataNo == 1) interval1 = a[0];
                  //  else interval1 = interval2;
                  //  interval2 = double.Parse(a);
                  //  if ((interval2 - interval1) >50)
                   // { 
                 //   Console.WriteLine(a);
              
                        if (dict.TryGetValue(new MessagePackObject("norm_pos"), out normPos))
                        {
                        // timestamp = dic['base_data']['timestamp'];
                        List<MessagePackObject> coord = normPos.AsList().ToList(); // 
                    //    Console.WriteLine("{0},{1}", coord[0].AsDouble(), coord[1].AsDouble());
                        Random random = new Random();
                        //window.Dispatcher.BeginInvoke(method, new Point(random.Next(0, 500), random.Next(0, 400)));
                        window.Dispatcher.BeginInvoke(method, new Point(coord[0].AsDouble(),coord[1].AsDouble())); // direct + indirect

                        }
                        else
                        {
                            throw new Exception("get norm_pos failed!");
                        }
                    //}
                }
            }
        }
    }
}
