﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GuessNumber.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayPage : ContentPage
    {
        GuessNumber service;

        public PlayPage()
        {
            InitializeComponent();
        }

        public string GetTurn
        {
            get
            {
                if (service == null)
                    return "إختر رقماً...";
                else
                {
                    if (service.CurrentTurn == Turn.Human)
                        return "دورك ... أدخل رقماً";
                    else
                        return "دوري ... سأدخل رقماً";
                }
            }
        }

        public Turn NextTurn
        {
            get
            {
                if (service.CurrentTurn == Turn.Human)
                    return Turn.Pc;
                else
                    return Turn.Human;
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;

            //If Delete button pressed and lblNumber has 1 number and more
            if (btn.Text == BtnD.Text)
            {
                //Delete one number from right
                lblNumber.Text = lblNumber.Text.Substring(0, lblNumber.Text.Length - 1);
            }
            //When Next Button pressed and lblNumber is compeated
            else if (btn.Text == BtnN.Text)
            {
                //If this is the human number
                if (service == null)
                {
                    //Prepare service 
                    service = new GuessNumber(lblNumber.Text);
                    lblHumanNumber.Text = lblNumber.Text;
                    if (service.CurrentTurn == Turn.Pc)
                        PcGeussNumber();

                    lblNumber.Text = string.Empty;
                    lblTitle.Text = GetTurn;
                }
                //If this is human choice
                else
                {
                    if (service.CurrentTurn == Turn.Pc)
                    {
                        lblNumber.Text = string.Empty;
                        PcGeussNumber();
                    }
                    else
                    {
                        lblTitle.Text = GetTurn;

                        service.HumanGuesses.Add(
                            new ListViewItem
                            {
                                Choice = lblNumber.Text,
                                Result = new string(Randomize<char>(service.GetResultFromPc(lblNumber.Text).ToArray()).ToArray()),
                            });
                        humanStackLayout.Children.Add(AddToHumanStackLayout(service.HumanGuesses.Last().Choice, service.HumanGuesses.Last().Result));

                        if (service.HumanGuesses.Last().Result == "AAAA")
                        {
                            service.WhoWin = Win.Human;
                            lblPcNumber.Text = service.PcSelectedNumber;
                            DisplayAlert("لقد فزت", "مبروك ، لقد فزت في هذه الجولة!", "موافق");
                        }
                        else
                        {
                            service.CurrentTurn = NextTurn;
                            lblNumber.Text = string.Empty;
                            PcGeussNumber();
                        }
                    }
                }

            }
            //When Other button is pressed
            else
            {
                //Add the number to the right of lblNumber
                lblNumber.Text = $"{lblNumber?.Text}{btn.Text}";
                if (lblNumber.Text.Length > 4)
                    lblNumber.Text = lblNumber.Text.Substring(0, 4);
            }

            UpdateButtons();
        }

        private View AddToHumanStackLayout(string choice, string result)
        {
            var sl = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(20, 0) };
            sl.Children.Add(new Label { Text = choice, TextColor = Color.FromHex("#222244"), FontSize = 20, HorizontalOptions = LayoutOptions.StartAndExpand, HorizontalTextAlignment = TextAlignment.Start });
            sl.Children.Add(new Label { Text = result.Replace("A", "ط ").Replace("Y", "ت ").Trim(), FontSize = 18, VerticalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.EndAndExpand, HorizontalTextAlignment = TextAlignment.End });
            return sl;
        }

        private View AddToPcStackLayout(string choice, string result)
        {
            var sl = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(20, 0) };
            sl.Children.Add(new Label { Text = result.Replace("A", "ط ").Replace("Y", "ت ").Trim(), FontSize = 18, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.StartAndExpand });
            sl.Children.Add(new Label { Text = choice, TextColor = Color.FromHex("#442222"), FontSize = 20, HorizontalTextAlignment = TextAlignment.End, HorizontalOptions = LayoutOptions.EndAndExpand });
            return sl;
        }

        public static T[] Randomize<T>(T[] items)
        {
            Random rand = new Random();

            // For each spot in the array, pick
            // a random item to swap into that spot.
            for (int i = 0; i < items.Length - 1; i++)
            {
                int j = rand.Next(i, items.Length);
                T temp = items[i];
                items[i] = items[j];
                items[j] = temp;
            }

            return items;
        }

        private void PcGeussNumber()
        {
            lblTitle.Text = GetTurn;
            var pcPossibility = service.GetNumberFromPcPossibilities();
            var result = service.GetResultFromHuman(pcPossibility);
            service.PcGuesses.Add(new ListViewItem { Choice = pcPossibility, Result = new string(Randomize<char>(result.ToArray()).ToArray()) });
            pcStackLayout.Children.Add(AddToPcStackLayout(service.PcGuesses.Last().Choice, service.PcGuesses.Last().Result));

            service.RemoveUnAcceptedPossibilities(pcPossibility, result);
            if (service.PcGuesses.Last().Result == "AAAA")
            {
                service.WhoWin = Win.Pc;
                lblPcNumber.Text = service.PcSelectedNumber;
                DisplayAlert("لقد خسرت", "للأسف ، لقد خسرت في هذه الجولة!", "موافق");
            }
            else
            {
                service.CurrentTurn = NextTurn;
                lblTitle.Text = GetTurn;
            }
        }

        private void UpdateButtons()
        {
            //If label is empty
            if (lblNumber.Text.Length == 0)
            {
                //Show all buttons execpt BtnDelete and BtnNext
                foreach (Button btn in grd.Children)
                    if (btn.Text != BtnD.Text && btn.Text != BtnN.Text)
                        btn.IsEnabled = true;
                    else
                        btn.IsEnabled = false;

                //Change label color to red
                lblNumber.TextColor = Color.FromHex("#442222");
            }
            //If label is full
            else if (lblNumber.Text.Length == 4)
            {
                //Hide all buttons execpt BtnDelete and BtnNext
                foreach (Button btn in grd.Children)
                    if (btn.Text != BtnD.Text && btn.Text != BtnN.Text)
                        btn.IsEnabled = false;
                    else
                        btn.IsEnabled = true;

                //Change label color to green
                lblNumber.TextColor = Color.FromHex("#224422");
            }
            //Otherwise 0 < lblNumber < 4
            else
            {
                foreach (Button btn in grd.Children)
                    if (btn.Text == BtnD.Text)
                        btn.IsEnabled = true;
                    else if (btn.Text == BtnN.Text)
                        btn.IsEnabled = false;
                    else if (lblNumber.Text.Contains(btn.Text))
                        btn.IsEnabled = false;
                    else
                        btn.IsEnabled = true;

                //Change label color to red
                lblNumber.TextColor = Color.FromHex("#442222");
            }

            if (service != null && service.WhoWin != Win.NoOne)
            {
                foreach (Button btn in grd.Children)
                    btn.IsEnabled = false;
                
                lblNumber.IsVisible = false;
                btnBack.IsVisible = true;
                lblTitle.Text = string.Empty;
            }
        }

        private async void btnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
