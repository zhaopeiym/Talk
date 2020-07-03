﻿using System;

namespace Talk.Extensions.Helper
{
    /// <summary>
    /// 相关计算
    /// </summary>
    public class CalculateHelper
    {
        private readonly static double[,] seedsNumber = new double[,]
                                            {{ -8.32, -8.12, -7.91, -7.70, -7.48, -7.27, -7.05, -6.84, -6.63, -6.42, -6.21, -6.01, -5.80, -5.61, -5.41, -5.21, -5.00 },
                                            {-7.54,-7.32,-7.10,-6.87,-6.64,-6.40,-6.17,-5.94,-5.72,-5.49,-5.27,-5.05,-4.84,-4.63,-4.42,-4.21,-4.00 },
                                            {-6.77,-6.53,-6.29,-6.04,-5.79,-5.54,-5.29,-5.05,-4.81,-4.57,-4.33,-4.10,-3.87,-3.65,-3.43,-3.21,-3.00 },
                                            {-6.00,-5.74,-5.48,-5.21,-4.94,-4.67,-4.41,-4.15,-3.89,-3.64,-3.39,-3.14,-2.90,-2.66,-2.43,-2.20,-2.00 },
                                            {-5.24,-4.96,-4.67,-4.38,-4.09,-3.81,-3.53,-3.25,-2.98,-2.71,-2.44,-2.19,-1.93,-1.68,-1.43,-1.19,-1.00 },
                                            {-4.47,-4.19,-3.90,-3.61,-3.31,-3.02,-2.72,-2.42,-2.13,-1.83,-1.54,-1.25,-0.95,-0.67,-0.38,-0.09,0.00  },
                                            {-3.76,-3.46,-3.16,-2.86,-2.55,-2.24,-1.93,-1.62,-1.32,-1.01,-0.70,-0.40,-0.10,0.20,0.50,0.79,1.00     },
                                            {-3.03,-2.71,-2.39,-2.07,-1.74,-1.41,-1.08,-0.75,-0.43,-0.10,0.22,0.54,0.86,1.17,1.48,1.80,2.00        },
                                            {-2.31,-1.97,-1.62,-1.28,-0.93,-0.58,-0.23,0.11,0.46,0.80,1.14,1.48,1.81,2.14,2.47,2.80,3.00           },
                                            {-1.59,-1.23,-0.86,-0.49,-0.12,0.25,0.62,0.98,1.34,1.70,2.06,2.41,2.76,3.11,3.46,3.80,4.00             },
                                            {-0.81,-0.49,-0.10,0.30,0.69,1.08,1.46,1.85,2.23,2.60,2.98,3.35,3.72,4.08,4.44,4.80,5.00               },
                                            {-0.16,0.25,0.66,1.08,1.49,1.90,2.30,2.71,3.11,3.50,3.89,4.28,4.67,5.05,5.43,5.80,6.00                 },
                                            {0.54,0.98,1.42,1.85,2.29,2.72,3.14,3.57,3.98,4.40,4.81,5.21,5.62,6.01,6.41,6.80,7.00                  },
                                            {1.24,1.71,2.17,2.63,3.08,3.54,3.98,4.42,4.86,5.29,5.72,6.14,6.56,6.97,7.38,7.79,8.00                  },
                                            {1.94,2.43,2.92,3.40,3.88,4.35,4.82,5.28,5.73,6.18,6.63,7.07,7.50,7.93,8.36,8.78,9.00                  },
                                            {2.55,3.06,3.58,4.09,4.58,5.07,5.54,6.02,6.49,6.95,7.41,7.86,8.29,8.73,9.16,9.59,10.00                 },
                                            {3.24,3.78,4.32,4.85,5.37,5.88,6.38,6.87,7.36,7.84,8.31,8.77,9.24,9.69,10.13,10.57,11.00               },
                                            {3.94,4.50,5.06,5.62,6.15,6.68,7.21,7.72,8.23,8.72,9.21,9.70,10.17,10.64,11.10,11.56,12.00             },
                                            {4.62,5.21,5.79,6.38,6.93,7.49,8.04,8.57,9.09,9.61,10.12,10.62,11.12,11.59,12.07,12.54,13.00           },
                                            {5.30,5.92,6.53,7.13,7.72,8.29,8.85,9.42,9.96,10.50,11.02,11.54,12.05,12.55,13.05,13.52,14.00          },
                                            {5.98,6.62,7.26,7.89,8.50,9.10,9.68,10.26,10.83,11.38,11.93,12.47,12.99,13.50,14.02,14.51,15.00        },
                                            {6.64,7.32,7.99,8.64,9.28,9.90,10.51,11.11,11.69,12.27,12.83,13.38,13.93,14.47,14.98,15.50,16.00       },
                                            {7.31,8.02,8.72,9.39,10.05,10.70,11.34,11.95,12.56,13.16,13.73,14.31,14.87,15.42,15.95,16.48,17.00     },
                                            {7.98,8.72,9.43,10.13,10.82,11.50,12.15,12.80,13.42,14.03,14.64,15.23,15.80,16.37,16.93,17.46,18.00    },
                                            {8.64,9.40,10.15,10.89,11.59,12.29,12.97,13.64,14.28,14.92,15.54,16.15,16.75,17.33,17.90,18.45,19.00   },
                                            {9.30,10.09,10.87,11.63,12.37,13.09,13.79,14.49,15.16,15.81,16.45,17.07,17.69,18.28,18.87,19.44,20.00  },
                                            {9.95,10.78,11.59,12.38,13.14,13.89,14.61,15.33,16.02,16.69,17.35,17.99,18.62,19.24,19.84,20.43,21.00  },
                                            {10.60,11.47,12.31,13.12,13.92,14.69,15.44,16.17,16.88,17.58,18.26,18.92,19.56,20.19,20.81,21.41,22.00 },
                                            {11.25,12.14,13.02,13.86,14.68,15.48,16.26,17.02,17.75,18.46,19.16,19.84,20.50,21.15,21.77,22.40,23.00 },
                                            {11.89,12.83,13.73,14.61,15.46,16.28,17.08,17.86,18.61,19.35,20.06,20.76,21.44,22.11,22.75,23.39,24.00 },
                                            {12.53,13.51,14.44,15.35,16.22,17.08,17.90,18.70,19.48,20.24,20.97,21.68,22.38,23.06,23.73,24.37,25.00 },
                                            {13.18,14.18,15.15,16.09,16.99,17.87,18.73,19.54,20.34,21.13,21.88,22.62,23.33,24.02,24.70,25.36,26.00 },
                                            {13.82,14.86,15.83,16.84,17.76,18.67,19.55,20.39,21.21,22.01,22.79,23.53,24.26,24.98,25.67,26.35,27.00 },
                                            {14.46,15.53,16.57,17.57,18.54,19.46,20.37,21.24,22.08,22.90,23.70,24.46,25.20,25.94,26.64,27.33,28.00 },
                                            {15.10,16.21,17.28,18.31,19.31,20.26,21.20,22.09,22.95,23.79,24.61,25.39,26.15,26.90,27.61,28.32,29.00 },
                                            {15.73,16.88,17.99,19.05,20.08,21.07,22.02,22.94,23.82,24.68,25.51,26.31,27.10,27.85,28.58,29.30,30.00 },
                                            {16.37,17.56,18.70,19.80,20.85,21.87,22.84,23.78,24.69,25.57,26.42,27.24,28.04,28.82,29.56,30.29,31.00 },
                                            {17.00,18.22,19.41,20.54,21.62,22.67,23.67,24.63,25.56,26.47,27.33,28.17,28.99,29.76,30.54,31.27,32.00 },
                                            {17.63,18.90,20.12,21.28,22.40,23.47,24.50,25.48,26.43,27.35,28.24,29.10,29.93,30.73,31.51,32.27,33.00 },
                                            {18.26,19.58,20.83,22.02,23.18,24.28,25.32,26.33,27.31,28.25,29.15,30.03,30.87,31.69,32.49,33.25,34.00 },
                                            {18.90,20.25,21.54,22.78,23.95,25.07,26.15,27.18,28.18,29.15,30.06,30.96,31.82,32.65,33.46,34.24,35.00 },
                                            {19.53,20.92,22.25,23.52,24.73,25.88,26.98,28.04,29.06,30.04,30.98,31.89,32.77,33.61,34.43,35.23,36.00 },
                                            {20.16,21.60,22.96,24.27,25.51,26.69,27.82,28.90,29.93,30.95,31.89,32.81,33.72,34.57,35.40,36.22,37.00 },
                                            {20.79,22.28,23.68,25.01,26.28,27.49,28.64,29.76,30.81,31.83,32.81,33.75,34.65,35.54,36.38,37.21,38.00 },
                                            {21.42,22.95,24.39,25.76,27.07,28.29,29.49,30.61,31.79,32.73,33.72,34.68,35.61,36.50,37.36,38.19,39.00 },
                                            {22.06,23.63,25.11,25.52,27.84,29.11,30.31,31.47,32.57,33.63,34.64,35.62,36.56,37.45,38.33,39.18,40.00 },
                                            {22.69,24.30,25.82,26.76,28.62,29.91,31.14,32.32,33.48,34.53,35.55,36.54,37.50,38.42,39.30,40.17,41.00 },
                                            {23.33,24.98,26.54,27.42,29.40,30.71,31.97,33.18,34.36,35.42,36.47,37.47,38.45,39.38,40.28,41.16,42.00 }};

