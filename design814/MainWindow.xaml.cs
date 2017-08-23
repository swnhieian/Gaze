using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using System.Windows.Threading;

namespace design814
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public delegate void MyDelegate(Object para);//定义委托  
    class CheckItem
    {
        private bool checkitem;
        public bool CHECKITEM
        {
            get { return checkitem; }
            set { checkitem = value; }
        }
    }
    class M_TEXTList
    {
        private TextRange range;
        private int index;
        private int lineth;
        public TextRange Range
        {
            get { return range; }
            set { range = value; }
        }
        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        public int Lineth
        {
            get { return lineth; }
            set { lineth = value; }
        }
        public M_TEXTList(TextRange xrange, int xindex,int xlineth)
        {
            range = xrange;
            index = xindex;
            lineth = xlineth;
        }
    }
    public partial class MainWindow : Window
    {
        CheckItem CIm;
        private Thread myThread = null;//定义线程  
        public MainWindow()
        {
            CIm = new CheckItem();
            InitializeComponent();

            //设置模式的选择binding，使得只有勾选checkbox的情况下才进入修改模式
            Binding bingding = new Binding();
            bingding.Source = CIm;
            bingding.Path = new PropertyPath("CHECKITEM");
            BindingOperations.SetBinding(this.checkBox, CheckBox.IsCheckedProperty, bingding);
            BackGroundCanvas.Children.Add(c); //*****************************************************
            //c.Visibility = Visibility.Hidden;

            myThread = new Thread(ThreadMethod);
            myThread.IsBackground = true;//后台线程  
            myThread.Start();


        }
        private void ThreadMethod()
        {
            Position position = new Position();
            MyDelegate myDelegate = new MyDelegate(DelegateMethod);
            position.start(this, myDelegate);
            /*while (true)
            {
                Random random = new Random();
                var point = new Point(random.Next(0, 500), random.Next(0, 400));
                this.Dispatcher.BeginInvoke(myDelegate, point);//调用委托  
                Thread.Sleep(500);//休眠500ms  
            }*/
        }
        private void DelegateMethod(object para)
        {
            Point point = (Point)para;
            if (point.X < 0)
            {
                point.X = 0;
                if(point.Y <= 0)
                {                    
                    point.Y = 0;
                }
                else
                {
                    point.Y = richTextBox.ActualHeight;
                }                
            }
            if(point.X> richTextBox.ActualWidth)
            {
                point.X = richTextBox.ActualWidth;
                if(point.Y <= 0)
                {
                    point.Y = 0;
                }
                else
                {
                    point.Y = richTextBox.ActualHeight;
                }
            }
            // lbl.Content = DateTime.Now.ToString();
            if (m_TextList != null && CIm.CHECKITEM == true && m_TextList.Count > 1)
            {
             //   var point = e.GetPosition(richTextBox);            //******************************该处为坐标接口***************
                                                                   //获取TextPointer

                var poz = richTextBox.GetPositionFromPoint(point, true);
                int MousePointindex = GetIndexFromPointer(poz);            //鼠标当前位置所在的text索引
                int MouseLineth = 1 + ALLlineth - GetLineth(poz);

                currNumber = FindNearesetString(MouseLineth, MousePointindex);
                //    TextRange range3 = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                //    string PPPretext = range3.Text;
                if (preNumber != -1 && preNumber < m_TextList.Count)
                {

                    SetBackGround(DefaultBackGroundColor, m_TextList[preNumber].Range);
                    if (preNumber != currNumber)
                    {
                        HoldFlag = false;
                        PreChangeFlag = true;
                        m_TextList[preNumber].Range.Text = PreSelectWords;//把上一个选中的词恢复
                        prelength = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text.Length;
                        selectcolor(Selectcolor, m_TextList[preNumber].Range);
                        PreChangeFlag = false;
                    }
                    else
                    {
                        HoldFlag = true;
                    }
                }
                if (HoldFlag == false) //当不是选中同一个词的时候               问题：选中不改
                {
                    PreChangeFlag = true;

                    PreSelectWords = m_TextList[currNumber].Range.Text;

                    // m_TextList[currNumber].Range.Text = m_TextList[currallNumber - 1].Range.Text;
                    m_TextList[currNumber].Range.Text = laststring;
                    TextRange range2 = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                    prelength = range2.Text.Length;
                    string PPretext = range2.Text;



                    PreChangeFlag = false;
                }
                SetBackGround(MouseSelectcolor, m_TextList[currNumber].Range);
                preNumber = currNumber;
                Space_LastTextrange = m_TextList[currNumber].Range;
                // m_TextList[currNumber].Range.Text = "xxx";
                //List<Paragraph> list = new List<Paragraph>();
                //List<Block> bc = richTextBox.Document.Blocks.ToList();
                //Run run = m_TextList[currNumber].Range;
                // m_TextList[currNumber].Range.Start.
                // richTextBox.ToolTip = m_TextList[currallNumber-1].Range.Text;
                // richTextBox.ToolTip.ToString


                // richTextBox.CaretPosition = richTextBox.Document.ContentEnd;
                // 
                // run.ToolTip
                /*            var nextPoz = poz.GetNextInsertionPosition(poz.LogicalDirection);
                            if (nextPoz != null)
                            {
                                var range = new TextRange(poz, nextPoz);

                                //设置TextRange属性
                                var brush = range.GetPropertyValue(TextElement.ForegroundProperty);
                                if (brush.Equals(Brushes.YellowGreen))
                                {
                                    range.ClearAllProperties();
                                }
                                else
                                {
                                    range.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.YellowGreen);
                                    range.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                                    range.ApplyPropertyValue(TextElement.FontSizeProperty, 30.0);
                                }
                            }
                */
            }
            //  }
            // else  //空格状态:椭圆选择状态
            //  {
            //    BackGroundCanvas.Children.Remove(c);
        //    if (SpaceHoldFalg == false)
        //    {
                //       var point2 = e.GetPosition(richTextBox);
                var point2 = point;
                LastMousePoint = point2;
                Canvas.SetLeft(c, point2.X - 80);
                Canvas.SetTop(c, point2.Y - 45);
        //    }
        }

        private string GetText(RichTextBox richTextBox)
        {
            TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            return getsubstring(textRange.Text);
//            richTextBox.Document.con
        }
        //获得a的最后一个序列
        private string getsubstring(string a)
        {
            int start=0;
            for(int i = a.Length - 1; i >= 0; i--)
            {
                if (a[i] == ' ')
                {
                    start = i+1;
                    break;
                }
            }
            return a.Substring(start, a.Length-2 - start);
        }
        private void richTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            
            if (CIm.CHECKITEM == true)
            {
                if (PreChangeFlag == true) return;
                if (SpaceFlag == true) return;
                TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                //试验:一个text里包含若干个paragraph.一个paragraph包含若干lines。paragraph以回车键区别，一个回车相当于两行.在text格式里，paragraph用/r/n区别，占两个字符。lines里没有/r/n字符
               // List<Paragraph> list = new List<Paragraph>();
                //                int Count = 1;
                //                var linePos = richTextBox.Document.ContentStart.GetLineStartPosition(0);
                //                while (true) { int result; linePos = linePos.GetLineStartPosition(1, out result); if (result == 0) { break; } ++Count; }
                //                List<Block> bc = richTextBox.Document.Blocks.ToList();
                //试验
                if (ChangeFlag == false)
                {
                    ChangeFlag = true;
                    resettext(textRange);
                    this.richTextBox.FontSize = FFontSize;
 //                   prelength = -1;
                }
                
                int nowlength = textRange.Text.Length;
                if (nowlength != prelength)
                {
                    prelength = nowlength;
                    //textRange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Defaultcolor));
                    resettext(textRange);
                    this.richTextBox.FontSize = FFontSize;
                    Pretext = textRange.Text;
                    m_TextList = new List<M_TEXTList>();
                    m_TextList.Clear();
                    SetBackGround(DefaultBackGroundColor, new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd));

                    string SubString = GetText(this.richTextBox);           //匹配目标
                    laststring = SubString;
                     if (Space_LastTextrange != null&& PreSelectWords!=null&&preNumber!=-1)
                        {
                            SpaceFlag = true;
                            Space_LastTextrange.Text = PreSelectWords;
                            prelength = textRange.Text.Length;
                            SpaceFlag = false;
                            preNumber = -1;
                            currNumber = -1;
                            HoldFlag = false;
                 //           PreSelectWords =
                 //           richTextBox.CaretPosition = richTextBox.Document.ContentEnd;

                        }
                    if (SubString.Length != 0)                               //以防用户输入字符串为空
                    {
                      //  BackGroundCanvas.Children.Remove(c);
                        SpaceFlagtoCircle = false;
                        currallNumber = 0;
                        RawText = textRange.Text;
                        ALLlineth = GetLineth(richTextBox.Document.ContentStart);
                        //                       MessageBox.Show(SubString);
                        ChangeColor(this.richTextBox, Selectcolor, SubString);

                    }
                    else
                    {

                   //     //如果为空格，说明准备下一次输入，清除之前的list。
                  //      m_TextList = new List<M_TEXTList>();
                 //       m_TextList.Clear();
                        
                        if (Space_LastTextrange != null&& PreSelectWords!=null)
                        {
                            SpaceFlag = true;
                            Space_LastTextrange.Text = PreSelectWords;
                            prelength = textRange.Text.Length;
                            SpaceFlag = false;
                            //     TextRange
                            Space_LastTextrange = null;
                            preNumber = -1;
                            HoldFlag = false;
                            //           PreSelectWords =
                            //           richTextBox.CaretPosition = richTextBox.Document.ContentEnd;

                        }
                        SpaceFlagtoCircle = true;
//                        PreSelectWords 
               ///         SetBackGround(DefaultBackGroundColor, new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd));
                    }
                }
                
            }
            else
            {
                ChangeFlag = false;
            }
        
        }
        //查找字符串的位置

        /// <summary>
        /// 设置背景色
        /// </summary>
        /// <param name="l"></param>
        /// <param name="textRange"></param>
        public void SetBackGround(Color l, TextRange textRange)
        {
            textRange.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(l));
