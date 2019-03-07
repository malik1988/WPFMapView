using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace MAP.MapViewObjects
{
    /// <summary>
    /// MapView.xaml 的交互逻辑
    /// </summary>
    public partial class MapView : UserControl
    {
        public string MapName { get { return this.label地图名.Text; } set { this.label地图名.Text = value; } }
        public MapView()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="start">起点</param>
        /// <param name="end">终点</param>
        /// <param name="left">是否顺时针，true-顺时针,false-逆时针</param>
        /// <returns></returns>
        Point GetRectPoint(Point start, Point end, bool left)
        {
            Point p = new Point();
            if (start.X < end.X && start.Y < end.Y)
            {
                if (left)
                {
                    p.X = end.X;
                    p.Y = start.Y;
                }
                else
                {
                    p.X = start.X;
                    p.Y = end.Y;
                }
            }
            else if (start.X < end.X && start.Y > end.Y)
            {
                if (left)
                {
                    p.X = start.X;
                    p.Y = end.Y;

                }
                else
                {
                    p.X = end.X;
                    p.Y = start.Y;
                }
            }
            else if (start.X > end.X && start.Y > end.Y)
            {
                if (left)
                {
                    p.X = end.X;
                    p.Y = start.Y;
                }
                else
                {
                    p.X = start.X;
                    p.Y = end.Y;
                }
            }
            else//start.X>End.X && start.Y<End.Y
            {
                if (left)
                {
                    p.X = start.X;
                    p.Y = end.Y;
                }
                else
                {
                    p.X = end.X;
                    p.Y = start.Y;
                }
            }


            return p;
        }

        Point GetRectPointReverse(Point start, Point end, bool left)
        {
            Point p = new Point();
            if (start.X < end.X && start.Y < end.Y)
            {
                if (left)
                {
                    p.X = start.X;
                    p.Y = end.Y;
                }
                else
                {
                    p.X = end.X;
                    p.Y = start.Y;
                }
            }
            else if (start.X < end.X && start.Y > end.Y)
            {
                if (left)
                {
                    p.X = end.X;
                    p.Y = start.Y;

                }
                else
                {
                    p.X = start.X;
                    p.Y = end.Y;
                }
            }
            else if (start.X > end.X && start.Y > end.Y)
            {
                if (left)
                {
                    p.X = start.X;
                    p.Y = end.Y;
                }
                else
                {
                    p.X = end.X;
                    p.Y = start.Y;
                }
            }
            else//start.X>End.X && start.Y<End.Y
            {
                if (left)
                {
                    p.X = end.X;
                    p.Y = start.Y;
                }
                else
                {
                    p.X = start.X;
                    p.Y = end.Y;
                }
            }


            return p;
        }
        void AddPath(string startCode, string endCode, string type, Point start, Point end, bool reverse = false, bool isDraw = true)
        {
            Path path = new Path()
            {
                Stroke = Brushes.Blue,
                StrokeThickness = 2
            };
            PathGeometry pg = new PathGeometry();
            PathFigure pf = new PathFigure();

            string key = "";
            if (!reverse)
            {
                key = startCode + "->" + endCode;

                pf.StartPoint = start;
                //左转曲线，逆时针
                if (type == "2")
                {
                    Point cp = GetRectPoint(start, end, true);
                    QuadraticBezierSegment qbs = new QuadraticBezierSegment(cp, end, true);
                    pf.Segments.Add(qbs);

                }
                //右转，顺时针
                else if (type == "3")
                {
                    Point cp = GetRectPoint(start, end, false);
                    QuadraticBezierSegment qbs = new QuadraticBezierSegment(cp, end, true);
                    pf.Segments.Add(qbs);
                }
                else
                {
                    LineSegment _lineSeg = new LineSegment(end, true);
                    pf.Segments.Add(_lineSeg);
                }

            }
            else
            {
                key = endCode + "->" + startCode;
                pf.StartPoint = end;
                //左转曲线
                if (type == "2")
                {
                    Point cp = GetRectPointReverse(end, start, true);
                    QuadraticBezierSegment qbs = new QuadraticBezierSegment(cp, start, true);
                    pf.Segments.Add(qbs);
                }
                //右转
                else if (type == "3")
                {
                    Point cp = GetRectPointReverse(end, start, false);
                    QuadraticBezierSegment qbs = new QuadraticBezierSegment(cp, start, true);
                    pf.Segments.Add(qbs);
                }
                else
                {
                    LineSegment _lineSeg = new LineSegment(start, true);
                    pf.Segments.Add(_lineSeg);
                }

            }

            pg.Figures.Add(pf);
            path.Data = pg;
            if (isDraw)//是否绘制这条线
                this.mapCanvas.Children.Add(path);

            Point outP = new Point();
            Point outTan = new Point();
            pg.GetPointAtFractionLength(0.3, out outP, out outTan);
            //绘制箭头
            DrawArrow(outP, outTan);
        }
        void DrawArrow(Point p, Point tan)
        {
            Arrow ar = new Arrow();
            ar.Position = p;

            ar.Angle = Math.Atan2(tan.Y, tan.X) * 180 / Math.PI;

            this.mapCanvas.Children.Add(ar);
        }


        public void SetMapModel(MapData data)
        {
            this.mapCanvas.Children.Clear();
            double maxX = 0;
            double maxY = 0;
            double circle_radius = 8;
            if(null!=data&&null!=data.lines)
            {
                this.MapName = data.title;
                foreach(var line in data.lines)
                {
                    Point start = new Point(line.startX, line.startY);
                    Point end = new Point(line.endX, line.endY);

                    TextBlock textStart = new TextBlock()
                    {
                        Text = line.start + "(" +start.ToString() + ")"
                    };
                    Canvas.SetLeft(textStart, line.startX);
                    Canvas.SetTop(textStart, line.startY);
                    this.mapCanvas.Children.Add(textStart);

                    Ellipse circle = new Ellipse()
                    {
                        Fill = Brushes.Yellow,
                        Width = circle_radius,
                        Height = circle_radius,
                        Stroke = Brushes.Red,
                        StrokeThickness = 2
                    };
                    Canvas.SetLeft(circle, line.startX - circle.Width / 2);
                    Canvas.SetTop(circle, line.startY - circle.Width / 2);
                    this.mapCanvas.Children.Add(circle);

                    //正向
                    AddPath(line.start, line.end, line.type, start, end);
                    if(line.isTwoWay)
                    {
                        //反向
                        AddPath(line.start, line.end, line.type, start, end, true, false);                            
                    }

                    TextBlock textEnd = new TextBlock()
                    {
                        Text = line.end + "(" + end.ToString() + ")"
                    };
                    Canvas.SetLeft(textEnd, line.endX);
                    Canvas.SetTop(textEnd, line.endY);
                    this.mapCanvas.Children.Add(textEnd);
                    Ellipse circleEnd = new Ellipse()
                    {
                        Fill = Brushes.Yellow,
                        Width = circle_radius,
                        Height = circle_radius,
                        Stroke = Brushes.Red,
                        StrokeThickness = 2
                    };
                    Canvas.SetLeft(circleEnd, line.endX - circleEnd.Width / 2);
                    Canvas.SetTop(circleEnd, line.endY - circleEnd.Width / 2);
                    this.mapCanvas.Children.Add(circleEnd);

                    if (maxX < start.X)
                        maxX = start.X;
                    if (maxX < end.X)
                        maxX = end.X;

                    if (maxY < start.Y)
                        maxY = start.Y;
                    if (maxY < end.Y)
                        maxY = end.Y;
                }
                //设置画布大小，适应大小
                this.mapCanvas.Width = maxX + 100;
                this.mapCanvas.Height = maxY + 100;
            }
        }
    }
}