        private readonly static double[] co1_scope = new double[] { 0, 5, 10, 35, 60, 90, 120, 150 };
        private readonly static double[] laqi_scope = new double[] { 0, 50, 100, 150, 200, 300, 400, 500 };
        /// <summary>
        /// 湿球温度计算
        /// </summary>
        /// <param name="humidity">湿度</param>
        /// <param name="temperature">干球温度</param>
        /// <returns></returns>
        public static double WetBulbTemperatureCalculate(int humidity, float temperature)
        {
            if (humidity < 20 || humidity > 100)
                throw new ArgumentNullException("湿度必须大于20并且小于100");
            if (temperature < -5 || temperature > 42)
                throw new ArgumentNullException("干球温度必须大于-5并且小于42");

            var x = (humidity - 20) / 5;
            var x_number = (humidity - 20) % 5;

            var y = temperature + 5;
            double temp = seedsNumber[(int)y, x];
            var y_number = (int)(y * 10) % 10;
            if (y_number != 0)
            {
                temp = temp + (0.66 + 0.02 * x) / 10 * y_number;
                if (x_number != 0)
                {
                    var temp2 = seedsNumber[(int)y, x + 1] + (0.66 + 0.02 * x) / 10 * y_number;
                    temp = temp + (temp2 - temp) / 5 * x_number;
                }
            }
            else if (x_number != 0)
            {
                temp = temp + (seedsNumber[(int)y, x + 1] - temp) / 5 * x_number;
            }
            return NumberHelper.Round(temp);
        }

