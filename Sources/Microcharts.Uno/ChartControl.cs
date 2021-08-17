using System;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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

            SetChartViewsAndCharts();

            if (GetTemplateChild("ContentPresenter") is UIElement contentGrid)
            {
                //contentGrid.AddHandler(TappedEvent, new TappedEventHandler(OnTappedEvent), true);
                //contentGrid.AddHandler(RightTappedEvent, new RightTappedEventHandler(OnRightTappedEvent), true);
                contentGrid.AddHandler(ManipulationStartedEvent, new ManipulationStartedEventHandler(OnManipulationStarted), true);
                contentGrid.AddHandler(ManipulationDeltaEvent, new ManipulationDeltaEventHandler(OnManipulationDelta), true);
                contentGrid.AddHandler(PointerWheelChangedEvent, new PointerEventHandler(OnPointerWheelChangedEvent), true);
            }
        }

        /// <summary>
        /// Identifies the <see cref="ChartView"/> dependency property
        /// </summary>
        private static readonly DependencyProperty ChartViewProperty = DependencyProperty.Register(nameof(ChartView),
            typeof(ChartView), typeof(ChartControl),
            new PropertyMetadata(null, (d, args) => ((ChartControl)d).SetChartViewsAndCharts()));
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
            new PropertyMetadata(null, (d, args) => ((ChartControl)d).SetChartViewsAndCharts()));
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
            new PropertyMetadata(null, (d, args) => ((ChartControl)d).SetChartViewsAndCharts()));
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
            new PropertyMetadata(false, (d, args) => ((ChartControl)d).SetChartViewsAndCharts()));
        /// <summary>
        /// Gets or sets the HardwareAccelerated
        /// </summary>
        public bool HardwareAccelerated
        {
            get => (bool)GetValue(HardwareAcceleratedProperty);
            set => SetValue(HardwareAcceleratedProperty, value);
        }

        public void SetChartViewsAndCharts()
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

        private void OnPointerWheelChangedEvent(object sender, PointerRoutedEventArgs e)
        {
            var pp = e.GetCurrentPoint(null);
            int wheelDelta = pp.Properties.MouseWheelDelta;

            if (HardwareAccelerated)
            {
                if (ChartViewGL?.Chart != null)
                    ChartViewGL.Chart.LabelTextSize += wheelDelta / 120f;
            }
            else
            {
                if (ChartView?.Chart != null)
                    ChartView.Chart.LabelTextSize += wheelDelta / 120f;
            }
        }

        private void OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (HardwareAccelerated && ChartViewGL?.Chart != null)
            {
                ChartViewGL.Chart.MaxValue += (float)e.Delta.Translation.Y;
                ChartViewGL.Chart.Start += (int)Math.Round(e.Delta.Translation.X);
            }
            else if (ChartView?.Chart != null)
            {
                ChartView.Chart.MaxValue += (float)e.Delta.Translation.Y;
                ChartView.Chart.Start += (int)Math.Round(e.Delta.Translation.X);
            }
        }
    }
}
