using System.Collections.Generic;
using System.Linq;
using SkiaSharp;

namespace Microcharts
{
    /// <summary>
    /// ![chart](../images/LineSeries.png)
    ///
    /// A grouped bar chart.
    /// </summary>
    public class MapChart : PointChart
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microcharts.LineSeriesChart"/> class.
        /// </summary>
        public MapChart() : base()
        {
        }

        #endregion

        #region Properties

        private Dictionary<ChartSerie, List<SKPoint>> pointsPerSerie = new Dictionary<ChartSerie, List<SKPoint>>();

        /// <summary>
        /// Gets or sets the size of the line.
        /// </summary>
        /// <value>The size of the line.</value>
        public float LineSize { get; set; } = 3;

        /// <summary>
        /// Gets or sets the line mode.
        /// </summary>
        /// <value>The line mode.</value>
        public LineMode LineMode { get; set; } = LineMode.Spline;

        /// <summary>
        /// Gets or sets the alpha of the line area.
        /// </summary>
        /// <value>The line area alpha.</value>
        public byte LineAreaAlpha { get; set; } = 32;

        /// <summary>
        /// Enables or disables a fade out gradient for the line area in the Y direction
        /// </summary>
        /// <value>The state of the fadeout gradient.</value>
        public bool EnableYFadeOutGradient { get; set; } = false;

        #endregion

        #region Methods

        /// <inheritdoc/>
        protected override float CalculateHeaderHeight(Dictionary<ChartEntry, SKRect> valueLabelSizes)
        {
            if (ValueLabelOption == ValueLabelOption.None || ValueLabelOption == ValueLabelOption.OverElement)
                return Margin;

            return base.CalculateHeaderHeight(valueLabelSizes);
        }

        /// <inheritdoc/>
        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            pointsPerSerie.Clear();

            var length = Series.Count;

            for (int i = 0; i < length; ++i)
            {
                var s = Series[i];
                pointsPerSerie.Add(s, new List<SKPoint>());
            }