        /// <summary>
        /// 含湿量计算
        /// </summary>
        /// <param name="humidity">湿度</param>
        /// <param name="temperature">干球温度</param>
        /// <returns></returns>
        public static MoistureContentData MoistureContentCalculate(int humidity, float temperature)
        {
            if (humidity < 5 || humidity > 100)
                throw new ArgumentNullException("湿度必须大于5并且小于100");
            if (temperature < 0 || temperature > 60)
                throw new ArgumentNullException("干球温度必须大于0并且小于60");
            var data = new MoistureContentData();
            data.PartialPressureSaturatedSteam = Math.Exp(12.062 - 4039.558 / (temperature + 235.379)) * 100000;
            data.MoistureContent = 0.622 * humidity * data.PartialPressureSaturatedSteam / 100 / (100090 - humidity * data.PartialPressureSaturatedSteam / 100) * 1000;
            data.Enthalpy = 1.01 * temperature + 0.001 * data.MoistureContent * (2500 + 1.84 * temperature);
            return data;
        }

        /// <summary>
        /// AQI计算
        /// </summary>
        /// <param name="co1">一氧化碳（mg/m³）</param>
        /// <returns></returns>
        public static double AQICalculate(double co1)
        {
            var co1_index = -1;
            for (int i = 0; i < co1_scope.Length - 1; i++)
            {
                if (co1 >= co1_scope[i] && co1 < co1_scope[i + 1])
                {
                    co1_index = i;
                }
            }

            if (co1_index == -1)
            {
                throw new Exception($"co1[{co1}]不在范围0-150");
            }

            var Cl = co1_scope[co1_index];
            var Ch = co1_scope[co1_index + 1];
            var Il = laqi_scope[co1_index];
            var Ih = laqi_scope[co1_index + 1];

            var value = (Ih - Il) / (Ch - Cl) * (co1 - Cl) + Il;
            return NumberHelper.KeepDigit(value, 0);
        }
    }
}
