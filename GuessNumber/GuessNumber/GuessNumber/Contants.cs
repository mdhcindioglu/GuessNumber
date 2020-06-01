using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xamarin.Forms;

namespace GuessNumber
{
    public static class Contants
    {
        public static string AdUnitIdTest
        {
            get
            {
                switch (Device.RuntimePlatform )
                {
                    case Device.Android:
                        return "ca-app-pub-7567430879346163/8493785388";

                    case Device.iOS:
                        return "";
                         
                    default:
                        throw new InvalidOperationException("Invalid platform");
                }
            }
        }
    }
}