//            textRange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(CommonColorsHelp.SelectedKeyFontColor));
        }
        /// <summary>
        /// 重新设置背景色
        /// </summary>
        /// <param name="textRange">关键字的的TextRange</param>
        /// <param name="isCurrKeyWord">是否是当前的关键字</param>
  //      public void ReSetBackGround(TextRange textRange, bool isCurrKeyWord)
  //      {
  //          textRange.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(CommonColorsHelp.DefaultBackGroundColor));
  //          if (isCurrKeyWord)
  //          {
  //              textRange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(CommonColorsHelp.KeyFontColor));
  //          }
  //          else
  //          {
  //              textRange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(CommonColorsHelp.DefaultFontColor));
  //          }

  //      }
        ///设置为默认字体格式
        private void resettext(TextRange textRange)
        {
            textRange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Defaultcolor));
        }
        /// <summary>
        /// 改变关键字的具体实现
        /// </summary>
        /// <param name="l"></param>
        /// <param name="richTextBox1"></param>
        /// <param name="selectLength"></param>
        /// <param name="tpStart"></param>
        /// <param name="tpEnd"></param>
        /// <returns></returns>
        private void selectcolor(Color l,TextRange range)
        {
//            TextRange range = richTextBox1.Selection;
//            range.Select(tpStart, tpEnd);
//           TextRange range = new TextRange(tpStart, tpEnd);
            //高亮选择
            range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(l));