            base.DrawContent(canvas, width, height);
        }

        /// <inheritdoc/>
        protected override void OnDrawContentEnd(SKCanvas canvas, SKSize itemSize, float origin, Dictionary<ChartEntry, SKRect> valueLabelSizes)
        {
            base.OnDrawContentEnd(canvas, itemSize, origin, valueLabelSizes);

            var length = pointsPerSerie.Count;

            for (int i = 0; i < length; ++i)
            {
                var pps = pointsPerSerie.ElementAt(i);
                DrawLineArea(canvas, pps.Key, pps.Value, itemSize, origin);
            }
            DrawSeriesLine(canvas, itemSize);
            DrawPoints(canvas);

            DrawValueLabels(canvas, itemSize, valueLabelSizes);
        }

        private void DrawPoints(SKCanvas canvas)
        {
            if (PointMode != PointMode.None)
            {
                var length = pointsPerSerie.Count;

                for (int j = 0; j < length; ++j)
                {
                    var pps = pointsPerSerie.ElementAt(j);

                    var entries = pps.Key.Entries.ToArray();
                    for (int i = 0; i < pps.Value.Count; i++)
                    {
                        var entry = entries[i];
                        var point = pps.Value.ElementAt(i);
                        canvas.DrawPoint(point, pps.Key.Color ?? entry.Color, PointSize, PointMode);
                    }
                }
            }
        }

        private void DrawValueLabels(SKCanvas canvas, SKSize itemSize, Dictionary<ChartEntry, SKRect> valueLabelSizes)
        {
            ValueLabelOption valueLabelOption = ValueLabelOption;
            if (ValueLabelOption == ValueLabelOption.TopOfChart && Series.Count() > 1)
                valueLabelOption = ValueLabelOption.TopOfElement;

            if (valueLabelOption == ValueLabelOption.TopOfElement || valueLabelOption == ValueLabelOption.OverElement)
            {
                var length = pointsPerSerie.Count;

                for (int j = 0; j < length; ++j)
                {
                    var pps = pointsPerSerie.ElementAt(j);
                    var entries = pps.Key.Entries.ToArray();
                    for (int i = 0; i < pps.Value.Count; i++)
                    {
                        var entry = entries[i];
                        if (!string.IsNullOrEmpty(entry.ValueLabel))
                        {
                            var drawedPoint = pps.Value.ElementAt(i);
                            SKPoint point;
                            YPositionBehavior yPositionBehavior = YPositionBehavior.None;
                            var valueLabelSize = valueLabelSizes[entry];
                            if (valueLabelOption == ValueLabelOption.TopOfElement)
                            {
                                point = new SKPoint(drawedPoint.X, drawedPoint.Y - (PointSize / 2) - (Margin / 2));
                                if (ValueLabelOrientation == Orientation.Vertical)
                                    yPositionBehavior = YPositionBehavior.UpToElementHeight;
                            }
                            else
                            {
                                if (ValueLabelOrientation == Orientation.Vertical)
                                    yPositionBehavior = YPositionBehavior.UpToElementMiddle;
                                else
                                    yPositionBehavior = YPositionBehavior.DownToElementMiddle;

                                point = new SKPoint(drawedPoint.X, drawedPoint.Y);

                            }

                            DrawHelper.DrawLabel(canvas, ValueLabelOrientation, yPositionBehavior, itemSize, point, entry.ValueLabelColor.WithAlpha((byte)(255 * AnimationProgress)), valueLabelSize, entry.ValueLabel, ValueLabelTextSize, Typeface);
                        }
                    }
                }
            }
        }

        public void AddRange(List<ChartEntry> list)
        {
            Series[0].Entries.AddRange(list);

            int count = Series[0].Entries.Count;

            if (count > MaxNrEnries)
            {
                Series[0].Entries.RemoveRange(0, count - MaxNrEnries);
            };

            Invalidate();
        }

        public void Add(ChartEntry chartEntry)
        {
            Series[0].Entries.Add(chartEntry);

            int count = Series[0].Entries.Count;

            if (count > MaxNrEnries)
            {
                Series[0].Entries.RemoveRange(0, count - MaxNrEnries);
            };

            Invalidate();
        }

        private void DrawSeriesLine(SKCanvas canvas, SKSize itemSize)
        {
            if (pointsPerSerie.Any() && pointsPerSerie.Values.First().Count > 1 && LineMode != LineMode.None)
            {
                var length = Series.Count;

                var path = new SKPath();
                using var paint2 = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = SKColors.Red,
                    StrokeWidth = LineSize,
                    IsAntialias = true,
                };

                for (int j = 0; j < length; ++j)
                {
                    var s = Series[j];

                    var points = pointsPerSerie[s];

                    //using (var paint = new SKPaint
                    //{
                    //    Style = SKPaintStyle.Stroke,
                    //    Color = s.Color ?? SKColors.White,
                    //    StrokeWidth = LineSize,
                    //    IsAntialias = true,
                    //})
                    {

                        //if (s.Color == null)
                        //    using (var shader = CreateXGradient(points, s.Entries, s.Color))
                        //        paint.Shader = shader;

                        //var path = new SKPath();
                        path.MoveTo(points.First());
                        var last = (LineMode == LineMode.Spline) ? points.Count - 1 : points.Count;
                        for (int i = 0; i < last; i++)
                        {
                            if (LineMode == LineMode.Spline)
                            {
                                var cubicInfo = CalculateCubicInfo(points, i, itemSize);
                                path.CubicTo(cubicInfo.control, cubicInfo.nextControl, cubicInfo.nextPoint);
                            }
                            else if (LineMode == LineMode.Straight)
                            {
                                path.LineTo(points[i]);
                            }
                        }
                        //canvas.DrawPath(path, paint);
                    }
                }

                //canvas.Clear();
                canvas.DrawPath(path, paint2);
            }
        }

        private void DrawLineArea(SKCanvas canvas, ChartSerie serie, List<SKPoint> points, SKSize itemSize, float origin)
        {
            if (LineAreaAlpha > 0 && points.Count > 1)
            {
                using (var paint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = SKColors.White,
                    IsAntialias = true,
                })
                {
                    using (var shaderX = CreateXGradient(points, serie.Entries, serie.Color, (byte)(LineAreaAlpha * AnimationProgress)))
                    using (var shaderY = CreateYGradient(points, (byte)(LineAreaAlpha * AnimationProgress)))
                    {
                        paint.Shader = EnableYFadeOutGradient ? SKShader.CreateCompose(shaderY, shaderX, SKBlendMode.SrcOut) : shaderX;

                        var path = new SKPath();

                        path.MoveTo(points.First().X, origin);
                        path.LineTo(points.First());

                        var last = (LineMode == LineMode.Spline) ? points.Count - 1 : points.Count;
                        for (int i = 0; i < last; i++)
                        {
                            if (LineMode == LineMode.Spline)
                            {
                                var cubicInfo = CalculateCubicInfo(points, i, itemSize);
                                path.CubicTo(cubicInfo.control, cubicInfo.nextControl, cubicInfo.nextPoint);
                            }
                            else if (LineMode == LineMode.Straight)
                            {
                                path.LineTo(points[i]);
                            }
                        }

                        path.LineTo(points.Last().X, origin);
                        path.Close();
                        canvas.DrawPath(path, paint);
                    }
                }
            }
        }

        /// <inheritdoc/>
        protected override void DrawValueLabel(SKCanvas canvas, Dictionary<ChartEntry, SKRect> valueLabelSizes, float headerWithLegendHeight, SKSize itemSize, SKSize barSize, ChartEntry entry, float barX, float barY, float itemX, float origin)
        {
            if (Series.Count() == 1 && ValueLabelOption == ValueLabelOption.TopOfChart)
                base.DrawValueLabel(canvas, valueLabelSizes, headerWithLegendHeight, itemSize, barSize, entry, barX, barY, itemX, origin);
        }

        /// <inheritdoc/>
        protected override void DrawBar(ChartSerie serie, SKCanvas canvas, float headerHeight, float itemX, SKSize itemSize, SKSize barSize, float origin, float barX, float barY, SKColor color)
        {
            //Drawing entry point at center of the item (label) part
            var point = new SKPoint(itemX, barY);
            if (pointsPerSerie.ContainsKey(serie))
                pointsPerSerie[serie].Add(point);
        }

        /// <inheritdoc/>
        protected override void DrawBarArea(SKCanvas canvas, float headerHeight, SKSize itemSize, SKSize barSize, SKColor color, float origin, float value, float barX, float barY)
        {
            //Area is draw on the OnDrawContentEnd
        }

        private (SKPoint control, SKPoint nextPoint, SKPoint nextControl) CalculateCubicInfo(List<SKPoint> points, int i, SKSize itemSize)
        {
            var point = points[i];
            var nextPoint = points[i + 1];
            var controlOffset = new SKPoint(itemSize.Width * 0.8f, 0);
            var currentControl = point + controlOffset;
            var nextControl = nextPoint - controlOffset;
            return (currentControl, nextPoint, nextControl);
        }

        private SKShader CreateXGradient(List<SKPoint> points, IEnumerable<ChartEntry> entries, SKColor? serieColor, byte alpha = 255)
        {
            var startX = points.First().X;
            var endX = points.Last().X;
            var rangeX = endX - startX;

            return SKShader.CreateLinearGradient(
                new SKPoint(startX, 0),
                new SKPoint(endX, 0),
                entries.Select(x => serieColor?.WithAlpha(alpha) ?? x.Color.WithAlpha(alpha)).ToArray(),
                null,
                SKShaderTileMode.Clamp);
        }

        private SKShader CreateYGradient(List<SKPoint> points, byte alpha = 255)
        {
            var startY = points.Max(i => i.Y);
            var endY = 0;

            return SKShader.CreateLinearGradient(
                new SKPoint(0, startY),
                new SKPoint(0, endY),
                new[] { SKColors.White.WithAlpha(alpha), SKColors.White.WithAlpha(0) },
                null,
                SKShaderTileMode.Clamp);
        }

        #endregion
    }
}
