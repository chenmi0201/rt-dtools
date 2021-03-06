﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DicomPanel.Core.Render;
using RT.Core.Utilities.RTMath;
using System.Windows.Media.Effects;
using RT.Core.DICOM;
using System.Windows;

namespace DicomPanel.Wpf.Rendering
{
    public class CanvasRenderContext : BaseRenderContext, IRenderContext
    {
        public double RelativeScale { get; set; }
        public Canvas Canvas;

        public CanvasRenderContext(Canvas canvas)
        {
            Canvas = canvas;
        }


        public void FillPixels(byte[] byteArray, Rectd destRect)
        {
            //Currently don't know how to do this...
        }

        public void DrawString(string text, double x, double y, double size, DicomColor color)
        {
            TextBlock tb = new TextBlock();
            var dse = new DropShadowEffect();
            dse.BlurRadius = 1;
            dse.ShadowDepth = 4;
            dse.Direction = 0;
            tb.FontSize = size;
            tb.Foreground = new SolidColorBrush(DicomColorConverter.FromDicomColor(color));
            tb.Text = text;
            Canvas.SetLeft(tb, x * Canvas.ActualWidth);
            Canvas.SetTop(tb, y * Canvas.ActualHeight);

            FormattedText fm = new FormattedText(tb.Text, System.Globalization.CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, new Typeface(tb.FontFamily, tb.FontStyle, tb.FontWeight, tb.FontStretch),tb.FontSize,tb.Foreground);

            FillRect(x , y , x + fm.Width/Canvas.ActualWidth, y + fm.Height /Canvas.ActualHeight, DicomColor.FromArgb(128, 0, 0, 0),DicomColor.FromArgb(0,0,0,0));

            Canvas?.Children.Add(tb);
        }

        public void DrawRect(double x0, double y0, double x1, double y1, DicomColor color)
        {
            x0 *= Canvas.ActualWidth;
            x1 *= Canvas.ActualWidth;
            y1 *= Canvas.ActualHeight;
            y0 *= Canvas.ActualHeight;

            Rectangle rectangle = new Rectangle();
            rectangle.Stroke = new SolidColorBrush(DicomColorConverter.FromDicomColor(color));
            rectangle.Width = Math.Abs(x1 - x0);
            rectangle.Height = Math.Abs(y1 - y0);
            Canvas.SetLeft(rectangle, Math.Min(x0, x1));
            Canvas.SetTop(rectangle, Math.Min(y0, y1));
            Canvas?.Children.Add(rectangle);
        }

        public void DrawLine(double x0, double y0, double x1, double y1, DicomColor color, LineType lineType)
        {
            x0 *= Canvas.ActualWidth;
            x1 *= Canvas.ActualWidth;
            y1 *= Canvas.ActualHeight;
            y0 *= Canvas.ActualHeight;
            Line line = new Line();
            line.Stroke = new SolidColorBrush(DicomColorConverter.FromDicomColor(color));
            if (lineType != LineType.Normal)
            {
                line.StrokeDashArray = new DoubleCollection();
                if (lineType == LineType.Dotted)
                {
                    line.StrokeDashArray.Add(1);
                    line.StrokeDashArray.Add(1);
                }else if(lineType == LineType.Dashed)
                {
                    line.StrokeDashArray.Add(2);
                    line.StrokeDashArray.Add(2);
                }
            }
            line.StrokeThickness = 1;
            line.X1 = x0;
            line.X2 = x1;
            line.Y1 = y0;
            line.Y2 = y1;
            Canvas?.Children.Add(line);
        }

        public void FillRect(double x0, double y0, double x1, double y1, DicomColor fill, DicomColor stroke)
        {
            x0 *= Canvas.ActualWidth;
            x1 *= Canvas.ActualWidth;
            y1 *= Canvas.ActualHeight;
            y0 *= Canvas.ActualHeight;

            Rectangle rectangle = new Rectangle();
            rectangle.Fill = new SolidColorBrush(DicomColorConverter.FromDicomColor(fill));
            rectangle.Stroke = new SolidColorBrush(DicomColorConverter.FromDicomColor(stroke));
            rectangle.Width = Math.Abs(x1 - x0);
            rectangle.Height = Math.Abs(y1 - y0);
            Canvas.SetLeft(rectangle, Math.Min(x0, x1));
            Canvas.SetTop(rectangle, Math.Min(y0, y1));
            Canvas?.Children.Add(rectangle);
        }

        public void BeginRender()
        {
            Canvas.Children.Clear();
            Canvas.BeginInit();
        }

        public void EndRender()
        {
            Canvas.EndInit();
        }

        public void DrawEllipse(double x0, double y0, double radiusX, double radiusY, DicomColor color)
        {
            x0 *= Canvas.ActualWidth;
            y0 *= Canvas.ActualHeight;
            radiusX *= Canvas.ActualWidth;
            radiusY *= Canvas.ActualHeight;

            Ellipse ellipse = new Ellipse();
            ellipse.Stroke = new SolidColorBrush(DicomColorConverter.FromDicomColor(color));
            ellipse.Width = Math.Abs(radiusX);
            ellipse.Height = Math.Abs(radiusY);
            Canvas.SetLeft(ellipse, x0-radiusX/2);
            Canvas.SetTop(ellipse, y0-radiusY/2);
            Canvas?.Children.Add(ellipse);
        }

        public void DrawLine(double x0, double y0, double x1, double y1, DicomColor color)
        {
            DrawLine(x0, y0, x1, y1, color, LineType.Normal);
        }

        public void DrawLines(double[] vertices, DicomColor color)
        {
            for(int i = 0; i < vertices.Length; i+=4)
            {
                DrawLine(vertices[i], vertices[i + 1], vertices[i + 2], vertices[i + 3], color);
            }
        }

        public void DrawLine(Point2d p1, Point2d p2, DicomColor color)
        {
            DrawLine(p1.X, p1.Y, p2.X, p2.Y, color);
        }

        public void DrawArrow(Point2d p1, Point2d p2, DicomColor color)
        {
            DrawArrow(p1.X, p1.Y, p2.X, p2.Y, color);
        }

        public void DrawArrow(double x0, double y0, double x1, double y1, DicomColor color)
        {
            throw new NotImplementedException();
        }
    }
}