//            tpEnd = tpEnd.GetPositionAtOffset(1);


        }
        private TextPointer selecta(Color l, RichTextBox richTextBox1, int selectLength, TextPointer tpStart, TextPointer tpEnd)
        {
//            TextRange range = richTextBox1.Selection;
//            range.Select(tpStart, tpEnd);
            //高亮选择
//            range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(l));
//            tpEnd = tpEnd.GetPositionAtOffset(1);
//            currallNumber++;

            
            return tpEnd.GetNextContextPosition(LogicalDirection.Forward);
        }

        /// <summary>
        /// 把所有背景恢复到默认
        /// </summary>
        //       private void ReSetBackGroundAll()
        //       {
        //           if (m_TextList != null)
        //           {
        //               foreach (TextRange textRange in m_TextList)
        //               {
        //                   ReSetBackGround(textRange, false);
        //               }
        //           }
        //       }
        /// <summary>
        /// 当前第几处关键字被选中
        /// </summary>
        ///ctrl+D 替换词，上下左右是定位
        private string RawText;
        int FFontSize = 30;                                 //字体大小
        private int currNumber = -1;                                             //当前鼠标选中词
        private int preNumber = -1;                                              //上次鼠标选中词
        private int currallNumber = 0;                                           //总候选词数
        private Color Defaultcolor = Color.FromArgb(255, 0, 0, 0);               //默认字体色
        private Color DefaultBackGroundColor= Color.FromArgb(255, 255, 255, 255);//默认背景色
        private Color Selectcolor = Color.FromArgb(255, 134, 34, 234);           //选中字体颜色
        private Color MouseSelectcolor= Color.FromArgb(255, 34, 134, 234);       //选中背景色
        private int prelength=0;                                                 //每一次修改前文本长度
        private bool ChangeFlag;                                                 //从编辑模式转到修改模式时为true
        private int ALLlineth;                                                   //文本总行数
        private string Pretext;
        private bool PreChangeFlag=false;                                              //预显示时的文字改变：目的是不引起主程序变化
        private string PreSelectWords;                                               //预显示之前的words，用于恢复原来的文字
        private bool HoldFlag = false;                                              //如果连续指在一个地方，为true。目的用于不让选中的词覆盖preselectwords
        private TextRange Space_LastTextrange;
        private bool SpaceFlag = false;
        private string laststring;
        private bool SameString = false;                                                //是否找到了相同的子串
        private List<TextRange> Samestringtextrang;
        private bool SpaceFlagtoCircle = true;                                    //true表示进入输入空格之后的选择状态，false表示进入的编辑修改状态
        private Ellipse c = new Ellipse() { Height = 180, Width = 280, Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)),StrokeThickness=2};//tiffiny蓝
        private Point LastMousePoint;
        private bool SpaceHoldFalg=false;    //true时进入圈内修改并固定圈
        //       private bool MousemoveFlag = false;//true时候可以使用鼠标选中
        /// <summary>
        /// 改变关键字字体颜色
        /// </summary>
        /// <param name="l">颜色</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
 /*       private void ChangeColor2(RichTextBox richTextBox, Color l, string keyword)
        {
            //            SelectSpiltWords = new List<String>();
            //            SpiltWords = new List<String>();
            //设置文字指针为Document初始位置           
            //richBox.Document.FlowDirection 
            if (keyword == "/r/n") return;          
            TextPointer position = richTextBox.Document.ContentStart;
            TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            string Alltext = textRange.Text;
            Alltext = Alltext.Replace('.', ' ');
            Alltext = Alltext.Replace(',', ' ');
            Alltext = Alltext.Replace('!', ' ');
            int keywordlength = keyword.Length;

            int threshold = 5;
            if (keyword.Length <= 2) threshold = keyword.Length;
            int[] index = { 0, 0 };
            //获得字符
            //****************
            //*****************
            foreach (string a in Alltext.Split(' '))
            {
                if (a == "") continue;
                if (a.Length == 0) continue;
                if (System.Math.Abs(a.Length - keywordlength) > 4) continue;     //长度相差太多直接剔除
                if (getEditDistance(a, keyword) <= threshold)
                {
                    FindAllMatchedTextRanges(richTextBox, a);
                    int add = 2;
                    for (int j = 0; ; j++)
                    {
                        index[0] = Alltext.IndexOf(a, j);
                        index[1] = a.Length;
                        if ((index[0] == 0 || Alltext[index[0] - 1] == ' ') && (index[0] + index[1] >= Alltext.Length || Alltext[index[0] + index[1]] == ' ')) break;
                    }
                    //                    index[0] = text.IndexOf(Temp_SpiltWords[i], 0);
                    //                    index[1] = Temp_SpiltWords[i].Length;
                    //                    return;
                    TextPointer start = position.GetPositionAtOffset(index[0]+add);
                    add += 2;
                    TextPointer Midim = start.GetPositionAtOffset(index[1] / 2);
                    //TextPointer end = start.GetPositionAtOffset(keyword.Length);
                    TextPointer end = start.GetPositionAtOffset(index[1]);
                    int tmp_index = GetIndexFromPointer(Midim);

                    TextRange tem = new TextRange(start, end);

                    m_TextList.Add(new M_TEXTList(tem, tmp_index, 1 + ALLlineth - GetLineth(end)));
                    
     //   selectcolor(l, tem);
     //   end = start.GetPositionAtOffset(index[1] + 1);
     //    position = position.GetNextContextPosition(LogicalDirection.Forward);
     //    currallNumber++;
                }

 //               TextPointer start = position.GetPositionAtOffset(index[0]);
 //               TextPointer Midim = start.GetPositionAtOffset(index[1] / 2);
                //TextPointer end = start.GetPositionAtOffset(keyword.Length);
 //               TextPointer end = start.GetPositionAtOffset(index[1]);
 //               int tmp_index = GetIndexFromPointer(Midim);

 //               TextRange tem = new TextRange(start, end);

 //               m_TextList.Add(new M_TEXTList(tem, tmp_index, 1 + ALLlineth - GetLineth(end)));
 //               selectcolor(l, tem);
                //                end = start.GetPositionAtOffset(index[1] + 1);
                //                position = selecta(l, richTextBox, keyword.Length, start, end);
 //               currallNumber++;

                //               SpiltWords.Add(a);
            }

            if (m_TextList != null && m_TextList.Count > 0)
            {
                //重置
                currNumber = -1;
                //                SetBackGround(MouseSelectcolor, m_TextList[currNumber]);

                //设置最后用户输入的字符-不自身匹配
                resettext(m_TextList[currallNumber - 1].Range);

                //设置当前为插入状态
                //                richTextBox.CaretPosition = richTextBox.Document.ContentEnd;
            }
            return ;
            //************************************************************************************find string
        }
 */
