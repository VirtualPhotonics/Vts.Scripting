﻿using Plotly.NET;
using Plotly.NET.LayoutObjects;
using Chart = Plotly.NET.CSharp.Chart;
using GenericChartExtensions = Plotly.NET.CSharp.GenericChartExtensions;

namespace Vts.Scripting;

public static class ScriptHelper
{
    /// <summary>
    /// Helper extension method that returns an array of midpoints, located halfway between the endpoints of the specified range
    /// </summary>
    /// <param name="endpointRange">The range of endpoints</param>
    /// <returns>The corresponding midpoint outputs</returns>
    public static double[] GetMidpoints(this DoubleRange endpointRange)
    {
        return endpointRange.ToArray().GetMidpoints();
    }

    /// <summary>
    /// Method to create a standard scatter chart from the specified x and y values using Plotly.NET
    /// </summary>
    /// <param name="xValues">The values for the x-axis.</param>
    /// <param name="yValues">The values for the y-axis.</param>
    /// <param name="xLabel">The label for the x-axis. Optional.</param>
    /// <param name="yLabel">The label for the y-axis. Optional.</param>
    /// <param name="title">The title of the chart. Optional.</param>
    /// <returns>A `GenericChart` instance representing the scatter chart.</returns>
    public static GenericChart ScatterChart(double[] xValues, double[] yValues, string xLabel = "", string yLabel = "", string title = "")
    {
        return Chart.Point<double, double, string>(xValues, yValues).WithStandardStyling(xLabel, yLabel, title);
    }

    /// <summary>
    /// Method to create a standard scatter chart from the specified x and y values using Plotly.NET
    /// </summary>
    /// <param name="xValues">The values for the x-axis.</param>
    /// <param name="yValues">The values for the y-axis.</param>
    /// <param name="xLabel">The label for the x-axis. Optional.</param>
    /// <param name="yLabel">The label for the y-axis. Optional.</param>
    /// <param name="title">The title of the chart. Optional.</param>
    /// <returns>A `GenericChart` instance representing the line chart.</returns>
    public static GenericChart LineChart(double[] xValues, double[] yValues, string xLabel = "", string yLabel = "", string title = "")
    {
        return Chart.Line<double, double, string>(xValues, yValues).WithStandardStyling(xLabel, yLabel, title);
    }

    /// <summary>
    /// Fluent helper method to apply standard styling to a chart
    /// </summary>
    /// <param name="chart">The `GenericChart` instance to apply styling to.</param>
    /// <param name="xLabel">The label for the x-axis. Optional.</param>
    /// <param name="yLabel">The label for the y-axis. Optional.</param>
    /// <param name="title">The title of the chart. Optional.</param>
    /// <returns>A `GenericChart` instance with standard styling applied.</returns>
    private static GenericChart WithStandardStyling(
        this GenericChart chart, string xLabel = "", string yLabel = "", string title = "")
    {
        // uses Plotly.NET.CSharp.ChartExtensions (adding Plotly.NET to the using statements above will break this)
        return GenericChartExtensions.WithTraceInfo(chart, title, ShowLegend: !string.IsNullOrWhiteSpace(title))
            .WithXAxisStyle<double, double, string>(Title: Title.init(xLabel))
            .WithYAxisStyle<double, double, string>(Title: Title.init(yLabel))
            .WithLegendStyle(X: 0, Y: 150);
    }

    /// <summary>
    /// Helper method to format a heatmap chart using Plotly.NET.
    /// </summary>
    /// <param name="values">An `IEnumerable` of double arrays specifying the z values of the heatmap.</param>
    /// <param name="x">An array of double values specifying the x values of the heatmap.</param>
    /// <param name="y">An array of double values specifying the y values of the heatmap.</param>
    /// <param name="xLabel">An optional label for the x-axis. Default is an empty string.</param>
    /// <param name="yLabel">An optional label for the y-axis. Default is an empty string.</param>
    /// <param name="title">An optional title for the chart. Default is an empty string.</param>
    /// <returns>A `GenericChart` instance representing the heatmap chart.</returns>
    public static GenericChart Heatmap(
        IEnumerable<double[]> values,
        double[] x,
        double[] y,
        string xLabel = "",
        string yLabel = "",
        string title = "")
    {
        // attn developers: for reference, the following are the type parameters used in the call to Chart2D.Chart.Heatmap:
        // Chart2D.Chart.Heatmap<a37: (row format), a38: (fluence value type), a39: X (rho value type), a40: Y (z value type), a41: Text type>(...)
        var chart = GenericChartExtensions.WithTraceInfo(Chart2D.Chart.Heatmap<IEnumerable<double>, double, double, double, string>(
            zData: values,
            X: x, Y: y,
            ReverseScale: false, ReverseYAxis: true,
            Transpose: true,
            Text: title,
            ColorScale: StyleParam.Colorscale.Hot
        ), title, ShowLegend: !string.IsNullOrWhiteSpace(title))
         .WithLegendStyle(X: 0, Y: 150);

        chart = chart
            .WithYAxis(LinearAxis.init<IConvertible, IConvertible, IConvertible, IConvertible, double, IConvertible, IConvertible, IConvertible>(AxisType: StyleParam.AxisType.Linear))
            .WithXAxisStyle<double, double, string>(Title: Title.init(xLabel), MinMax: new Tuple<double, double>(x[0], x[^1]))
            .WithYAxisStyle<double, double, string>(Title: Title.init(yLabel), MinMax: new Tuple<double, double>(y[0], y[^1]))
            .WithColorbar(title: Title.init(title));

        return chart;
    }
}
