using SkiaSharp;
using System;
using System.Collections.Generic;

namespace Microcharts.Uno
{
    public static class Samples
    {

        public static LineChart CreateChartOld()
        {
            var list = new[]
            {
                new ChartEntry(100)
                {
                        Label = "Week 1",
                        ValueLabel = "200",
                        Color = SKColor.Parse("#266489")
                },
                new ChartEntry(400)
                {
                        Label = "Week 2",
                        ValueLabel = "400",
                        Color = SKColor.Parse("#68B9C0")
                },
                new ChartEntry(100)
                {
                        Label = "Week 3",
                        ValueLabel = "100",
                        Color = SKColor.Parse("#90D585")
                },
                new ChartEntry(600)
                {
                    Label = "Week 4",
                    ValueLabel = "600",
                    Color = SKColor.Parse("#32a852")
                },
                new ChartEntry(200)
                {
                    Label = "Week 5",
                    ValueLabel = "1600",
                    Color = SKColor.Parse("#8EC0D8")
                }
            };

            return new LineChart
            {
                Entries = new List<ChartEntry>(list),
                LabelTextSize = 10,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelTextSize = 10,
                Margin = 10,
                BackgroundColor = new SKColor(10, 10, 10, 10),
            };
        }

        public static LineChart CreateChart()
        {
            int nrEntries = 5000;
            float maxValue = 100f;

            var entriesList = new List<ChartEntry>(nrEntries);

            for (int i = 0; i < nrEntries; i = i + 2)
            {
                entriesList.Add(new ChartEntry(0) { ValueLabel="a", Label="1"});
                entriesList.Add(new ChartEntry(i * maxValue / nrEntries - 100 * (float)Math.Sin(0.0001 * i)) { ValueLabel = "a", Label = "1" });
            }

            return new LineChart
            {
                IsAnimated = false,
                Entries = entriesList,
                PointSize = 0,
                EnableYFadeOutGradient = false,
                LineSize = 1,
                LineMode = LineMode.Straight,
                MaxValue = maxValue,
                MinValue = 0, // -maxValue,
                LineAreaAlpha = 0,
                PointAreaAlpha = 0,
                ValueLabelTextSize = 10,
                LabelTextSize = 10,
                LabelOrientation = Orientation.Horizontal,
                Margin = 10,
                BackgroundColor = SKColors.Transparent,
                PointMode = PointMode.None
            };
        }

        public static MapChart CreateMapChart()
        {
            int nrEntries = 5000;
            float maxValue = 100f;

            var entriesList = new List<ChartEntry>(nrEntries);

            for (int i = 0; i < nrEntries; i = i + 2)
            {
                entriesList.Add(new ChartEntry(0) { ValueLabel = "a", Label = "1" });
                entriesList.Add(new ChartEntry(i * maxValue / nrEntries - 100 * (float)Math.Sin(0.0001 * i)) { ValueLabel = "a", Label = "1" });
            }

            return new MapChart
            {
                IsAnimated = false,
                Entries = entriesList,
                PointSize = 0,
                EnableYFadeOutGradient = false,
                LineSize = 1,
                LineMode = LineMode.Straight,
                MaxValue = maxValue,
                MinValue = 0, // -maxValue,
                LineAreaAlpha = 0,
                PointAreaAlpha = 0,
                ValueLabelTextSize = 10,
                LabelTextSize = 10,
                LabelOrientation = Orientation.Horizontal,
                Margin = 10,
                BackgroundColor = SKColors.Transparent,
                PointMode = PointMode.None
            };
        }
    }
}
