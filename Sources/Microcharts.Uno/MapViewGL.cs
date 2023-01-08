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

    public partial class MapViewGL : SKSwapChainPanel
    {
        #region Constructors

        public MapViewGL()
        {
            this.PaintSurface += OnPaintCanvas;
        }

        #endregion

        #region Static fields

        public static readonly DependencyProperty ChartProperty = DependencyProperty.Register("ChartMap",
            typeof(Chart), typeof(MapViewGL),
            new PropertyMetadata(null, (d, args) => ((MapViewGL)d).OnChartChanged(d, args)));

        #endregion

        #region Fields

        private InvalidatedWeakEventHandler<MapViewGL>? handler;

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
                var view = d as MapViewGL;

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

        private void OnPaintCanvas(object sender, SKPaintGLSurfaceEventArgs e)
        {
            try
            {
                if (this.chart != null)
                {
                    this.chart.Draw(e.Surface.Canvas, e.BackendRenderTarget.Width, e.BackendRenderTarget.Height);
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

        public static implicit operator MapViewGL(MapChart v)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
