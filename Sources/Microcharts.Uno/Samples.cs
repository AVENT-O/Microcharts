using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microcharts.Uno
{
    public static class Samples
    {
        public static Chart CreateChart()
        {
            return CreateChart1();
        }

        public static Chart CreateChart1()
        {
            var entries = new[]
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
                Entries = entries,
                LabelTextSize = 10,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelTextSize = 10,
                Margin = 10,
                BackgroundColor = new SKColor(10, 10, 10, 10),
            };
        }

        public static Chart CreateChart2()
        {
            int nrEntries = 500;
            float maxValue = 100f;

            var entriesList = new ChartEntry[nrEntries];

            for (int i = 0; i < nrEntries; i = i + 2)
            {
                entriesList[i] = new ChartEntry(0);
                entriesList[i + 1] = new ChartEntry(i * maxValue / nrEntries - 1000 * (float)Math.Sin(0.0001 * i));
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
                LabelTextSize = 55,
                LabelOrientation = Orientation.Horizontal,
                Margin = 0
            };
        }
    }
}
