using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdvertisementSuperSlayer.Browser
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BrowserPage : ContentPage
    {
        public BrowserPage()
        {
            InitializeComponent();
            Browser.Source = "https://accounts.google.com/signin/oauth/oauthchooseaccount?client_id=666426372827-mfsinhpd7km8lf7uoqal65c15tsnm8f1.apps.googleusercontent.com&as=39luXP4A4ayqSd4qTjU9mQ&destination=http%3A%2F%2Flocalhost%3A5000&approval_state=!ChQ5OTdNTjhQWEo2VEhvbHA2M3RnVRIfNF9nU3lqTWljY0VaOERFdWhZOThQYzhsUXlaMzd4WQ%E2%88%99AJDr988AAAAAXfLXOyBpBbZyC7hMtNYJjurFZ4p7aAIJ&oauthgdpr=1&xsrfsig=ChkAeAh8T0UKCD6duQ5Phn39vSzHy1U6aZFMEg5hcHByb3ZhbF9zdGF0ZRILZGVzdGluYXRpb24SBXNvYWN1Eg9vYXV0aHJpc2t5c2NvcGU&flowName=GeneralOAuthFlow";
        }
    }
}