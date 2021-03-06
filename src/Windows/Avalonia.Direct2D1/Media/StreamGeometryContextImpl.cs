// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using Avalonia.Media;
using Avalonia.Platform;
using SharpDX.Direct2D1;
using SweepDirection = SharpDX.Direct2D1.SweepDirection;
using D2D = SharpDX.Direct2D1;

namespace Avalonia.Direct2D1.Media
{
    public class StreamGeometryContextImpl : IStreamGeometryContextImpl
    {
        private readonly GeometrySink _sink;

        public StreamGeometryContextImpl(GeometrySink sink)
        {
            _sink = sink;
        }

        public void ArcTo(
            Point point,
            Size size,
            double rotationAngle,
            bool isLargeArc,
            Avalonia.Media.SweepDirection sweepDirection)
        {
            _sink.AddArc(new D2D.ArcSegment
            {
                Point = point.ToSharpDX(),
                Size = size.ToSharpDX(),
                RotationAngle = (float)rotationAngle,
                ArcSize = isLargeArc ? ArcSize.Large : ArcSize.Small,
                SweepDirection = (SweepDirection)sweepDirection,
            });
        }

        public void BeginFigure(Point startPoint, bool isFilled)
        {
            _sink.BeginFigure(startPoint.ToSharpDX(), isFilled ? FigureBegin.Filled : FigureBegin.Hollow);
        }

        public void CubicBezierTo(Point point1, Point point2, Point point3)
        {
            _sink.AddBezier(new D2D.BezierSegment
            {
                Point1 = point1.ToSharpDX(),
                Point2 = point2.ToSharpDX(),
                Point3 = point3.ToSharpDX(),
            });
        }

        public void QuadraticBezierTo(Point control, Point dest)
        {
            _sink.AddQuadraticBezier(new D2D.QuadraticBezierSegment
            {
                Point1 = control.ToSharpDX(),
                Point2 = dest.ToSharpDX()
            });
        }

        public void LineTo(Point point)
        {
            _sink.AddLine(point.ToSharpDX());
        }

        public void EndFigure(bool isClosed)
        {
            _sink.EndFigure(isClosed ? FigureEnd.Closed : FigureEnd.Open);
        }

        public void SetFillRule(FillRule fillRule)
        {
            _sink.SetFillMode(fillRule == FillRule.EvenOdd ? FillMode.Alternate : FillMode.Winding);
        }

        public void Dispose()
        {
            _sink.Close();
            _sink.Dispose();
        }
    }
}
