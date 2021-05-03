using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microcharts.Uno
{
    public partial class ChartControl: ContentControl
    {
        public ChartControl()
        {
            DefaultStyleKey = typeof(ChartControl);
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="Control.ApplyTemplate"/>.
        /// In simplest terms, this means the method is called just before a UI element displays in your app
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var chartView = ChartView;
            var chartViewGL = ChartViewGL;

            if (HardwareAccelerated)
            {
                if (chartView != null)
                {
                    chartView.Chart = null;
                    chartView = null;
                }
                if (chartViewGL == null) chartViewGL = new ChartViewGL();
                chartViewGL.Chart = Chart;
            }
            else
            {
                if (chartViewGL != null)
                {
                    chartViewGL.Chart = null;
                    chartViewGL = null;
                }
                if (chartView == null) chartView = new ChartView();
                chartView.Chart = Chart;
            }

            ChartView = chartView;
            ChartViewGL = chartViewGL;
        }

        /// <summary>
        /// Identifies the <see cref="ChartView"/> dependency property
        /// </summary>
        private static readonly DependencyProperty ChartViewProperty = DependencyProperty.Register(nameof(ChartView),
            typeof(ChartView), typeof(ChartControl),
            new PropertyMetadata(null, (d, args) => ((ChartControl)d).OnChartViewChanged((ChartView)args.OldValue)));
        /// <summary>
        /// Gets or sets the ChartView
        /// </summary>
        public ChartView? ChartView
        {
            get => (ChartView)GetValue(ChartViewProperty);
            set => SetValue(ChartViewProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ChartViewGL"/> dependency property
        /// </summary>
        private static readonly DependencyProperty ChartViewGLProperty = DependencyProperty.Register(nameof(ChartViewGL),
            typeof(ChartViewGL), typeof(ChartControl),
            new PropertyMetadata(null, (d, args) => ((ChartControl)d).OnChartViewGLChanged((ChartViewGL)args.OldValue)));
        /// <summary>
        /// Gets or sets the ChartViewGL
        /// </summary>
        public ChartViewGL? ChartViewGL
        {
            get => (ChartViewGL)GetValue(ChartViewGLProperty);
            set => SetValue(ChartViewGLProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Chart"/> dependency property
        /// </summary>
        private static readonly DependencyProperty ChartProperty = DependencyProperty.Register(nameof(Chart),
            typeof(Chart), typeof(ChartControl),
            new PropertyMetadata(null, (d, args) => ((ChartControl)d).OnChartChanged((Chart)args.OldValue)));
        /// <summary>
        /// Gets or sets the Chart
        /// </summary>
        public Chart? Chart
        {
            get => (Chart)GetValue(ChartProperty);
            set => SetValue(ChartProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="HardwareAccelerated"/> dependency property
        /// </summary>
        private static readonly DependencyProperty HardwareAcceleratedProperty = DependencyProperty.Register(nameof(HardwareAccelerated),
            typeof(bool), typeof(ChartControl),
            new PropertyMetadata(false, (d, args) => ((ChartControl)d).OnHardwareAcceleratedChanged((bool)args.OldValue)));
        /// <summary>
        /// Gets or sets the HardwareAccelerated
        /// </summary>
        public bool HardwareAccelerated
        {
            get => (bool)GetValue(HardwareAcceleratedProperty);
            set => SetValue(HardwareAcceleratedProperty, value);
        }

        private void OnChartViewChanged(ChartView? oldValue = null)
        {
            var chartView = ChartView;
            if (chartView != null)
            {

            }
        }

        private void OnChartViewGLChanged(ChartViewGL? oldValue = null)
        {
            var chartViewGL = ChartViewGL;
            if (chartViewGL != null)
            {

            }
        }

        private void OnChartChanged(Chart? oldValue = null)
        {
            var chart = Chart;
            if (chart != null)
            {
                if (ChartView != null)
                {
                    ChartView.Chart = chart;
                }
            }
        }

        private void OnHardwareAcceleratedChanged(bool oldValue)
        {
            var chartView = ChartView;
            var chartViewGL = ChartViewGL;

            if (HardwareAccelerated)
            {
                if (chartView != null)
                {
                    chartView.Chart = null;
                    chartView = null;
                }
                if (chartViewGL == null) chartViewGL = new ChartViewGL();
                chartViewGL.Chart = Chart;
            }
            else
            {
                if (chartViewGL != null)
                {
                    chartViewGL.Chart = null;
                    chartViewGL = null;
                }
                if (chartView == null) chartView = new ChartView();
                chartView.Chart = Chart;
            }

            ChartView = chartView;
            ChartViewGL = chartViewGL;
        }
    }
}
