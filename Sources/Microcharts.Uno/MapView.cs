// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Microcharts.Uno
{
    using global::Uno.Extensions;
    using Microsoft.Extensions.Logging;
    using SkiaSharp;
    using SkiaSharp.Views.UWP;
    using System;
    using Windows.UI.Xaml;

    public partial class MapView : SKXamlCanvas
    {
        #region Constructors

        public MapView()
        {
            this.PaintSurface += OnPaintCanvas;
        }

        #endregion

        #region Static fields

        public static readonly DependencyProperty ChartProperty = DependencyProperty.Register(nameof(Chart),
            typeof(Chart), typeof(ChartView),
            new PropertyMetadata(null, (d, args) => ((MapView)d).OnChartChanged(d, args)));

        #endregion

        #region Fields

        private InvalidatedWeakEventHandler<MapView>? handler;

        private Chart? chart;

        #endregion

        #region Properties

        public Chart? Chart
        {
            get { return (Chart)GetValue(ChartProperty); }
            set { SetValue(ChartProperty, value); }
        }

        #endregion

        #region Methods

        private void OnChartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                var view = d as MapView;

                if (view == null) return;

                if (view.chart != null)
                {
                    view.handler?.Dispose();
                    view.handler = null;
                }

                view.chart = e.NewValue as Chart;
                view.Invalidate();

                if (view.chart != null)
                {
                    view.handler = view.chart.ObserveInvalidate(view, (v) => v.Invalidate());
                }
            }
            catch (Exception f)
            {
                this.Log().LogError(f.ToString());
            }
        }

        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            try
            {
                if (this.chart != null)
                {
                    this.chart.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
                }
                else
                {
                    e.Surface.Canvas.Clear(SKColors.Transparent);
                }
            }
            catch (Exception f)
            {
                this.Log().LogError(f.ToString());
            }
        }

        public static implicit operator MapView(MapChart v)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
