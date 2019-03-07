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
    /// Arrow.xaml 的交互逻辑
    /// </summary>
    public partial class Arrow : UserControl
    {
        public Point Position
        {
            get
            {
                return new Point((double)this.GetValue(Canvas.LeftProperty), (double)this.GetValue(Canvas.TopProperty));
            }
            set
            {
                this.SetCurrentValue(Canvas.LeftProperty, value.X);
                this.SetCurrentValue(Canvas.TopProperty, value.Y);
            }
        }
        public double Angle
        {
            get
            {
                RotateTransform rt = this.icon.RenderTransform as RotateTransform;
                return rt.Angle;
            }
            set
            {
                RotateTransform rt = this.icon.RenderTransform as RotateTransform;

                rt.Angle = value;
                // System.Diagnostics.Debug.WriteLine("Angle:" + value.ToString());
            }
        }
        public Arrow()
        {
            InitializeComponent();
        }
    }
}