/*        private void FindAllMatchedTextRanges(RichTextBox richBox, string keyword)
        {
 //           List<TextRange> trList = new List<TextRange>();
            //设置文字指针为Document初始位置
            TextPointer position = richBox.Document.ContentStart;
            while (position != null)
            {
                //向前搜索,需要内容为Text
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    //拿出Run的Text
                    string text = position.GetTextInRun(LogicalDirection.Forward);
                    //可能包含多个keyword,做遍历查找
                    int index = 0;
                    while (index < text.Length)
                    {
                        //                   index = text.IndexOf(keyword, index);
                       

                        index = text.IndexOf(keyword, index);
         //               if (((index == 0 || text[index - 1] == ' ') && (index + keyword.Length >= text.Length || text[index + keyword.Length] == ' ')) == false) { index += keyword.Length;continue; } 
                        if (index == -1)
                        {
                            break;
                        }
                        else
                        {
       //                     index = text.IndexOf(keyword, index);
                            if (((index == 0 || text[index - 1] == ' ') && (index + keyword.Length >= text.Length || text[index + keyword.Length] == ' ')) == false) { index += keyword.Length; continue; }
                            //添加为新的Range
                            TextPointer start = position.GetPositionAtOffset(index);
                            TextPointer Midim = start.GetPositionAtOffset(keyword.Length / 2);
                            TextPointer end = start.GetPositionAtOffset(keyword.Length);

                            int tmp_index = GetIndexFromPointer(Midim);

                            TextRange tem = new TextRange(start, end);

                            m_TextList.Add(new M_TEXTList(tem, tmp_index, 1 + ALLlineth - GetLineth(end)));
                            selectcolor(Selectcolor, tem);
                            currallNumber++;
                            index += keyword.Length;
                        }
                    }
                }
                //文字指针向前偏移
                position = position.GetNextContextPosition(LogicalDirection.Forward);
            }
            return ;
        }
        */
        private void ChangeColor(RichTextBox richTextBox,Color l, string keyword)
        {
            //            m_TextList = new List<M_TEXTList>();
            //            SelectSpiltWords = new List<String>();
            //            SpiltWords = new List<String>();
            //设置文字指针为Document初始位置           
            //richBox.Document.FlowDirection  
            
            List<TextRange>Samestringtextrang  = new List<TextRange>();
            TextPointer position = richTextBox.Document.ContentStart;
            TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            TextRange tem;
            //获得字符
            //            foreach(string a in textRange.Text.Split(' '))
            //            {
            //                SpiltWords.Add(a);
            //            }

            while (position != null)                                                   //时间主要损耗的地方
            {
                //向前搜索,需要内容为Text       
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    //拿出Run的Text        
                    string text = position.GetTextInRun(LogicalDirection.Forward);
                    //可能包含多个keyword,做遍历查找           
                    //int index = 0;
                    int[] index = { 0,0 };
                    //index = text.IndexOf(keyword, 0);                  //*******************************修改****************
                    FindString(text,index,keyword);
                    //if (index != -1)
                    if (index[0] != -1)
                    {

                        //TextPointer start = position.GetPositionAtOffset(index);
                        TextPointer start = position.GetPositionAtOffset(index[0]);
                        TextPointer Midim= start.GetPositionAtOffset(index[1]/2);
                        //TextPointer end = start.GetPositionAtOffset(keyword.Length);
                        TextPointer end = start.GetPositionAtOffset(index[1]);
                        int tmp_index = GetIndexFromPointer(Midim);
                        // if (tmp_index < 2)
                        //  {
                        //      tmp_index += 2;
                        //  }
                        //   else if (tmp_index == 2)
                        //   {
                        //       tmp_index += 1;
                        //   }
                      //  int c = GetLineth(end);
                        tem = new TextRange(start, end);
                        if (SameString == true)
                        {
                            SameString = false;
                            Samestringtextrang.Add(new TextRange(start, end));
                            PreChangeFlag = true;
                            selectcolor(l, tem);
                            PreChangeFlag = false;

                        }
                        else
                        {
                            m_TextList.Add(new M_TEXTList(new TextRange(start, end), tmp_index, 1+ALLlineth-GetLineth(end)));
                            PreChangeFlag = true;
                            selectcolor(l, tem);
                            PreChangeFlag = false;
                            currallNumber++;
                        }
                        //  if (tmp_index  < tem.Text.Length)
                        //  {
                        //      c--;
                        //  }
                       // selectcolor(l, tem);
                        end = start.GetPositionAtOffset(index[1]+1);
                        position = selecta(l, richTextBox, keyword.Length, start, end);
                    }
                }
                //文字指针向前偏移   
                position = position.GetNextContextPosition(LogicalDirection.Forward);
            }

            if (m_TextList != null&& Samestringtextrang.Count>0)
            {
                //重置
                currNumber = -1;
                m_TextList.Add(new M_TEXTList(Samestringtextrang[Samestringtextrang.Count - 1], 0, 0));
                currallNumber++;
//                SetBackGround(MouseSelectcolor, m_TextList[currNumber]);
                for(int i=0;i< Samestringtextrang.Count; i++)
                {
                    PreChangeFlag = true;
                    resettext(Samestringtextrang[i]);
                    PreChangeFlag = false;

                }                      
                //设置最后用户输入的字符-不自身匹配
                //resettext(m_TextList[currallNumber-1].Range); 

                //设置当前为插入状态
//                richTextBox.CaretPosition = richTextBox.Document.ContentEnd;
            }

            return;
        }
        /// <summary>
        /// 当前关键字共搜索出的结果集合
        /// </summary>
        //private List<TextRange> m_TextList;
        private List<M_TEXTList> m_TextList;
        //text中所有的words
