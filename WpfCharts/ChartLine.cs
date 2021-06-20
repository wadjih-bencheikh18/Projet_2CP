using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace WpfCharts
{
    /// <summary>
    /// Represents a single curve in the radar/spider chart.
    /// </summary>
    public class ChartLine
    {
        public ChartLine()
        {
            FillColor = Colors.Transparent;
            LineThickness = 1;
        }

        /// <summary>
        ///   Gets or sets the point data source. 
        ///   Note that there should be as many data source entries as there are axis in the spider chart.
        /// </summary>
        /// <value>The point data source.</value>
        public List<double> PointDataSource { get; set; }

        /// <summary>
        ///   Gets or sets the name.
        /// </summary>
        /// <value>The name of the line graph.</value>
        public string Name { get; set; }

        /// <summary>
        ///   Gets or sets the line color.
        /// </summary>
        /// <value>The line color.</value>
        public Color LineColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the stroke.
        /// </summary>
        /// <value>
        /// The color of the stroke.
        /// </value>
        public Color FillColor { get; set; }

        /// <summary>
        ///   Gets or sets the entity id.
        /// </summary>
        /// <value>The entity id.</value>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Line thickness
        /// </summary>
        public double LineThickness { get; set; }
    }
}