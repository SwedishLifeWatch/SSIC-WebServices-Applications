var AnalysisPortal;
(function (AnalysisPortal) {
    (function (Statistics) {
        /**
        * ColorInterpolationMode enum represents different color interpolation modes.
        */
        (function (ColorInterpolationMode) {
            ColorInterpolationMode[ColorInterpolationMode["linear"] = 0] = "linear";
            ColorInterpolationMode[ColorInterpolationMode["pow2"] = 1] = "pow2";
            ColorInterpolationMode[ColorInterpolationMode["pow3"] = 2] = "pow3";
            ColorInterpolationMode[ColorInterpolationMode["pow4"] = 3] = "pow4";
            ColorInterpolationMode[ColorInterpolationMode["pow5"] = 4] = "pow5";
            ColorInterpolationMode[ColorInterpolationMode["pow6"] = 5] = "pow6";
            ColorInterpolationMode[ColorInterpolationMode["pow7"] = 6] = "pow7";
            ColorInterpolationMode[ColorInterpolationMode["pow8"] = 7] = "pow8";
            ColorInterpolationMode[ColorInterpolationMode["pow9"] = 8] = "pow9";
            ColorInterpolationMode[ColorInterpolationMode["pow10"] = 9] = "pow10";
            ColorInterpolationMode[ColorInterpolationMode["log2"] = 10] = "log2";
            ColorInterpolationMode[ColorInterpolationMode["log3"] = 11] = "log3";
            ColorInterpolationMode[ColorInterpolationMode["log4"] = 12] = "log4";
            ColorInterpolationMode[ColorInterpolationMode["log5"] = 13] = "log5";
            ColorInterpolationMode[ColorInterpolationMode["log6"] = 14] = "log6";
            ColorInterpolationMode[ColorInterpolationMode["log7"] = 15] = "log7";
            ColorInterpolationMode[ColorInterpolationMode["log8"] = 16] = "log8";
            ColorInterpolationMode[ColorInterpolationMode["log9"] = 17] = "log9";
            ColorInterpolationMode[ColorInterpolationMode["log10"] = 18] = "log10";
        })(Statistics.ColorInterpolationMode || (Statistics.ColorInterpolationMode = {}));
        var ColorInterpolationMode = Statistics.ColorInterpolationMode;

        /**
        * GradientStop class represents a color stop in a GradientColorCollection.
        */
        var GradientStop = (function () {
            /**
            * GradientStop constructor
            * @param color a color
            * @param offset a number between 0.0 and 1.0
            */
            function GradientStop(color, offset) {
                this.color = null;
                this.offset = 0.0;
                this.color = color;
                this.offset = offset;
            }
            return GradientStop;
        })();
        Statistics.GradientStop = GradientStop;

        /**
        * This class contains a collection of Gradient stops
        * and has methods to calculate new Colors from those.
        */
        var GradientColorCollection = (function () {
            /**
            * Creates a new GradientColorCollection
            */
            function GradientColorCollection(startValue, endValue, gradientStops) {
                this.startValue = 0.0;
                this.endValue = 0.0;
                this.startValue = startValue;
                this.endValue = endValue;
                this.gradientStops = gradientStops;
            }
            /**
            * Gets a color that is calculated from the color collection
            * @param offset a value between 0.0 and 1.0
            */
            GradientColorCollection.prototype.getColorFromGradient = function (offset) {
                var i, calcOffset, a, r, g, b;
                var color, color2;

                if (offset <= this.gradientStops[0].offset) {
                    return this.gradientStops[0].color;
                }
                if (offset >= this.gradientStops[this.gradientStops.length - 1].offset) {
                    return this.gradientStops[this.gradientStops.length - 1].color;
                }
                for (i = 1; i < this.gradientStops.length; i++) {
                    if (offset <= this.gradientStops[i].offset) {
                        calcOffset = (this.gradientStops[i].offset == this.gradientStops[i - 1].offset) ? 0.0 : ((offset - this.gradientStops[i - 1].offset) / (this.gradientStops[i].offset - this.gradientStops[i - 1].offset));
                        color = this.gradientStops[i - 1].color;
                        color2 = this.gradientStops[i].color;
                        a = Math.round(color.a + ((calcOffset * (color2.a - color.a))));
                        r = Math.round(color.r + ((calcOffset * (color2.r - color.r))));
                        g = Math.round(color.g + ((calcOffset * (color2.g - color.g))));
                        b = Math.round(color.b + ((calcOffset * (color2.b - color.b))));
                        color = new Color(r, g, b, a);
                        return color;
                    }
                }
                color = new Color(r, g, b, a);
                return color;
            };

            /**
            * Gets a interpolated color that is calculated from the color collection
            * @param offset a value between 0.0 and 1.0
            * @param interpolationMode the interpolation method to use
            */
            GradientColorCollection.prototype.getInterpolatedColorFromGradient = function (offset, interpolationMode) {
                var interpolationFunction = GradientColorCollection.getInterpolationFunction(interpolationMode);
                var interpolatedValue = interpolationFunction(offset);
                var color = this.getColorFromGradient(interpolatedValue);
                return color;
            };

            /**
            * Gets a color
            * @param val
            */
            GradientColorCollection.prototype.getColor = function (val) {
                var offset = 0.0;
                if (this.startValue != this.endValue) {
                    offset = ((val - this.startValue) / (this.endValue - this.startValue));
                }
                return this.getColorFromGradient(offset);
            };

            /**
            * Gets a color
            * @param val The value (e.g. number of observations)
            * @param binDelta The bin interval (bin.upperBound - bin.lowerBound)
            */
            GradientColorCollection.prototype.getBinColor = function (val, binInterval) {
                var offset = 0.0;
                if (this.startValue != this.endValue) {
                    var startValue = this.startValue - (binInterval / 2.0);
                    var endValue = this.endValue + (binInterval / 2.0);
                    offset = ((val - startValue) / (endValue - startValue));
                }
                return this.getColorFromGradient(offset);
            };

            /**
            * Creates a gradient color collection with linear interval between color offsets.
            */
            GradientColorCollection.createGradientColorCollection = function (startValue, endValue, colors) {
                var interval = 1.0 / (colors.length - 1);
                var gradientStops = [];
                for (var i = 0; i < colors.length; i++) {
                    var gradientStop = new GradientStop(colors[i], i * interval);
                    gradientStops.push(gradientStop);
                }
                var colorCollection = new GradientColorCollection(startValue, endValue, gradientStops);
                return colorCollection;
            };

            /**
            * Creates a sample gradient color collection that ranges
            * from red to yellow to green
            * red offset = 0.0
            * yellow offset = 0.5
            * green offset = 1.0
            */
            GradientColorCollection.createRedYellowGreenGradientColorCollection = function (startValue, endValue) {
                return GradientColorCollection.createGradientColorCollection(startValue, endValue, new Array(new Color(255, 0, 0), new Color(255, 255, 0), new Color(0, 255, 0)));
            };

            /**
            * Returns a JavaScript function that can be used to interpolate a value.
            */
            GradientColorCollection.getInterpolationFunction = function (interpolationMode) {
                switch (interpolationMode) {
                    case 0 /* linear */:
                        return GradientColorCollection.interpolateColorFunctionLinear;
                    case 1 /* pow2 */:
                        return GradientColorCollection.interpolateColorFunctionPow2;
                    case 2 /* pow3 */:
                        return GradientColorCollection.interpolateColorFunctionPow3;
                    case 3 /* pow4 */:
                        return GradientColorCollection.interpolateColorFunctionPow4;
                    case 4 /* pow5 */:
                        return GradientColorCollection.interpolateColorFunctionPow5;
                    case 5 /* pow6 */:
                        return GradientColorCollection.interpolateColorFunctionPow6;
                    case 6 /* pow7 */:
                        return GradientColorCollection.interpolateColorFunctionPow7;
                    case 7 /* pow8 */:
                        return GradientColorCollection.interpolateColorFunctionPow8;
                    case 8 /* pow9 */:
                        return GradientColorCollection.interpolateColorFunctionPow9;
                    case 9 /* pow10 */:
                        return GradientColorCollection.interpolateColorFunctionPow10;
                    case 10 /* log2 */:
                        return GradientColorCollection.interpolateColorFunctionLog2;
                    case 11 /* log3 */:
                        return GradientColorCollection.interpolateColorFunctionLog3;
                    case 12 /* log4 */:
                        return GradientColorCollection.interpolateColorFunctionLog4;
                    case 13 /* log5 */:
                        return GradientColorCollection.interpolateColorFunctionLog5;
                    case 14 /* log6 */:
                        return GradientColorCollection.interpolateColorFunctionLog6;
                    case 15 /* log7 */:
                        return GradientColorCollection.interpolateColorFunctionLog7;
                    case 16 /* log8 */:
                        return GradientColorCollection.interpolateColorFunctionLog8;
                    case 17 /* log9 */:
                        return GradientColorCollection.interpolateColorFunctionLog9;
                    case 18 /* log10 */:
                        return GradientColorCollection.interpolateColorFunctionLog10;

                    default:
                        return GradientColorCollection.interpolateColorFunctionLinear;
                }
            };

            GradientColorCollection.interpolateColorFunctionLinear = function (x) {
                return x;
            };
            GradientColorCollection.interpolateColorFunctionPow2 = function (x) {
                return Math.pow(x, 2);
            };
            GradientColorCollection.interpolateColorFunctionPow3 = function (x) {
                return Math.pow(x, 3);
            };
            GradientColorCollection.interpolateColorFunctionPow4 = function (x) {
                return Math.pow(x, 4);
            };
            GradientColorCollection.interpolateColorFunctionPow5 = function (x) {
                return Math.pow(x, 5);
            };
            GradientColorCollection.interpolateColorFunctionPow6 = function (x) {
                return Math.pow(x, 6);
            };
            GradientColorCollection.interpolateColorFunctionPow7 = function (x) {
                return Math.pow(x, 7);
            };
            GradientColorCollection.interpolateColorFunctionPow8 = function (x) {
                return Math.pow(x, 8);
            };
            GradientColorCollection.interpolateColorFunctionPow9 = function (x) {
                return Math.pow(x, 9);
            };
            GradientColorCollection.interpolateColorFunctionPow10 = function (x) {
                return Math.pow(x, 10);
            };
            GradientColorCollection.interpolateColorFunctionLog2 = function (x) {
                return 1 - Math.pow(1 - x, 2);
            };
            GradientColorCollection.interpolateColorFunctionLog3 = function (x) {
                return 1 - Math.pow(1 - x, 3);
            };
            GradientColorCollection.interpolateColorFunctionLog4 = function (x) {
                return 1 - Math.pow(1 - x, 4);
            };
            GradientColorCollection.interpolateColorFunctionLog5 = function (x) {
                return 1 - Math.pow(1 - x, 5);
            };
            GradientColorCollection.interpolateColorFunctionLog6 = function (x) {
                return 1 - Math.pow(1 - x, 6);
            };
            GradientColorCollection.interpolateColorFunctionLog7 = function (x) {
                return 1 - Math.pow(1 - x, 7);
            };
            GradientColorCollection.interpolateColorFunctionLog8 = function (x) {
                return 1 - Math.pow(1 - x, 8);
            };
            GradientColorCollection.interpolateColorFunctionLog9 = function (x) {
                return 1 - Math.pow(1 - x, 9);
            };
            GradientColorCollection.interpolateColorFunctionLog10 = function (x) {
                return 1 - Math.pow(1 - x, 10);
            };

            /**
            * Creates a gradient object to use in HTML5 canvas.
            */
            GradientColorCollection.prototype.createHTML5Gradient = function (context, x0, y0, x1, y1) {
                var gradient = context.createLinearGradient(x0, y0, x1, y1);
                var i;
                for (i = 0; i < this.gradientStops.length; i++) {
                    gradient.addColorStop(this.gradientStops[i].offset, this.gradientStops[i].color.rgbaString());
                }
                return gradient;
            };
            return GradientColorCollection;
        })();
        Statistics.GradientColorCollection = GradientColorCollection;

        /**
        * Color class
        */
        var Color = (function () {
            /**
            * Creates a new color object.
            */
            function Color(r, g, b, a) {
                this.r = 0;
                this.g = 0;
                this.b = 0;
                this.a = 255;
                this.r = r;
                this.g = g;
                this.b = b;
                this.a = a || 255;
            }
            /**
            * Gets the color as hex string. For example. #aa10b1
            */
            Color.prototype.toHexString = function () {
                return "#" + ((1 << 24) + (this.r << 16) + (this.g << 8) + this.b).toString(16).slice(1);
            };

            /**
            * Creates a color object from a hex string.
            */
            Color.colorFromHexString = function (hex) {
                // Expand shorthand form (e.g. "03F") to full form (e.g. "0033FF")
                var shorthandRegex = /^#?([a-f\d])([a-f\d])([a-f\d])$/i;
                hex = hex.replace(shorthandRegex, function (m, r, g, b) {
                    return r + r + g + g + b + b;
                });
                var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
                if (result != null) {
                    var r = parseInt(result[1], 16);
                    var g = parseInt(result[2], 16);
                    var b = parseInt(result[3], 16);
                    return new Color(r, g, b, 255);
                }
                return null;
            };

            /**
            * Creates a rgba string.
            */
            Color.prototype.rgbaString = function () {
                return "rgba(" + this.r + ", " + this.g + ", " + this.b + ", " + this.a / 255.0 + ")";
            };
            return Color;
        })();
        Statistics.Color = Color;

        /**
        * This class is a histogram bin that should be part of a Histogram.
        */
        var HistogramBin = (function () {
            /**
            * Creates a new HistogramBin.
            * @param lowerBound The lower bound. Can be null.
            * @param upperBound The upper bound. Can be null.
            * @param frequency The number of items.
            * @param relativeFrequency The frequency compared to total number of items.
            * @param color The histogram bin color.
            */
            function HistogramBin(lowerBound, upperBound, frequency, relativeFrequency, color) {
                this.lowerBound = 0;
                this.upperBound = 0;
                this.frequency = 0;
                this.relativeFrequency = 0.0;
                this.relativeFrequencyToAllItemsPlacedInBins = 0.0;
                this.lowerBound = lowerBound;
                this.upperBound = upperBound;
                this.frequency = frequency || 0;
                this.relativeFrequency = relativeFrequency || 0.0;
                this.color = color || null;
            }
            /**
            * Checks if a value belongs to this bin.
            */
            HistogramBin.prototype.isValueInBin = function (val) {
                if (this.lowerBound == null)
                    return val <= this.upperBound;
                if (this.upperBound == null)
                    return val >= this.lowerBound;
                if (this.lowerBound == null && this.upperBound == null)
                    return true;
                return val >= this.lowerBound && val <= this.upperBound;
            };

            /**
            * Gets a string representation of the bin
            */
            HistogramBin.prototype.toString = function () {
                if (this.lowerBound == null)
                    return '<= ' + this.upperBound;
                if (this.upperBound == null)
                    return '>= ' + this.lowerBound;
                if (this.lowerBound == null && this.upperBound == null)
                    return "*";
                if (this.lowerBound == this.upperBound)
                    return this.lowerBound.toString();
                return this.lowerBound + ' - ' + this.upperBound;
            };
            return HistogramBin;
        })();
        Statistics.HistogramBin = HistogramBin;

        var Histogram = (function () {
            function Histogram(bins) {
                this.minValue = 0;
                this.maxValue = 0;
                this.colorInterpolationMode = 0 /* linear */;
                this.bins = bins;
            }
            Histogram.prototype.calculateHistogramBinColors = function (colorCollection, interpolationMode) {
                if (interpolationMode == null) {
                    interpolationMode = this.colorInterpolationMode;
                }
                var fracDelta = 1.0 / (this.bins.length - 1);

                for (var i = 0; i < this.bins.length; i++) {
                    var bin = this.bins[i];
                    if (i == 0)
                        bin.color = colorCollection.getInterpolatedColorFromGradient(0, interpolationMode);
                    else if (i == this.bins.length - 1)
                        bin.color = colorCollection.getInterpolatedColorFromGradient(1, interpolationMode);
                    else
                        bin.color = colorCollection.getInterpolatedColorFromGradient(i * fracDelta, interpolationMode);
                    //if (i == 0)
                    //    bin.color = colorCollection.getColorFromGradient(0);
                    //else if (i == this.bins.length - 1)
                    //    bin.color = colorCollection.getColorFromGradient(1);
                    //else
                    //    bin.color = colorCollection.getColorFromGradient(i * fracDelta);
                }
            };

            Histogram.prototype.calculateHistogramBinColors2 = function (colorCollection) {
                var fracDelta = 1.0 / (this.bins.length - 1);
                var binDelta = 0;
                if (this.bins.length > 2)
                    binDelta = this.bins[1].upperBound - this.bins[0].upperBound;

                for (var i = 0; i < this.bins.length; i++) {
                    var bin = this.bins[i];
                    if (i == 0)
                        bin.color = colorCollection.getColorFromGradient(0);
                    else if (i == this.bins.length - 1)
                        bin.color = colorCollection.getColorFromGradient(1);
                    else
                        bin.color = colorCollection.getBinColor((bin.lowerBound + bin.upperBound) / 2.0, binDelta);
                }
            };

            Histogram.calculateIntegerBoundedHistogram = function (sortedValues, nrBins) {
                var bins = new Array();
                var i, j;
                var lowestValue = sortedValues[0];
                var highestValue = sortedValues[sortedValues.length - 1];
                var numberOfItemsPlacedInBins = 0;
                var relativeFrequencyInEachBin = 1.0 / nrBins;

                if (nrBins == 1) {
                    bins.push(new HistogramBin(lowestValue, highestValue));
                } else if (nrBins == 2) {
                    var midValue = sortedValues[Math.floor((sortedValues.length - 1) * relativeFrequencyInEachBin)];
                    bins.push(new HistogramBin(lowestValue, midValue));
                    var bin2 = new HistogramBin(midValue + 1, highestValue);
                    if (bin2.upperBound >= bin2.lowerBound)
                        bins.push(bin2);
                } else if (nrBins > 2) {
                    var firstBin = new HistogramBin(lowestValue, sortedValues[Math.floor((sortedValues.length - 1) * relativeFrequencyInEachBin)]);
                    var prevBin = firstBin;
                    bins.push(firstBin);
                    for (i = 1; i < nrBins; i++) {
                        var lowerBound = prevBin.upperBound + 1;
                        var upperBound = sortedValues[Math.floor((sortedValues.length - 1) * (relativeFrequencyInEachBin * (i + 1)))];

                        // hack - last bin will have upperBound=highestValue
                        if (i == nrBins - 1)
                            upperBound = highestValue;
                        
                        // change bounds if error
                        if (upperBound < lowerBound)
                            continue;
                        //if (upperBound < lowerBound)
                        //    upperBound = lowerBound;
                        var bin = new HistogramBin(lowerBound, upperBound);
                        bins.push(bin);
                        prevBin = bin;
                    }
                }

                var binIndex = 0;
                i = 0;
                while (binIndex < bins.length && i < sortedValues.length) {
                    if (bins[binIndex].isValueInBin(sortedValues[i])) {
                        bins[binIndex].frequency++;
                        numberOfItemsPlacedInBins++;
                        i++;
                        continue;
                    } else {
                        binIndex++;
                        if (binIndex >= bins.length)
                            break;
                        if (sortedValues[i] < bins[binIndex].lowerBound) {
                            i++;
                        }
                    }
                }

                for (j = 0; j < bins.length; j++) {
                    bins[j].relativeFrequency = bins[j].frequency / sortedValues.length;
                    if (numberOfItemsPlacedInBins != 0)
                        bins[j].relativeFrequencyToAllItemsPlacedInBins = bins[j].frequency / numberOfItemsPlacedInBins;
                    else
                        bins[j].relativeFrequencyToAllItemsPlacedInBins = 0;
                }

                var histogram = new Histogram(bins);
                histogram.minValue = lowestValue;
                histogram.maxValue = highestValue;
                return histogram;
            };

            Histogram.calculateHistogramWithPredefinedBins = function (sortedValues, bins) {
                var i = 0, j;
                var binIndex = 0;
                var numberOfItemsPlacedInBins = 0;
                var lowestValue = sortedValues[0];
                var highestValue = sortedValues[sortedValues.length - 1];

                while (binIndex < bins.length && i < sortedValues.length) {
                    if (bins[binIndex].isValueInBin(sortedValues[i])) {
                        bins[binIndex].frequency++;
                        i++;
                        numberOfItemsPlacedInBins++;
                        continue;
                    } else {
                        if (sortedValues[i] > bins[binIndex].upperBound)
                            binIndex++;
                        if (binIndex >= bins.length)
                            break;
                        if (sortedValues[i] < bins[binIndex].lowerBound) {
                            i++;
                        }
                    }
                }

                for (j = 0; j < bins.length; j++) {
                    bins[j].relativeFrequency = bins[j].frequency / sortedValues.length;
                    if (numberOfItemsPlacedInBins != 0)
                        bins[j].relativeFrequencyToAllItemsPlacedInBins = bins[j].frequency / numberOfItemsPlacedInBins;
                    else
                        bins[j].relativeFrequencyToAllItemsPlacedInBins = 0;
                }

                var histogram = new Histogram(bins);
                histogram.minValue = lowestValue;
                histogram.maxValue = highestValue;
                return histogram;
                //return new Histogram(bins);
            };

            Histogram.calculateHistogram = function (sortedValues, startValue, endValue, nrBins) {
                var interval = (endValue - startValue) / (nrBins - 2);
                var bins = new Array();
                var i, j;
                var numberOfItemsPlacedInBins;
                var lowestValue = sortedValues[0];
                var highestValue = sortedValues[sortedValues.length - 1];

                if (nrBins == 1) {
                    bins.push(new HistogramBin(null, null));
                } else if (nrBins == 2) {
                    var midValue = (startValue + endValue) / 2;
                    bins.push(new HistogramBin(null, midValue));
                    bins.push(new HistogramBin(midValue, null));
                } else if (nrBins > 2) {
                    bins.push(new HistogramBin(0, startValue));
                    for (i = 0; i < (nrBins - 2) ; i++) {
                        bins.push(new HistogramBin(startValue + (interval * i), startValue + (interval * (i + 1))));
                    }
                    bins.push(new HistogramBin(endValue, 99999999999));
                }

                var binIndex = 0;
                for (i = 0; i < sortedValues.length; i++) {
                    while (sortedValues[i] > bins[binIndex].upperBound) {
                        binIndex++;
                    }
                    bins[binIndex].frequency++;
                    numberOfItemsPlacedInBins++;
                    //bins[binIndex].distance += (sortedValues[i] / this.nrSecondsSamplingInterval);
                    //bins[binIndex].time += this.nrSecondsSamplingInterval;
                }

                for (j = 0; j < bins.length; j++) {
                    bins[j].relativeFrequency = bins[j].frequency / sortedValues.length;
                    if (numberOfItemsPlacedInBins != 0)
                        bins[j].relativeFrequencyToAllItemsPlacedInBins = bins[j].frequency / numberOfItemsPlacedInBins;
                    else
                        bins[j].relativeFrequencyToAllItemsPlacedInBins = 0;
                }

                var histogram = new Histogram(bins);
                histogram.minValue = lowestValue;
                histogram.maxValue = highestValue;
                return histogram;
            };

            Histogram.getNumberOfDifferentValues = function (sortedValues) {
                if (sortedValues == null || sortedValues.length == 0)
                    return 0;
                var count = 1;
                var currentVal = sortedValues[0];
                for (var i = 1; i < sortedValues.length; i++) {
                    var val = sortedValues[i];
                    if (currentVal != val) {
                        count++;
                        currentVal = val;
                    }
                }
                return count;
            };

            Histogram.calcNumberOfBins = function (sortedValues, minMaxDiff) {
                var numberOfDifferentValues = Histogram.getNumberOfDifferentValues(sortedValues);
                var maxNrBins = Math.max(Math.floor(minMaxDiff) + 1, 2);
                if (numberOfDifferentValues <= 2)
                    return 2;
                if (numberOfDifferentValues <= 7)
                    return Math.min(maxNrBins, numberOfDifferentValues);
                return Math.min(7, maxNrBins);
            };
            Histogram.getPercentiles = function (sortedValues, lowFractional, highFractional) {
                var lowValue = sortedValues[Math.floor((sortedValues.length - 1) * lowFractional)];
                var highValue = sortedValues[Math.floor((sortedValues.length - 1) * highFractional)];
                return [lowValue, highValue];
            };
            return Histogram;
        })();
        Statistics.Histogram = Histogram;
    })(AnalysisPortal.Statistics || (AnalysisPortal.Statistics = {}));
    var Statistics = AnalysisPortal.Statistics;
})(AnalysisPortal || (AnalysisPortal = {}));
//# sourceMappingURL=AnalysisPortal.Statistics.js.map
