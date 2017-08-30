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
            subscriberSocket.Subscribe("gaze");
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
                MsgPack.MessagePackObject gos = mpoDict[new MsgPack.MessagePackObject("gaze_normals_3d")]; // choose gaze on suf mode
                //List<MessagePackObject> dataList = gos.AsList().ToList();
                MessagePackObjectDictionary dict = gos.AsDictionary();
                int dataNo = 0;
                //foreach (var data in dataList) // = gaze on suf
                {
                    dataNo += 1;
                    // Console.WriteLine(dataNo);
                    //   MessagePackObjectDictionary dict = data.AsDictionary();
                    MessagePackObject normPos = gos;
                  //  MessagePackObject ts;


                    //if (dict.TryGetValue(new MessagePackObject("gaze_normals_3d"), out normPos))
                    {
                        MessagePackObject eye0, eye1;
                        List<MessagePackObject> points0, points1;
                        MessagePackObjectDictionary eyes = normPos.AsDictionary();
                        System.Windows.Media.Media3D.Vector3D point = new System.Windows.Media.Media3D.Vector3D(0, 0, 0);
                        int num = 0;
                        if (eyes.TryGetValue(new MessagePackObject(0), out eye0))
                        {
                            points0 = eye0.AsList().ToList();
                            point.X += points0[0].AsDouble();
                            point.Y += points0[1].AsDouble();
                            point.Z += points0[2].AsDouble();
                            num += 1;
                        } else
                        {
                           // throw new Exception("aaaaa");
                        }
                        if (eyes.TryGetValue(new MessagePackObject(1), out eye1))
                        {
                            points1 = eye1.AsList().ToList();
                            point.X += points1[0].AsDouble();
                            point.Y += points1[1].AsDouble();
                            point.Z += points1[2].AsDouble();
                            num += 1;
                        } else
                        {
                           // throw new Exception("aaaabbbba");
                        }
                        if (num > 0)
                        {
                            point.X /= num;
                            point.Y /= num;
                            point.Z /= num;
                        }                        
                        window.Dispatcher.BeginInvoke(method, point);

                    }
          /*          else
                    {
                        throw new Exception("get norm_pos failed!");
                    }*/
                    //}
                }
            }
        }
        public void start_surf(MainWindow window, MyDelegate method)
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

                        window.Dispatcher.BeginInvoke(method, new Point(coord[0].AsDouble(), coord[1].AsDouble())); // direct + indirect

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
