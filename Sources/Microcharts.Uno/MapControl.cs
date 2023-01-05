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

            if (GetTemplateChild("ContentPresenterMapViewGL") is UIElement contentGridMapViewGL)
            {
                contentGridMapViewGL.AddHandler(TappedEvent, new TappedEventHandler(OnTappedEvent), true);
                contentGridMapViewGL.AddHandler(PointerMovedEvent, new PointerEventHandler(OnPointerMovedEvent), true);
                //contentGridMapViewGL.AddHandler(RightTappedEvent, new RightTappedEventHandler(OnRightTappedEvent), true);
                contentGridMapViewGL.AddHandler(ManipulationStartedEvent, new ManipulationStartedEventHandler(OnManipulationStarted), true);
                contentGridMapViewGL.AddHandler(ManipulationDeltaEvent, new ManipulationDeltaEventHandler(OnManipulationDelta), true);
#if NETFX_CORE
                contentGridMapViewGL.AddHandler(PointerWheelChangedEvent, new PointerEventHandler(OnPointerWheelChangedEvent), true);
#endif
            }

            if (GetTemplateChild("ContentPresenterMapView") is UIElement contentGridMapView)
            {
                contentGridMapView.AddHandler(TappedEvent, new TappedEventHandler(OnTappedEvent), true);
                contentGridMapView.AddHandler(PointerMovedEvent, new PointerEventHandler(OnPointerMovedEvent), true);
                //contentGridMapView.AddHandler(RightTappedEvent, new RightTappedEventHandler(OnRightTappedEvent), true);
                contentGridMapView.AddHandler(ManipulationStartedEvent, new ManipulationStartedEventHandler(OnManipulationStarted), true);
                contentGridMapView.AddHandler(ManipulationDeltaEvent, new ManipulationDeltaEventHandler(OnManipulationDelta), true);
#if NETFX_CORE
                contentGridMapView.AddHandler(PointerWheelChangedEvent, new PointerEventHandler(OnPointerWheelChangedEvent), true);
#endif
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
        /// Identifies the <see cref="MapViewGL"/> dependency property
        /// </summary>
        private static readonly DependencyProperty MapViewGLProperty = DependencyProperty.Register(nameof(MapViewGL),
            typeof(MapViewGL), typeof(MapControl),
            new PropertyMetadata(null, (d, args) => ((MapControl)d).SetMapViewsAndCharts()));
        /// <summary>
        /// Gets or sets the MapViewGL
        /// </summary>
        public MapViewGL? MapViewGL
        {
            get => (MapViewGL)GetValue(MapViewGLProperty);
            set => SetValue(MapViewGLProperty, value);
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
            var chartViewGL = MapViewGL;

            if (HardwareAccelerated)
            {
                if (chartView != null)
                {
                    chartView.Chart = null;
                    chartView = null;
                }
                chartViewGL ??= new MapViewGL();
                chartViewGL.Chart = Chart;
            }
            else
            {
                if (chartViewGL != null)
                {
                    chartViewGL.Chart = null;
                    chartViewGL = null;
                }
                chartView ??= new MapView();
                chartView.Chart = Chart;
            }

            MapView = chartView;
            MapViewGL = chartViewGL;
        }

        private void OnPointerWheelChangedEvent(object sender, PointerRoutedEventArgs e)
        {
            var pp = e.GetCurrentPoint(null);
            int wheelDelta = pp.Properties.MouseWheelDelta;

            if (wheelDelta == 0) return;

            var scale = wheelDelta < 0 ? 0.9f : 1.1f;

            if (HardwareAccelerated)
            {
                if (MapViewGL?.Chart != null)
                    MapViewGL.Chart.MapScale *= scale;
            }
            else
            {
                if (MapView?.Chart != null)
                    MapView.Chart.MapScale *= scale;
            }
        }

        private void OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (HardwareAccelerated && MapViewGL?.Chart != null)
            {
                MapViewGL.Chart.MaxValue += (float)e.Delta.Translation.Y;
                MapViewGL.Chart.Start += (int)Math.Round(e.Delta.Translation.X);
            }
            else if (MapView?.Chart != null)
            {
                MapView.Chart.MaxValue += (float)e.Delta.Translation.Y;
                MapView.Chart.Start += (int)Math.Round(e.Delta.Translation.X);
            }
        }

        public void OnTappedEvent(object sender, TappedRoutedEventArgs e)
        {
            var pos = e.GetPosition(this);

            if (HardwareAccelerated && MapViewGL?.Chart != null)
            {
                MapViewGL.Chart.SetSKPath((float)pos.X, (float)pos.Y);
            }
            else if (MapView?.Chart != null)
            {
                MapView.Chart.SetSKPath((float)pos.X, (float)pos.Y);
            }
        }

        public void OnPointerMovedEvent(object sender, PointerRoutedEventArgs e)
        {
            var pos = e.GetCurrentPoint(this);

            if (HardwareAccelerated && MapViewGL?.Chart != null)
            {
                MapViewGL.Chart.HoverSKPath((float)pos.Position.X, (float)pos.Position.Y);
            }
            else if (MapView?.Chart != null)
            {
                MapView.Chart.HoverSKPath((float)pos.Position.X, (float)pos.Position.Y);
            }

        }
    }
}
