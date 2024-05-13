using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MiauToo;
using Microsoft.Maui.Controls;

namespace MiauToo
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<string> _wordList = new ObservableCollection<string>
        {
            "programmeerimine",
            "arendaja",
            "mudel",
            "tsükkel",
            "tähenduses",
            "veebiarendus",
            "kood",
            "masiiv"
        };

        private ObservableCollection<string> _translationList = new ObservableCollection<string>
        {
            "программирование",
            "разработчик",
            "модель",
            "цикл",
            "значение",
            "веб разработка",
            "код",
            "массив"
        };

        public MainPage()
        {
            InitializeComponent();

            var addButton = new Button
            {
                Text = "Добавить",
                HorizontalOptions = LayoutOptions.Center
            };
            addButton.Clicked += OnAddButtonClicked;

            var stackLayout = new StackLayout();
            stackLayout.Children.Add(addButton);

            var carouselView = new CarouselView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                ItemTemplate = new DataTemplate(() =>
                {
                    var frame = new Frame
                    {
                        CornerRadius = 0, 
                        Padding = 20,
                        HasShadow = true,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        WidthRequest = 300, 
                        HeightRequest = 300 
                    };

                    var label = new Label
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    };
                    frame.Content = label;

                    label.SetBinding(Label.TextProperty, ".");

                    TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += async(s,e)=>
                    {
                        var frame = s as Frame;
                        double currentRotation = frame.Rotation;
                        var label = frame.Content as Label;
                        var word = label.Text;
                        await frame.RotateTo(360, 1000);


                        if (_wordList.Contains(word))
                        {
                            
                            var translationIndex = _wordList.IndexOf(word);
                            var translation = _translationList[translationIndex];
                            label.Text = translation;
                        }
                        else if (_translationList.Contains(word))
                        {
                            
                            var originalIndex = _translationList.IndexOf(word);
                            var originalWord = _wordList[originalIndex];
                            label.Text = originalWord;
                        }

                        
                        await frame.RotateYTo(-90, 250, Easing.CubicInOut);
                        await frame.RotateYTo(0, 250, Easing.CubicInOut);
                    };
                    frame.GestureRecognizers.Add(tapGestureRecognizer);

                    return frame;
                })
            };

            carouselView.ItemsSource = _wordList;

            stackLayout.Children.Add(carouselView);

            Content = stackLayout;
        }

        private EventHandler<TappedEventArgs> OnFrameTapped()
        {
            throw new NotImplementedException();
        }

        private async void OnFrameTapped(object sender, EventArgs e)
        {
            var frame = sender as Frame;
            var label = frame.Content as Label;
            var word = label.Text;

            
            if (_wordList.Contains(word))
            {
                
                var translationIndex = _wordList.IndexOf(word);
                var translation = _translationList[translationIndex];
                label.Text = translation;
            }
            else if (_translationList.Contains(word))
            {
                
                var originalIndex = _translationList.IndexOf(word);
                var originalWord = _wordList[originalIndex];
                label.Text = originalWord;
            }

            await frame.RotateTo(360, 1000);
            await frame.RotateYTo(-90, 250, Easing.CubicInOut);
            await frame.RotateYTo(0, 250, Easing.CubicInOut);
        }





        private async void OnAddButtonClicked(object sender, EventArgs e)
        {
            
            await Navigation.PushAsync(new AddCardPage(_wordList, _translationList));
        }
    }

    public partial class AddCardPage : ContentPage
    {
        private ObservableCollection<string> _wordList;
        private ObservableCollection<string> _translationList;

        private Entry _wordEntry;
        private Entry _translationEntry;

        public AddCardPage(ObservableCollection<string> wordList, ObservableCollection<string> translationList)
        {
           

            _wordList = wordList;
            _translationList = translationList;

            _wordEntry = new Entry
            {
                Placeholder = "Эстонское слово"
            };

            _translationEntry = new Entry
            {
                Placeholder = "Перевод"
            };

            var saveButton = new Button
            {
                Text = "Сохранить",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End
            };
            saveButton.Clicked += OnSaveButtonClicked;

            Content = new StackLayout
            {
                Children = { _wordEntry, _translationEntry, saveButton }
            };
        }

        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            // Добавляем новое слово и его перевод в коллекции
            _wordList.Add(_wordEntry.Text);
            _translationList.Add(_translationEntry.Text);

            // Возвращаемся на предыдущую страницу
            Navigation.PopAsync();
        }
    }
}