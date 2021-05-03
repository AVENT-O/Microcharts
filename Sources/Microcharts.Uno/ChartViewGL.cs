// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Microcharts.Uno
{
    using SkiaSharp;
    using SkiaSharp.Views.UWP;
    using System;
    using Windows.UI.Xaml;

    public partial class ChartViewGL : SKSwapChainPanel
    {
        #region Constructors

        public ChartViewGL()
        {
            this.PaintSurface += OnPaintCanvas;
        }

        #endregion

        #region Static fields

        public static readonly DependencyProperty ChartProperty = DependencyProperty.Register(nameof(Chart), typeof(Chart), typeof(ChartViewGL), new PropertyMetadata(null, new PropertyChangedCallback(OnChartChanged)));

        #endregion

        #region Fields

        private InvalidatedWeakEventHandler<ChartViewGL> handler;

        private Chart chart;

        #endregion

        #region Properties

        public Chart Chart
        {
            get { return (Chart)GetValue(ChartProperty); }
            set { SetValue(ChartProperty, value); }
        }

        #endregion

        #region Methods

        private static void OnChartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as ChartViewGL;

            if (view.chart != null)
            {
                view.handler.Dispose();
                view.handler = null;
            }

            view.chart = e.NewValue as Chart;
            view.Invalidate();

            if (view.chart != null)
            {
                view.handler = view.chart.ObserveInvalidate(view, (v) => v.Invalidate());
            }
        }

        private void OnPaintCanvas(object sender, SKPaintGLSurfaceEventArgs e)
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

        public static implicit operator ChartViewGL(LineChart v)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
