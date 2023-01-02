using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microcharts.Uno
{
    public partial class MapControl: ContentControl
    {
        public MapControl()
        {
            DefaultStyleKey = typeof(MapControl);
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="Control.ApplyTemplate"/>.
        /// In simplest terms, this means the method is called just before a UI element displays in your app
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SetMapViewsAndCharts();

            if (GetTemplateChild("ContentPresenterMapView") is UIElement contentGridMapView)
            {
                //contentGridMapView.AddHandler(TappedEvent, new TappedEventHandler(OnTappedEvent), true);
                //contentGridMapView.AddHandler(RightTappedEvent, new RightTappedEventHandler(OnRightTappedEvent), true);
                contentGridMapView.AddHandler(ManipulationStartedEvent, new ManipulationStartedEventHandler(OnManipulationStarted), true);
                contentGridMapView.AddHandler(ManipulationDeltaEvent, new ManipulationDeltaEventHandler(OnManipulationDelta), true);
            }
        }

        /// <summary>
        /// Identifies the <see cref="MapView"/> dependency property
        /// </summary>
        private static readonly DependencyProperty MapViewProperty = DependencyProperty.Register(nameof(MapView),
            typeof(MapView), typeof(MapControl),
            new PropertyMetadata(null, (d, args) => ((MapControl)d).SetMapViewsAndCharts()));
        /// <summary>
        /// Gets or sets the MapView
        /// </summary>
        public MapView? MapView
        {
            get => (MapView)GetValue(MapViewProperty);
            set => SetValue(MapViewProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Chart"/> dependency property
        /// </summary>
        private static readonly DependencyProperty ChartProperty = DependencyProperty.Register(nameof(Chart),
            typeof(Chart), typeof(MapControl),
            new PropertyMetadata(null, (d, args) => ((MapControl)d).SetMapViewsAndCharts()));
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
            typeof(bool), typeof(MapControl),
            new PropertyMetadata(false, (d, args) => ((MapControl)d).SetMapViewsAndCharts()));
        /// <summary>
        /// Gets or sets the HardwareAccelerated
        /// </summary>
        public bool HardwareAccelerated
        {
            get => (bool)GetValue(HardwareAcceleratedProperty);
            set => SetValue(HardwareAcceleratedProperty, value);
        }

        public void SetMapViewsAndCharts()
        {
            if (Chart == null) return;

            var chartView = MapView;

            chartView ??= new MapView();
            chartView.Chart = Chart;

            MapView = chartView;
        }

        private void OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (MapView?.Chart != null)
            {
                MapView.Chart.MaxValue += (float)e.Delta.Translation.Y;
                MapView.Chart.Start += (int)Math.Round(e.Delta.Translation.X);
            }
        }
    }
}