//        private List<string> SpiltWords;
        //text中选中的所有words
//        private List<string> SelectSpiltWords;
//        private int MousePointindex;            //鼠标当前位置所在的text索引
//        private int MouseLineth = 1 + ALLlineth - GetLineth(poz);
        private int Max(int a,int b)
        {
            if (a > b) return a;
            else return b;
        }

        //寻找符合条件的string
        private void FindString(string text, int [] index,string keyword)
        {
            if (text == null)
            {
                index[0] = -1;
                return;
            }
            if (keyword == "/r/n") return;
            //定义匹配阈值
            int threshold=5;
            if (keyword.Length <= 2) threshold = 3;
//            else if(keyword.Length<=4) threshold = keyword.Length-1;
//            else if (keyword.Length <= 6) threshold = keyword.Length - 2;
//            else threshold = keyword.Length - 2;

            List<string> Temp_SpiltWords=new List<string>();
            text = text.Replace('.', ' ');
            text = text.Replace(',', ' ');
            text = text.Replace('!', ' ');
            foreach (string a in text.Split(' '))
            {
                if (a == "") continue;
                if (a.Length == 0) continue;
                //if (Temp_SpiltWords[i] == " ") continue;
                int distance = getEditDistance(a, keyword);
                if (distance <= threshold)
                {
                    if (distance == 0) SameString = true;                                   //?????
                                                                                            //                    SelectSpiltWords.Add(Temp_SpiltWords[i]);
                    for (int j = 0; ; j += a.Length)
                    {
                        index[0] = text.IndexOf(a, j);
                        index[1] = a.Length;
                        if ((index[0] == 0 || text[index[0] - 1] == ' ') && (index[0] + index[1] >= text.Length || text[index[0] + index[1]] == ' ')) break;
                    }
                    //                    index[0] = text.IndexOf(Temp_SpiltWords[i], 0);
                    //                    index[1] = Temp_SpiltWords[i].Length;
                    return;

                }
            }
            /*for(int i = 0; i < Temp_SpiltWords.Count; i++)
            {
                if (Temp_SpiltWords[i] == "ennvironment")
                {
                    i++;
                    i--;
                }
                if (Temp_SpiltWords[i].Length == 0) continue;
                //if (Temp_SpiltWords[i] == " ") continue;
                int distance = getEditDistance(Temp_SpiltWords[i], keyword);
                if (distance <= threshold)
                {
                    if (distance == 0) SameString=true;                                   //?????
//                    SelectSpiltWords.Add(Temp_SpiltWords[i]);
                    for(int j=0; ; j+= Temp_SpiltWords[i].Length)
                    {
                        index[0] = text.IndexOf(Temp_SpiltWords[i], j);
                        index[1] = Temp_SpiltWords[i].Length;
                        if ((index[0] == 0 || text[index[0] - 1] == ' ') && (index[0] + index[1]>=text.Length||text[index[0] + index[1]] == ' ')) break;
                    }
//                    index[0] = text.IndexOf(Temp_SpiltWords[i], 0);
//                    index[1] = Temp_SpiltWords[i].Length;
                    return;

                }
            }*/
            index[0] = -1;
        }
        //定义两个字符串的相异程度:编辑距离
        private static int Minimum(int a, int b, int c)
        {
            int mi;

            mi = a;
            if (b < mi)
            {
                mi = b;
            }
            if (c < mi)
            {
                mi = c;
            }
            return mi;
        }

        /**
         * 计算两个字符串间的编辑距离Mar 1, 2007
         *  @param s
         *  @param t
         *  @return
         */
         private static int[,] chartochar = new int[26, 26] { 
             { 0, 3, 3, 3,3,3,3,3,3,3,3,3,3,3,3,3,1,3,1,3,3,3,3,3,3,1 },//a
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 3, 0, 3, 3,3,3,1,1,3,3,3,3,3,1,3,3,3,3,3,3,3,1,3,3,3,3 },//b
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 3, 3, 0, 1,3,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,3,1,3,3 },//c
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 3, 3, 1, 0,1,1,3,3,3,3,3,3,3,3,3,3,3,3,1,3,3,3,3,3,3,3 },//d
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 3, 3, 3, 1,0,3,3,3,3,3,3,3,3,3,3,3,3,1,3,3,3,3,1,3,3,3 },//e
            // a  b  c  d e 1 g h i j k l m n o p q r s t u v w x y z
             { 3, 3, 3, 1,3,0,1,3,3,3,3,3,3,3,3,3,3,1,3,3,3,1,3,3,3,3 },//f
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 3, 1, 3, 3,3,1,0,1,3,3,3,3,3,3,3,3,3,3,3,1,3,3,3,3,3,3 },//g
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 3, 3, 3, 3,3,3,1,0,3,1,3,3,3,1,3,3,3,3,3,3,3,3,3,3,1,3 },//h
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 3, 3, 3, 3,3,3,3,3,0,3,1,3,3,3,1,3,3,3,3,3,1,3,3,3,3,3 },//i
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 3, 3, 3, 3,3,3,3,1,1,0,1,3,1,1,3,3,3,3,3,3,1,3,3,3,3,3 },//j
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 3, 3, 3, 3,3,3,3,3,1,1,0,1,1,3,3,3,3,3,3,3,3,3,3,3,3,3 },//k
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 3, 3, 3, 3,3,3,3,3,3,3,1,0,3,3,1,3,3,3,3,3,3,3,3,3,3,3 },//l
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 3, 3, 3, 3,3,3,3,3,3,1,1,3,0,1,3,3,3,3,3,3,3,3,3,3,3,3 },//m
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 3, 1, 3, 3,3,3,3,1,3,1,3,3,1,0,3,3,3,3,3,3,3,3,3,3,3,3 },//n
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 3, 3, 3, 3,3,3,3,3,1,3,1,1,3,3,0,1,3,3,3,3,3,3,3,3,3,3 },//o
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 3, 3, 3, 3,3,3,3,3,3,3,3,3,3,3,1,0,3,3,3,3,3,3,3,3,3,3 },//p
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 1, 3, 3, 3,3,3,3,3,3,3,3,3,3,3,3,3,0,3,3,3,3,3,1,3,3,3 },//q
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 3, 3, 3, 3,1,1,3,3,3,3,3,3,3,3,3,3,3,0,3,1,3,3,3,3,3,3 },//r
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 1, 3, 3, 1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,0,3,3,3,1,1,3,3 },//s
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 3, 3, 3, 3,3,3,1,3,3,3,3,3,3,3,3,3,3,1,3,0,3,3,3,3,1,3 },//t
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 3, 3, 3, 3,3,3,3,3,1,1,3,3,3,3,3,3,3,3,3,3,0,3,3,3,1,3 },//u
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 3, 1, 1, 3,3,1,3,3,3,1,3,3,3,3,3,3,3,3,3,3,3,0,3,3,3,3 },//v
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 3, 3, 3, 3,1,3,3,3,3,3,3,3,3,3,3,3,1,3,1,3,3,3,0,3,3,3 },//w
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 3, 3, 1, 1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,3,3,3,3,0,3,1 },//x
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 3, 3, 3, 3,3,3,3,1,3,3,3,3,3,3,3,3,3,3,3,1,1,3,3,3,0,3 },//y
            // a  b  c  d e f g h i j k l m n o p q r s t u v w x y z
             { 1, 3, 3, 3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,3,3,3,3,1,3,0 },//z
         };
        private int chartocharx(char a,char b)
        {
            int x1 = Char.ToLower(a) - 'a';
            int x2 = Char.ToLower(b) - 'a';
            if (x1 < 0 || x1 > 25 || x2 < 0 || x2 > 25)
            {
                return 3;
            }
            else
            {
                return chartochar[x1,x2];
            }
        }
        public int getEditDistance(String s, String t)
        {
            ///编辑距离调整：1.长度差距很大，惩罚比较大。 2.字母之间的差距，和键盘相互结合
            int[,] d; // matrix
            int n = 0; // length of s
            int m = 0; // length of t
            int i; // iterates through s
            int j; // iterates through t
            char s_i; // ith character of s
            char t_j; // jth character of t
            int cost; // cost

            // Step 1

            n = s.Length;
            m = t.Length;
            if (n == 0)
            {
                return m;
            }
            if (m == 0)
            {
                return n;
            }
            d = new int[n + 1, m + 1];
            //d = new int[n+1][+1];

            // Step 2

            for (i = 0; i <= n; i++)
            {
                d[i, 0] = 1;
            }

            for (j = 0; j <= m; j++)
            {
                d[0, j] = j;
            }

            // Step 3

            for (i = 1; i <= n; i++)
            {
                s_i = s[i - 1];
                // Step 4
                for (j = 1; j <= m; j++)
                {
                    t_j = t[j - 1];
                    // Step 5
                    cost = chartocharx(s_i, t_j);
                    if (i>1&&j>1)
                    {
                        if (t[j - 1] == s[i - 2] && t[j - 2] == s[i - 1]) cost = 0;
                    }
                  //  if (s_i == t_j)
                  //  {
                  //      cost = 0;
                 //   }
                 //   else
                 //   {
                 //       cost = 1;
                 //   }
                    // Step 6
                    d[i, j] = Minimum(d[i - 1, j] + 3, d[i, j - 1] + 3,
                            d[i - 1, j - 1] + cost);
                }
            }
            int charzhi = System.Math.Abs(m - n);
            if (charzhi == 2)
            {
                d[n, m] += 3;
            }
            else if (charzhi > 2)
            {
                d[n, m] += charzhi*3;
            }
            // Step 7
            return d[n, m];

        }
        private int GetIndexFromPointer(TextPointer point)
        {

            TextPointer start = point.GetLineStartPosition(0);
            TextRange range1 = new TextRange(start, point);
            return range1.Text.Length;

        }
        private int GetLineth(TextPointer pointer)
        {
            int Count = 1;
            var linePos = pointer.GetLineStartPosition(0);
            while (true) { int result; linePos = linePos.GetLineStartPosition(1, out result); if (result == 0) { break; } ++Count; }
            //           List<Block> bc = richTextBox.Document.Blocks.ToList();
            return Count;
        }
        private int Distance(int x1,int y1,int x2,int y2)
        {
            return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
        }
        private int FindNearesetString(int MouseLineth, int MousePointindex)
        {
            int mini=0;
            int distance = 100000;
            for(int i = 0; i < m_TextList.Count - 1; i++)
            {
                int tmp_distance = Distance(MouseLineth, MousePointindex, m_TextList[i].Lineth, m_TextList[i].Index);
                if (tmp_distance < distance)
                {
                    distance = tmp_distance;
                    mini = i;
                }
            }
            return mini;
        }

   /*     private void richTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            //if (CIm.CHECKITEM == false) return;
           // if (SpaceFlagtoCircle == false)
           // {
                if (m_TextList != null && CIm.CHECKITEM == true && m_TextList.Count > 1)
                {
                    var point = e.GetPosition(richTextBox);            //******************************该处为坐标接口***************
                                                                       //获取TextPointer

                    var poz = richTextBox.GetPositionFromPoint(point, true);
                    int MousePointindex = GetIndexFromPointer(poz);            //鼠标当前位置所在的text索引
                    int MouseLineth = 1 + ALLlineth - GetLineth(poz);

                    currNumber = FindNearesetString(MouseLineth, MousePointindex);
                    //    TextRange range3 = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                    //    string PPPretext = range3.Text;
                    if (preNumber != -1 && preNumber < m_TextList.Count)
                    {

                        SetBackGround(DefaultBackGroundColor, m_TextList[preNumber].Range);
                        if (preNumber != currNumber)
                        {
                            HoldFlag = false;
                            PreChangeFlag = true;
                            m_TextList[preNumber].Range.Text = PreSelectWords;//把上一个选中的词恢复
                            prelength = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text.Length;
                            selectcolor(Selectcolor, m_TextList[preNumber].Range);
                            PreChangeFlag = false;
                        }
                        else
                        {
                            HoldFlag = true;
                        }
                    }
                    if (HoldFlag == false) //当不是选中同一个词的时候               问题：选中不改
                    {
                        PreChangeFlag = true;

                        PreSelectWords = m_TextList[currNumber].Range.Text;

                        // m_TextList[currNumber].Range.Text = m_TextList[currallNumber - 1].Range.Text;
                        m_TextList[currNumber].Range.Text = laststring;
                        TextRange range2 = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                        prelength = range2.Text.Length;
                        string PPretext = range2.Text;



                        PreChangeFlag = false;
                    }
                    SetBackGround(MouseSelectcolor, m_TextList[currNumber].Range);
                    preNumber = currNumber;
                    Space_LastTextrange = m_TextList[currNumber].Range;
                    // m_TextList[currNumber].Range.Text = "xxx";
                    //List<Paragraph> list = new List<Paragraph>();
                    //List<Block> bc = richTextBox.Document.Blocks.ToList();
                    //Run run = m_TextList[currNumber].Range;
                    // m_TextList[currNumber].Range.Start.
                    // richTextBox.ToolTip = m_TextList[currallNumber-1].Range.Text;
                    // richTextBox.ToolTip.ToString


                    // richTextBox.CaretPosition = richTextBox.Document.ContentEnd;
                    // 
                    // run.ToolTip
                   
                }
            //  }
            // else  //空格状态:椭圆选择状态
            //  {
            //    BackGroundCanvas.Children.Remove(c);
            if (SpaceHoldFalg == false)
            {
                var point2 = e.GetPosition(richTextBox);
                LastMousePoint = point2;
                Canvas.SetLeft(c, point2.X-80 );
                Canvas.SetTop(c, point2.Y-45);
            }
               
            //    c.Visibility = Visibility.Hidden
           //     BackGroundCanvas.Children.Add(c);

         //   }
        }
        */
      //  private String LastKeyDownTime;
      //  private int CountRight = 0;
        private void richTextBox_KeyDown(object sender, KeyEventArgs e)
        {
        //    KeyboardDevice kd = e.KeyboardDevice;
        //    bool isenter=false;
        //    bool isshift=false;
        //    if ((kd.GetKeyStates(Key.Enter) & System.Windows.Input.KeyStates.Down) > 0)
       //     {
       //         isenter = true;
       //         
      //      }
      //      if((kd.GetKeyStates(Key.RightShift) & System.Windows.Input.KeyStates.Down) > 0)
      //      {
      ////          isshift = true;
      //      }
            
     //       if (isenter&&isshift)
              if(e.Key==Key.Enter)
            {
                if(currNumber != -1 && m_TextList != null&&m_TextList.Count>0)
                {
                    PreSelectWords = null;
                    preNumber = -1;
                    HoldFlag = false;
                    // PreChangeFlag = false;
                    m_TextList[m_TextList.Count - 1].Range.Text = "";
                    e.Handled = true;
                }
                //MessageBox.Show("666");
                // PreChangeFlag = true;
            }

            //if ((e.Key == Key.Left|| e.Key == Key.Right|| e.Key == Key.Up|| e.Key == Key.Down) && SpaceFlagtoCircle==true)
            if((e.Key==Key.LeftShift||e.Key==Key.RightShift) && SpaceFlagtoCircle == true)
            {
                SpaceFlagtoCircle = false;
                SpaceHoldFalg = true;
                var poz = richTextBox.GetPositionFromPoint(LastMousePoint, true);
                richTextBox.CaretPosition = poz;
           //     BackGroundCanvas.Children.Remove(c);
                e.Handled = true;
            }
            if(e.Key==Key.Escape&& SpaceHoldFalg == true)
            {
                SpaceFlagtoCircle = true;
                SpaceHoldFalg = false;
                richTextBox.CaretPosition = richTextBox.Document.ContentEnd;
            }
        }

      /*  private void richTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                CountRight++;
                if (CountRight == 1)
                {
                    LastKeyDownTime= DateTime.Now.ToString("mmssfff");
                }
                else if (CountRight == 3)
                {
                    string a = DateTime.Now.ToString("mmssfff");//前四位得相同
                    CountRight = 0;
                    if(a.Substring(0,4)== LastKeyDownTime.Substring(0, 4)&&((a[4]- LastKeyDownTime[4])*10+(a[5] - LastKeyDownTime[5]))<32)
                    {
                        richTextBox.CaretPosition = richTextBox.Document.ContentEnd;
                        e.Handled = true;
                    }
              //      SpaceFlagtoCircle = true;
                }
                //DateTime d1 = DateTime.Now.ToShortTimeString
            }
            else
            {
                CountRight = 0;
            }
        }
*/
        // }
    }
}
